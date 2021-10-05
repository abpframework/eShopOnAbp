using System.Collections.Generic;

namespace EShopOnAbp.Shared.Hosting.Gateways
{
    public class OcelotConfiguration
    {
        public string ServiceName { get; set; }
        public string DownstreamPathTemplate { get; set; }
        public string DownstreamScheme { get; set; }
        public string UpstreamPathTemplate { get; set; }
        public List<string> UpstreamHttpMethod { get; set; }
        public List<HostAndPort> DownstreamHostAndPorts { get; set; }
    }

    public class HostAndPort
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }
}