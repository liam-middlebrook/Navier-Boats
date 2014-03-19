using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;

namespace Navier_Boats.Engine.Pathfinding
{
    // not actually a thread, just a class that manages threads
    public class PathThread
    {
        private Pathfinder pathfinder = null;
        private Thread thread = null;

        private Vector2 start;
        private Vector2 end;
        private float size;

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

        public bool Running
        {
            get
            {
                if (this.thread == null)
                    return false;

                return this.thread.ThreadState == ThreadState.Running;
            }
        }

        public PathThread(Pathfinder pathfinder)
        {
            this.pathfinder = pathfinder;
            this.Done = false;
            this.Error = null;
            this.Result = null;
        }

        public void Run(Vector2 start, Vector2 end, float size)
        {
            this.Done = false;
            this.Result = null;
            this.start = start;
            this.end = end;
            this.size = size;

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
                this.Result = this.pathfinder.FindPath(this.start, this.end, size);
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
