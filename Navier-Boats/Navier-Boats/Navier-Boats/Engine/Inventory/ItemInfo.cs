using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Navier_Boats.Engine.Inventory
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ItemInfo
    {
        [JsonProperty]
        public string Type
        {
            get;
            set;
        }

        [JsonProperty]
        public string Name
        {
            get;
            set;
        }
        [JsonProperty]
        public string Image
        {
            get;
            set;
        }
        [JsonProperty]
        public string inventoryImage
        {
            get;
            set;
        }
        [JsonProperty]
        public string Description
        {
            get;
            set;
        }
        [JsonProperty]
        public int Stack
        {
            get;
            set;
        }
        [JsonProperty]
        public int Cost
        {
            get;
            set;
        }

        [JsonProperty]
        public double Damage
        {
            get;
            set;
        }

        [JsonProperty]
        public double Heal
        {
            get;
            set;
        }

        [JsonProperty]
        public double Range
        {
            get;
            set;
        }
    }
}
