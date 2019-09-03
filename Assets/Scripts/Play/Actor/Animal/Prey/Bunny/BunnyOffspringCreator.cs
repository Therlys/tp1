namespace Game
{
    public sealed class BunnyOffspringCreator : OffspringCreator
    {
        protected override Animal CreateOffspringPrefab(Animal otherAnimal)
        {
            var id = Animal.name + "+" + otherAnimal.name;
            return PrefabFactory.CreateBunny(id, Animal.Position, FaunaRoot);
        }
    }
}