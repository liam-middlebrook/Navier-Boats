using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Navier_Boats.Engine.Level;
using Navier_Boats.Engine.Entities;

namespace Navier_Boats.Engine.Pathfinding
{
    // if you want to use threading with this, use the PathThread class
    public class Pathfinder
    {
        public delegate float Heuristic(Vector2 current, Vector2 end, float resistance);

        public static float GetTileWalkSpeed(short collisionData)
        {
            switch(collisionData)
            {
                case 0:
                    return Entity.ROAD_SPEED_MULT;

                case 1:
                    return Entity.GRASS_SPEED_MULT;

                case 2:
                    return Entity.SAND_SPEED_MULT;

                case 3:
                    return Entity.WATER_SPEED_MULT;
            }

            return float.PositiveInfinity;
        }

        private ConcurrentDictionary<Vector2, SearchNode> searchNodes = new ConcurrentDictionary<Vector2, SearchNode>();
        private List<SearchNode> openList = new List<SearchNode>();
        private List<SearchNode> closedList = new List<SearchNode>();

        private CurrentLevel level = null;

        public ConcurrentDictionary<Vector2, SearchNode> SearchNodes
        {
            get
            {
                return searchNodes;
            }
        }

        public Stopwatch Timer
        {
            get;
            protected set;
        }

        public Pathfinder(CurrentLevel level)
        {
            this.level = level;
            this.Timer = new Stopwatch();
        }

        public PathNode QueryPosition(Vector2 position)
        {
            PathNode node = new PathNode();
            node.Position = position;
            short tileData = this.level.GetTileDataAtPoint(TileLayer.COLLISION_LAYER, position);
            node.Walkable = GetTileWalkSpeed(tileData) != float.PositiveInfinity;
            node.Resistance = GetTileWalkSpeed(tileData);
            return node;
        }

        public SearchNode GetNode(Vector2 position)
        {
            SearchNode node = null;
            if (searchNodes.TryGetValue(position, out node)) // if we already built this node
                return node;                                // return it

            node = new SearchNode();
            node.Node = QueryPosition(position); // otherwise, query the position
            searchNodes.TryAdd(position, node);
            return node;
        }

        public List<Vector2> FindPath(Vector2 startPoint, Vector2 endPoint, Heuristic heuristic, float size, float maxTime)
        {
            try
            {
                this.Timer.Restart();
                if (startPoint == endPoint) // don't do any calculations if we aren't going anywhere
                    return new List<Vector2>();

                searchNodes = new ConcurrentDictionary<Vector2, SearchNode>(); // clear out everything
                openList = new List<SearchNode>();
                closedList = new List<SearchNode>();

                Vector2 newStart = new Vector2(startPoint.X - startPoint.X % size, startPoint.Y - startPoint.Y % size);
                Vector2 newEnd = new Vector2(endPoint.X - endPoint.X % size, endPoint.Y - endPoint.Y % size);

                SearchNode startNode = GetNode(newStart); // get the start and end nodes
                SearchNode endNode = GetNode(newEnd);

                startNode.InOpenList = true;
                startNode.DistanceToGoal = heuristic(startPoint, endPoint, startNode.Node.Resistance);
                startNode.DistanceTraveled = 0;
                openList.Add(startNode);

                Vector2[] neighbors = new Vector2[]
                {
                    new Vector2(size, 0),
                    new Vector2(-size, 0),
                    new Vector2(0, size),
                    new Vector2(0, -size),
                    new Vector2(size, size),
                    new Vector2(size, -size),
                    new Vector2(-size, size),
                    new Vector2(-size, -size)
                };

                while (openList.Count > 0)
                {
                    if (this.Timer.Elapsed.TotalSeconds > maxTime)
                    {
                        throw new PathException("No path found (maximum time exceeded)");
                    }

                    SearchNode currentNode = FindBestNode();

                    if (currentNode == null)
                    {
                        throw new PathException("No path found");
                    }

                    if (currentNode.Equals(endNode))
                    {
                        return FindFinalPath(startNode, endNode);
                    }

                    for (int i = 0; i < neighbors.Length; i++)
                    {
                        SearchNode neighbor = GetNode(neighbors[i] + currentNode.Node.Position);

                        if (neighbor == null || !neighbor.Node.Walkable)
                            continue;

                        float distanceTraveled = currentNode.DistanceTraveled + 1;
                        float h = heuristic(neighbor.Node.Position, endPoint, neighbor.Node.Resistance);

                        if (!neighbor.InOpenList && !neighbor.InClosedList)
                        {
                            neighbor.DistanceTraveled = distanceTraveled;
                            neighbor.DistanceToGoal = distanceTraveled + h;
                            neighbor.Parent = currentNode;
                            neighbor.InOpenList = true;
                            openList.Add(neighbor);
                        }
                        else
                        {
                            if (neighbor.DistanceTraveled > distanceTraveled)
                            {
                                neighbor.DistanceTraveled = distanceTraveled;
                                neighbor.DistanceToGoal = distanceTraveled + h;
                                neighbor.Parent = currentNode;
                            }
                        }
                    }

                    openList.Remove(currentNode);
                    currentNode.InClosedList = true;
                }

                throw new PathException("Open list ended without finding result");
            }
            finally
            {
                this.Timer.Stop();
            }
        }

        private SearchNode FindBestNode()
        {
            SearchNode current = openList[0];
            float smallestDistanceToGoal = float.MaxValue;
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].DistanceToGoal < smallestDistanceToGoal)
                {
                    current = openList[i];
                    smallestDistanceToGoal = current.DistanceToGoal;
                }
            }

            return current;
        }

        private List<Vector2> FindFinalPath(SearchNode startNode, SearchNode endNode)
        {
            closedList.Add(endNode);

            SearchNode parent = endNode.Parent;
            while (parent != startNode)
            {
                if (parent == null)
                    throw new PathException("Unable to find final path (node parent is null)");

                closedList.Add(parent);
                parent = parent.Parent;
                if (parent == null)
                    return new List<Vector2>();
            }

            List<Vector2> finalPath = new List<Vector2>();
            for (int i = closedList.Count - 1; i >= 0; i--)
            {
                SearchNode node = closedList[i];
                node.InFinalPath = true;
                Vector2 pos = node.Node.Position;
                finalPath.Add(pos);
            }

            return finalPath;
        }
    }
}
