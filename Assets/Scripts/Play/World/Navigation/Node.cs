using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Node
    {
        private const int DEFAULT_NEIGHBOURS_COUNT = 4;

        private readonly List<Node> neighbours;

        public Vector2 Position2D { get; }
        public Vector3 Position3D { get; }

        public IReadOnlyList<Node> Neighbours => neighbours;

        public Node(Vector3 position3D)
        {
            Position2D = new Vector2(position3D.x, position3D.z);
            Position3D = position3D;
            neighbours = new List<Node>(DEFAULT_NEIGHBOURS_COUNT);
        }

        public void AddNeighbour(Node neighbour)
        {
            if (!neighbours.Contains(neighbour)) neighbours.Add(neighbour);
        }

        public void RemoveNeighbour(Node neighbour)
        {
            if (neighbours.Contains(neighbour)) neighbours.Remove(neighbour);
        }

        public override string ToString()
        {
            return Position2D.ToString();
        }
    }
}