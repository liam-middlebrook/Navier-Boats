using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Graphics;
using Navier_Boats.Engine.Entities;
using Navier_Boats.Engine.Inventory;

namespace Navier_Boats.Game.Items
{
    [Serializable]
    class HealthPack : BaseGameItem
    {
        public double HealAmount
        {
            get;
            set;
        }

        public HealthPack()
        {
        }

        public HealthPack(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            HealAmount = info.GetDouble("healAmount");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("healAmount", HealAmount);
        }

        public override void OnAction(LivingEntity executor)
        {
            executor.TakeDamage(-HealAmount);
        }

        public override string getItemType()
        {
            return "Items.HealthPack";
        }

        public override void ImportItem(ItemInfo info)
        {
            base.ImportItem(info);
            HealAmount = info.Heal;
        }
    }
}
