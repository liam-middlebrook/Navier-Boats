using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Navier_Boats.Engine.Level;

namespace Navier_Boats.Engine.Pathfinding.Threading
{
    public class PathThreadPool
    {
        public delegate void JobCallback(PathResult result);

        public const int ThreadCount = 4;

        private static PathThreadPool instance = null;

        public static PathThreadPool GetInstance()
        {
            if (instance == null)
                instance = new PathThreadPool(ThreadCount);
            return instance;
        }

        private PathThread[] threads = null;

        private LinkedList<PathJob> jobQueue = new LinkedList<PathJob>();

        private LinkedList<PathThread> workingThreads = new LinkedList<PathThread>();

        public PathThreadPool(int threadCount)
        {
            threads = new PathThread[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                Pathfinder pathfinder = new Pathfinder(CurrentLevel.GetInstance());
                threads[i] = new PathThread(pathfinder);
            }
        }
    }
}
