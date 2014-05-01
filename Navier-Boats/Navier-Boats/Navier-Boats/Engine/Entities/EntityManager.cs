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

        public readonly string entitySaveLocation = "./LevelData/";

        public List<Entity> Entities {get {return entities;}}

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

        public void RemoveEntity(Entity e)
        {
            entities.Remove(e);
        }

        public void SaveEntities(Vector2 chunkPos)
        {
            string file = entitySaveLocation + Chunk.CoordsToChunkID(chunkPos).Split('.')[0] + ".ent";
            List<Entity> ents = new List<Entity>();
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i] != Player && CurrentLevel.GetEnclosingChunk(entities[i].Position) == chunkPos)
                {
                    ents.Add(entities[i]);
                    entities[i].Unload();
                    entities.RemoveAt(i);
                    --i;
                }
            }

            // NOPE NOPE NOPE: If there was a previously saved entity that no longer exists, we still need to save an empty list of entities to overwrite the old file
            //if (ents.Count == 0) return; //No entities, don't need to save

            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(stream, ents);
            }
        }

        public void LoadEntities(Vector2 chunkPos)
        {
            IFormatter formatter = new BinaryFormatter();
            List<Entity> ents = null;

            string file = entitySaveLocation + Chunk.CoordsToChunkID(chunkPos).Split('.')[0] + ".ent";
            
            if (!File.Exists(file))
            {/*
                Random rnd = CurrentLevel.GetRandom();
                int numZombies = rnd.Next(1, 4);
                Wanderer z;

                for (int i = 1; i <= numZombies; i++)
                {
                    
                    Vector2 pos = Vector2.Zero; 
                    //Work in progress

                    z = new Wanderer(pos);
                    
                    z.Texture = TextureManager.GetInstance().LoadTexture("playerTexture");
                    z.HeadTexture = TextureManager.GetInstance().LoadTexture("playerHeadTexture");
                    
                    this.AddEntity(z);
                }*/
                return;
            }

            using (Stream stream = new FileStream(file, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                ents = (List<Entity>)formatter.Deserialize(stream);
            }

            foreach (Entity ent in ents)
            {
                this.AddEntity(ent);
            }
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
                    LivingEntity ent = entities[i] as LivingEntity;
                    ent.OnDeath();

                    if (ent is Wanderer)
                    {
                        entities.RemoveAt(i);
                        --i;
                    }
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
