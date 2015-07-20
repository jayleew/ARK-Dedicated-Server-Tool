using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ServerUpdateService.JSONDataContracts
{
    [JsonObject]
    public class ServerInfoRoot
    {
        [JsonProperty(PropertyName = "server")]
        public ServerInfoType ServerInfo { get; set; }
    }

    [JsonObject]
    public class ServerInfoType
    {
        [JsonProperty(PropertyName = "version")]
        public decimal Version { get; set; }
    }
}
