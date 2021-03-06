﻿using System;
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

        private PathJob job = null;

        public bool Done
        {
            get;
            set;
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

        public PathJob CurrentJob
        {
            get
            {
                return job;
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

        public void Run(PathJob job)
        {
            if (Running)
                throw new PathException("Path thread already running");

            this.Done = false;
            this.Result = null;
            this.Error = null;
            this.job = job;
            this.thread = new Thread(new ThreadStart(this.PathfinderTask));
            this.thread.Start();
        }

        public void Stop()
        {
            this.thread.Abort();
        }

        public void Reset()
        {
            this.Done = false;
            this.Result = null;
            this.Error = null;
        }

        protected void PathfinderTask()
        {
            try
            {
                this.Result = this.pathfinder.FindPath(job.Start, job.End, job.Heuristic, job.NodeSize, job.MaxTime);
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
