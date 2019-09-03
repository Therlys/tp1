using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class AStarPathFinder : PathFinder
    {
        protected override List<Node> FindPath(Node startNode, Node endNode)
        {
            //From each node, which node it can most efficiently be reached from.
            var previous = new Dictionary<Node, Node>();
            //Known nodes not yet evaluated.
            var openedNodes = new HashSet<Node>();
            //Evaluated nodes
            var closedNodes = new HashSet<Node>();
            //Cost from start node to a specific node.
            var costToNode = new Dictionary<Node, float>();
            //Cost from start node to end node, passing by a specific node.
            var costToEnd = new Dictionary<Node, float>();

            //Cost to start node is 0.
            openedNodes.Add(startNode);
            costToNode[startNode] = 0;
            costToEnd[startNode] = Vector2.Distance(startNode.Position2D, endNode.Position2D);

            bool pathFound = false;
            while (openedNodes.Count > 0)
            {
                var currentNode = GetLeastCostToEndNode(openedNodes, costToEnd);

                if (currentNode == endNode)
                {
                    pathFound = true;
                    break;
                }

                openedNodes.Remove(currentNode);
                closedNodes.Add(currentNode);

                var costToCurrentNode = costToNode.ContainsKey(currentNode) ? costToNode[currentNode] : float.MaxValue;

                foreach (var neighbourNode in currentNode.Neighbours)
                {
                    if (!closedNodes.Contains(neighbourNode))
                    {
                        openedNodes.Add(neighbourNode); //openedNodes is "HashSet". Therefore, it can't have duplicates.

                        var newCostToNeighbour = costToCurrentNode + Vector2.Distance(currentNode.Position2D, neighbourNode.Position2D);

                        var costToNeighbourNode = costToNode.ContainsKey(neighbourNode) ? costToNode[neighbourNode] : float.MaxValue;
                        if (newCostToNeighbour < costToNeighbourNode)
                        {
                            var neighbourCostToEnd = newCostToNeighbour + Vector2.Distance(currentNode.Position2D, endNode.Position2D);

                            previous[neighbourNode] = currentNode;
                            costToNode[neighbourNode] = newCostToNeighbour;
                            costToEnd[neighbourNode] = neighbourCostToEnd;
                        }
                    }
                }
            }

            return pathFound ? GetPathFromPrevious(previous, endNode) : null;
        }

        private static Node GetLeastCostToEndNode(IEnumerable<Node> openedNodes, IReadOnlyDictionary<Node, float> costToEnd)
        {
            var leastCost = float.MaxValue;
            Node leastCostNode = null;
            foreach (var currentNode in openedNodes)
            {
                var nodeCostToEnd = costToEnd.ContainsKey(currentNode) ? costToEnd[currentNode] : float.MaxValue;
                if (nodeCostToEnd < leastCost)
                {
                    leastCost = nodeCostToEnd;
                    leastCostNode = currentNode;
                }
            }

            return leastCostNode;
        }

        private static List<Node> GetPathFromPrevious(IReadOnlyDictionary<Node, Node> previous, Node endNode)
        {
            var path = new List<Node>();

            var currentNode = endNode;
            path.Add(endNode);

            while (previous.ContainsKey(currentNode))
            {
                currentNode = previous[currentNode];
                path.Insert(0, currentNode);
            }

            path.RemoveAt(0); //Remove first, because that's the start of the path, and we're already there.
            
            return path;
        }
    }
}