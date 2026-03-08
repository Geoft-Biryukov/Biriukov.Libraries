namespace Biriukov.OrdinaryDifferentialEquations.Solvers
{
    public interface IAnalyticalSolutionProvider
    {
        StateVector GetAnalyticalSolution(double time);
    }
}
