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
using System.Net.Sockets;

namespace Drama.Core.Gateway.Networking
{
  public class TcpSession
  {
    public Guid Id { get; }
    internal TcpServer Server { get; }
    internal Socket Socket { get; }

    public event EventHandler<DataReceivedEventArgs> DataReceived;
    public event EventHandler<DataSentEventArgs> DataSent;
    public event EventHandler<ClientDisconnectedEventArgs> Disconnected;

    public TcpSession(TcpServer server, Socket socket)
    {
      Server = server;
      Socket = socket;
      Id = Guid.NewGuid();
    }

    public void Send(byte[] data) => Server.Send(this, data);
    public void Send(byte[] data, int offset, int length) => Server.Send(this, data, offset, length);

    internal virtual void OnDataReceived(DataReceivedEventArgs e) => DataReceived?.Invoke(this, e);
    internal virtual void OnDataSent(DataSentEventArgs e) => DataSent?.Invoke(this, e);
    internal virtual void OnDisconnected(ClientDisconnectedEventArgs e) => Disconnected?.Invoke(this, e);
  }
}
