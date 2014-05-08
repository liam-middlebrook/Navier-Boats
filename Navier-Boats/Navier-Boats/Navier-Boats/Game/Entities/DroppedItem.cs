using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Navier_Boats.Engine.Entities;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using libXNADeveloperConsole;
using Navier_Boats.Engine.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Navier_Boats.Engine.Inventory;
using Navier_Boats.Engine.System;
using Navier_Boats.Engine.Level;
using System.Runtime.Serialization;

namespace Navier_Boats.Game.Entities
{
    [Serializable]
    class DroppedItem : Entity, ISerializable
    {
        private ItemStack item;

        public ItemStack Item
        {
            get { return item; }
            set { item = value; }
        }

        public DroppedItem()
        {
        }

        protected DroppedItem(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.item = (ItemStack)info.GetValue("droppedItem", typeof(ItemStack));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("droppedItem", item);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(item.Item.ItemTexture, this.Position, Color.White);
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (Entity entity in EntityManager.GetInstance().Entities)
            {
                LivingEntity living = entity as LivingEntity;
                if (living == null)
                    continue;

                Rectangle otherRect = living.Texture.Bounds;
                otherRect = new Rectangle((int)living.Position.X, (int)living.Position.Y, otherRect.Width, otherRect.Height);

                Rectangle myRect = Item.Item.ItemTexture.Bounds;
                myRect = new Rectangle((int)Position.X, (int)Position.Y, myRect.Width, myRect.Height);

                if (otherRect.Intersects(myRect))
                {
                    living.Items.AddItem(Item);
                    this.ShouldDestroy = true;
                    break;
                }
            }
        }
    }
}
