using System;
using System.Collections.Generic;

namespace Drama.Auth.Gateway.Configuration
{
  public class OrleansClientConfiguration
  {
    public IList<OrleansSiloEndpoint> Silos { get; set; }
  }
}
