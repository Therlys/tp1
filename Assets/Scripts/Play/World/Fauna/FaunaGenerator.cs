using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Game
{
    public class FaunaGenerator : MonoBehaviour
    {
        private const string FAUNA_ROOT_NAME = "FaunaGameObjects";

        [SerializeField] [Range(0f, 1f)] private float bunniesDensity = 0.01f;
        [SerializeField] [Range(0f, 1f)] private float foxDensity = 0.001f;

        private RandomSeed randomSeed;
        private PrefabFactory prefabFactory;
        private NavigationMesh navigationMesh;

        private GameObject faunaRoot;

        private void Awake()
        {
            randomSeed = Finder.RandomSeed;
            prefabFactory = Finder.PrefabFactory;
            navigationMesh = Finder.NavigationMesh;

            faunaRoot = CreateRoot(FAUNA_ROOT_NAME);
        }

        private void OnEnable()
        {
            navigationMesh.OnNavigationMeshChanged += Generate;
        }

        private void OnDisable()
        {
            navigationMesh.OnNavigationMeshChanged += Generate;
        }

        private GameObject CreateRoot(string gameObjectName)
        {
            var rootGameObject = new GameObject(gameObjectName);
            rootGameObject.transform.parent = transform;
            return rootGameObject;
        }

        private void Generate()
        {
            DestroyFauna();

            CreateFauna();
        }

        private void CreateFauna()
        {
            var nodes = navigationMesh.Nodes.ToList();
            var nbGrassBlocks = nodes.Count;

            var faunaPrefabs = new Dictionary<int, int>
            {
                [0] = (int) (nbGrassBlocks * bunniesDensity),
                [1] = (int) (nbGrassBlocks * foxDensity),
            };

            var bunnyId = 0ul;
            var foxId = 0ul;
            var random = randomSeed.CreateRandom();

            while (faunaPrefabs.Count > 0 && nodes.Count > 0)
            {
                var position = nodes.RemoveRandom(random).Position3D;
                var prefab = faunaPrefabs.SubtractRandom(random);

                if (prefab == 0) prefabFactory.CreateBunny(bunnyId++.ToString(), position, faunaRoot);
                else prefabFactory.CreateFox(foxId++.ToString(), position, faunaRoot);
            }
        }

        public void DestroyFauna()
        {
            for (var i = 0; i < faunaRoot.transform.childCount; i++)
                Destroy(faunaRoot.transform.GetChild(i).gameObject);
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (EditorApplication.isPlaying && isActiveAndEnabled) Generate();
        }
#endif
    }
}