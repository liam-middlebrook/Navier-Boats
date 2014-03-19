using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Navier_Boats.Engine.Level;

namespace Navier_Boats.Engine.Pathfinding
{
    // if you want to use threading with this, use the PathThread class
    public class Pathfinder
    {
        public delegate float Heuristic(Vector2 a, Vector2 b);

        private Dictionary<Vector2, SearchNode> searchNodes = new Dictionary<Vector2, SearchNode>();
        private List<SearchNode> openList = new List<SearchNode>();
        private List<SearchNode> closedList = new List<SearchNode>();

        private CurrentLevel level = null;

        private Heuristic heuristic = null;

        public Pathfinder(CurrentLevel level, Heuristic heuristic)
        {
            this.level = level;
            this.heuristic = heuristic;
        }

        public PathNode QueryPosition(Vector2 position)
        {
            PathNode node = new PathNode();
            node.Position = position;
            short tileData = this.level.GetTileDataAtPoint(TileLayer.COLLISION_LAYER, position);
            node.Walkable = tileData == 0;
            return node;
        }

        public SearchNode GetNode(Vector2 position)
        {
            SearchNode node = null;
            if (searchNodes.TryGetValue(position, out node)) // if we already built this node
                return node;                                // return it

            node = new SearchNode();
            node.Node = QueryPosition(position); // otherwise, query the position
            return node;
        }

        public List<Vector2> FindPath(Vector2 startPoint, Vector2 endPoint, float size)
        {
            if (startPoint == endPoint) // don't do any calculations if we aren't going anywhere
                return new List<Vector2>();

            searchNodes.Clear(); // clear out everything
            openList.Clear();
            closedList.Clear();

            Vector2 newStart = new Vector2(startPoint.X - startPoint.X % size, startPoint.Y - startPoint.Y % size);
            Vector2 newEnd = new Vector2(endPoint.X - endPoint.X % size, endPoint.Y - endPoint.Y % size);

            SearchNode startNode = GetNode(newStart); // get the start and end nodes
            SearchNode endNode = GetNode(newEnd);

            startNode.InOpenList = true;
            startNode.DistanceToGoal = this.heuristic(startPoint, endPoint);
            startNode.DistanceTraveled = 0;
            openList.Add(startNode);

            Vector2[] neighbors = new Vector2[] // diagonal pathing is not supported right now, might be soon
                {
                    new Vector2(size, 0),
                    new Vector2(-size, 0),
                    new Vector2(0, size),
                    new Vector2(0, -size)
                };

            while (openList.Count > 0)
            {
                SearchNode currentNode = FindBestNode();

                if (currentNode == null)
                {
                    throw new PathException("No path found");
                }

                if (currentNode.Equals(endNode))
                {
                    endNode.Parent = currentNode;
                    return FindFinalPath(startNode, endNode);
                }

                for (int i = 0; i < neighbors.Length; i++)
                {
                    SearchNode neighbor = GetNode(neighbors[i] + currentNode.Node.Position);

                    if (neighbor == null || !neighbor.Node.Walkable)
                        continue;

                    float distanceTraveled = currentNode.DistanceTraveled + 1;
                    float heuristic = this.heuristic(neighbor.Node.Position, endPoint);

                    if (!neighbor.InOpenList && !neighbor.InClosedList)
                    {
                        neighbor.DistanceTraveled = distanceTraveled;
                        neighbor.DistanceToGoal = distanceTraveled + heuristic;
                        neighbor.Parent = currentNode;
                        neighbor.InOpenList = true;
                        openList.Add(neighbor);
                    }
                    else
                    {
                        if(neighbor.DistanceTraveled > distanceTraveled)
                        {
                            neighbor.DistanceTraveled = distanceTraveled;
                            neighbor.DistanceToGoal = distanceTraveled + heuristic;
                            neighbor.Parent = currentNode;
                        }
                    }
                }

                openList.Remove(currentNode);
                currentNode.InClosedList = true;
            }

            throw new PathException("Open list ended without finding result");
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
                closedList.Add(parent);
                parent = parent.Parent;
            }

            List<Vector2> finalPath = new List<Vector2>();
            for (int i = closedList.Count - 1; i >= 0; i--)
            {
                Vector2 pos = closedList[i].Node.Position;
                finalPath.Add(pos);
            }

            return finalPath;
        }
    }
}
