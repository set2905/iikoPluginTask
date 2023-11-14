using iikoPluginTask.DTO.Constructor;
using Newtonsoft.Json;
using System;

namespace iikoPluginTask.DTO
{
    internal class RpcRequestConstructor<T>
    {
        public string ResultRequest { get; set; }

        public RpcRequestConstructor(string _method, T _params)
        {
            JsonRpcStruct<T> JsonToSerialize = new JsonRpcStruct<T>
            {
                jsonrpc = "2.0",
                method = _method,
                _params = _params,
                id = Guid.NewGuid().ToString()
            };
            ResultRequest = JsonConvert.SerializeObject(JsonToSerialize);
        }
    }
}
