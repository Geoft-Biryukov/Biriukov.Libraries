namespace Biriukov.Optimization.LevenbergMarquardtAlgorithm
{
    /// <summary>
    /// Результат выполнения оптимизации Левенберга-Марквардта
    /// </summary>
    public class LevenbergMarquardtResult
    {
        ///<summary>
        ///Оптимизированные параметры
        ///</summary>
        public double[] Parameters { get; set; }

        ///<summary>
        ///Финальное значение суммы квадратов остатков (ошибка)
        ///</summary>
        public double FinalError { get; set; }

        ///<summary>
        ///Фактическое количество пройденных итераций
        ///</summary>
        public int IterationsCount { get; set; }

        ///<summary>Флаг успешной сходимости по заданному допуску (Tolerance)
        ///</summary>
        public bool IsConverged { get; set; }

        ///<summary>
        ///Текстовое сообщение о причине остановки
        ///</summary>
        public string TerminationMessage { get; set; }
    }
}
