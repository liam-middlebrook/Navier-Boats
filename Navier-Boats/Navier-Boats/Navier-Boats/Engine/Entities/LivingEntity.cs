using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Navier_Boats.Engine.Graphics;
using Navier_Boats.Game.Entities;
using Navier_Boats.Engine.Inventory;
using Navier_Boats.Engine.System;


namespace Navier_Boats.Engine.Entities
{
    [Serializable]
    public abstract class LivingEntity : Entity, IInteractable
    {
        #region Fields

        /// <summary>
        /// The Health of the LivingEntity
        /// </summary>
        private double health;

        /// <summary>
        /// The Sprite for the LivingEntity's Head
        /// </summary>
        protected Sprite headSprite;


        //The weapon of the entity
        protected ItemInHand weapon;

        //The time in milliseconds since the Entity attacked
        protected int milliSinceAttack;

        public const int ATTACK_FLASH_TIMER = 500;

        protected Texture2D pow;
        #endregion

        #region Properties

        /// <summary>
        /// The Texture for the LivingEntity's Head
        /// </summary>
        public Texture2D HeadTexture { get { return headSprite.Texture; } set { headSprite.Texture = value; } }

        /// <summary>
        /// The Health of the LivingEntity
        /// </summary>
        public double Health { get { return health; } }

        /// <summary>
        /// The amount of money (in the unit 'monies') owned by a LivingEntity
        /// </summary>
        public double Money { get; set; }

        /// <summary>
        /// The Inventory of the LivingEntity
        /// </summary>
        public Inventory.Inventory Items
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Creates a new LivingEntity
        /// </summary>
        /// <param name="initialHealth">The initial health of a LivingEntity</param>
        /// <param name="inventorySize">The maximum size of the inventory of a LivingEntity</param>
        public LivingEntity(double initialHealth, int inventorySize = 32)
            : base()
        {
            headSprite = new Sprite();
            health = initialHealth;
            this.Items = new Inventory.Inventory(inventorySize);
            milliSinceAttack = ATTACK_FLASH_TIMER;
            pow = TextureManager.GetInstance().LoadTexture("pow");
        }

        protected LivingEntity(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.health = info.GetDouble("health");
            this.headSprite = (Sprite)info.GetValue("headSprite", typeof(Sprite));
            this.Items = (Inventory.Inventory)info.GetValue("items", typeof(Inventory.Inventory));
            this.milliSinceAttack = 0;
            this.pow = TextureManager.GetInstance().LoadTexture("pow");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("health", health);
            info.AddValue("headSprite", headSprite);
            info.AddValue("items", Items);
        }

        #region Methods

        /// <summary>
        /// Subtracts health from a LivingEntity
        /// </summary>
        /// <param name="damage">The Amount of Damage to Take</param>
        /// <remarks>A Negative Value will ADD health to the LivingEntity</remarks>
        public void TakeDamage(double damage)
        {

            milliSinceAttack = 0;

            if (ConsoleVars.GetInstance().GodMode) return;

            health -= damage;
            if (health > 100)
            {
                health = 100;
            }
        }

        public virtual void OnDeath()
        {
            List<int> indexList = Items.FindAll<IGameItem>();
            foreach (int index in indexList)
            {
                DroppedItem item = new DroppedItem();
                item.Item = Items.Items[index];
                item.Position = this.Position;
                EntityManager.GetInstance().AddEntity(item);
            }
        }

        /// <summary>
        /// Checks if an entity is interacting with this LivingEntity
        /// </summary>
        /// <param name="entities">The List of Entities to check through for interactions</param>
        public void CheckInteractions(List<Entity> entities)
        {
            foreach (Entity entity in entities)
            {
                if (entity is IInteractable && entity != this)
                {
                    IInteractable interactee = entity as IInteractable;
                    if (this is Player && ((Player)this).CurState == Player.PlayerState.dead)
                    {
                        continue;
                    }
                    else if (Vector2.DistanceSquared(entity.Position, Position) <= 100 * 100)
                    {
                        interactee.Interact(this);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the interaction between a LivingEntity and an IInteractable
        /// </summary>
        /// <param name="interactor">The IIteractable that interacted with this LivingEntity</param>
        public abstract void Interact(IInteractable interactor);

        #endregion

        #region Overridden Methods

        /// <summary>
        /// Updates the LivingEntity
        /// </summary>
        /// <param name="gameTime">Data about the time between update cycles for our game</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (weapon != null)
                weapon.Update(gameTime);
            headSprite.Position = Position;
            milliSinceAttack += gameTime.ElapsedGameTime.Milliseconds;
        }

        /// <summary>
        /// Draws a LivingEntity
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch object to draw the LivingEntity with</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (weapon != null)
                weapon.Draw(spriteBatch, this.Items.SelectedItem.Item.ItemTexture);
            if (milliSinceAttack < ATTACK_FLASH_TIMER)
            {
                spriteBatch.Draw(pow, BoundingRectangle() , new Color(255,255,255,200));
            }
            headSprite.Draw(spriteBatch);
        }

        #endregion

    }
}
