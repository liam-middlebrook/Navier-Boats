using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Navier_Boats.Engine.Entities;
using Navier_Boats.Engine.Inventory;
using Navier_Boats.Game.Graphics;

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
            bool hitEnemy = false;

            List<Entity> entities = EntityManager.GetInstance().Entities;
            for(int i = 0; i < entities.Count;++i)
            {
                if (entities[i] != executor && entities[i] is LivingEntity && Vector2.DistanceSquared(entities[i].Position, executor.Position) < Range * Range)
                {
                    Vector2 executorToEnemy = entities[i].Position - executor.Position;

                    float attackAngle = (float)Math.Atan2(-executorToEnemy.Y, executorToEnemy.X);
                    //Console.WriteLine(executorToEnemy);

                    float angleDifference = (float)Math.Abs(180 * (executor.HeadRotation % 360) / Math.PI + 180 * (attackAngle % 360) / Math.PI);
                    //Console.WriteLine("Angle Difference: " + angleDifference);

                    if (angleDifference <= 10.0f)
                    {
                        ((LivingEntity)entities[i]).TakeDamage(Damage);

                        if (!hitEnemy)
                        {
                            hitEnemy = true;
                            TracerManager.GetInstance().AddTracer(executor.Position, (float)executor.HeadRotation, (float)Vector2.Distance(executor.Position, entities[i].Position));
                        }
                    }
                }
            }

            if(!hitEnemy)
            {
                TracerManager.GetInstance().AddTracer(executor.Position, (float)executor.HeadRotation, (float)this.Range);
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
