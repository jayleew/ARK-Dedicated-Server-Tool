using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ServerUpdateService.JSONDataContracts
{
    [JsonObject]    
    public class GameVersionInfo
    {
        [JsonProperty(PropertyName = "current")]
        public decimal Version { get; set; }
    }
}
