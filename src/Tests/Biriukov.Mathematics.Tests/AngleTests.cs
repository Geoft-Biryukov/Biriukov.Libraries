

using Biriukov.Mathematics.Geometry;

namespace Biriukov.Mathematics.Tests
{
    public class AngleTests
    {
        private const double tolerance = 1e-10;

        #region Creates
        [Test]
        public void CanCreateFromDegrees()
        {
            double angleDeg = 180.0;
            double angleRad = angleDeg * Math.PI / 180.0;

            var alpha = Angle.FromDeg(angleDeg);
            Assert.Multiple(() =>
            {
                Assert.That(alpha.Deg, Is.EqualTo(angleDeg));
                Assert.That(alpha.Rad, Is.EqualTo(angleRad).Within(tolerance));
            });
        }

        [Test]
        public void CanCreateFromRadian()
        {
            double angleDeg = 180.0;
            double angleRad = angleDeg * Math.PI / 180.0;

            var alpha = Angle.FromRad(angleRad);
            Assert.Multiple(() =>
            {
                Assert.That(alpha.Deg, Is.EqualTo(angleDeg));
                Assert.That(alpha.Rad, Is.EqualTo(angleRad).Within(tolerance));
            });
        }
        #endregion

       
        #region FoldPerigon
        [Test]
        public void FoldPerigonNormalizesCorrectly()
        {
            var angle = Angle.FromDeg(450);
            var normalized = Angle.FoldPerigon(angle);
            Assert.That(normalized.Deg, Is.EqualTo(90).Within(tolerance));
        }

        [Test]
        public void FoldPerigonWithIncludePerigonTrueIncludes360()
        {
            var angle = Angle.FromDeg(360);
            var result = Angle.FoldPerigon(angle, includePerigon: true);
            Assert.That(result.Deg, Is.EqualTo(360).Within(tolerance));
        }

        [Test]
        public void FoldPerigonWithNegativeAngleNormalizesCorrectly()
        {
            var angle = Angle.FromDeg(-90);
            var result = Angle.FoldPerigon(angle);
            Assert.That(result.Deg, Is.EqualTo(270).Within(tolerance));
        }
        #endregion

        #region Operators
        [Test]
        public void AdditionWorksCorrectly()
        {
            var a1 = Angle.FromDeg(30);
            var a2 = Angle.FromDeg(45);
            var result = a1 + a2;
            Assert.That(result.Deg, Is.EqualTo(75).Within(tolerance));
        }

        [Test]
        public void MultiplicationByScalarWorksCorrectly()
        {
            var angle = Angle.FromDeg(60);
            var result = angle * 2;
            Assert.That(result.Deg, Is.EqualTo(120).Within(tolerance));
        }

        [Test]
        public void SubtractionWorksCorrectly()
        {
            var a1 = Angle.FromDeg(90);
            var a2 = Angle.FromDeg(45);
            var result = a1 - a2;
            Assert.That(result.Deg, Is.EqualTo(45).Within(tolerance));
        }

        [Test]
        public void UnaryMinusWorksCorrectly()
        {
            var angle = Angle.FromDeg(45);
            var result = -angle;
            Assert.That(result.Deg, Is.EqualTo(-45).Within(tolerance));
        }

        [Test]
        public void DivisionByScalarWorksCorrectly()
        {
            var angle = Angle.FromDeg(90);
            var result = angle / 2;
            Assert.That(result.Deg, Is.EqualTo(45).Within(tolerance));
        }

        [Test]
        public void ModuloWorksCorrectly()
        {
            var a1 = Angle.FromDeg(370);
            var a2 = Angle.FromDeg(90);
            var result = a1 % a2;
            Assert.That(result.Deg, Is.EqualTo(10).Within(tolerance));
        }

        [Test]
        public void DivisionAngleByAngleWorksCorrectly()
        {
            var a1 = Angle.FromDeg(180);
            var a2 = Angle.FromDeg(90);
            var result = a1 / a2;
            Assert.That(result, Is.EqualTo(2).Within(tolerance));
        }

        [Test]
        public void ComparisonOperatorsWorkCorrectly()
        {
            var smaller = Angle.FromDeg(30);
            var larger = Angle.FromDeg(60);

            Assert.Multiple(() =>
            {
                Assert.That(smaller < larger, Is.True);
                Assert.That(larger > smaller, Is.True);
                Assert.That(smaller <= larger, Is.True);
                Assert.That(larger >= smaller, Is.True);
                Assert.That(smaller <= smaller, Is.True);
                Assert.That(smaller >= smaller, Is.True);
            });
        }
        #endregion

        #region Compare and Equals
        [Test]
        public void CompareToReturnsCorrectOrder()
        {
            var a1 = Angle.FromDeg(30);
            var a2 = Angle.FromDeg(60);

            Assert.That(a1.CompareTo(a2), Is.LessThan(0));
            Assert.That(a2.CompareTo(a1), Is.GreaterThan(0));
            Assert.That(a1.CompareTo(a1), Is.EqualTo(0));
        }

        [Test]
        public void CompareToWithNonAngleThrowsException()
        {
            var angle = Angle.FromDeg(45);
            Assert.Throws<ArgumentException>(() => angle.CompareTo("not an angle"));
        }

        [Test]
        public void NaNEqualsNaNReturnsTrue()
        {
            Assert.Multiple(() =>
            {
                Assert.That(Angle.NaN == Angle.NaN, Is.True);
                Assert.That(Angle.NaN.Equals(Angle.NaN), Is.True);
            });
        }

        [Test]
        public void EqualsWithNullReturnsFalse()
        {
            var angle = Angle.FromDeg(45);
            Assert.That(angle.Equals(null), Is.False);
        }

        [Test]
        public void EqualsWithDifferentTypeReturnsFalse()
        {
            var angle = Angle.FromDeg(45);
            Assert.That(angle.Equals("string"), Is.False);
        }
        #endregion

        [Test]
        public void ConstantsHaveCorrectValues()
        {
            Assert.Multiple(() =>
            {
                Assert.That(Angle.Zero.Rad, Is.EqualTo(0));
                Assert.That(Angle.Deg90.Deg, Is.EqualTo(90).Within(tolerance));
                Assert.That(Angle.Deg180.Deg, Is.EqualTo(180).Within(tolerance));
                Assert.That(Angle.Deg270.Deg, Is.EqualTo(270).Within(tolerance));
                Assert.That(Angle.Deg360.Deg, Is.EqualTo(360).Within(tolerance));

                Assert.That(Angle.NaN.Rad, Is.EqualTo(double.NaN));
                Assert.That(Angle.PositiveInfinity.Rad, Is.EqualTo(double.PositiveInfinity));
                Assert.That(Angle.NegativeInfinity.Rad, Is.EqualTo(double.NegativeInfinity));
            });
        }

        [Test]
        public void IsZeroReturnsCorrectly()
        {
            Assert.Multiple(() =>
            {
                Assert.That(Angle.IsZero(Angle.Zero), Is.True);
                Assert.That(Angle.IsZero(Angle.FromDeg(0.000001)), Is.False);
            });
        }

        [Test]
        public void IsNaNReturnsCorrectly()
        {
            Assert.Multiple(() =>
            {
                Assert.That(Angle.IsNaN(Angle.NaN), Is.True);
                Assert.That(Angle.IsNaN(Angle.Zero), Is.False);
            });
        }

        [Test]
        public void IsInfinityReturnsCorrectly()
        {
            Assert.Multiple(() =>
            {
                Assert.That(Angle.IsInfinity(Angle.PositiveInfinity), Is.True);
                Assert.That(Angle.IsInfinity(Angle.NegativeInfinity), Is.True);
                Assert.That(Angle.IsInfinity(Angle.Zero), Is.False);
            });
        }

        [Test]
        public void OperationsWithNaNReturnNaN()
        {
            var nan = Angle.NaN;
            var normal = Angle.FromDeg(30);

            Assert.Multiple(() =>
            {
                Assert.That(Angle.IsNaN(nan + normal), Is.True);
                Assert.That(Angle.IsNaN(nan - normal), Is.True);
                Assert.That(Angle.IsNaN(nan * 2), Is.True);
                Assert.That(Angle.IsNaN(nan / 2), Is.True);
            });
        }

        [Test]
        public void OperationsWithInfinityReturnInfinity()
        {
            var inf = Angle.PositiveInfinity;
            var normal = Angle.FromDeg(30);

            Assert.Multiple(() =>
            {
                Assert.That((inf + normal).Rad, Is.EqualTo(double.PositiveInfinity));
                Assert.That((inf * 2).Rad, Is.EqualTo(double.PositiveInfinity));
            });
        }

        [Test]
        public void DivisionByZeroReturnsInfinity()
        {
            var angle = Angle.FromDeg(90);
            var result = angle / 0;
            Assert.That(result.Rad, Is.EqualTo(double.PositiveInfinity));
        }

        #region HashCode
        [Test]
        public void GetHashCodeForEqualAnglesReturnsSameValue()
        {
            var a1 = Angle.FromDeg(45);
            var a2 = Angle.FromDeg(45);

            Assert.That(a1.GetHashCode(), Is.EqualTo(a2.GetHashCode()));
        }       
        #endregion

        [Test]
        public void ToStringReturnsExpectedFormat()
        {
            var angle = Angle.FromDeg(45.5);
            var result = angle.ToString();

            // Проверка формата (зависит от культуры)
            StringAssert.Contains("Degrees", result);
            // Или более конкретно:
            // Assert.That(result, Is.EqualTo("45.5 Degrees"));
        }
    }
}