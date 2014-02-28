using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Navier_Boats.Engine.Entities;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Navier_Boats.Game.Entities;
using libXNADeveloperConsole;
using Microsoft.Xna.Framework.Input;

namespace Navier_Boats.Engine.Level
{
    class CurrentLevel
    {
        Chunk[] chunks;
        List<LivingEntity> entities;
        Random randy;
            //Chunks[0] = Quadrant 1 or Top Right
            //Chunks[1] = Quadrant 2 or Top Left
            //Chunks[2] = Quadrant 3 or Bottom Left
            //Chunks[3] = Quadrant 4 or Bottom Right

        private string chunkSaveDirectory = "./LevelData";

        public CurrentLevel()
        {
            entities = new List<LivingEntity>();

            entities.Add(new Player(new Vector2(100, 100)));
            randy = new Random();
            if (!Directory.Exists(chunkSaveDirectory))
            {
                Directory.CreateDirectory(chunkSaveDirectory);
            }
            chunks = new Chunk[4];

            if (File.Exists(Path.Combine(chunkSaveDirectory, "0_0.chunk"))
                && File.Exists(Path.Combine(chunkSaveDirectory, "0_-1.chunk"))
                && File.Exists(Path.Combine(chunkSaveDirectory, "-1_0.chunk"))
                && File.Exists(Path.Combine(chunkSaveDirectory, "-1_-1.chunk")))
            {
                chunks[0] = new Chunk("0_0.chunk", chunkSaveDirectory);
                chunks[1] = new Chunk("0_-1.chunk", chunkSaveDirectory);
                chunks[2] = new Chunk("-1_0.chunk", chunkSaveDirectory);
                chunks[3] = new Chunk("-1_-1.chunk", chunkSaveDirectory);
            }
            else
            {
                chunks[0] = new Chunk(Chunk.CoordsToChunkID(Vector2.Zero)+".chunk", chunkSaveDirectory);
                chunks[1] = new Chunk(Chunk.CoordsToChunkID(new Vector2(-1, 0)) + ".chunk", chunkSaveDirectory);
                chunks[2] = new Chunk(Chunk.CoordsToChunkID(new Vector2(-1, -1)) + ".chunk", chunkSaveDirectory);
                chunks[3] = new Chunk(Chunk.CoordsToChunkID(new Vector2(0, -1)) + ".chunk", chunkSaveDirectory);
            }
            foreach (Chunk chunk in chunks)
                {
                    Console.WriteLine(chunk.CHUNK_ID);
                    SaveChunk(chunk);
                }
        }

        public void LoadContent(ContentManager Content)
        {

            entities[0].Texture = Content.Load<Texture2D>("playerTexture");
            entities[0].HeadTexture = Content.Load<Texture2D>("playerHeadTexture");
            ConsoleWindow.GetInstance().AddCommand(
                new ConsoleCommand(
                    "spawn",
                    (args, logQueue)
                        =>
                    {
                        int i = entities.Count;
                        entities.Add(new Wanderer(new Vector2(250, 250), randy.Next(int.MaxValue)));
                        entities[i].Texture = Content.Load<Texture2D>("playerTexture");
                        entities[i].HeadTexture = Content.Load<Texture2D>("playerHeadTexture");
                        ; return 0;
                    }));
        }

        private void SaveChunk(Chunk chunkToSave)
        {
            chunkToSave.Save(chunkSaveDirectory);
        }

        public void UpdateChunks(GameTime gameTime, KeyboardState keyState, KeyboardState prevKeyState, MouseState mouseState, MouseState prevMouseState)
        {


            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Update(gameTime);
                if (entities[i] is IInputControllable)
                {
                    ((IInputControllable)entities[i]).HandleInput(keyState, prevKeyState, mouseState, prevMouseState);
                }
                if (entities[i].Health < 0)
                {
                    entities.RemoveAt(i);
                    --i;
                }
            }
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i] is ILateUpdateable)
                {
                    ((ILateUpdateable)entities[i]).LateUpdate(gameTime);
                }
            }


            Vector2 playerPos = entities[0].Position * new Vector2(Chunk.CHUNK_WIDTH, Chunk.CHUNK_HEIGHT);//%= new Vector2(Chunk.CHUNK_WIDTH, Chunk.CHUNK_HEIGHT);
            Vector2 chunkOffset = Vector2.Zero;
            Vector2 centerPosition = chunks[0].Position * new Vector2(Chunk.CHUNK_WIDTH, Chunk.CHUNK_HEIGHT) + new Vector2(Chunk.CHUNK_WIDTH, Chunk.CHUNK_HEIGHT);
            //Console.WriteLine("PlayerPos: {0}", playerPosition);
            Console.WriteLine("ChunkCenter: {0}", centerPosition);
            
            // Is the Player Outside of a Reasonable Range - What direction?
            if (Math.Abs(playerPos.X - centerPosition.X) > Chunk.CHUNK_WIDTH / 2)
            {
                chunkOffset.X = (playerPos.X > centerPosition.X) ? 1 : -1;
            }

            // Is the Player Outside of a Reasonable Range - What direction?
            if (Math.Abs(playerPos.Y - centerPosition.Y) > Chunk.CHUNK_HEIGHT / 2)
            {
                chunkOffset.Y = (playerPos.Y > centerPosition.Y) ? 1 : -1;
            }
            //Console.WriteLine("X: {0}", Math.Abs(playerPosition.X - centerPosition.X));
            //Console.WriteLine("Y: {0}", Math.Abs(playerPosition.Y - centerPosition.Y));
            Console.WriteLine("Chunk Offset: {0}", chunkOffset);
            
            if (chunkOffset.X > 0)
            {
                //Console.WriteLine("Removing Chunks in Quadrants 1 and 4");
                chunks[0] = chunks[1];
                chunks[3] = chunks[2];
                Vector2[] newChunks = CheckChunkOffset(chunkOffset, new Vector2[] { chunks[1].Position, chunks[2].Position });
                chunks[1] = new Chunk(Chunk.CoordsToChunkID(newChunks[0]) + ".chunk", chunkSaveDirectory);
                chunks[2] = new Chunk(Chunk.CoordsToChunkID(newChunks[1]) + ".chunk", chunkSaveDirectory);
            }
            else
            {
                //Console.WriteLine("Removing Chunks in Quadrants 2 and 3");
                chunks[1] = chunks[0];
                chunks[2] = chunks[3];
                Vector2[] newChunks = CheckChunkOffset(chunkOffset, new Vector2[] { chunks[0].Position, chunks[3].Position });
                chunks[0] = new Chunk(Chunk.CoordsToChunkID(newChunks[0]) + ".chunk", chunkSaveDirectory);
                chunks[3] = new Chunk(Chunk.CoordsToChunkID(newChunks[1]) + ".chunk", chunkSaveDirectory);
            }
            if (chunkOffset.Y > 0)
            {
                //Console.WriteLine("Removing Chunks in Quadrants 1 and 2");
                chunks[0] = chunks[3];
                chunks[1] = chunks[2];
                Vector2[] newChunks = CheckChunkOffset(chunkOffset, new Vector2[] { chunks[3].Position, chunks[2].Position });
                chunks[3] = new Chunk(Chunk.CoordsToChunkID(newChunks[0]) + ".chunk", chunkSaveDirectory);
                chunks[2] = new Chunk(Chunk.CoordsToChunkID(newChunks[1]) + ".chunk", chunkSaveDirectory);
            }
            else
            {
                //Console.WriteLine("Removing Chunks in Quadrants 3 and 4");
                chunks[3] = chunks[0];
                chunks[2] = chunks[1];
                Vector2[] newChunks = CheckChunkOffset(chunkOffset, new Vector2[] { chunks[0].Position, chunks[1].Position });
                chunks[0] = new Chunk(Chunk.CoordsToChunkID(newChunks[0]) + ".chunk", chunkSaveDirectory);
                chunks[1] = new Chunk(Chunk.CoordsToChunkID(newChunks[1]) + ".chunk", chunkSaveDirectory);
            }
            //*/
        }

        private Vector2[] CheckChunkOffset(Vector2 direction, Vector2[] chunksToReplace)
        {
            Vector2[] newChunks = new Vector2[chunksToReplace.Length];
            
            for (int i = 0; i < chunksToReplace.Length; i++)
            {
                newChunks[i] = chunksToReplace[i] + direction;
            }

            return newChunks;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (LivingEntity entity in entities)
            {
                entity.Draw(spriteBatch);
            }
        }
    }
}
