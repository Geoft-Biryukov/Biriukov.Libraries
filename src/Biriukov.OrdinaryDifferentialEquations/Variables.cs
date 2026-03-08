namespace Biriukov.OrdinaryDifferentialEquations
{
    /// <summary>
    /// Вектор состояния в момент времени t
    /// </summary>
    /// <param name="t">Заданный момент времени</param>
    /// <param name="state">Вектор состояния</param>
    public class Variables(double t, StateVector state)
    {
        /// <summary>
        /// Значение независимой переменной
        /// </summary>
        public double Time { get; } = t;

        /// <summary>
        /// Значение вектора состояния в точке t
        /// </summary>
        public StateVector State { get; } = state ?? throw new ArgumentNullException(nameof(state));
    }
}
