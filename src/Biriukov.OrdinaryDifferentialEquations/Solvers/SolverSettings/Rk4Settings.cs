namespace Biriukov.OrdinaryDifferentialEquations.Solvers.SolverSettings
{
    public class Rk4Settings(double step) : IOdeSolverSettings
    {
        public double Step { get; } = step;
    }
}
