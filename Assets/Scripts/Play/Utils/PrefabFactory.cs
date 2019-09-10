using UnityEngine;
using Random = System.Random;

namespace Game
{
    public class PrefabFactory : MonoBehaviour
    {
        [Header("Animals")] [SerializeField] private GameObject bunny = null;
        [SerializeField] private GameObject fox = null;
        [Header("Plants")] [SerializeField] private GameObject[] grass = new GameObject[3];
        [SerializeField] private GameObject[] tree = new GameObject[3];
        [Header("Objects")] [SerializeField] private GameObject[] rocks = new GameObject[3];
        [Header("Misc")] [SerializeField] private GameObject water = null;

        private RandomSeed randomSeed;

        private void Awake()
        {
            randomSeed = Finder.RandomSeed;
        }

        public Bunny CreateBunny(string id, Vector3 position, GameObject parent = null)
        {
            var newBunny = Create<Bunny>(bunny, position, parent);
            newBunny.name = $"Bunny({id})";
            Finder.GetStatisticGenerator().BunnySpawn();
            return newBunny;
        }

        public Fox CreateFox(string id, Vector3 position, GameObject parent = null)
        {
            var newFox = Create<Fox>(fox, position, parent);
            newFox.name = $"Fox({id})";
            Finder.GetStatisticGenerator().FoxSpawn();
            return newFox;
        }

        public Grass CreateGrass(string id, Vector3 position, Random random, GameObject parent = null)
        {
            var newGrass = Create<Grass>(grass.Random(random), position, parent);
            newGrass.name = $"Grass({id})";
            return newGrass;
        }

        public GameObject CreateTree(string id, Vector3 position, Random random, GameObject parent = null)
        {
            var newTree = Create(tree.Random(random), position, parent);
            newTree.name = $"Tree({id})";
            return newTree;
        }

        public GameObject CreateRock(string id, Vector3 position, Random random, GameObject parent = null)
        {
            var newRock = Create(rocks.Random(random), position, parent);
            newRock.name = $"Rock({id})";
            return newRock;
        }

        public Water CreateWater(string id, Vector3 position, GameObject parent = null)
        {
            var newWater = Create<Water>(water, position, parent);
            newWater.name = $"Water({id})";
            return newWater;
        }

        private static GameObject Create(GameObject prefab, Vector3 position, GameObject parent = null)
        {
            if (!ReferenceEquals(parent, null)) //Comparing a GameObject to null is expensive, because of lifecycle checks.
            {
                return Instantiate(prefab, position, Quaternion.identity, parent.transform);
            }
            else
            {
                return Instantiate(prefab, position, Quaternion.identity);
            }
        }

        private static T Create<T>(GameObject prefab, Vector3 position, GameObject parent = null)
        {
            if (!ReferenceEquals(parent, null)) //Comparing a GameObject to null is expensive, because of lifecycle checks.
            {
                return Instantiate(prefab, position, Quaternion.identity, parent.transform).GetComponent<T>();
            }
            else
            {
                return Instantiate(prefab, position, Quaternion.identity).GetComponent<T>();
            }
        }
    }
}