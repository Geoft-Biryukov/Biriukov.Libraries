namespace Biriukov.Optimization.LevenbergMarquardtAlgorithm
{
    /// <summary>
    /// Настройки для алгоритма Левенберга-Марквардта
    /// </summary>
    public class LevenbergMarquardtOptions
    {
        ///<summary>
        ///Максимальное количество итераций (по умолчанию 100)
        ///</summary>
        public int MaxIterations { get; set; } = 100;

        ///<summary>
        ///Критерий остановки по изменению ошибки (по умолчанию 1e-6)
        ///</summary>
        public double Tolerance { get; set; } = 1e-6;

        ///<summary>
        ///Шаг приращения для численного дифференцирования (по умолчанию 1e-6)
        ///</summary>
        public double Eps { get; set; } = 1e-6;

        ///<summary>
        ///Начальное значение параметра демпфирования лямбда (по умолчанию 0.001)
        ///</summary>
        public double InitialLambda { get; set; } = 0.001;

        ///<summary>
        ///Множитель изменения лямбды при удачном/неудачном шаге (по умолчанию 2.0)
        ///</summary>
        public double LambdaFactor { get; set; } = 2.0;
    }

}
