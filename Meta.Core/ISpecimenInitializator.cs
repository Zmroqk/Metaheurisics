namespace Meta.Core
{
    public interface ISpecimenInitializator<T> where T : ISpecimen<T>
    {
        void Initialize(T specimen);
    }
}
