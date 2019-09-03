using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Game
{
    public class FloraGridGenerator : MonoBehaviour
    {
        [Header("Grass")] [SerializeField] [Range(0f, 1f)] private float grassDensity = 0.01f;
        [Header("Trees")] [SerializeField] [Range(0f, 1f)] private float treesDensity = 0.05f;
        [Header("Rocks")] [SerializeField] [Range(0f, 1f)] private float rocksDensity = 0.01f;

        private RandomSeed randomSeed;
        private TerrainGrid terrain;
        private FloraGrid flora;

        private void Awake()
        {
            randomSeed = Finder.RandomSeed;
            terrain = Finder.Terrain;
            flora = Finder.Flora;
        }

        private void OnEnable()
        {
            terrain.OnTerrainChanged += Generate;
        }

        private void OnDisable()
        {
            terrain.OnTerrainChanged -= Generate;
        }

        public void Generate()
        {
            flora.Blocks = CreateFlora();
        }

        private FloraType[,] CreateFlora()
        {
            var gridSize = terrain.GridSize;
            var grassBlocks = terrain.GrassBlocks.ToList();
            var nbGrassBlocks = grassBlocks.Count;

            var floraTypes = new Dictionary<FloraType, int>
            {
                [FloraType.Grass] = (int) (nbGrassBlocks * grassDensity),
                [FloraType.Tree] = (int) (nbGrassBlocks * treesDensity),
                [FloraType.Rock] = (int) (nbGrassBlocks * rocksDensity)
            };

            var random = randomSeed.CreateRandom();

            var floraObjects = new FloraType[gridSize.x, gridSize.y].Fill(FloraType.None);
            while (floraTypes.Count > 0 && grassBlocks.Count > 0)
            {
                var position = grassBlocks.RemoveRandom(random).GridPosition;
                var floraType = floraTypes.SubtractRandom(random);

                floraObjects[position.x, position.y] = floraType;
            }

            return floraObjects;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (EditorApplication.isPlaying && isActiveAndEnabled) Generate();
        }
#endif
    }
}