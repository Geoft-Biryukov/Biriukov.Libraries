namespace Biriukov.Orientation.Core
{
    /// <summary>
    /// Статические методы расширения для преобразования кватерниона ориентации в углы ориентации и обратно   
    /// Ю.Н. Челноков Кватернионные и бикватернионные модели и методы механики твердого тела и их приложения. Геометрия и кинематика движения
    /// </summary>
    public static class OrientationConverters
    {
        #region ToQuaternion
        /// <summary>
        /// Преобразует углы Эйлера в кватернион
        /// </summary>
        /// <param name="angles">Углы Эйлера</param>
        /// <returns>Кватернион ориентации</returns>
        public static Quaternion ToQuaternion(this EulerAngles angles)
        {
            var psi = angles.Psi;
            var theta = angles.Theta;
            var phi = angles.Phi;

            return angles.AnglesType switch
            {
                EulerAnglesTypes.Classic => QuaternionFromClassicAngles(psi, theta, phi),
                EulerAnglesTypes.Krylov => QuaternionFromKrylovAngles(psi, theta, phi),
                _ => throw new NotSupportedException($"Неизвестный тип углов Эйлера: {angles.AnglesType}"),
            };
        }

        /// <summary>
        /// Преобразует DCM в кватернион
        /// </summary>
        public static Quaternion ToQuaternion(this DirectionCosineMatrix dcm)
        {
            double trace = dcm.R11 + dcm.R22 + dcm.R33;

            if (trace > 0)
            {
                double s = 0.5 / Math.Sqrt(trace + 1.0);
                double w = 0.25 / s;
                double x = (dcm.R32 - dcm.R23) * s;
                double y = (dcm.R13 - dcm.R31) * s;
                double z = (dcm.R21 - dcm.R12) * s;
                return new Quaternion((float)x, (float)y, (float)z, (float)w);
            }
            else if (dcm.R11 > dcm.R22 && dcm.R11 > dcm.R33)
            {
                double s = 2.0 * Math.Sqrt(1.0 + dcm.R11 - dcm.R22 - dcm.R33);
                double w = (dcm.R32 - dcm.R23) / s;
                double x = 0.25 * s;
                double y = (dcm.R12 + dcm.R21) / s;
                double z = (dcm.R13 + dcm.R31) / s;
                return new Quaternion((float)x, (float)y, (float)z, (float)w);
            }
            else if (dcm.R22 > dcm.R33)
            {
                double s = 2.0 * Math.Sqrt(1.0 + dcm.R22 - dcm.R11 - dcm.R33);
                double w = (dcm.R13 - dcm.R31) / s;
                double x = (dcm.R12 + dcm.R21) / s;
                double y = 0.25 * s;
                double z = (dcm.R23 + dcm.R32) / s;
                return new Quaternion((float)x, (float)y, (float)z, (float)w);
            }
            else
            {
                double s = 2.0 * Math.Sqrt(1.0 + dcm.R33 - dcm.R11 - dcm.R22);
                double w = (dcm.R21 - dcm.R12) / s;
                double x = (dcm.R13 + dcm.R31) / s;
                double y = (dcm.R23 + dcm.R32) / s;
                double z = 0.25 * s;
                return new Quaternion((float)x, (float)y, (float)z, (float)w);
            }
        }

        /// <summary>
        /// Стр. 151
        /// </summary>        
        private static Quaternion QuaternionFromClassicAngles(Angle psi, Angle theta, Angle phi)
        {
            var halfPsi = 0.5 * psi;
            var halfTheta = 0.5 * theta;
            var halfPhi = 0.5 * phi;

            double cosHalfTheta = MathAngle.Cos(halfTheta);
            double sinHalfTheta = MathAngle.Sin(halfTheta);
            
            double cosHalfPsi = MathAngle.Cos(halfPsi);
            double sinHalfPsi = MathAngle.Sin(halfPsi);
           
            double cosHalfPhi = MathAngle.Cos(halfPhi);
            double sinHalfPhi = MathAngle.Sin(halfPhi);

            // Умножение кватернионов: q = q_z(psi) * q_x(theta) * q_z(phi)
            double w = cosHalfPsi * cosHalfTheta * cosHalfPhi - sinHalfPsi * cosHalfTheta * sinHalfPhi;
            double x = cosHalfPsi * sinHalfTheta * cosHalfPhi + sinHalfPsi * sinHalfTheta * sinHalfPhi;
            double y = sinHalfPsi * sinHalfTheta * cosHalfPhi - cosHalfPsi * sinHalfTheta * sinHalfPhi;
            double z = cosHalfPsi * cosHalfTheta * sinHalfPhi + sinHalfPsi * cosHalfTheta * cosHalfPhi;

            return new Quaternion(w, x, y, z);
        }

       /// <summary>
       /// Стр. 162-163
       /// </summary>       
        private static Quaternion QuaternionFromKrylovAngles(Angle psi, Angle theta, Angle phi)
        {
            var halfPsi = 0.5 * psi;
            var halfTheta = 0.5 * theta;
            var halfPhi = 0.5 * phi ;      
            
            double cosHalfPsi = MathAngle.Cos(halfPsi);
            double sinHalfPsi = MathAngle.Sin(halfPsi);

            double cosHalfTheta = MathAngle.Cos(halfTheta);
            double sinHalfTheta = MathAngle.Sin(halfTheta);

            double cosHalfPhi = MathAngle.Cos(halfPhi);
            double sinHalfPhi = MathAngle.Sin(halfPhi);

            // Умножение кватернионов: q = q_y(yaw) * q_z(pitch) * q_x(roll)
            double w = cosHalfPsi * cosHalfTheta * cosHalfPhi - sinHalfPsi * sinHalfTheta * sinHalfPhi;
            double x = sinHalfPsi * sinHalfTheta * cosHalfPhi + cosHalfPsi * cosHalfTheta * sinHalfPhi;
            double y = sinHalfPsi * cosHalfTheta * cosHalfPhi + cosHalfPsi * sinHalfTheta * sinHalfPhi;
            double z = cosHalfPsi * sinHalfTheta * cosHalfPhi - sinHalfPsi * cosHalfTheta * sinHalfPhi;

            return new Quaternion(w, x, y, z);
        }
        #endregion

        #region ToEulerAngles

        /// <summary>
        /// Преобразует кватернион в набор углов заданного типа
        /// </summary>
        /// <param name="q">Кватернион ориентации</param>
        /// <param name="anglesType">Тип набора углов</param>
        /// <returns>Углы ориентации заданного типа</returns>
        /// <exception cref="NotSupportedException"></exception>
        public static EulerAngles ToEulerAngles(this Quaternion q, EulerAnglesTypes anglesType)
        => anglesType switch
        { 
            EulerAnglesTypes.Classic => q.ToClassicEulerAngles(),
            EulerAnglesTypes.Krylov => q.ToKrylovEulerAngles(),
            _ => throw new NotSupportedException($"Неизвестный тип углов Эйлера: {anglesType}")
        };

        /// <summary>        
        /// Преобразует кватернион в классические углы Эйлера (ZXZ)       
        /// </summary>
        /// <param name="q">>Кватернион ориентации</param>
        /// <returns>Классические углы Эйлера</returns>
        /// <exception cref="NotImplementedException">
        /// Пока не удалось реализовать этот метод.
        /// Проблема при Theta = 0.
        /// </exception>
        public static EulerAngles ToClassicEulerAngles(this Quaternion q)
        {
            // Модульный тест не работает. Перепробовал разные формулы. Нужно разбираться...
            throw new NotImplementedException();

            //double psi = Math.Atan2((q[1] * q[3] + q[0] * q[2]), (q[2] * q[3] - q[0] * q[1]));
            //double theta = Math.Acos(q[0] * q[0] - q[1] * q[1] - q[2] * q[2] + q[3] * q[3]);
            //double phi = Math.Atan2((q[1] * q[3] - q[0] * q[2]), (q[2] * q[3] + q[0] * q[1]));

            //return EulerAngles.CreateClassic(Angle.FromRad(psi), Angle.FromRad(theta), Angle.FromRad(phi));

        }

        /// <summary>
        /// Преобразует кватернион в углы Эйлера-Крылова (самолетные углы) (YZX)   
        /// Стр. 379
        /// </summary>
        /// <param name="q">Кватернион ориентации</param>
        /// <returns>Углы Эйлера-Крылова</returns>
        public static EulerAngles ToKrylovEulerAngles(this Quaternion q)
        {
            double psi = Math.Atan2(q[0] * q[2] - q[1] * q[3], q[0] * q[0] + q[1] * q[1] - 0.5);
            double theta = Math.Asin(2.0 * (q[1] * q[2] + q[0] * q[3]));
            double phi = Math.Atan2(q[0] * q[1] - q[2] * q[3], q[0] * q[0] + q[2] * q[2] - 0.5);

            return EulerAngles.CreateKrylov(Angle.FromRad(psi), Angle.FromRad(theta), Angle.FromRad(phi));

        }

        #endregion

        #region ToDirectionCosineMatrix
        /// <summary>
        /// Преобразует кватернион в матрицу направляющих косинусов
        /// </summary>
        public static DirectionCosineMatrix ToDirectionCosineMatrix(this Quaternion q)
        {
            double w = q.W, x = q.X, y = q.Y, z = q.Z;

            double w2 = w * w;
            double x2 = x * x;
            double y2 = y * y;
            double z2 = z * z;

            double r11 = w2 + x2 - y2 - z2;
            double r12 = 2 * (x * y + w * z);
            double r13 = 2 * (x * z - w * y);

            double r21 = 2 * (x * y - w * z);
            double r22 = w2 - x2 + y2 - z2;
            double r23 = 2 * (y * z + w * x);

            double r31 = 2 * (x * z + w * y);
            double r32 = 2 * (y * z - w * x);
            double r33 = w2 - x2 - y2 + z2;

            return new DirectionCosineMatrix(r11, r12, r13, r21, r22, r23, r31, r32, r33);
        }

        public static DirectionCosineMatrix ToDirectionCosineMatrix(this EulerAngles a)
        {
            return a.AnglesType switch
            {
                EulerAnglesTypes.Classic => DCMFromClassicAngles(a.Psi, a.Theta, a.Phi),
                EulerAnglesTypes.Krylov => DCMFromKrylovAngles(a.Psi, a.Theta, a.Phi),
                _ => throw new NotSupportedException()
            };

        }

        private static DirectionCosineMatrix DCMFromClassicAngles(Angle psi, Angle theta, Angle phi)
        {
            double cosPsi = MathAngle.Cos(psi);
            double sinPsi = MathAngle.Sin(psi);

            double cosTheta = MathAngle.Cos(theta);
            double sinTheta = MathAngle.Sin(theta);

            double cosPhi = MathAngle.Cos(phi);
            double sinPhi = MathAngle.Sin(phi);

            double r11 = cosPhi * cosPsi - sinPhi * cosTheta * sinPsi;
            double r12 = cosPhi * sinPsi + sinPhi * cosTheta * cosPsi;
            double r13 = sinPhi * sinTheta;

            double r21 = -sinPhi * cosPsi - cosPhi * cosTheta * sinPsi;
            double r22 = -sinPhi * sinPsi + cosPhi * cosTheta * cosPsi;
            double r23 = cosPhi * sinTheta;

            double r31 = sinTheta * sinPsi;
            double r32 =-sinTheta * cosPsi;
            double r33 = cosTheta;

            return new DirectionCosineMatrix(
                r11, r12, r13,
                r21, r22, r23,
                r31, r32, r33);
        }

        private static DirectionCosineMatrix DCMFromKrylovAngles(Angle psi, Angle theta, Angle phi)
        {            
            double cosPsi = MathAngle.Cos(psi);
            double sinPsi = MathAngle.Sin(psi);

            double cosTheta = MathAngle.Cos(theta);
            double sinTheta = MathAngle.Sin(theta);

            double cosPhi = MathAngle.Cos(phi);
            double sinPhi = MathAngle.Sin(phi);

            double r11 = cosTheta * cosPsi;
            double r12 = sinTheta;
            double r13 = -cosTheta * sinPsi;

            double r21 = sinPhi * sinPsi - cosPhi * sinTheta * cosPsi;
            double r22 = cosPhi * cosTheta;
            double r23 = sinPhi * cosPsi + cosPhi * sinTheta * sinPsi;

            double r31 = cosPhi * sinPsi + sinPhi * sinTheta * cosPsi;
            double r32 = -sinPhi * cosTheta;
            double r33 = cosPhi * cosTheta - sinPhi * sinTheta * sinPsi;

            return new DirectionCosineMatrix(
                r11, r12, r13,
                r21, r22, r23,
                r31, r32, r33);
        }
        #endregion
    }
}
