/* 
 * The Drama project: what you get when a bunch of actors try to host a game.
 * Copyright (C) 2017 Soulson
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Drama.Core.Gateway.Networking
{
	public abstract class PacketRouter
  {
    private readonly ManualResetEvent initialized;

    protected TcpSession Session { get; }

    private readonly EventHandler<DataReceivedEventArgs> sessionDataReceived;
    private readonly EventHandler<ClientDisconnectedEventArgs> sessionDisconnected;

    public PacketRouter(TcpSession session)
    {
      Session = session;
      initialized = new ManualResetEvent(false);

      // session events are raised in IOCP threads—under Windows, at least—so it's important not to wait within their handlers
      sessionDataReceived = async (sender, e) => await SessionDataReceivedAsync(sender, e);
      sessionDisconnected = async (sender, e) => await SessionDisconnectedAsync(sender, e);
      Session.DataReceived += sessionDataReceived;
      Session.Disconnected += sessionDisconnected;
    }

    public async Task InitializeAsync()
    {
      await OnInitialize();
      initialized.Set();
    }

    protected abstract Task OnInitialize();

    private Task SessionDataReceivedAsync(object sender, DataReceivedEventArgs e)
    {
      WaitForInitialization();
      return OnSessionDataReceived(e);
    }

    protected abstract Task OnSessionDataReceived(DataReceivedEventArgs e);

    private async Task SessionDisconnectedAsync(object sender, ClientDisconnectedEventArgs e)
    {
      // waiting for initialization proves self is non-null
      WaitForInitialization();
      await OnSessionDisconnected(e);

      // need to clean up event hooks here so that this object can be GC'd
      Session.DataReceived -= sessionDataReceived;
      Session.Disconnected -= sessionDisconnected;

      initialized.Dispose();
    }

    protected abstract Task OnSessionDisconnected(ClientDisconnectedEventArgs e);

    protected void WaitForInitialization()
    {
      // we need to wait until InitializeAsync finishes before sending any data to the host, or we may not
      //  be able to read the response
      if (!initialized.WaitOne(TimeSpan.FromSeconds(16)))
        throw new InvalidOperationException($"{nameof(PacketRouter)} was not initialized");
    }
  }
}
