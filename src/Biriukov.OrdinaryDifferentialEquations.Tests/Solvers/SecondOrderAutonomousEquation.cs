using Biriukov.OrdinaryDifferentialEquations.Solvers;

namespace Biriukov.OrdinaryDifferentialEquations.Tests.Solvers
{
    /// <summary>
    /// Система ОДЕ вида
    /// y[0]' = y[1]
    /// y[1]' = y[0]
    /// решение:
    /// y[0](t) = C1 * exp(t) + C2 * exp(-t)
    /// y[1](t) = C1 * exp(t) - C2 * exp(-t)
    /// </summary>
    internal class SecondOrderAutonomousEquation : IDifferentialEquation, IAnalyticalSolutionProvider
    {
        private readonly double[] c;

        public SecondOrderAutonomousEquation(double initialTime, StateVector initialConditions)
        {
            c = CalculateConstants(initialTime, initialConditions);
        }
        
        public StateVector Evaluate(double t, StateVector stateVector) 
            => new([stateVector[1], stateVector[0]]);

        #region Аналитическое решение
        /// <summary>
        /// Вычисляет постоянные интегрирования по н.у.
        /// </summary>
        /// <param name="t">начальное время</param>
        /// <param name="y">начальное значение</param>
        /// <returns>Массив постоянных интегрирования</returns>
        private static double[] CalculateConstants(double t, StateVector y)
        {
            double c1 = (y[0] + y[1]) / (2.0 * Math.Exp(t));
            double c2 = (y[0] - y[1]) / (2.0 * Math.Exp(-t));

            return [c1, c2]; 
        }
       
        public StateVector GetAnalyticalSolution(double time) => new([Y0(time), Y1(time)]);

        private double Y0(double t) => c[0] * Math.Exp(t) + c[1] * Math.Exp(-t);
        private double Y1(double t) => c[0] * Math.Exp(t) - c[1] * Math.Exp(-t);
        #endregion
    }
}
