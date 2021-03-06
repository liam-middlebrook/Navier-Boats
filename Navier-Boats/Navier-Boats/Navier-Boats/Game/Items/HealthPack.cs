﻿using System;
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
            if (executor.Health >= 100)
                return;

            executor.TakeDamage(-HealAmount);
            executor.Items.RemoveItem(this);
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

        public override bool Equals(object obj)
        {
            HealthPack other = obj as HealthPack;
            if (other == null)
                return false;

            return other.InventoryTexture == this.InventoryTexture && other.ItemTexture == this.ItemTexture && other.MaxStack == this.MaxStack &&
                other.Cost == this.Cost && other.Description == this.Description && other.HealAmount == this.HealAmount;
        }
    }
}
