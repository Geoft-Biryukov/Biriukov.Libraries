namespace Biriukov.OrdinaryDifferentialEquations
{
    /// <summary>
    /// Решение системы ОДУ в точке
    /// </summary>
    /// <param name="t">Значение независимой переменной</param>
    /// <param name="state">Значение вектора состояния в точке t</param>
    public class DifferentialEquationSolution(double t, StateVector state)
    {
        /// <summary>
        /// Значение независимой переменной
        /// </summary>
        public double T { get; } = t;

        /// <summary>
        /// Значение вектора состояния в точке t
        /// </summary>
        public StateVector State { get; } = state ?? throw new ArgumentNullException(nameof(state));
    }
}
