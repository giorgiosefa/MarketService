using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketService.Api.Extensions
{
    public static class RequestExtensions
    {
        
        public static string ToJson(this object response)
        {
            var resToSer = response;
            return JsonConvert.SerializeObject(resToSer, new JsonSerializerSettings() { Formatting = Formatting.Indented });
        }
    }
}
