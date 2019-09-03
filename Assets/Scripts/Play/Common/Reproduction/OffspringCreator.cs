using UnityEngine;

namespace Game
{
    public abstract class OffspringCreator : MonoBehaviour
    {
        [SerializeField] private float reproductionMaxRange = 1f;

        private PrefabFactory prefabFactory;
        private Animal animal;
        private GameObject faunaRoot;

        protected PrefabFactory PrefabFactory => prefabFactory;
        protected Animal Animal => animal;
        protected GameObject FaunaRoot => faunaRoot;

        public float ReproductionMaxRange => reproductionMaxRange;

        public event OffspringCreatorEventHandler OnOffspringCreated;

        private void Awake()
        {
            var transformParent = transform.parent;
            prefabFactory = Finder.PrefabFactory;
            animal = transformParent.GetComponent<Animal>();
            faunaRoot = transformParent.parent.gameObject;
        }

        public bool CreateOffspringWith(Animal otherAnimal)
        {
            if (IsInReach(otherAnimal))
            {
                CreateOffspringPrefab(otherAnimal);

                var effect = new LoseReproductiveUrge();
                effect.ApplyOn(animal.gameObject);
                effect.ApplyOn(otherAnimal.gameObject);

                NotifyOffspringCreated();

                return true;
            }

            return false;
        }

        protected abstract Animal CreateOffspringPrefab(Animal otherAnimal);

        private bool IsInReach(Animal otherAnimal)
        {
            //Use a square to compute max distance for performance reasons.
            if (Mathf.Abs(animal.Position.x - otherAnimal.Position.x) <= reproductionMaxRange) return true;
            if (Mathf.Abs(animal.Position.y - otherAnimal.Position.y) <= reproductionMaxRange) return true;
            return false;
        }

        private void NotifyOffspringCreated()
        {
            if (OnOffspringCreated != null) OnOffspringCreated();
        }
    }

    public delegate void OffspringCreatorEventHandler();
}