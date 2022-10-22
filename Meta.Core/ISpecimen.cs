namespace Meta.Core
{
    public interface ISpecimen<T> where T : ISpecimen<T>
    {
        ISpecimenInitializator<T> SpecimenInitialization { get; }

        void Init();
        void Fix();
        double Evaluate();
        T Clone();
    }
}
