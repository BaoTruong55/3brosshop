using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ShoppingCartItem
    {
        [JsonProperty]
        public Receipt Receipt { get; set; }
        [JsonProperty]
        public int Count { get; set; }
        [JsonProperty]
        public decimal Price { get; set; }
        public decimal Total => Price * Count;
    }
}
