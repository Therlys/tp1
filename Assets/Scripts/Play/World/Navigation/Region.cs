using System;
using System.Collections.Generic;
using System.Linq;
using Harmony;
using UnityEngine;

//TODO : Lire ce commentaire.
//       Vous devez identifier ce sur quoi vous avez travaillé. Pour ce faire, ajoutez un commentaire au dessus
//       des classes/méthodes sur lesquelles vous avez travaillé.
//
//       Consultez cette classe en guise d'exemple.
//
//       Si vous êtes le seul à avoir travaillé sur une classe, ajoutez seulement un commentaire en haut de fichier.
namespace Game
{
    //Author : Benjamin Lemelin, John Smith, Mary Smith
    public class Region
    {
        private const int MAX_NODE_COUNT = 4;
        private const int NB_SUB_REGIONS = 4;
        private const int NORTH_EAST_REGION_INDEX = 0;
        private const int NORTH_WEST_REGION_INDEX = 1;
        private const int SOUTH_EAST_REGION_INDEX = 2;
        private const int SOUTH_WEST_REGION_INDEX = 3;

        private int nodeCount;
        private Node[] nodes;
        private readonly Region[] subregions;

        private readonly Vector2 topLeft;
        private readonly Vector2 bottomRight;
        private readonly Vector2 center;
        private readonly Vector2 size;

        //Reduces memory allocation when, for example, "Find" is called a lot of times.
        //Minimizing memory allocation, and thus garbage collection, greatly improves performances.
        //
        //EX : In a scenario where 12 000 consecutives calls where made to this method.
        //
        //     Before : Alloc -> 0.96 GB, Delay -> 20 seconds
        //     After :  Alloc -> 0.9 MB,  Delay -> 4 miliseconds
        private readonly ReusableList<Node> reusableNodeList;

        public IEnumerable<Node> Nodes => GetNodesEnumerable();
        public IEnumerable<Region> Subregions => GetRegionsEnumerable();

        public Vector2 TopCenter => new Vector2(center.x, topLeft.y);
        public Vector2 BottomCenter => new Vector2(center.x, bottomRight.y);
        public Vector2 LeftCenter => new Vector2(topLeft.x, center.y);
        public Vector2 RightCenter => new Vector2(bottomRight.x, center.y);
        public Vector2 TopLeft => topLeft;
        public Vector2 TopRight => new Vector2(bottomRight.x, topLeft.y);
        public Vector2 BottomLeft => new Vector2(topLeft.x, bottomRight.y);
        public Vector2 BottomRight => bottomRight;
        public Vector2 Center => center;

        public Vector2 Size => size;

        private Region NorthEast => subregions[NORTH_EAST_REGION_INDEX] ?? (subregions[NORTH_EAST_REGION_INDEX] = new Region(TopCenter, RightCenter));
        private Region NorthWest => subregions[NORTH_WEST_REGION_INDEX] ?? (subregions[NORTH_WEST_REGION_INDEX] = new Region(TopLeft, Center));
        private Region SouthEast => subregions[SOUTH_EAST_REGION_INDEX] ?? (subregions[SOUTH_EAST_REGION_INDEX] = new Region(Center, BottomRight));
        private Region SouthWest => subregions[SOUTH_WEST_REGION_INDEX] ?? (subregions[SOUTH_WEST_REGION_INDEX] = new Region(LeftCenter, BottomCenter));

        //Author : Benjamin Lemelin, John Smith, Mary Smith
        public Region(Vector2 topLeft, Vector2 bottomRight)
        {
            if (topLeft.x > bottomRight.x || topLeft.y < bottomRight.y)
                throw new ArgumentException("Unable to create region : top left limit is below or to the right of the bottom right limit.");

            nodeCount = 0;
            nodes = new Node[MAX_NODE_COUNT];
            subregions = new Region[NB_SUB_REGIONS];

            this.topLeft = topLeft;
            this.bottomRight = bottomRight;
            size = new Vector2(bottomRight.x - topLeft.x, topLeft.y - bottomRight.y);
            center = new Vector2(bottomRight.x - size.x / 2, topLeft.y - size.y / 2);

            reusableNodeList = new ReusableList<Node>();
        }

        //Author : Benjamin Lemelin
        public bool HasSubRegions()
        {
            return nodeCount > MAX_NODE_COUNT; //Once there's more than "MaxNodeCount" nodes, there's at least one subregion.
        }

        //Author : Benjamin Lemelin
        public bool Encloses(Vector2 position)
        {
            return position.x >= topLeft.x && position.x <= bottomRight.x &&
                   position.y <= topLeft.y && position.y >= bottomRight.y;
        }

        //Author : Benjamin Lemelin
        public Node Find(Vector2 position, float maxDistance = float.MaxValue)
        {
            /* For performance reasons, we use a square, not a circle, to compute the distance.
             * Ex :
             * 
             *         *  * 
             *      *        * 
             *     *          *
             *     *          *
             *      *        * 
             *         *  *
             * 
             * VS
             * 
             *     +----------+
             *     |          |
             *     |          |
             *     |          |
             *     |          |
             *     +----------+
             */
            return reusableNodeList.Use(candidates =>
            {
                FindAllCandidatesNoAlloc(position, maxDistance, candidates);
                return FindNearestCandidateNoAlloc(position, maxDistance, candidates);
            });
        }

        //Author : Benjamin Lemelin
        public void Add(Node node)
        {
            if (!Encloses(node.Position2D))
                throw new ArgumentException("Node cannot be added into region : it doesn't encloses the node position.");

            if (!IsFull())
            {
                AddIntoRegion(node);
            }
            else
            {
                MigrateIntoSubregions();

                AddIntoSubregions(node);
            }

            nodeCount++;
        }

        //Author : Benjamin Lemelin
        private bool IsFull()
        {
            return nodeCount >= MAX_NODE_COUNT;
        }

        //Author : Benjamin Lemelin, John Smith
        private void AddIntoRegion(Node node)
        {
            if (IsFull())
                throw new ArgumentException("Cannot add node into region : it's full.");

            nodes[nodeCount] = node;
        }

        //Author : Benjamin Lemelin, John Smith
        private void AddIntoSubregions(Node node)
        {
            if (!IsFull())
                throw new ArgumentException("Cannot add node into subregions : there's still space in current region.");

            GetEnclosingSubregion(node.Position2D).Add(node);
        }

        //Author : Benjamin Lemelin, John Smith
        private void MigrateIntoSubregions()
        {
            if (!IsFull())
                throw new ArgumentException("Cannot migrate nodes into subregions : there's still space in current region.");

            if (nodes != null)
            {
                foreach (var node in nodes)
                    AddIntoSubregions(node);

                nodes = null;
            }
        }

        //Author : Benjamin Lemelin
        private Region GetEnclosingSubregion(Vector2 position)
        {
            //Note : When inserting in subregions, we first find which subregion encloses the node position. Because subregions overlap
            //       on their edges, there will be times where two subregions enclose the same position.
            //
            //       Therefore, when we add a node in subregions, we must traverse the sub-regions in the same order as when we perform a 
            //       node search.
            if (position.y >= center.y)
                return position.x >= center.x ? NorthEast : NorthWest;
            else
                return position.x >= center.x ? SouthEast : SouthWest;
        }

        //Author : Benjamin Lemelin
        private bool IntersectWithCircle(Vector2 position, float radius)
        {
            //Find the point of the rectangle that is the closest to the circle's center.
            //Check if that point is in the circle.
            var distanceX = position.x - Mathf.Max(topLeft.x, Mathf.Min(position.x, topLeft.x + size.x));
            var distanceY = position.y - Mathf.Max(bottomRight.y, Mathf.Min(position.y, bottomRight.y + size.y));
            return distanceX * distanceX + distanceY * distanceY < radius * radius;
        }
        
        //Author : Benjamin Lemelin
        private void FindAllCandidatesNoAlloc(Vector2 position, float maxDistance, IList<Node> candidates)
        {
            if (IntersectWithCircle(position, maxDistance))
            {
                if (HasSubRegions())
                {
                    foreach (var subregion in subregions)
                        subregion?.FindAllCandidatesNoAlloc(position, maxDistance, candidates);
                }
                else
                {
                    var maxDistanceSquared = maxDistance * maxDistance;
                    foreach (var node in nodes)
                    {
                        if (node != null)
                        {
                            var distanceSquared = position.SqrDistanceTo(node.Position2D);
                            if (distanceSquared < maxDistanceSquared)
                            {
                                candidates.Add(node);
                            }
                        }
                    }
                }
            }
        }
        
        //Author : Benjamin Lemelin
        private Node FindNearestCandidateNoAlloc(Vector2 position, float maxDistance, IList<Node> candidates)
        {
            Node nearestNode = null;
            var maxDistanceSquared = maxDistance * maxDistance;
            var nearestDistanceSquared = float.MaxValue;
            foreach (var node in candidates)
            {
                if (node != null)
                {
                    var distanceSquared = position.SqrDistanceTo(node.Position2D);
                    if (distanceSquared < maxDistanceSquared && distanceSquared < nearestDistanceSquared)
                    {
                        nearestNode = node;
                        nearestDistanceSquared = distanceSquared;
                    }
                }
            }

            return nearestNode;
        }

        //Author : John Smith, Mary Smith
        private IEnumerable<Node> GetNodesEnumerable()
        {
            if (HasSubRegions())
            {
                foreach (var subregion in subregions)
                    if (subregion != null)
                        foreach (var subNode in subregion.Nodes)
                            yield return subNode;
            }
            else
            {
                foreach (var node in nodes)
                    if (node != null)
                        yield return node;
            }
        }

        //Author : John Smith, Mary Smith
        private IEnumerable<Region> GetRegionsEnumerable()
        {
            foreach (var subregion in subregions)
            {
                if (subregion != null)
                {
                    yield return subregion;

                    foreach (var subSubregion in subregion.Subregions)
                        yield return subSubregion;
                }
            }
        }

        //Author : John Smith, Mary Smith
        public override string ToString()
        {
            return $"Center : {Center}, Size : {Size}";
        }
    }
}