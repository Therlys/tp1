using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Graph
    {
        private readonly Region root;
        private readonly List<Node> nodes; //Cache of all nodes. Faster to access randomly for example.
        private readonly List<Region> regions; //Cache of all regions. Faster to access randomly for example.

        public IReadOnlyList<Node> Nodes => nodes;
        public IReadOnlyList<Region> Regions => regions;

        private bool isInTransaction = false;

        public Graph(Vector3 topLeft, Vector3 bottomRight)
        {
            root = new Region(new Vector2(topLeft.x, topLeft.z), new Vector2(bottomRight.x, bottomRight.y));
            nodes = new List<Node>(0);
            regions = new List<Region>(0);
        }

        public Node Find(Vector3 position, float maxDistance = float.MaxValue)
        {
            return root.Find(new Vector2(position.x, position.z), maxDistance);
        }

        public void BeginTransaction()
        {
            isInTransaction = true;
        }

        public void Add(Node node)
        {
            if (!isInTransaction) throw new Exception("You must open a transaction to edit a Graph.");

            root.Add(node);
        }

        public void EndTransaction()
        {
            nodes.Clear();
            regions.Clear();

            nodes.AddRange(root.Nodes);
            regions.Add(root);
            regions.AddRange(root.Subregions);

            isInTransaction = false;
        }
    }
}