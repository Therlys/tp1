namespace Game
{
    public sealed class FoxOffspringCreator : OffspringCreator
    {
        protected override Animal CreateOffspringPrefab(Animal otherAnimal)
        {
            var id = "4";
            return PrefabFactory.CreateFox(id, Animal.Position, FaunaRoot);
        }
    }
}