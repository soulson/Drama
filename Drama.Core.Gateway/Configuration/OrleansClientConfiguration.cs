using System;
using System.Collections.Generic;

namespace Drama.Core.Gateway.Configuration
{
  public class OrleansClientConfiguration
  {
    public IList<OrleansSiloEndpoint> Silos { get; set; }
  }
}
