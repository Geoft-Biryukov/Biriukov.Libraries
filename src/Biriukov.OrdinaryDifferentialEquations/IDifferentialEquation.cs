namespace Biriukov.OrdinaryDifferentialEquations
{
    /// <summary>
    /// Правые части дифференциальных уравнений
    /// y' = f(t, y)
    /// y = (y0, y1, ... , yn)
    /// </summary>
    public interface IDifferentialEquation
    {
        /// <summary>
        /// Вычисляет значение правых частей в заданной точке фазового пространства
        /// </summary>
        /// <param name="t">Независимая переменная, например, время</param>
        /// <param name="stateVector">Вектор состояния</param>
        /// <returns>Вектор состояния в заданный момент времени</returns>
        StateVector Evaluate(double t, StateVector stateVector);
    }
}
