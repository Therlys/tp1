using UnityEngine;

#if UNITY_EDITOR

#endif

namespace Game
{
    public class WaterGenerator : MonoBehaviour
    {
        private const string WATER_ROOT_NAME = "WaterGameObjects";

        private PrefabFactory prefabFactory;
        private TerrainGrid terrain;

        private GameObject waterRoot;

        private void Awake()
        {
            prefabFactory = Finder.PrefabFactory;
            terrain = Finder.Terrain;

            waterRoot = CreateRoot(WATER_ROOT_NAME);
        }

        private void OnEnable()
        {
            terrain.OnTerrainChanged += Generate;
        }

        private void OnDisable()
        {
            terrain.OnTerrainChanged -= Generate;
        }

        private GameObject CreateRoot(string gameObjectName)
        {
            var rootGameObject = new GameObject(gameObjectName);
            rootGameObject.transform.parent = transform;
            return rootGameObject;
        }

        public void Generate()
        {
            DestroyWater();

            CreateWater();
        }

        private void CreateWater()
        {
            //Make sure the water points are inside the navigation mesh, so they are reachable.

            var waterBlocks = terrain.WaterBlocks;
            var gridSize = terrain.GridSize;

            var water = new bool[gridSize.x, gridSize.y].Fill(false);
            foreach (var waterBlock in waterBlocks)
            {
                if (waterBlock.IsEdge)
                {
                    var position = waterBlock.GridPosition;
                    var waterBlockEdges = waterBlock.Edges;

                    if (position.y > 0 && waterBlockEdges.HasFlag(TerrainEdge.North))
                        water[position.x, position.y - 1] = true;
                    if (position.x < gridSize.x - 1 && waterBlockEdges.HasFlag(TerrainEdge.East))
                        water[position.x + 1, position.y] = true;
                    if (position.x > 0 && waterBlockEdges.HasFlag(TerrainEdge.West))
                        water[position.x - 1, position.y] = true;
                    if (position.y < gridSize.y - 1 && waterBlockEdges.HasFlag(TerrainEdge.South))
                        water[position.x, position.y + 1] = true;
                }
            }

            var waterYPosition = Mathf.Max(terrain.WaterBlockHeight, terrain.SandBlockHeight, terrain.GrassBlockHeight);
            var positionOffset = terrain.Scale / 2;
            for (var x = 0; x < gridSize.x; x++)
            {
                for (var y = 0; y < gridSize.y; y++)
                {
                    if (water[x, y])
                    {
                        var position = new Vector3(x + positionOffset, waterYPosition, y + positionOffset);
                        var id = $"{position.x}x{position.z}";

                        prefabFactory.CreateWater(id, position, waterRoot);
                    }
                }
            }
        }

        private void DestroyWater()
        {
            for (var i = 0; i < waterRoot.transform.childCount; i++)
                Destroy(waterRoot.transform.GetChild(i).gameObject);
        }
    }
}