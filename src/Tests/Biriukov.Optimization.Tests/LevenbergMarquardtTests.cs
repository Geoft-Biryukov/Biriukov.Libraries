using Biriukov.Optimization.LevenbergMarquardtAlgorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biriukov.Optimization.Tests
{  
    [TestFixture]
    public class LevenbergMarquardtTests
    {
        /// <summary>
        /// Тест 1: Проверка на простой линейной функции y = k*x + b.
        /// Алгоритм должен легко находить точные коэффициенты.
        /// </summary>
        [Test]
        public void Optimize_LinearRegression_ReturnsCorrectParameters()
        {
            // Исходные данные для линии y = 2x + 5
            double[] xData = { 1.0, 2.0, 3.0, 4.0, 5.0 };
            double[] yData = { 7.0, 9.0, 11.0, 13.0, 15.0 };

            // Определение вектора остатков (Residuals)
            LevenbergMarquardt.ResidualsDelegate residuals = p =>
            {
                double k = p[0];
                double b = p[1];
                double[] r = new double[xData.Length];
                for (int i = 0; i < xData.Length; i++)
                {
                    r[i] = yData[i] - (k * xData[i] + b);
                }
                return r;
            };

            // Начальное приближение далеко от правильного решения
            double[] initialGuess = { 0.0, 0.0 };

            var options = new LevenbergMarquardtOptions
            {
                Tolerance = 1e-8
            };

            // Действие (Act)
            LevenbergMarquardtResult result = LevenbergMarquardt.Optimize(residuals, initialGuess, options);

            // Проверка (Assert)
            Assert.Multiple(() =>
            {
                Assert.That(result.IsConverged, Is.True, "Алгоритм должен успешно сойтись.");
                Assert.That(result.Parameters[0], Is.EqualTo(2.0).Within(1e-5), "Коэффициент k должен быть равен 2.");
                Assert.That(result.Parameters[1], Is.EqualTo(5.0).Within(1e-5), "Коэффициент b должен быть равен 5.");
                Assert.That(result.FinalError, Is.LessThan(1e-10), "Финальная ошибка должна быть близка к нулю.");
            });
        }

        /// <summary>
        /// Тест 2: Проверка на нелинейной экспоненциальной функции y = a * e^(b * x).
        /// </summary>
        [Test]
        public void Optimize_ExponentialCurveFitting_ReturnsCorrectParameters()
        {
            // Данные для функции y = 2.5 * e^(0.8 * x)
            double[] xData = { 0.5, 1.0, 1.5, 2.0, 2.5 };
            double[] yData = { 3.7294, 5.5636, 8.3003, 12.3826, 18.4726 };

            LevenbergMarquardt.ResidualsDelegate residuals = p =>
            {
                double a = p[0];
                double b = p[1];
                double[] r = new double[xData.Length];
                for (int i = 0; i < xData.Length; i++)
                {
                    r[i] = yData[i] - (a * Math.Exp(b * xData[i]));
                }
                return r;
            };

            // Начальное приближение
            double[] initialGuess = { 1.0, 0.5 };
            var options = new LevenbergMarquardtOptions { Tolerance = 1e-6 };

            // Действие (Act)
            LevenbergMarquardtResult result = LevenbergMarquardt.Optimize(residuals, initialGuess, options);

            // Проверка (Assert)
            Assert.Multiple(() =>
            {
                Assert.That(result.IsConverged, Is.True);
                Assert.That(result.Parameters[0], Is.EqualTo(2.5).Within(1e-3), "Параметр 'a' определен неверно.");
                Assert.That(result.Parameters[1], Is.EqualTo(0.8).Within(1e-3), "Параметр 'b' определен неверно.");
            });
        }

        /// <summary>
        /// Тест 3: Проверка поведения системы, когда задан жесткий лимит итераций.
        /// Алгоритм не должен успеть сойтись.
        /// </summary>
        [Test]
        public void Optimize_LowIterationLimit_ReturnsFalseConvergence()
        {
            double[] xData = { 1, 2, 3 };
            double[] yData = { 10, 20, 30 };

            LevenbergMarquardt.ResidualsDelegate residuals = p =>
                new double[] { yData[0] - p[0], yData[1] - p[0], yData[2] - p[0] };

            double[] initialGuess = { 100.0 };

            // Ставим лимит всего в 1 итерацию
            var options = new LevenbergMarquardtOptions
            {
                MaxIterations = 1,
                Tolerance = 1e-12
            };

            // Действие (Act)
            LevenbergMarquardtResult result = LevenbergMarquardt.Optimize(residuals, initialGuess, options);

            // Проверка (Assert)
            Assert.Multiple(() =>
            {
                Assert.That(result.IsConverged, Is.False, "Алгоритм не должен был сойтись за 1 итерацию.");
                Assert.That(result.IterationsCount, Is.EqualTo(1));
                Assert.That(result.TerminationMessage, Does.Contain("максиональное количество итераций").Or.Contain("максимальное"));
            });
        }
    }

}
