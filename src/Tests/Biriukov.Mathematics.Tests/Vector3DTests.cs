using Biriukov.Mathematics.Geometry;

namespace Biriukov.Mathematics.Tests
{
    public class Vector3DTests
    {
        private const double Tolerance = 1e-10;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CanCreateVector3dWithoutParameters()
        {
            var v = new Vector3D();

            Assert.Multiple(() =>
            {
                Assert.That(v.X, Is.EqualTo(0.0).Within(Tolerance));
                Assert.That(v.Y, Is.EqualTo(0.0).Within(Tolerance));
                Assert.That(v.Z, Is.EqualTo(0.0).Within(Tolerance));
            });            
        }

        [Test]
        [TestCase(1.0, 2.0, 3.0)]
        [TestCase(154.0203, 22.125467, 534.00001)]
        [TestCase(double.NaN, double.NaN, double.NaN)]
        public void CanCreateVector3dWithParameters(double x, double y, double z)
        {
            var v = new Vector3D(x ,y, z);

            Assert.Multiple(() =>
            {
                Assert.That(v.X, Is.EqualTo(x).Within(Tolerance));
                Assert.That(v.Y, Is.EqualTo(y).Within(Tolerance));
                Assert.That(v.Z, Is.EqualTo(z).Within(Tolerance));
            });
        }

        #region Test operators
        [Test]
        [TestCase(2, 3, 4, 6, 7, 8, 8, 10, 12)]
        [TestCase( -2, -3, -4, 2, 3, 4, 0, 0, 0)]
        public void AdditionOperator_ValidVectors_ReturnsCorrectResult(
        double x1, double y1, double z1,
        double x2, double y2, double z2,
        double expectedX, double expectedY, double expectedZ)
        {
            // Arrange
            var v1 = new Vector3D(x1, y1, z1);
            var v2 = new Vector3D(x2, y2, z2);

            // Act
            var result = v1 + v2;

            // Assert
            Assert.Multiple(() =>
            {                
                Assert.That(result.X, Is.EqualTo(expectedX).Within(Tolerance));
                Assert.That(result.Y, Is.EqualTo(expectedY).Within(Tolerance));
                Assert.That(result.Z, Is.EqualTo(expectedZ).Within(Tolerance));
            });
        }

        [Test]
        [TestCase(6, 7, 8, 2, 3, 4, 4, 4, 4)]
        [TestCase(0, 0, 0, 2, 3, 4, -2, -3, -4)]
        public void SubtractionOperator_ValidVectors_ReturnsCorrectResult(
       double x1, double y1, double z1,
       double x2, double y2, double z2,
       double expectedX, double expectedY, double expectedZ)
        {
            // Arrange
            var v1 = new Vector3D(x1, y1, z1);
            var v2 = new Vector3D(x2, y2, z2);

            // Act
            var result = v1 - v2;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.X, Is.EqualTo(expectedX).Within(Tolerance));
                Assert.That(result.Y, Is.EqualTo(expectedY).Within(Tolerance));
                Assert.That(result.Z, Is.EqualTo(expectedZ).Within(Tolerance));
            });
        }
        #endregion

        #region Test products
        /*
         import numpy as np
         
         # Define two 3D vectors
         vector_a = np.array([1, 2, 3])
         vector_b = np.array([4, 5, 6])
         
         # Calculate the cross product
         cross_product = np.cross(vector_a, vector_b)
         
         # Print the result
         print(f"Vector A: {vector_a}")
         print(f"Vector B: {vector_b}")
         print(f"Cross Product: {cross_product}")
         */
        [Test]
        [TestCase(1, 2, 3, 4, 5, 6, -3,  6, - 3)]
        public void CrossProduct_ValidVectors_ReturnsCorrectResult(
            double x1, double y1, double z1,
            double x2, double y2, double z2,
            double expectedX, double expectedY, double expectedZ)
        {
            // Arrange
            var v1 = new Vector3D(x1, y1, z1);
            var v2 = new Vector3D(x2, y2, z2);

            // Act
            var result = Vector3D.CrossProduct(v1, v2);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.X, Is.EqualTo(expectedX).Within(Tolerance));
                Assert.That(result.Y, Is.EqualTo(expectedY).Within(Tolerance));
                Assert.That(result.Z, Is.EqualTo(expectedZ).Within(Tolerance));
            });
        }

        [Test]
        [TestCase(2, 3, 4, 1, 5, 2, 25)]
        public void DotProduct_ValidVectors_ReturnsCorrectResult(
            double x1, double y1, double z1,
            double x2, double y2, double z2,
            double expected)
        {
            // Arrange
            var v1 = new Vector3D(x1, y1, z1);
            var v2 = new Vector3D(x2, y2, z2);

            // Act
            var result = Vector3D.DotProduct(v1, v2);

            // Assert
            
            Assert.That(result, Is.EqualTo(expected).Within(Tolerance));                            
        }
        #endregion

        #region Test equals
        [Test]
        public void Equals_IdenticalVectors_ReturnsTrue()
        {
            // Arrange
            var v1 = new Vector3D(1, 2, 3);
            var v2 = new Vector3D(1, 2, 3);

            // Act & Assert
            Assert.Multiple(() =>
            {
                Assert.That(v1.Equals(v2), Is.True);
                Assert.That(v1 == v2, Is.True);
                Assert.That(v1 != v2, Is.False);
            });
        }

        [Test]
        public void Equals_DifferentQuaternions_ReturnsFalse()
        {
            // Arrange
            var v1 = new Vector3D(1, 2, 3);
            var v2 = new Vector3D(5, 6, 7);

            // Act & Assert
            Assert.Multiple(() =>
            {
                Assert.That(v1.Equals(v2), Is.False);
                Assert.That(v1 == v2, Is.False);
                Assert.That(v1 != v2, Is.True);
            });
        }

        [Test]
        public void EqualsWithTolerance_NearlyEqualQuaternions_ReturnsTrue()
        {
            // Arrange
            var v1 = new Vector3D(1.0, 2.0, 3.0);
            var v2 = new Vector3D(1.0000000001, 2.0000000001, 3.0000000001);

            // Act & Assert
            Assert.Multiple(() =>
            {
                Assert.That(v1.Equals(v2, 1e-9), Is.True);
                Assert.That(v1.Equals(v2), Is.False); // Exact comparison should fail
            });
        }

        [Test]
        public void Equals_NullObject_ReturnsFalse()
        {
            // Arrange
            var v = new Vector3D(1, 2, 3);
            object nullObj = null;

            // Act & Assert
            Assert.That(v.Equals(nullObj), Is.False);
        }

        [Test]
        public void Equals_DifferentType_ReturnsFalse()
        {
            // Arrange
            var v = new Vector3D(1, 2, 3);
            var notQuaternion = new object();

            // Act & Assert
            Assert.That(v.Equals(notQuaternion), Is.False);
        }
        #endregion
    }
}