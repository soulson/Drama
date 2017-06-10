using System;

namespace Drama.Auth.Gateway.Configuration
{
  public class TcpServerConfiguration
  {
    public string BindAddress { get; set; }
    public int BindPort { get; set; }
    public int AcceptQueue { get; set; }
    public int ReceiveBufferBlockSize { get; set; }
    public int ReceiveBufferPoolSize { get; set; }
  }
}
