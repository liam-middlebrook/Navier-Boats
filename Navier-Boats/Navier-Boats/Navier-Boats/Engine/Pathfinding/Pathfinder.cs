using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Navier_Boats.Engine.Level;

namespace Navier_Boats.Engine.Pathfinding
{
    public class Pathfinder
    {
        public delegate float Heuristic(Vector2 a, Vector2 b);

        public static float BasicHeuristic(Vector2 a, Vector2 b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

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
            // placeholder, needs to query the walkable state from level
            return null;
        }

        public SearchNode GetNode(Vector2 position)
        {
            SearchNode node = null;
            if (searchNodes.TryGetValue(position, out node))
                return node;

            node = new SearchNode();
            node.Node = QueryPosition(position);
            return node;
        }

        public List<Vector2> FindPath(Vector2 startPoint, Vector2 endPoint)
        {
            if (startPoint == endPoint)
                return new List<Vector2>();

            searchNodes.Clear();
            openList.Clear();
            closedList.Clear();

            SearchNode startNode = GetNode(startPoint);
            SearchNode endNode = GetNode(endPoint);

            startNode.InOpenList = true;
            startNode.DistanceToGoal = this.heuristic(startPoint, endPoint);
            startNode.DistanceTraveled = 0;
            openList.Add(startNode);

            Vector2[] neighbors = new Vector2[]
                {
                    new Vector2(1, 0),
                    new Vector2(-1, 0),
                    new Vector2(0, 1),
                    new Vector2(0, -1)
                };

            while (openList.Count > 0)
            {
                SearchNode currentNode = FindBestNode();

                if (currentNode == null)
                {
                    Console.WriteLine("Pathfinder: No path found");
                    break;
                }

                if (currentNode == endNode)
                {
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

            return new List<Vector2>();
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
