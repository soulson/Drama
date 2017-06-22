using System;
using Drama.Core.Gateway.Configuration;

namespace Drama.Auth.Gateway.Configuration
{
  public class AuthGatewayConfiguration
  {
    public TcpServerConfiguration Server { get; set; }
    public OrleansClientConfiguration Orleans { get; set; }
  }
}
