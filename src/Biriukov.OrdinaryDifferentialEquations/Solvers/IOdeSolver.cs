namespace Biriukov.OrdinaryDifferentialEquations.Solvers
{
    /// <summary>
    /// Решатель задачи Коши для системы ОДУ
    /// </summary>
    public interface IOdeSolver
    {
        IEnumerable<Variables> Solve(InitialValueProblem problem, double finalTime);            
    }
}
