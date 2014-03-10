using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Navier_Boats.Engine.Level;
using Microsoft.Xna.Framework;
using System.IO;

namespace Navier_Boats.Engine.Entities
{
    public class EntityManager
    {
        private List<Entity>[,] entityChunks;
        private string entitySaveDir;

        public EntityManager(string saveDir)
        {
            this.entitySaveDir = saveDir;

            entityChunks = new List<Entity>[2, 2];
            for (int y = 0; y < entityChunks.GetLength(1); y++)
            {
                for (int x = 0; x < entityChunks.GetLength(0); x++)
                {
                    entityChunks[y, x] = new List<Entity>();
                }
            }
        }

        public void LoadChunk(Vector2 arraySlot, string newChunkID)
        {
            entityChunks[(int)arraySlot.X, (int)arraySlot.Y] = LoadEntityData(newChunkID);
        }

        private List<Entity> LoadEntityData(string chunkID)
        {
            string fileLocation = Path.Combine(entitySaveDir, chunkID + ".entities");
            Console.WriteLine(fileLocation);
            return new List<Entity>();
        }
    }
}
