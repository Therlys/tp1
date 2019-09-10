

namespace Game
{
    public sealed class BunnyOffspringCreator : OffspringCreator
    {
        protected override Animal CreateOffspringPrefab(Animal otherAnimal)
        {
            var id = UnityEngine.Random.Range(0, 300).ToString();
            return PrefabFactory.CreateBunny(id, Animal.Position, FaunaRoot);
        }
    }
}