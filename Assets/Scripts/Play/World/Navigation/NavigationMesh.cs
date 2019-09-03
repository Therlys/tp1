using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class NavigationMesh : MonoBehaviour
    {
#if UNITY_EDITOR
        [Header("Debug")] [SerializeField] private DebugType show = DebugType.NodesAndLinks;
#endif

        private TerrainGrid terrain;
        private FloraGrid flora;
        private Graph graph;

        public IReadOnlyList<Node> Nodes => graph.Nodes;

        public event NavigationMeshChangedEventHandler OnNavigationMeshChanged;

        private void Awake()
        {
            terrain = Finder.Terrain;
            flora = Finder.Flora;
        }

        private void Start()
        {
            UpdateGraph();
        }

        private void OnEnable()
        {
            terrain.OnTerrainChanged += UpdateGraph;
            flora.OnFloraChanged += UpdateGraph;
        }

        private void OnDisable()
        {
            terrain.OnTerrainChanged -= UpdateGraph;
            flora.OnFloraChanged -= UpdateGraph;
        }

        public Node Find(Vector3 position, float maxDistance = float.MaxValue)
        {
            return graph.Find(position, maxDistance);
        }

        private void UpdateGraph()
        {
            //That way, we update the mesh only once per frame.
            StopAllCoroutines();
            StartCoroutine(UpdateGraphRoutine());
        }

        private IEnumerator UpdateGraphRoutine()
        {
            yield return new WaitForEndOfFrame();

            graph = null;

            var terrainBlocks = terrain.Blocks;
            var floraBlocks = flora.Blocks;

            if (terrainBlocks != null)
            {
                var terrainPosition = terrain.transform.position;
                var terrainWorldSize = terrain.WorldSize;

                graph = new Graph(
                    new Vector3(terrainPosition.x, terrainPosition.y, terrainPosition.z + terrainWorldSize.z),
                    new Vector3(terrainPosition.x + terrainWorldSize.x, terrainPosition.y, terrainPosition.z)
                );

                var terrainGridSize = terrain.GridSize;
                var nodes = new Node[terrainGridSize.x, terrainGridSize.y];

                for (var x = 0; x < terrainGridSize.x; x++)
                {
                    for (var y = 0; y < terrainGridSize.y; y++)
                    {
                        var terrainBlock = terrainBlocks[x, y];
                        var floraBlock = floraBlocks == null ? FloraType.None : floraBlocks[x, y];

                        if (terrainBlock.IsWalkable && floraBlock != FloraType.Tree && floraBlock != FloraType.Rock)
                            nodes[x, y] = new Node(terrainBlock.WorldCenterPosition);
                    }
                }

                graph.BeginTransaction();

                for (var x = 0; x < terrainGridSize.x; x++)
                {
                    for (var y = 0; y < terrainGridSize.y; y++)
                    {
                        var node = nodes[x, y];
                        if (node != null)
                        {
                            var northNode = y == 0 ? null : nodes[x, y - 1];
                            var eastNode = x == terrainGridSize.x - 1 ? null : nodes[x + 1, y];
                            var southNode = y == terrainGridSize.y - 1 ? null : nodes[x, y + 1];
                            var westNode = x == 0 ? null : nodes[x - 1, y];

                            if (northNode != null) node.AddNeighbour(northNode);
                            if (eastNode != null) node.AddNeighbour(eastNode);
                            if (southNode != null) node.AddNeighbour(southNode);
                            if (westNode != null) node.AddNeighbour(westNode);

                            graph.Add(node);
                        }
                    }
                }

                graph.EndTransaction();
            }

            NotifyNavigationMeshChanged();
        }

        private void NotifyNavigationMeshChanged()
        {
            if (OnNavigationMeshChanged != null) OnNavigationMeshChanged();
        }

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            if (graph != null)
            {
                switch (show)
                {
                    case DebugType.Nodes:
                        ShowNodes();
                        break;
                    case DebugType.Links:
                        ShowLinks();
                        break;
                    case DebugType.NodesAndLinks:
                        ShowNodes();
                        ShowLinks();
                        break;
                    case DebugType.Regions:
                        ShowRegions();
                        break;
                }
            }
        }

        private void ShowNodes()
        {
            foreach (var node in graph.Nodes)
            {
                GizmosExtensions.DrawPoint(node.Position3D);
            }
        }

        private void ShowLinks()
        {
            foreach (var node in graph.Nodes)
            {
                foreach (var neighbour in node.Neighbours)
                {
                    GizmosExtensions.DrawLine(node.Position3D, neighbour.Position3D, Color.cyan);
                }
            }
        }

        private void ShowRegions()
        {
            var y = Mathf.Max(terrain.GrassBlockHeight, terrain.SandBlockHeight, terrain.WaterBlockHeight);

            foreach (var region in graph.Regions)
            {
                var topLeft2D = region.TopLeft;
                var topRight2D = region.TopRight;
                var bottomLeft2D = region.BottomLeft;
                var bottomRight2D = region.BottomRight;

                var topLeft3D = new Vector3(topLeft2D.x, y, topLeft2D.y);
                var topRight3D = new Vector3(topRight2D.x, y, topRight2D.y);
                var bottomLeft3D = new Vector3(bottomLeft2D.x, y, bottomLeft2D.y);
                var bottomRight3D = new Vector3(bottomRight2D.x, y, bottomRight2D.y);

                GizmosExtensions.DrawLine(topLeft3D, topRight3D, Color.cyan);
                GizmosExtensions.DrawLine(bottomLeft3D, bottomRight3D, Color.cyan);
                GizmosExtensions.DrawLine(topLeft3D, bottomLeft3D, Color.cyan);
                GizmosExtensions.DrawLine(topRight3D, bottomRight3D, Color.cyan);
            }
        }


        private enum DebugType
        {
            // ReSharper disable once UnusedMember.Local
            None,
            Nodes,
            Links,
            NodesAndLinks,
            Regions,
        }

#endif
    }

    public delegate void NavigationMeshChangedEventHandler();
}