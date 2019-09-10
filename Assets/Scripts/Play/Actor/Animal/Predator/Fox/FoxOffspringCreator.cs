namespace Game
{
    public sealed class FoxOffspringCreator : OffspringCreator
    {
        protected override Animal CreateOffspringPrefab(Animal otherAnimal)
        {
            var id = UnityEngine.Random.Range(0, 300).ToString();
            return PrefabFactory.CreateFox(id, Animal.Position, FaunaRoot);
        }
    }
}