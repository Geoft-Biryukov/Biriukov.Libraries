namespace Biriukov.Optimization.LevenbergMarquardtAlgorithm
{    
    public class LevenbergMarquardt
    {
        public delegate double[] ResidualsDelegate(double[] parameters);

        /// <summary>
        /// Оптимизирует параметры и возвращает подробную статистику выполнения.
        /// </summary>
        public static LevenbergMarquardtResult Optimize(
            ResidualsDelegate residualsFunc,
            double[] initialParameters,
            LevenbergMarquardtOptions options = null)
        {
            options ??= new LevenbergMarquardtOptions();

            int pCount = initialParameters.Length;
            double[] p = (double[])initialParameters.Clone();

            double lambda = options.InitialLambda;
            double v = options.LambdaFactor;

            double[] r = residualsFunc(p);
            double currentError = ComputeSumOfSquares(r);

            int actualIterations = 0;
            bool converged = false;
            string terminationMessage = "Достигнуто максимальное количество итераций.";

            for (int iter = 0; iter < options.MaxIterations; iter++)
            {
                actualIterations++;
                int rCount = r.Length;

                // Численный Якобиан
                double[,] J = ComputeNumericalJacobian(residualsFunc, p, rCount, options.Eps);

                double[,] JtJ = MultiplyTransposeFirst(J, rCount, pCount);
                double[] JtR = MultiplyTransposeVector(J, r, rCount, pCount);

                double[] deltaP = null;
                double newError = double.MaxValue;
                double[] nextP = new double[pCount];
                bool lambdaLimitExceeded = false;

                while (true)
                {
                    double[,] A = (double[,])JtJ.Clone();
                    for (int i = 0; i < pCount; i++)
                    {
                        A[i, i] += lambda * (JtJ[i, i] == 0 ? 1.0 : JtJ[i, i]);
                    }

                    deltaP = SolveLinearSystem(A, JtR);
                    if (deltaP == null)
                    {
                        lambda *= v;
                        continue;
                    }

                    for (int i = 0; i < pCount; i++)
                    {
                        nextP[i] = p[i] - deltaP[i];
                    }

                    double[] nextR = residualsFunc(nextP);
                    newError = ComputeSumOfSquares(nextR);

                    if (newError < currentError)
                    {
                        lambda /= v;
                        p = (double[])nextP.Clone();
                        r = nextR;
                        break;
                    }
                    else
                    {
                        lambda *= v;
                        if (lambda > 1e10)
                        {
                            lambdaLimitExceeded = true;
                            break;
                        }
                    }
                }

                if (lambdaLimitExceeded)
                {
                    terminationMessage = "Превышен предел затухания (лямбда слишком велика). Дальнейшее улучшение невозможно.";
                    break;
                }

                // Проверка сходимости
                if (Math.Abs(currentError - newError) < options.Tolerance)
                {
                    converged = true;
                    terminationMessage = "Алгоритм успешно сошелся по критерию Tolerance.";
                    currentError = newError;
                    break;
                }

                currentError = newError;
            }

            return new LevenbergMarquardtResult
            {
                Parameters = p,
                FinalError = currentError,
                IterationsCount = actualIterations,
                IsConverged = converged,
                TerminationMessage = terminationMessage
            };
        }

        private static double[,] ComputeNumericalJacobian(ResidualsDelegate residualsFunc, double[] p, int rCount, double eps)
        {
            int pCount = p.Length;
            double[,] J = new double[rCount, pCount];
            double[] pPlus = (double[])p.Clone();
            double[] pMinus = (double[])p.Clone();

            for (int j = 0; j < pCount; j++)
            {
                pPlus[j] = p[j] + eps;
                pMinus[j] = p[j] - eps;

                double[] rPlus = residualsFunc(pPlus);
                double[] rMinus = residualsFunc(pMinus);

                for (int i = 0; i < rCount; i++)
                {
                    J[i, j] = (rPlus[i] - rMinus[i]) / (2.0 * eps);
                }

                pPlus[j] = p[j];
                pMinus[j] = p[j];
            }
            return J;
        }

        private static double ComputeSumOfSquares(double[] vector)
        {
            double sum = 0;
            for (int i = 0; i < vector.Length; i++) sum += vector[i] * vector[i];
            return sum;
        }

        private static double[,] MultiplyTransposeFirst(double[,] J, int rows, int cols)
        {
            double[,] result = new double[cols, cols];
            for (int i = 0; i < cols; i++)
                for (int j = 0; j < cols; j++)
                    for (int k = 0; k < rows; k++)
                        result[i, j] += J[k, i] * J[k, j];
            return result;
        }

        private static double[] MultiplyTransposeVector(double[,] J, double[] r, int rows, int cols)
        {
            double[] result = new double[cols];
            for (int i = 0; i < cols; i++)
                for (int k = 0; k < rows; k++)
                    result[i] += J[k, i] * r[k];
            return result;
        }

        private static double[] SolveLinearSystem(double[,] A, double[] B)
        {
            int n = B.Length;
            double[,] matrix = new double[n, n + 1];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++) matrix[i, j] = A[i, j];
                matrix[i, n] = B[i];
            }

            for (int i = 0; i < n; i++)
            {
                int maxRow = i;
                for (int k = i + 1; k < n; k++)
                    if (Math.Abs(matrix[k, i]) > Math.Abs(matrix[maxRow, i])) maxRow = k;

                for (int k = i; k < n + 1; k++)
                {
                    double tmp = matrix[maxRow, k];
                    matrix[maxRow, k] = matrix[i, k];
                    matrix[i, k] = tmp;
                }

                if (Math.Abs(matrix[i, i]) < 1e-12) return null;

                for (int k = i + 1; k < n; k++)
                {
                    double factor = matrix[k, i] / matrix[i, i];
                    for (int j = i; j < n + 1; j++) matrix[k, j] -= factor * matrix[i, j];
                }
            }

            double[] x = new double[n];
            for (int i = n - 1; i >= 0; i--)
            {
                x[i] = matrix[i, n];
                for (int j = i + 1; j < n; j++) x[i] -= matrix[i, j] * x[j];
                x[i] /= matrix[i, i];
            }
            return x;
        }
    }

}
