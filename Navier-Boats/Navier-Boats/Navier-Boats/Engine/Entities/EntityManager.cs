using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using Navier_Boats.Engine.Level;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Navier_Boats.Engine.Graphics;
using Microsoft.Xna.Framework.Input;
using Navier_Boats.Game.Entities;

namespace Navier_Boats.Engine.Entities
{
    public class EntityManager
    {
        #region SINGLETON_MEMBERS

        private static EntityManager _instance;

        /// <summary>
        /// Gets the instance of EntityManger
        /// </summary>
        /// <returns>The instance of EntityManger</returns>
        public static EntityManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new EntityManager();
            }
            return _instance;
        }

        #endregion

        /// <summary>
        /// A list of entities that are loaded in game actively
        /// </summary>
        private List<Entity> entities;

        /// <summary>
        /// A property to access the player
        /// </summary>
        public Player Player{ get { return (Player)entities[0];}}

        /// <summary>
        /// Creates a new instance of EntityManager
        /// </summary>
        private EntityManager()
        {
            entities = new List<Entity>();
        }

        /// <summary>
        /// Adds an entity to EntityManager
        /// </summary>
        /// <param name="e">The entity to add</param>
        public void AddEntity(Entity e)
        {
            entities.Add(e);
        }

        public void SaveEntities(string file, params Entity[] ents)
        {
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(stream, ents);
            }
        }

        public Entity[] LoadEntities(string file)
        {
            IFormatter formatter = new BinaryFormatter();
            Entity[] ents = null;

            using (Stream stream = new FileStream(file, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                ents = (Entity[])formatter.Deserialize(stream);
            }

            foreach (Entity ent in ents)
            {
                this.AddEntity(ent);
            }

            return ents;
        }

        /// <summary>
        /// Updates all of the entities loaded into EntityManager
        /// </summary>
        /// <param name="gameTime">Data about the time between update cycles for our game</param>
        /// <param name="keyState">The current state of the keyboard</param>
        /// <param name="prevKeyState">The previous state of the keyboard</param>
        /// <param name="mouseState">The current state of the mouse</param>
        /// <param name="prevMouseState">The previous state of the mouse</param>
        public void Update(GameTime gameTime, KeyboardState keyState, KeyboardState prevKeyState, MouseState mouseState, MouseState prevMouseState)
        {
            //Update each entity
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Update(gameTime);

                //Update each IInputControllable
                if (entities[i] is IInputControllable)
                {
                    ((IInputControllable)entities[i]).HandleInput(keyState, prevKeyState, mouseState, prevMouseState);
                }

                //Update each IInteractable
                if (entities[i] is IInteractable)
                {
                    ((IInteractable)entities[i]).CheckInteractions(entities);
                }

                //Ensure that all LivingEntities are still alive
                if (((LivingEntity)entities[i]).Health < 0)
                {
                    entities.RemoveAt(i);
                    --i;
                }
            }
        }

        /// <summary>
        /// Preforms a second update call (without user input) on all Entities that implement ILateUpdatable
        /// </summary>
        /// <param name="gameTime">Data about the time between update cycles for our game</param>
        public void LateUpdate(GameTime gameTime)
        {
            foreach (Entity entity in entities)
            {
                if (entity is ILateUpdateable)
                {
                    (entity as ILateUpdateable).LateUpdate(gameTime);
                }
            }
        }

        /// <summary>
        /// Draws each Entity loaded into EntityManager taking into account the camera matrix
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch object to draw each entity with</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Entity e in entities)
            {
                e.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Draws each Entity's GUI elements
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch object to draw with</param>
        public void DrawGUI(SpriteBatch spriteBatch)
        {
            foreach (Entity e in entities)
            {
                if (e is IDrawableGUI)
                {
                    ((IDrawableGUI)e).DrawGUI(spriteBatch);
                }
            }
        }
    }
}
