namespace Meta.Core
{
    public interface ISpecimenFactory<T> where T : ISpecimen<T>
    {
        T CreateSpecimen();
    }
}
