using System.Numerics;

namespace Biriukov.Orientation.Core
{
    /// <summary>
    /// Матрица направляющих косинусов (DCM) 3x3
    /// Представляет ориентацию связанной системы координат относительно опорной
    /// </summary>
    public struct DirectionCosineMatrix : IEquatable<DirectionCosineMatrix>
    {
        // Элементы матрицы в формате row-major
        // R = [R11, R12, R13]
        //     [R21, R22, R23]
        //     [R31, R32, R33]

        private readonly double[,] matrix;

        #region Компоненты матрицы направляющих косинусов
        /// <summary>
        /// Элемент (1,1) - проекция оси X связанной СК на ось X опорной СК
        /// </summary>
        public double R11 => matrix[0, 0];

        /// <summary>
        /// Элемент (1,2) - проекция оси Y связанной СК на ось X опорной СК
        /// </summary>
        public double R12 => matrix[0, 1];

        /// <summary>
        /// Элемент (1,3) - проекция оси Z связанной СК на ось X опорной СК
        /// </summary>
        public double R13 => matrix[0, 2];

        /// <summary>
        /// Элемент (2,1) - проекция оси X связанной СК на ось Y опорной СК
        /// </summary>
        public double R21 => matrix[1, 0];

        /// <summary>
        /// Элемент (2,2) - проекция оси Y связанной СК на ось Y опорной СК
        /// </summary>
        public double R22 => matrix[1, 1];

        /// <summary>
        /// Элемент (2,3) - проекция оси Z связанной СК на ось Y опорной СК
        /// </summary>
        public double R23 => matrix[1, 2];

        /// <summary>
        /// Элемент (3,1) - проекция оси X связанной СК на ось Z опорной СК
        /// </summary>
        public double R31 => matrix[2, 0];

        /// <summary>
        /// Элемент (3,2) - проекция оси Y связанной СК на ось Z опорной СК
        /// </summary>
        public double R32 => matrix[2, 1];

        /// <summary>
        /// Элемент (3,3) - проекция оси Z связанной СК на ось Z опорной СК
        /// </summary>
        public double R33 => matrix[2, 2];
        #endregion


        #region Конструкторы
        /// <summary>
        /// Создает матрицу направляющих косинусов из массива 3x3
        /// </summary>
        /// <param name="matrix">Массив элементов 3x3 (row-major)</param>
        public DirectionCosineMatrix(double[,] matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix));

            if (matrix.GetLength(0) != 3 || matrix.GetLength(1) != 3)
                throw new ArgumentException("Матрица должна быть размером 3x3");

            this.matrix = (double[,])matrix.Clone();

            // Проверяем ортонормированность (с допуском)
            if (!IsOrthonormal(1e-6))
                // Попробуем ортонормировать
                Orthonormalize();
        }

        /// <summary>
        /// Создает матрицу направляющих косинусов из отдельных элементов
        /// </summary>
        public DirectionCosineMatrix(
            double r11, double r12, double r13,
            double r21, double r22, double r23,
            double r31, double r32, double r33)
        {
            matrix = new double[3, 3];
            matrix[0, 0] = r11; matrix[0, 1] = r12; matrix[0, 2] = r13;
            matrix[1, 0] = r21; matrix[1, 1] = r22; matrix[1, 2] = r23;
            matrix[2, 0] = r31; matrix[2, 1] = r32; matrix[2, 2] = r33;

            // Проверяем ортонормированность
            if (!IsOrthonormal(1e-6))
                Orthonormalize();
        }
        #endregion

        /// <summary>
        /// Единичная матрица (отсутствие вращения)
        /// </summary>
        public static DirectionCosineMatrix Identity => new DirectionCosineMatrix(
            1, 0, 0,
            0, 1, 0,
            0, 0, 1);

        /// <summary>
        /// Проверяет, является ли матрица ортонормированной
        /// </summary>
        /// <param name="tolerance">Допуск</param>
        public bool IsOrthonormal(double tolerance = 1e-9)
        {
            // Проверка ортогональности: R * R^T = I

            // Первая строка должна быть единичным вектором
            double len1 = R11 * R11 + R12 * R12 + R13 * R13;
            if (Math.Abs(len1 - 1.0) > tolerance) return false;

            // Вторая строка должна быть единичным вектором
            double len2 = R21 * R21 + R22 * R22 + R23 * R23;
            if (Math.Abs(len2 - 1.0) > tolerance) return false;

            // Третья строка должна быть единичным вектором
            double len3 = R31 * R31 + R32 * R32 + R33 * R33;
            if (Math.Abs(len3 - 1.0) > tolerance) return false;

            // Строки должны быть ортогональны
            double dot12 = R11 * R21 + R12 * R22 + R13 * R23;
            if (Math.Abs(dot12) > tolerance) return false;

            double dot13 = R11 * R31 + R12 * R32 + R13 * R33;
            if (Math.Abs(dot13) > tolerance) return false;

            double dot23 = R21 * R31 + R22 * R32 + R23 * R33;
            if (Math.Abs(dot23) > tolerance) return false;

            return true;
        }

        /// <summary>
        /// Ортонормирует матрицу (метод Грама-Шмидта)
        /// </summary>
        private void Orthonormalize()
        {
            // Создаем копию матрицы для работы
            double[,] m = matrix;

            // Первый столбец (ось X) нормализуем
            double norm = Math.Sqrt(m[0, 0] * m[0, 0] + m[1, 0] * m[1, 0] + m[2, 0] * m[2, 0]);
            m[0, 0] /= norm; m[1, 0] /= norm; m[2, 0] /= norm;

            // Второй столбец (ось Y) делаем ортогональным к первому
            double dot = m[0, 0] * m[0, 1] + m[1, 0] * m[1, 1] + m[2, 0] * m[2, 1];
            m[0, 1] -= dot * m[0, 0];
            m[1, 1] -= dot * m[1, 0];
            m[2, 1] -= dot * m[2, 0];

            // Нормализуем второй столбец
            norm = Math.Sqrt(m[0, 1] * m[0, 1] + m[1, 1] * m[1, 1] + m[2, 1] * m[2, 1]);
            m[0, 1] /= norm; m[1, 1] /= norm; m[2, 1] /= norm;

            // Третий столбец (ось Z) как векторное произведение первых двух
            m[0, 2] = m[1, 0] * m[2, 1] - m[2, 0] * m[1, 1];
            m[1, 2] = m[2, 0] * m[0, 1] - m[0, 0] * m[2, 1];
            m[2, 2] = m[0, 0] * m[1, 1] - m[1, 0] * m[0, 1];
        }

        /// <summary>
        /// Получить элемент матрицы по индексам
        /// </summary>
        public double this[int row, int col]
        {
            get
            {
                if (row < 0 || row > 2 || col < 0 || col > 2)
                    throw new IndexOutOfRangeException("Индексы должны быть от 0 до 2");
                return matrix[row, col];
            }
        }

        /// <summary>
        /// Транспонирование матрицы (обратное вращение)
        /// </summary>
        public DirectionCosineMatrix Transpose()
        {
            return new DirectionCosineMatrix(
                R11, R21, R31,
                R12, R22, R32,
                R13, R23, R33);
        }

        /// <summary>
        /// Умножение матриц (композиция вращений)
        /// </summary>
        public static DirectionCosineMatrix operator *(DirectionCosineMatrix a, DirectionCosineMatrix b)
        {
            double r11 = a.R11 * b.R11 + a.R12 * b.R21 + a.R13 * b.R31;
            double r12 = a.R11 * b.R12 + a.R12 * b.R22 + a.R13 * b.R32;
            double r13 = a.R11 * b.R13 + a.R12 * b.R23 + a.R13 * b.R33;

            double r21 = a.R21 * b.R11 + a.R22 * b.R21 + a.R23 * b.R31;
            double r22 = a.R21 * b.R12 + a.R22 * b.R22 + a.R23 * b.R32;
            double r23 = a.R21 * b.R13 + a.R22 * b.R23 + a.R23 * b.R33;

            double r31 = a.R31 * b.R11 + a.R32 * b.R21 + a.R33 * b.R31;
            double r32 = a.R31 * b.R12 + a.R32 * b.R22 + a.R33 * b.R32;
            double r33 = a.R31 * b.R13 + a.R32 * b.R23 + a.R33 * b.R33;

            return new DirectionCosineMatrix(r11, r12, r13, r21, r22, r23, r31, r32, r33);
        }

        /// <summary>
        /// Умножение матрицы на вектор (преобразование вектора из связанной СК в опорную)
        /// </summary>
        public static Vector3 operator *(DirectionCosineMatrix matrix, Vector3 vector)
        {
            double x = matrix.R11 * vector.X + matrix.R12 * vector.Y + matrix.R13 * vector.Z;
            double y = matrix.R21 * vector.X + matrix.R22 * vector.Y + matrix.R23 * vector.Z;
            double z = matrix.R31 * vector.X + matrix.R32 * vector.Y + matrix.R33 * vector.Z;

            return new Vector3((float)x, (float)y, (float)z);
        }        

        ///// <summary>
        ///// Создает DCM из углов Эйлера ZYX (yaw-pitch-roll)
        ///// </summary>
        //public static DirectionCosineMatrix FromEulerZYX(double yaw, double pitch, double roll)
        //{
        //    double cy = Math.Cos(yaw);
        //    double sy = Math.Sin(yaw);
        //    double cp = Math.Cos(pitch);
        //    double sp = Math.Sin(pitch);
        //    double cr = Math.Cos(roll);
        //    double sr = Math.Sin(roll);

        //    double r11 = cy * cp;
        //    double r12 = cy * sp * sr - sy * cr;
        //    double r13 = cy * sp * cr + sy * sr;

        //    double r21 = sy * cp;
        //    double r22 = sy * sp * sr + cy * cr;
        //    double r23 = sy * sp * cr - cy * sr;

        //    double r31 = -sp;
        //    double r32 = cp * sr;
        //    double r33 = cp * cr;

        //    return new DirectionCosineMatrix(r11, r12, r13, r21, r22, r23, r31, r32, r33);
        //}

        ///// <summary>
        ///// Создает DCM из углов Эйлера ZXZ
        ///// </summary>
        //public static DirectionCosineMatrix FromEulerZXZ(double psi, double theta, double phi)
        //{
        //    double c1 = Math.Cos(psi);
        //    double s1 = Math.Sin(psi);
        //    double c2 = Math.Cos(theta);
        //    double s2 = Math.Sin(theta);
        //    double c3 = Math.Cos(phi);
        //    double s3 = Math.Sin(phi);

        //    double r11 = c1 * c3 - s1 * c2 * s3;
        //    double r12 = -c1 * s3 - s1 * c2 * c3;
        //    double r13 = s1 * s2;

        //    double r21 = s1 * c3 + c1 * c2 * s3;
        //    double r22 = -s1 * s3 + c1 * c2 * c3;
        //    double r23 = -c1 * s2;

        //    double r31 = s2 * s3;
        //    double r32 = s2 * c3;
        //    double r33 = c2;

        //    return new DirectionCosineMatrix(r11, r12, r13, r21, r22, r23, r31, r32, r33);
        //}

        ///// <summary>
        ///// Преобразует DCM в кватернион
        ///// </summary>
        //public Quaternion ToQuaternion()
        //{
        //    double trace = R11 + R22 + R33;

        //    if (trace > 0)
        //    {
        //        double s = 0.5 / Math.Sqrt(trace + 1.0);
        //        double w = 0.25 / s;
        //        double x = (R32 - R23) * s;
        //        double y = (R13 - R31) * s;
        //        double z = (R21 - R12) * s;
        //        return new Quaternion((float)x, (float)y, (float)z, (float)w);
        //    }
        //    else if (R11 > R22 && R11 > R33)
        //    {
        //        double s = 2.0 * Math.Sqrt(1.0 + R11 - R22 - R33);
        //        double w = (R32 - R23) / s;
        //        double x = 0.25 * s;
        //        double y = (R12 + R21) / s;
        //        double z = (R13 + R31) / s;
        //        return new Quaternion((float)x, (float)y, (float)z, (float)w);
        //    }
        //    else if (R22 > R33)
        //    {
        //        double s = 2.0 * Math.Sqrt(1.0 + R22 - R11 - R33);
        //        double w = (R13 - R31) / s;
        //        double x = (R12 + R21) / s;
        //        double y = 0.25 * s;
        //        double z = (R23 + R32) / s;
        //        return new Quaternion((float)x, (float)y, (float)z, (float)w);
        //    }
        //    else
        //    {
        //        double s = 2.0 * Math.Sqrt(1.0 + R33 - R11 - R22);
        //        double w = (R21 - R12) / s;
        //        double x = (R13 + R31) / s;
        //        double y = (R23 + R32) / s;
        //        double z = 0.25 * s;
        //        return new Quaternion((float)x, (float)y, (float)z, (float)w);
        //    }
        //}

        /// <summary>
        /// Преобразует DCM в углы Эйлера ZYX (yaw-pitch-roll)
        /// </summary>
        public (double yaw, double pitch, double roll) ToEulerZYX()
        {
            double pitch = Math.Asin(-R31);
            
            double yaw = Math.Atan2(R21, R11);
            double roll = Math.Atan2(R32, R33);

            // Проверка на gimbal lock
            if (Math.Abs(Math.Abs(R31) - 1.0) < 1e-9)
            {
                yaw = Math.Atan2(R12, R22);
                roll = 0.0;
                return (yaw, pitch, roll);
            }

            yaw = Math.Atan2(R21, R11);
            roll = Math.Atan2(R32, R33);

            return (yaw, pitch, roll);
        }

        /// <summary>
        /// Преобразует DCM в углы Эйлера ZXZ
        /// </summary>
        public (double psi, double theta, double phi) ToEulerZXZ()
        {
            double theta = Math.Acos(Math.Clamp(R33, -1.0, 1.0));

            double sinTheta = Math.Sin(theta);
            double psi, phi;

            if (Math.Abs(sinTheta) > 1e-12)
            {
                psi = Math.Atan2(R31, -R32);
                phi = Math.Atan2(R13, R23);
            }
            else
            {
                // Сингулярность
                psi = 0.0;
                phi = Math.Atan2(-R12, R11);
            }

            return (psi, theta, phi);
        }

        /// <summary>
        /// Получить матрицу в виде двумерного массива
        /// </summary>
        public double[,] ToArray()
        {
            return (double[,])matrix.Clone();
        }

        /// <summary>
        /// Преобразует вектор из связанной СК в опорную
        /// </summary>
        public Vector3 TransformVector(Vector3 vectorInBodyFrame)
        {
            return this * vectorInBodyFrame;
        }

        /// <summary>
        /// Преобразует вектор из опорной СК в связанную
        /// </summary>
        public Vector3 InverseTransformVector(Vector3 vectorInWorldFrame)
        {
            DirectionCosineMatrix transposed = Transpose();
            return transposed * vectorInWorldFrame;
        }

        /// <summary>
        /// Получает ось X связанной СК в опорной СК
        /// </summary>
        public Vector3 GetXAxis() => new Vector3((float)R11, (float)R21, (float)R31);

        /// <summary>
        /// Получает ось Y связанной СК в опорной СК
        /// </summary>
        public Vector3 GetYAxis() => new Vector3((float)R12, (float)R22, (float)R32);

        /// <summary>
        /// Получает ось Z связанной СК в опорной СК
        /// </summary>
        public Vector3 GetZAxis() => new Vector3((float)R13, (float)R23, (float)R33);

        #region Реализация интерфейсов и переопределение методов

        public bool Equals(DirectionCosineMatrix other)
        {
            const double tolerance = 1e-9;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (Math.Abs(matrix[i, j] - other.matrix[i, j]) > tolerance)
                        return false;
            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is DirectionCosineMatrix other && Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    hash = hash * 31 + matrix[i, j].GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return string.Format(
                "DCM:\n[{0:F6}, {1:F6}, {2:F6}]\n[{3:F6}, {4:F6}, {5:F6}]\n[{6:F6}, {7:F6}, {8:F6}]",
                R11, R12, R13, R21, R22, R23, R31, R32, R33);
        }

        public static bool operator ==(DirectionCosineMatrix left, DirectionCosineMatrix right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DirectionCosineMatrix left, DirectionCosineMatrix right)
        {
            return !left.Equals(right);
        }

        #endregion
    }

    //// Пример использования
    //public class DCMExample
    //{
    //    public static void RunExample()
    //    {
    //        Console.WriteLine("=== Пример работы с матрицей направляющих косинусов ===\n");

    //        // 1. Создание DCM из углов Эйлера
    //        Console.WriteLine("1. DCM из углов Эйлера ZYX (yaw=30°, pitch=20°, roll=10°):");
    //        double yaw = 30 * Math.PI / 180;
    //        double pitch = 20 * Math.PI / 180;
    //        double roll = 10 * Math.PI / 180;

    //        DirectionCosineMatrix dcm1 = DirectionCosineMatrix.FromEulerZYX(yaw, pitch, roll);
    //        Console.WriteLine(dcm1);

    //        // 2. Преобразование в кватернион и обратно
    //        Console.WriteLine("\n2. Преобразование DCM → кватернион → DCM:");
    //        Quaternion q = dcm1.ToQuaternion();
    //        DirectionCosineMatrix dcm2 = DirectionCosineMatrix.FromQuaternion(q);
    //        Console.WriteLine("Матрицы равны: " + (dcm1 == dcm2));

    //        // 3. Умножение матриц (композиция вращений)
    //        Console.WriteLine("\n3. Композиция вращений:");
    //        DirectionCosineMatrix rotX = DirectionCosineMatrix.FromEulerZYX(Math.PI / 6, 0, 0); // 30° по рысканию
    //        DirectionCosineMatrix rotY = DirectionCosineMatrix.FromEulerZYX(0, Math.PI / 6, 0); // 30° по тангажу
    //        DirectionCosineMatrix combined = rotX * rotY;
    //        Console.WriteLine("Результат умножения:");
    //        Console.WriteLine(combined);

    //        // 4. Преобразование вектора
    //        Console.WriteLine("\n4. Преобразование вектора:");
    //        Vector3 vectorInBody = new Vector3(1, 0, 0); // Вектор вперед в связанной СК
    //        Vector3 vectorInWorld = dcm1.TransformVector(vectorInBody);
    //        Console.WriteLine($"Вектор в связанной СК: {vectorInBody}");
    //        Console.WriteLine($"Вектор в опорной СК: {vectorInWorld}");

    //        // 5. Получение осей связанной СК
    //        Console.WriteLine("\n5. Оси связанной системы координат:");
    //        Console.WriteLine($"Ось X: {dcm1.GetXAxis()}");
    //        Console.WriteLine($"Ось Y: {dcm1.GetYAxis()}");
    //        Console.WriteLine($"Ось Z: {dcm1.GetZAxis()}");

    //        // 6. DCM из углов Эйлера ZXZ
    //        Console.WriteLine("\n6. DCM из углов Эйлера ZXZ:");
    //        DirectionCosineMatrix dcmZxz = DirectionCosineMatrix.FromEulerZXZ(
    //            Math.PI / 4, Math.PI / 3, Math.PI / 6); // 45°, 60°, 30°
    //        Console.WriteLine(dcmZxz);

    //        var anglesZxz = dcmZxz.ToEulerZXZ();
    //        Console.WriteLine($"Обратное преобразование: ψ={anglesZxz.psi * 180 / Math.PI:F2}°, " +
    //                         $"θ={anglesZxz.theta * 180 / Math.PI:F2}°, " +
    //                         $"φ={anglesZxz.phi * 180 / Math.PI:F2}°");
    //    }
    //}
}
