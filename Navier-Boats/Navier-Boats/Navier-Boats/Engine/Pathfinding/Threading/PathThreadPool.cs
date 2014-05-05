using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Navier_Boats.Engine;
using Navier_Boats.Engine.Level;
using Navier_Boats.Engine.System;

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

        public PathThreadPool(int threadCount)
        {
            threads = new PathThread[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                Pathfinder pathfinder = new Pathfinder(CurrentLevel.GetInstance());
                threads[i] = new PathThread(pathfinder);
            }
        }

        public void SubmitJob(PathJob job)
        {
            jobQueue.AddLast(job);
        }

        public void CancelAll()
        {
            foreach (PathThread thread in threads)
            {
                if (thread.Running)
                    thread.Stop();
            }

            jobQueue.Clear();
        }

        public void Update()
        {
            for (LinkedListNode<PathJob> node = jobQueue.First; node != jobQueue.Last; node = node.Next)
            {
                if (node.Value.Cancelled)
                {
                    jobQueue.Remove(node);
                }
            }

            foreach (PathThread thread in threads)
            {
                if (thread.CurrentJob != null && thread.CurrentJob.Cancelled == true && thread.Running)
                {
                    thread.Stop();
                    thread.Reset();
                }

                if (!thread.Running)
                {
                    if (thread.Done)
                    {
                        PathResult result = BuildResult(thread);
                        PathJob job = thread.CurrentJob;
                        job.Callback(result);
                        thread.Done = false;
                    }

                    if (jobQueue.Count > 0)
                    {
                        PathJob job = jobQueue.First.Value;
                        jobQueue.RemoveFirst();
                        thread.Reset();
                        thread.Run(job);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            if (ConsoleVars.GetInstance().DebugPathing)
            {
                foreach (PathThread thread in threads)
                {
                    foreach (KeyValuePair<Vector2, SearchNode> entry in thread.Pathing.SearchNodes)
                    {
                        Color color = Color.Red;
                        if (entry.Value.InFinalPath)
                            color = Color.LimeGreen;
                        else if (entry.Value.InClosedList)
                            color = Color.Blue;
                        else if (entry.Value.InOpenList)
                            color = Color.White;
                        spriteBatch.Draw(texture, entry.Key, color);
                    }
                }
            }
        }

        protected PathResult BuildResult(PathThread thread)
        {
            PathResult result = new PathResult();
            result.Path = thread.Result;
            result.SearchNodes = thread.Pathing.SearchNodes;
            result.Error = thread.Error;
            result.ExecuteTime = thread.Pathing.Timer.Elapsed;
            return result;
        }
    }
}
