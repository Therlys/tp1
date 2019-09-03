namespace Game
{
    public sealed class FoxOffspringCreator : OffspringCreator
    {
        protected override Animal CreateOffspringPrefab(Animal otherAnimal)
        {
            var id = Animal.name + "+" + otherAnimal.name;
            return PrefabFactory.CreateFox(id, Animal.Position, FaunaRoot);
        }
    }
}