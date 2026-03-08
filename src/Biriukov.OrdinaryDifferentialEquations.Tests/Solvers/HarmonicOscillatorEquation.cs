using Biriukov.OrdinaryDifferentialEquations.Solvers;

namespace Biriukov.OrdinaryDifferentialEquations.Tests.Solvers
{
    public class HarmonicOscillatorEquation : IDifferentialEquation, IAnalyticalSolutionProvider
    {
        private readonly double omega;

        public HarmonicOscillatorEquation(double omega)
        {
            this.omega = omega;
        }

        public StateVector Evaluate(double t, StateVector y)
        {
            // dy1/dt = y2
            // dy2/dt = -ω²y1
            return new StateVector(
            [
                y[1],
            -omega * omega * y[0]
            ]);
        }

        public StateVector GetAnalyticalSolution(double t)
        {
            // Аналитическое решение: y1 = A*cos(ωt), y2 = -A*ω*sin(ωt)
            // Для начальных условий [0.1, 0] получаем A = 0.1
            return new StateVector(
            [
                0.1 * Math.Cos(omega * t),
            -0.1 * omega * Math.Sin(omega * t)
            ]);
        }
    }
}
