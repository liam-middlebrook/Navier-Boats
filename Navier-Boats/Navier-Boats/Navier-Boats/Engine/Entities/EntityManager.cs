using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private static EntityManager _instance;
        public static EntityManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new EntityManager();
            }
            return _instance;
        }


        private List<Entity> entities;

        public Player Player{ get { return (Player)entities[0];}}

        private EntityManager()
        {
            entities = new List<Entity>();
        }

        public void AddEntity(Entity e)
        {
            entities.Add(e);
        }

        public void Update(GameTime gameTime, KeyboardState keyState, KeyboardState prevKeyState, MouseState mouseState, MouseState prevMouseState)
        {

            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Update(gameTime);
                if (entities[i] is IInputControllable)
                {
                    ((IInputControllable)entities[i]).HandleInput(keyState, prevKeyState, mouseState, prevMouseState);
                }
                if (entities[i] is IInteractable)
                {
                    ((IInteractable)entities[i]).CheckInteractions(entities);
                }
                if (((LivingEntity)entities[i]).Health < 0)
                {
                    entities.RemoveAt(i);
                    --i;
                }
            }
        }

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

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Entity e in entities)
            {
                e.Draw(spriteBatch);
            }
        }

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
