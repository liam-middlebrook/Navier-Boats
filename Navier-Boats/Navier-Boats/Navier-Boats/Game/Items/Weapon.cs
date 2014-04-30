﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Navier_Boats.Engine.Entities;
using Navier_Boats.Engine.Inventory;

namespace Navier_Boats.Game.Items
{
    [Serializable]
    class Weapon : BaseGameItem
    {
        public double Range
        {
            get;
            set;
        }

        public double Damage
        {
            get;
            set;
        }

        public Weapon()
        {
        }

        public Weapon(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Range = info.GetDouble("range");
            Damage = info.GetDouble("damage");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("range", Range);
            info.AddValue("damage", Damage);
        }

        public override void OnAction(LivingEntity executor)
        {
            List<Entity> entities = EntityManager.GetInstance().Entities;
            for(int i = 0; i < entities.Count;++i)
            {
                if (entities[i] != executor && entities[i] is LivingEntity && Vector2.DistanceSquared(entities[i].Position, executor.Position) < Range * Range)
                {
                    ((LivingEntity)entities[i]).TakeDamage(Damage);
                }
            }
            executor.Items.RemoveItem(this);
        }

        public override string getItemType()
        {
            return "Items.Weapon";
        }

        public override void ImportItem(ItemInfo info)
        {
            base.ImportItem(info);
            Range = info.Range;
            Damage = info.Damage;
        }
    }
}
