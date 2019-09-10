namespace Game
{
    public sealed class BunnyOffspringCreator : OffspringCreator
    {
        protected override Animal CreateOffspringPrefab(Animal otherAnimal)
        {
            var id = "4";
            return PrefabFactory.CreateBunny(id, Animal.Position, FaunaRoot);
        }
    }
}