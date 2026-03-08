namespace Biriukov.Orientation.Core
{
    /// <summary>
    /// Структура для хранения угла
    /// </summary>
    public struct Angle : IComparable, IComparable<Angle>, IEquatable<Angle>
    {
        #region Consts
        private const double degToRad = Math.PI / 180.0;
        private const double radToDeg = 180.0 / Math.PI;

        private const double tolerance = 1e-10;

        public static readonly Angle Zero = FromRad(0);
        public static readonly Angle Perigon = FromRad(2 * Math.PI);

        public static readonly Angle Deg0 = Zero;
        public static readonly Angle Deg90 = FromRad(0.5 * Math.PI);
        public static readonly Angle Deg180 = FromRad(Math.PI);
        public static readonly Angle Deg270 = FromRad(1.5 * Math.PI);
        public static readonly Angle Deg360 = FromRad(2 * Math.PI);

        public static readonly Angle NaN = FromRad(double.NaN);
        public static readonly Angle PositiveInfinity = FromRad(double.PositiveInfinity);
        public static readonly Angle NegativeInfinity = FromRad(double.NegativeInfinity);
        #endregion

        #region Properties
        /// <summary>
        /// Угол в радианах
        /// </summary>
        public double Rad { get; }

        /// <summary>
        /// Угол в градусах
        /// </summary>
        public double Deg => Rad * radToDeg;
        #endregion

        #region Creators
        public static Angle FromRad(double radians) => new(radians);

        public static Angle FromDeg(double degrees) => new(degrees * degToRad);
        #endregion

        private Angle(double radians)
        {
            Rad = radians;            
        }

        #region IComparable<Angle>

        /// <summary>
        /// Производит сравнение углов по значению
        /// </summary>
        /// <param name="other">Сравниваемый угол</param>
        /// <returns>Меньше нуля - other больше, ноль - равны, больше нуля - other меньше</returns>
        public int CompareTo(Angle other) => Rad.CompareTo(other.Rad);

        #endregion

        #region IComparable

        /// <summary>
        /// Производит сравнение углов по значению
        /// </summary>
        /// <param name="obj">Сравниваемый угол</param>
        /// <returns>Меньше нуля - obj больше, ноль - равны, больше нуля - obj меньше</returns>
        public int CompareTo(object obj)
        {
            if (obj is Angle other)
                return CompareTo(other);

            throw new ArgumentException($"Object must be of type {nameof(Angle)}", nameof(obj));
        }

        #endregion

        #region Object override
        public override readonly string ToString()
            => string.Format(System.Globalization.CultureInfo.CurrentCulture, "{0} Degrees", Deg);

        /// <summary>
        /// Сравниевает на равенство
        /// </summary>
        /// <param name="other">Сравниваемый угол</param>
        /// <returns>true - равны</returns>
        public readonly bool Equals(Angle other)
        {           
            return Rad.Equals(other.Rad);
        }


        /// <summary>
        /// Сравниевает на равенство
        /// </summary>
        /// <param name="obj">Сравниваемый угол</param>
        /// <returns>true - равны</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            return obj is Angle other && Equals(other);
        }

        public override int GetHashCode() => Rad.GetHashCode();
        #endregion

        /// <summary>
        /// Если includePerigon == false, сворачивает в [0, 2pi)
        /// Если includePerigon == true, сворачивает в [0, 2pi]
        /// </summary>
        public static Angle FoldPerigon(Angle angle, bool includePerigon = false)
        {
            if (includePerigon && Math.Abs(angle.Rad - Perigon.Rad) < tolerance)
                return angle;

            var folded = angle.Rad % Perigon.Rad;
            if (folded < 0)
                folded += Perigon.Rad;

            return new Angle(folded);
        }

        public static bool IsZero(Angle angle)
            => angle.Rad == 0;


        public static bool IsNaN(Angle angle)
            => double.IsNaN(angle.Rad);


        public static bool IsInfinity(Angle angle)
            => double.IsInfinity(angle.Rad);

        public static double FromDegToRad(double degrees)
            => degrees * degToRad;

        public static double FromRadToDeg(double radians)
            => radians * radToDeg;

        #region Overloaded operators
        public static Angle operator +(Angle p1, Angle p2) => FromRad(p1.Rad + p2.Rad);


        public static Angle operator -(Angle p1, Angle p2) => FromRad(p1.Rad - p2.Rad);


        public static Angle operator +(Angle a) => a;


        public static Angle operator -(Angle a) => FromRad(-a.Rad);


        public static Angle operator *(double scale, Angle a) => FromRad(scale * a.Rad);


        public static Angle operator *(Angle a, double scale) => FromRad(scale * a.Rad);


        public static Angle operator /(Angle a, double divisor) => FromRad(a.Rad / divisor);


        public static double operator /(Angle a, Angle b) => a.Rad / b.Rad;


        public static Angle operator %(Angle a1, Angle a2) => FromRad(a1.Rad % a2.Rad);


        public static bool operator ==(Angle left, Angle right) => left.Rad.Equals(right.Rad);

        public static bool operator !=(Angle left, Angle right) => !left.Rad.Equals(right.Rad);


        public static bool operator <(Angle left, Angle right) => left.Rad < right.Rad;
        public static bool operator <=(Angle left, Angle right) => left.Rad <= right.Rad;


        public static bool operator >(Angle left, Angle right) => left.Rad > right.Rad;
        public static bool operator >=(Angle left, Angle right) => left.Rad >= right.Rad;
        #endregion
    }
}
