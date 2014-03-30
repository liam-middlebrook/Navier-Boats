using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Navier_Boats.Engine.Pathfinding.Threading
{
    // not actually a thread, just a class that manages threads
    public class PathThread
    {
        private Pathfinder pathfinder = null;
        private Thread thread = null;

        private Vector2 start;
        private Vector2 end;
        private Pathfinder.Heuristic heuristic;
        private float size;
        private float maxTime;

        public bool Done
        {
            get;
            protected set;
        }

        public PathException Error
        {
            get;
            protected set;
        }

        public List<Vector2> Result
        {
            get;
            protected set;
        }

        public Pathfinder Pathing
        {
            get
            {
                return pathfinder;
            }
        }

        public bool Running
        {
            get
            {
                if (this.thread == null)
                    return false;

                return this.thread.ThreadState == global::System.Threading.ThreadState.Running;
            }
        }

        public PathThread(Pathfinder pathfinder)
        {
            this.pathfinder = pathfinder;
            this.Done = false;
            this.Error = null;
            this.Result = null;
        }

        public void Run(Vector2 start, Vector2 end, Pathfinder.Heuristic heuristic, float size, float maxTime)
        {
            this.Done = false;
            this.Result = null;
            this.Error = null;
            this.start = start;
            this.end = end;
            this.heuristic = heuristic;
            this.size = size;
            this.maxTime = maxTime;

            this.thread = new Thread(new ThreadStart(this.PathfinderTask));
            this.thread.Start();
        }

        public void Stop()
        {
            this.thread.Abort();
        }

        protected void PathfinderTask()
        {
            try
            {
                this.Result = this.pathfinder.FindPath(this.start, this.end, this.heuristic, this.size, this.maxTime);
            }
            catch (PathException e)
            {
                this.Error = e;
            }
            finally
            {
                this.Done = true;
            }
        }
    }
}
