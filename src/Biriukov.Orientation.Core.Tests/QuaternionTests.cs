using Biriukov.Orientation.Core;

namespace Orientation.Core.Tests;

public class QuaternionTests
{
    private const double Tolerance = 1e-10;

    #region Constructor tests
    [Test]
    public void CanCreateQuaternion()
    {
        // Arrange
        double w = 0.1, x = 0.2, y = 0.3, z = 0.4;

        // Act
        var q_empty = new Quaternion();       
        var q_full = new Quaternion(w, x, y, z);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(q_empty.W, Is.EqualTo(0.0));
            Assert.That(q_empty.X, Is.EqualTo(0.0));
            Assert.That(q_empty.Y, Is.EqualTo(0.0));
            Assert.That(q_empty.Z, Is.EqualTo(0.0));           

            Assert.That(q_full.W, Is.EqualTo(w));
            Assert.That(q_full.X, Is.EqualTo(x));
            Assert.That(q_full.Y, Is.EqualTo(y));
            Assert.That(q_full.Z, Is.EqualTo(z));

        });
    }
    [Test]
    public void Indexer_ValidIndex_ReturnsCorrectValue()
    {
        // Arrange
        double w = 0.1, x = 0.2, y = 0.3, z = 0.4;

        // Act
        var q = new Quaternion(w, x, y, z);
        
        // Assert
        Assert.Multiple(() =>
        {            
            Assert.That(q[0], Is.EqualTo(w));
            Assert.That(q[1], Is.EqualTo(x));
            Assert.That(q[2], Is.EqualTo(y));
            Assert.That(q[3], Is.EqualTo(z));
        });
    }

    [Test]
    public void Indexer_InvalidIndex_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var q = new Quaternion(1.0, 2.0, 3.0, 4.0);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => { var _ = q[-1]; });
        Assert.Throws<ArgumentOutOfRangeException>(() => { var _ = q[4]; });
    }

    #endregion

    #region Conversion Operators Tests

    [Test]
    public void ImplicitConversion_FromDouble_CreatesQuaternionWithCorrectValues()
    {
        // Arrange
        double value = 5.0;

        // Act
        Quaternion q = value;

        // Assert
        Assert.Multiple(() =>
        {            
            Assert.That(q.W, Is.EqualTo(5.0));
            Assert.That(q.X, Is.EqualTo(0.0));
            Assert.That(q.Y, Is.EqualTo(0.0));
            Assert.That(q.Z, Is.EqualTo(0.0));
        });
    }

    [Test]
    public void ExplicitConversion_ToDouble_ReturnsScalarPart()
    {
        // Arrange
        var q = new Quaternion(1.0, 2.0, 3.0, 4.0);

        // Act
        double result = (double)q;

        // Assert
        Assert.That(result, Is.EqualTo(1.0));
    }

    #endregion

    #region Operator Tests

    [Test]
    [TestCase(1, 2, 3, 4, 5, 6, 7, 8, 6, 8, 10, 12)]
    [TestCase(-1, -2, -3, -4, 1, 2, 3, 4, 0, 0, 0, 0)]
    public void AdditionOperator_ValidQuaternions_ReturnsCorrectResult(
        double w1, double x1, double y1, double z1,
        double w2, double x2, double y2, double z2,
        double expectedW, double expectedX, double expectedY, double expectedZ)
    {
        // Arrange
        var q1 = new Quaternion(w1, x1, y1, z1);
        var q2 = new Quaternion(w2, x2, y2, z2);

        // Act
        var result = q1 + q2;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.W, Is.EqualTo(expectedW).Within(Tolerance));
            Assert.That(result.X, Is.EqualTo(expectedX).Within(Tolerance));
            Assert.That(result.Y, Is.EqualTo(expectedY).Within(Tolerance));
            Assert.That(result.Z, Is.EqualTo(expectedZ).Within(Tolerance));
        });
    }

    [Test]
    [TestCase(5, 6, 7, 8, 1, 2, 3, 4, 4, 4, 4, 4)]
    [TestCase(0, 0, 0, 0, 1, 2, 3, 4, -1, -2, -3, -4)]
    public void SubtractionOperator_ValidQuaternions_ReturnsCorrectResult(
        double w1, double x1, double y1, double z1,
        double w2, double x2, double y2, double z2,
        double expectedW, double expectedX, double expectedY, double expectedZ)
    {
        // Arrange
        var q1 = new Quaternion(w1, x1, y1, z1);
        var q2 = new Quaternion(w2, x2, y2, z2);

        // Act
        var result = q1 - q2;

        // Assert
        Assert.Multiple(() =>
        {            
            Assert.That(result.W, Is.EqualTo(expectedW).Within(Tolerance));
            Assert.That(result.X, Is.EqualTo(expectedX).Within(Tolerance));
            Assert.That(result.Y, Is.EqualTo(expectedY).Within(Tolerance));
            Assert.That(result.Z, Is.EqualTo(expectedZ).Within(Tolerance));
        });
    }

    [Test]
    [TestCase(0.1, 0.2, 0.3, 0.4, -0.1, -0.2, -0.3, -0.4)]
    [TestCase(1, 2, 3, 4, -1, -2, -3, -4)]
    public void UnaryMinusOperator_ValidQuaternion_ReturnsNegatedQuaternion(
        double w, double x, double y, double z,
        double expectedW, double expectedX, double expectedY, double expectedZ)
    {
        // Arrange
        var q = new Quaternion(w, x, y, z);

        // Act
        var result = -q;

        // Assert
        Assert.Multiple(() =>
        {            
            Assert.That(result.W, Is.EqualTo(expectedW).Within(Tolerance));
            Assert.That(result.X, Is.EqualTo(expectedX).Within(Tolerance));
            Assert.That(result.Y, Is.EqualTo(expectedY).Within(Tolerance));
            Assert.That(result.Z, Is.EqualTo(expectedZ).Within(Tolerance));
        });
    }

    /*
     **** Jupyter-Notebook ****
     
      import numpy as np
      import quaternion
      q1 = np.quaternion(1, 2, 3, 4)
      q2 = np.quaternion( 2, 5, 6, 7)
      res = q1 * q2
      print(res)
     
      Output: quaternion(-54, 6, 18, 12)
     */

    [Test]
    [TestCase(1, 2, 3, 4, 2, 5, 6, 7, -54, 6, 18, 12)]
    [TestCase(0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1)] // i * j = k
    public void MultiplicationOperator_ValidQuaternions_ReturnsCorrectResult(
        double w1, double x1, double y1, double z1,
        double w2, double x2, double y2, double z2,
        double expectedW, double expectedX, double expectedY, double expectedZ)
    {
        // Arrange
        var q1 = new Quaternion(w1, x1, y1, z1);
        var q2 = new Quaternion(w2, x2, y2, z2);

        // Act
        var result = q1 * q2;

        // Assert
        Assert.Multiple(() =>
        {            
            Assert.That(result.W, Is.EqualTo(expectedW).Within(Tolerance));
            Assert.That(result.X, Is.EqualTo(expectedX).Within(Tolerance));
            Assert.That(result.Y, Is.EqualTo(expectedY).Within(Tolerance));
            Assert.That(result.Z, Is.EqualTo(expectedZ).Within(Tolerance));
        });
    }

    [Test]
    [TestCase(2, 4, 6, 8, 2, 1, 2, 3, 4)]
    [TestCase(1, 2, 3, 4, 0.5, 2, 4, 6, 8)]
    public void DivisionByScalarOperator_ValidQuaternion_ReturnsCorrectResult(
        double w, double x, double y, double z,
        double divisor,
        double expectedW, double expectedX, double expectedY, double expectedZ)
    {
        // Arrange
        var q = new Quaternion(w, x, y, z);

        // Act
        var result = q / divisor;

        // Assert
        Assert.Multiple(() =>
        {            
            Assert.That(result.W, Is.EqualTo(expectedW).Within(Tolerance));
            Assert.That(result.X, Is.EqualTo(expectedX).Within(Tolerance));
            Assert.That(result.Y, Is.EqualTo(expectedY).Within(Tolerance));
            Assert.That(result.Z, Is.EqualTo(expectedZ).Within(Tolerance));
        });
    }    

    [Test]
    [TestCase(1, 2, 3, 4, 2, 2, 4, 6, 8)]
    [TestCase(2, 3, 4, 5, -1, -2, -3, -4, -5)]
    public void MultiplicationByScalarOperator_ValidQuaternion_ReturnsCorrectResult(
        double w, double x, double y, double z,
        double scalar,
        double expectedW, double expectedX, double expectedY, double expectedZ)
    {
        // Arrange
        var q = new Quaternion(w, x, y, z);

        // Act
        var result = q * scalar;

        // Assert
        Assert.Multiple(() =>
        {            
            Assert.That(result.W, Is.EqualTo(expectedW).Within(Tolerance));
            Assert.That(result.X, Is.EqualTo(expectedX).Within(Tolerance));
            Assert.That(result.Y, Is.EqualTo(expectedY).Within(Tolerance));
            Assert.That(result.Z, Is.EqualTo(expectedZ).Within(Tolerance));
        });
    }

    #endregion

    #region Equality Tests

    [Test]
    public void Equals_IdenticalQuaternions_ReturnsTrue()
    {
        // Arrange
        var q1 = new Quaternion(1, 2, 3, 4);
        var q2 = new Quaternion(1, 2, 3, 4);

        // Act & Assert
        Assert.Multiple(() =>
        {            
            Assert.That(q1.Equals(q2), Is.True);
            Assert.That(q1 == q2, Is.True);
            Assert.That(q1 != q2, Is.False);
        });
    }

    [Test]
    public void Equals_DifferentQuaternions_ReturnsFalse()
    {
        // Arrange
        var q1 = new Quaternion(1, 2, 3, 4);
        var q2 = new Quaternion(5, 6, 7, 8);

        // Act & Assert
        Assert.Multiple(() =>
        {            
            Assert.That(q1.Equals(q2), Is.False);
            Assert.That(q1 == q2, Is.False);
            Assert.That(q1 != q2, Is.True);
        });
    }

    [Test]
    public void EqualsWithTolerance_NearlyEqualQuaternions_ReturnsTrue()
    {
        // Arrange
        var q1 = new Quaternion(1.0, 2.0, 3.0, 4.0);
        var q2 = new Quaternion(1.0000000001, 2.0000000001, 3.0000000001, 4.0000000001);

        // Act & Assert
        Assert.Multiple(() =>
        {            
            Assert.That(q1.Equals(q2, 1e-9), Is.True);
            Assert.That(q1.Equals(q2), Is.False); // Exact comparison should fail
        });
    }

    [Test]
    public void Equals_NullObject_ReturnsFalse()
    {
        // Arrange
        var q = new Quaternion(1, 2, 3, 4);
        object nullObj = null;

        // Act & Assert
        Assert.That(q.Equals(nullObj), Is.False);
    }

    [Test]
    public void Equals_DifferentType_ReturnsFalse()
    {
        // Arrange
        var q = new Quaternion(1, 2, 3, 4);
        var notQuaternion = new object();

        // Act & Assert
        Assert.That(q.Equals(notQuaternion), Is.False);
    }

    [Test]
    public void GetHashCode_EqualQuaternions_ReturnsSameHashCode()
    {
        // Arrange
        var q1 = new Quaternion(1, 2, 3, 4);
        var q2 = new Quaternion(1, 2, 3, 4);

        // Act
        var hash1 = q1.GetHashCode();
        var hash2 = q2.GetHashCode();

        // Assert
        Assert.That(hash2, Is.EqualTo(hash1));
    }

    #endregion

    #region Quaternion Operations Tests

    [Test]
    public void ScalarPart_ReturnsCorrectValue()
    {
        // Arrange
        var q = new Quaternion(1, 2, 3, 4);

        // Act
        var scalar = q.ScalarPart;

        // Assert
        Assert.That(scalar, Is.EqualTo(1));
    }

    [Test]
    public void VectorPart_ReturnsCorrectValue()
    {
        // Arrange
        var q = new Quaternion(1, 2, 3, 4);

        // Act
        var vector = q.VectorPart;

        // Assert
        Assert.Multiple(() =>
        {            
            Assert.That(vector.W, Is.EqualTo(0));
            Assert.That(vector.X, Is.EqualTo(2));
            Assert.That(vector.Y, Is.EqualTo(3));
            Assert.That(vector.Z, Is.EqualTo(4));
        });
    }

    [Test]
    public void Conjugate_ValidQuaternion_ReturnsCorrectConjugate()
    {
        // Arrange
        var q = new Quaternion(1, 2, 3, 4);

        // Act
        var conjugate = q.Conjugate();

        // Assert
        Assert.Multiple(() =>
        {           
            Assert.That(conjugate.W, Is.EqualTo(1));
            Assert.That(conjugate.X, Is.EqualTo(-2));
            Assert.That(conjugate.Y, Is.EqualTo(-3));
            Assert.That(conjugate.Z, Is.EqualTo(-4));
        });
    }

    [Test]
    public void Norm_ValidQuaternion_ReturnsCorrectNorm()
    {
        // Arrange
        var q = new Quaternion(1, 2, 3, 4);

        // Act
        var norm = q.Norm();

        // Assert
        Assert.That(norm, Is.EqualTo(1 * 1 + 2 * 2 + 3 * 3 + 4 * 4).Within(Tolerance));
    }

    [Test]
    public void Inverse_ValidQuaternion_ReturnsCorrectInverse()
    {
        // Arrange
        var q = new Quaternion(1, 2, 3, 4);
        var norm = q.Norm();

        // Act
        var inverse = q.Inverse();

        // Assert
        Assert.Multiple(() =>
        {            
            Assert.That(inverse.W, Is.EqualTo(1 / norm).Within(Tolerance));
            Assert.That(inverse.X, Is.EqualTo(-2 / norm).Within(Tolerance));
            Assert.That(inverse.Y, Is.EqualTo(-3 / norm).Within(Tolerance));
            Assert.That(inverse.Z, Is.EqualTo(-4 / norm).Within(Tolerance));
        });
    }    

    [Test]
    [TestCase(1, 0, 0, 0, true)]  // Identity quaternion
    [TestCase(0.7071067811865, 0.7071067811865, 0, 0, true)]  // Approximately normalized
    [TestCase(1, 1, 1, 1, false)]  // Not normalized
    public void IsNormalized_VariousQuaternions_ReturnsCorrectValue(
        double w, double x, double y, double z, bool expected)
    {
        // Arrange
        var q = new Quaternion(w, x, y, z);

        // Act
        var isNormalized = q.IsNormalized;

        // Assert
        Assert.That(isNormalized, Is.EqualTo(expected));
    }

    [Test]
    public void StaticI1I2I3_ReturnCorrectImaginaryUnits()
    {
        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(Quaternion.I1, Is.EqualTo(new Quaternion(0, 1, 0, 0)));
            Assert.That(Quaternion.I2, Is.EqualTo(new Quaternion(0, 0, 1, 0)));
            Assert.That(Quaternion.I3, Is.EqualTo(new Quaternion(0, 0, 0, 1)));
        });
    }

    #endregion

    #region Static Methods Tests

    [Test]
    public void StaticConjugate_ReturnsSameAsInstanceMethod()
    {
        // Arrange
        var q = new Quaternion(1, 2, 3, 4);

        // Act
        var instanceConjugate = q.Conjugate();
        var staticConjugate = Quaternion.Conjugate(q);

        // Assert
        Assert.That(staticConjugate, Is.EqualTo(instanceConjugate));
    }

    [Test]
    public void StaticInverse_ReturnsSameAsInstanceMethod()
    {
        // Arrange
        var q = new Quaternion(1, 2, 3, 4);

        // Act
        var instanceInverse = q.Inverse();
        var staticInverse = Quaternion.Inverse(q);

        // Assert
        Assert.That(staticInverse, Is.EqualTo(instanceInverse));
    }

    [Test]
    public void StaticNorm_ReturnsSameAsInstanceMethod()
    {
        // Arrange
        var q = new Quaternion(1, 2, 3, 4);

        // Act
        var instanceNorm = q.Norm();
        var staticNorm = Quaternion.Norm(q);

        // Assert
        Assert.That(staticNorm, Is.EqualTo(instanceNorm));
    }

    [Test]
    public void StaticAdd_ReturnsSameAsAdditionOperator()
    {
        // Arrange
        var q1 = new Quaternion(1, 2, 3, 4);
        var q2 = new Quaternion(5, 6, 7, 8);

        // Act
        var operatorResult = q1 + q2;
        var staticResult = Quaternion.Add(q1, q2);

        // Assert
        Assert.That(staticResult, Is.EqualTo(operatorResult));
    }

    [Test]
    public void StaticMultiply_ReturnsSameAsMultiplicationOperator()
    {
        // Arrange
        var q1 = new Quaternion(1, 2, 3, 4);
        var q2 = new Quaternion(5, 6, 7, 8);

        // Act
        var operatorResult = q1 * q2;
        var staticResult = Quaternion.Multiply(q1, q2);

        // Assert
        Assert.That(staticResult, Is.EqualTo(operatorResult));
    }

    #endregion    
        
}
