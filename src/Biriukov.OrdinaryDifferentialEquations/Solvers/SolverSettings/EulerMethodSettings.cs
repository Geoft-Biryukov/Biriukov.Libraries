namespace Biriukov.OrdinaryDifferentialEquations.Solvers.SolverSettings
{
    public class EulerMethodSettings(double step) : IOdeSolverSettings
    {
        public double Step { get; } = step;        
    }    
}
