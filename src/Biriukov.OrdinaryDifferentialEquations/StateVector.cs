using System.Collections;

namespace Biriukov.OrdinaryDifferentialEquations
{
    /// <summary>
    /// Вектор состояния системы дифференциальных уравнений
    /// </summary>
    public class StateVector : IEnumerable<double>
    {
        #region fields
        private readonly double[] vector;
        #endregion

        #region Constructors
        /// <summary>
        /// Создает вектор состояний для системы заданного порядка
        /// </summary>
        /// <param name="order">Порядок системы дифф. ур-ий</param>
        /// <exception cref="ArgumentOutOfRangeException">Возникает, если order <= 0</exception>
        public StateVector(int order)
        {
            if (order <= 0) 
                throw new ArgumentOutOfRangeException(nameof(order));

            Order = order;
            vector = new double[order];
        }

        /// <summary>
        /// Создаёт вектор состояний с заданными значениями
        /// </summary>
        /// <param name="values">Значения вектора состояния</param>
        /// <exception cref="ArgumentNullException">Возникает, если передаваемый массив null</exception>
        /// <exception cref="ArgumentException">Возникает, если передаваемый массив не содержит элементов</exception>
        public StateVector(double[] values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));
            if (values.Length == 0)
                throw new ArgumentException("Array cannot be empty", nameof(values));

            Order = values.Length;
            vector = (double[])values.Clone(); // Защита от внешних изменений
        }
        #endregion

        #region Essential
        /// <summary>
        /// Порядок системы дифференциальных уравнений
        /// </summary>
        public int Order { get; }

        /// <summary>
        /// Возвращает или устанавливает значение по заданному индекс
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public double this[int index]
        {
            get
            {
                if (!CheckIndex(index))
                    throw new ArgumentOutOfRangeException(nameof(index));

                return vector[index];
            }

            set
            {
                if (!CheckIndex(index))
                    throw new ArgumentOutOfRangeException(nameof(index));

                vector[index] = value;
            }
        }
        #endregion

        #region Operators
        /// <summary>
        /// Умножение вектора состояния на число
        /// </summary>
        /// <param name="number">Заданное число</param>
        /// <param name="vector">Вектор состояния</param>
        /// <returns>Результат умножения</returns>
        public static StateVector operator * (double number, StateVector vector)
        {
            StateVector result = new(vector.Order);

            for (int i = 0; i < vector.Order; i++)
                result[i] = number * vector[i];

            return result;
        }

        /// <summary>
        /// Умножение вектора состояния на число
        /// </summary>
        /// <param name="number">Заданное число</param>
        /// <param name="vector">Вектор состояния</param>
        /// <returns>Результат умножения</returns>
        public static StateVector operator *(StateVector vector, double number)
        {
            StateVector result = new(vector.Order);

            for (int i = 0; i < vector.Order; i++)
                result[i] = number * vector[i];

            return result;
        }

        /// <summary>
        /// Сумма двух векторов состояния
        /// </summary>
        /// <param name="vector1">Первое слагаемое</param>
        /// <param name="vector2">Второе слагаемое</param>
        /// <returns>Сумму vector1 + vector2</returns>
        /// <exception cref="ArgumentException"></exception>
        public static StateVector operator + (StateVector vector1, StateVector vector2)
        {
            ArgumentNullException.ThrowIfNull(vector1);
            ArgumentNullException.ThrowIfNull(vector2);

            if (vector1.Order != vector2.Order)
                throw new ArgumentException($"Сложение вектров состояния: не совпали размерности");

            StateVector result = new(vector1.Order);

            for (int i = 0; i < vector1.Order; i++)
                result[i] = vector1[i] + vector2[i];

            return result;
        }

        /// <summary>
        /// Разность двух векторов состояния
        /// </summary>
        /// <param name="vector1">Уменьшаемое</param>
        /// <param name="vector2">Вычитаемое</param>
        /// <returns>Разность vector1 - vector2</returns>
        /// <exception cref="ArgumentException"></exception>
        public static StateVector operator -(StateVector vector1, StateVector vector2)
        {
            ArgumentNullException.ThrowIfNull(vector1);
            ArgumentNullException.ThrowIfNull(vector2);

            if (vector1.Order != vector2.Order)
                throw new ArgumentException($"Разность вектров состояния: не совпали размерности");

            StateVector result = new(vector1.Order);

            for (int i = 0; i < vector1.Order; i++)
                result[i] = vector1[i] - vector2[i];

            return result;
        }       
        #endregion

        #region Helpers
        private bool CheckIndex(int index)
            => index >= 0 && index < Order;
        #endregion

        #region Utils
        /// <summary>
        /// Создает и возвращает копию вектора состояния в виде массива
        /// </summary>
        /// <returns>Новый массив double со значениями вектора состояния</returns>
        public double[] ToArray() 
            => (double[])vector.Clone();

        /// <summary>
        /// Копирует вектор состояния в заданный массив 
        /// </summary>
        /// <param name="array">Массив для копирования</param>
        /// <param name="index">Начальный индекс</param>
        public void CopyTo(double[] array, int index)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            
            if (index < 0 || index + vector.Length > array.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            vector.CopyTo(array, index);
        }

        /// <summary>
        /// Заполняет вектор состояния заданным значением
        /// </summary>
        /// <param name="value">Значение для заполнения</param>
        public void Fill(double value) 
            => Array.Fill(vector, value);
        #endregion

        #region interface implementation
        public IEnumerator<double> GetEnumerator()
        {
            foreach (var value in vector)
                yield return value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion       
    }
}
