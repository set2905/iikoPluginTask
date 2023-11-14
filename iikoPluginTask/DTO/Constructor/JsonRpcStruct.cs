﻿using Newtonsoft.Json;

namespace iikoPluginTask.DTO.Constructor
{
    internal class JsonRpcStruct<T>
    {
        [JsonProperty("jsonrpc")]
        internal string jsonrpc { get; set; }

        [JsonProperty("method")]
        internal string method { get; set; }

        [JsonProperty("params")]
        internal T _params { get; set; }

        [JsonProperty("id")]
        internal string id { get; set; }
    }
}
