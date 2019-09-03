using System.Collections.Generic;
using System.Linq;
using Harmony;
using UnityEngine;
using Random = System.Random;

namespace Game
{
    public abstract class PathFinder : MonoBehaviour
    {
        private RandomSeed randomSeed;
        private NavigationMesh navigationMesh;
        private Random random;

        protected void Awake()
        {
            randomSeed = Finder.RandomSeed;
            navigationMesh = Finder.NavigationMesh;
        }

        private void OnEnable()
        {
            navigationMesh.OnNavigationMeshChanged += UpdateRandom;
        }

        private void OnDisable()
        {
            navigationMesh.OnNavigationMeshChanged -= UpdateRandom;
        }

        private void UpdateRandom()
        {
            random = randomSeed.CreateRandom();
        }

        public List<Node> FindPath(Vector3 start, Vector3 end, float maxDistanceToStart = float.MaxValue, float maxDistanceToEnd = float.MaxValue)
        {
            var startNode = navigationMesh.Find(start, maxDistanceToStart);
            if (startNode == null) return null;

            var endNode = navigationMesh.Find(end, maxDistanceToEnd);
            if (endNode == null) return null;

            return FindPath(startNode, endNode);
        }

        public List<Node> FindRandomWalk(Vector3 start, int maxSteps, float maxDistanceToStart = float.MaxValue)
        {
            var startNode = navigationMesh.Find(start, maxDistanceToStart);
            if (startNode == null) return null;

            var path = new List<Node>();
            var pathOptions = new List<Node>();

            var nbSteps = random.Next(1, maxSteps);
            var currentNode = startNode;
            var sqrDistanceFromStart = 0f;
            for (var i = 0; i < nbSteps && currentNode != null; i++)
            {
                pathOptions.AddRange(currentNode.Neighbours);

                while (pathOptions.Count > 0)
                {
                    currentNode = pathOptions.RemoveRandom(random);
                    var sqDistanceToCurrentNode = startNode.Position2D.SqrDistanceTo(currentNode.Position2D);
                    if (sqDistanceToCurrentNode > sqrDistanceFromStart)
                    {
                        path.Add(currentNode);
                        sqrDistanceFromStart = sqDistanceToCurrentNode;
                        break;
                    }

                    //No more options are available to go farther.
                    if (pathOptions.Count == 0)
                    {
                        currentNode = null;
                        break;
                    }
                }

                pathOptions.Clear();
            }

            return path;
        }

        public Node FindFleePath(Vector3 start, Vector3 fleePosition, float maxDistanceToStart = float.MaxValue)
        {
            var startNode = navigationMesh.Find(start, maxDistanceToStart);
            if (startNode == null) return null;

            var sqrDistanceFleeToStart = fleePosition.SqrDistanceTo(start);
            var fleeOptions = startNode.Neighbours.ToList();
            while (fleeOptions.Count > 0)
            {
                var currentNode = fleeOptions.RemoveRandom(random);
                if (fleePosition.SqrDistanceTo(currentNode.Position3D) > sqrDistanceFleeToStart)
                {
                    return currentNode;
                }
            }

            return null;
        }

        protected abstract List<Node> FindPath(Node startNode, Node endNode);
    }
}