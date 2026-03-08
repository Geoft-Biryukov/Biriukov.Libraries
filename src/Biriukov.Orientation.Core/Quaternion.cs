namespace Biriukov.Orientation.Core
{    
    public readonly struct Quaternion : IEquatable<Quaternion>, IFormattable
    {
        private const double tolerance = 1e-10;
        #region ctors and conversion operators        
        
        public Quaternion(double w, double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public static implicit operator Quaternion(double w) => new(w, 0.0, 0.0, 0.0);

        public static explicit operator double(Quaternion q) => q.W;
        
        #endregion

        #region properties
        public double W { get; }        
        public double X { get; }        
        public double Y { get; }        
        public double Z { get; }

        public double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return W;
                    case 1: return X;
                    case 2: return Y;
                    case 3: return Z;
                    default: throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
        #endregion

        #region operators
        public static Quaternion operator +(Quaternion q) => new(q.W, q.X, q.Y, q.Z);

        public static Quaternion Plus(Quaternion q) => +q;
        
        public static Quaternion operator -(Quaternion q) => new(-q.W, -q.X, -q.Y, -q.Z);

        public static Quaternion Negate(Quaternion q) => -q;        

        public static Quaternion operator +(Quaternion q1, Quaternion q2)
            => new(q1.W + q2.W, q1.X + q2.X, q1.Y + q2.Y, q1.Z + q2.Z);
        
        public static Quaternion Add(Quaternion q1, Quaternion q2) => q1 + q2;
        
        public static Quaternion operator -(Quaternion q1, Quaternion q2)
            => new(q1.W - q2.W, q1.X - q2.X, q1.Y - q2.Y, q1.Z - q2.Z);
        
        public static Quaternion Subtract(Quaternion q1, Quaternion q2) => q1 - q2;
        
        public static Quaternion operator *(Quaternion q1, Quaternion q2)
        {
            double w = q1.W * q2.W - q1.X * q2.X - q1.Y * q2.Y - q1.Z * q2.Z;
            double x = q1.W * q2.X + q1.X * q2.W + q1.Y * q2.Z - q1.Z * q2.Y;
            double y = q1.W * q2.Y + q1.Y * q2.W + q1.Z * q2.X - q1.X * q2.Z;
            double z = q1.W * q2.Z + q1.Z * q2.W + q1.X * q2.Y - q1.Y * q2.X;

            return new Quaternion(w, x, y, z);
        }
        public static Quaternion Multiply(Quaternion q1, Quaternion q2) => q1 * q2;        

        public static Quaternion operator /(Quaternion q, double d)
        {
            double m = 1.0 / d;
            return new Quaternion(q.W * m, q.X * m, q.Y * m, q.Z * m);
        }
        public static Quaternion Divide(Quaternion q, double d) => q / d;        

        public static Quaternion operator *(double d, Quaternion q)
            => new(d * q.W, d * q.X, d * q.Y, d * q.Z);
        
        public static Quaternion Multiply(double d, Quaternion q) => d * q;
        
        public static Quaternion operator *(Quaternion q, double d) 
            => new Quaternion(d * q.W, d * q.X, d * q.Y, d * q.Z);
        
        public static Quaternion Multiply(Quaternion q, double d) => q * d;
        
        public static bool operator ==(Quaternion q1, Quaternion q2) => q1.Equals(q2);

        public static bool Equals(Quaternion q1, Quaternion q2) => q1 == q2;
        
        public static bool operator !=(Quaternion q1, Quaternion q2) => !q1.Equals(q2);

        #endregion

        #region quaternionic operations
        public double ScalarPart => W;              
        public Quaternion VectorPart => new(0, X, Y, Z);               
        
        public Quaternion Conjugate() => new(W, -X, -Y, -Z);

        public Quaternion Inverse() => Conjugate() / Norm();
              
        public double Norm() => W * W + X * X + Y * Y + Z * Z;
        
        public static Quaternion Conjugate(Quaternion q) => q.Conjugate();
        public static Quaternion Inverse(Quaternion q) => q.Inverse();                
        public static double Norm(Quaternion q) => q.Norm();

        public static Quaternion I1 => new(0, 1, 0, 0);
        
        public static Quaternion I2 => new(0, 0, 1, 0);
        
        public static Quaternion I3 => new(0, 0, 0, 1);

        public bool IsNormalized => Math.Abs(Norm() - 1.0) < tolerance;

        #endregion        

        #region interfaces implementation

        public bool Equals(Quaternion other, double tolerance = 1e-10)
            => Math.Abs(W - other.W) < tolerance &&
             Math.Abs(X - other.X) < tolerance &&
             Math.Abs(Y - other.Y) < tolerance &&
             Math.Abs(Z - other.Z) < tolerance;

        public bool Equals(Quaternion other) 
            => W == other.W && X == other.X && Y == other.Y && Z == other.Z;

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
                format = @"({0}, {1}, {2}, {3})";
            return string.Format(format, W.ToString(formatProvider), X.ToString(formatProvider), Y.ToString(formatProvider), Z.ToString(formatProvider));
        }
        #endregion

        public override bool Equals(object obj) => obj is Quaternion other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(W, X, Y, Z);
        public override string ToString() => $"({W:G6}, {X:G6}, {Y:G6}, {Z:G6})";

    }
}
