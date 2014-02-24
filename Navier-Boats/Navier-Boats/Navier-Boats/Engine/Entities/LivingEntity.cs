using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Navier_Boats.Engine.Graphics;

namespace Navier_Boats.Engine.Entities
{
    abstract class LivingEntity : Entity
    {
        #region Fields

        private double health;

        protected Sprite headSprite;

        #endregion

        #region Properties

        public Texture2D HeadTexture { get { return headSprite.Texture; } set { headSprite.Texture = value; } }

        public double Health { get { return health; } }

        #endregion

        public LivingEntity(double initialHealth)
            : base()
        {
            headSprite = new Sprite();
            health = initialHealth;
        }

        #region Methods

        public void TakeDamage(double damage)
        {
            health -= damage;
        }

        #endregion

        #region Overridden Methods

        public abstract override void Interact(LivingEntity interactor);

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            headSprite.Position = Position;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            headSprite.Draw(spriteBatch);
        }

        #endregion

    }
}
