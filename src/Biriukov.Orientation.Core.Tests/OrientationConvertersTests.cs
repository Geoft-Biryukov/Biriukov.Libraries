using Biriukov.Orientation.Core;

namespace Orientation.Core.Tests;

[TestFixture]
public class OrientationConvertersTests
{
    #region ToQuaternion
    [Test]
    [TestCase(0.0, 0.0, 0.0)]
    [TestCase(30.0, 0.0, 0.0)]
    [TestCase(30.0, 45.0, 60.0)]
    public void ToQuaternion_ClassicAngles_ReturnsCorrectValue(double psi, double theta, double phi)
    {
        // Arrange
        var psiAngle = Angle.FromDeg(psi);
        var thetaAngle = Angle.FromDeg(theta);
        var phiAngle = Angle.FromDeg(phi);

        var q1 = CreateRotationQuaternion(Angle.FromDegToRad(psi), 3);
        var q2 = CreateRotationQuaternion(Angle.FromDegToRad(theta), 1);
        var q3 = CreateRotationQuaternion(Angle.FromDegToRad(phi), 3);

        var expected_result = q1 * q2 * q3;

        var euler = EulerAngles.CreateClassic(psiAngle, thetaAngle, phiAngle);

        // Act
        var result = euler.ToQuaternion();

        // Assert
        Assert.That(result.Equals(expected_result, 1e-10), Is.True);
    }

    [Test]
    [TestCase(0.0, 0.0, 0.0)]
    [TestCase(30.0, 0.0, 0.0)]
    [TestCase(30.0, 45.0, 60.0)]
    public void ToQuaternion_KrylovAngles_ReturnsCorrectValue(double psi, double theta, double phi)
    {
        // Arrange
        var psiAngle = Angle.FromDeg(psi);
        var thetaAngle = Angle.FromDeg(theta);
        var phiAngle = Angle.FromDeg(phi);

        var q1 = CreateRotationQuaternion(Angle.FromDegToRad(psi), 2);
        var q2 = CreateRotationQuaternion(Angle.FromDegToRad(theta), 3);
        var q3 = CreateRotationQuaternion(Angle.FromDegToRad(phi), 1);

        var expected_result = q1 * q2 * q3;

        var euler = EulerAngles.CreateKrylov(psiAngle, thetaAngle, phiAngle);

        // Act
        var result = euler.ToQuaternion();

        // Assert
        Assert.That(result.Equals(expected_result, 1e-10), Is.True);
    }
    #endregion

    #region ToEulerAngles

    //[Test]
    //[TestCase(0.0, 0.0, 0.0)]
    ////[TestCase(30.0, 0.0, 0.0)]
    //[TestCase(30.0, 45.0, 60.0)]
    //public void ToClassicEulerAngles_ReturnsCorrectValue(double psi, double theta, double phi)
    //{
    //    // Arrange
    //    var psiAngle = Angle.FromDeg(psi);
    //    var thetaAngle = Angle.FromDeg(theta);
    //    var phiAngle = Angle.FromDeg(phi);

    //    var q1 = CreateRotationQuaternion(Angle.FromDegToRad(psi), 3);
    //    var q2 = CreateRotationQuaternion(Angle.FromDegToRad(theta), 1);
    //    var q3 = CreateRotationQuaternion(Angle.FromDegToRad(phi), 3);

    //    var q = q1 * q2 * q3;

    //    //Act
    //    var result = q.ToClassicEulerAngles();

    //    // Assert
    //    Assert.Multiple(() =>
    //    {
    //        Assert.That(result.Psi.Deg, Is.EqualTo(psi).Within(1e-10));
    //        Assert.That(result.Theta.Deg, Is.EqualTo(theta).Within(1e-10));
    //        Assert.That(result.Phi.Deg, Is.EqualTo(phi).Within(1e-10));
    //    });        
    //}

    [Test]
    [TestCase(0.0, 0.0, 0.0)]
    [TestCase(30.0, 0.0, 0.0)]
    [TestCase(30.0, 45.0, 60.0)]
    public void ToKrylovEulerAngles_ReturnsCorrectValue(double psi, double theta, double phi)
    {
        // Arrange
        var psiAngle = Angle.FromDeg(psi);
        var thetaAngle = Angle.FromDeg(theta);
        var phiAngle = Angle.FromDeg(phi);

        var q1 = CreateRotationQuaternion(Angle.FromDegToRad(psi), 2);
        var q2 = CreateRotationQuaternion(Angle.FromDegToRad(theta), 3);
        var q3 = CreateRotationQuaternion(Angle.FromDegToRad(phi), 1);

        var q = q1 * q2 * q3;

        //Act
        var result = q.ToKrylovEulerAngles();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Psi.Deg, Is.EqualTo(psi).Within(1e-10));
            Assert.That(result.Theta.Deg, Is.EqualTo(theta).Within(1e-10));
            Assert.That(result.Phi.Deg, Is.EqualTo(phi).Within(1e-10));
        });
    }

    #endregion

    #region ToDirectionCosineMatrix
    [Test]
    [TestCase(0.0, 0.0, 0.0)]
    [TestCase(30.0, 0.0, 0.0)]
    [TestCase(30.0, 45.0, 60.0)]
    public void ToDirectionCosineMatrix_ClassicAnglesAndQuaternion_ReturnsCorrectValue(double psi, double theta, double phi)
    {
        // Arrange
        var psiAngle = Angle.FromDeg(psi);
        var thetaAngle = Angle.FromDeg(theta);
        var phiAngle = Angle.FromDeg(phi);

        var q1 = CreateRotationQuaternion(Angle.FromDegToRad(psi), 3);
        var q2 = CreateRotationQuaternion(Angle.FromDegToRad(theta), 1);
        var q3 = CreateRotationQuaternion(Angle.FromDegToRad(phi), 3);

        var expected_result = q1 * q2 * q3;

        var euler = EulerAngles.CreateClassic(psiAngle, thetaAngle, phiAngle);

        // Act
        var resultFromAngles = euler.ToDirectionCosineMatrix();
        var resultFromQuaternion = expected_result.ToDirectionCosineMatrix();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(resultFromAngles.R11, Is.EqualTo(resultFromQuaternion.R11).Within(1e-10));
            Assert.That(resultFromAngles.R12, Is.EqualTo(resultFromQuaternion.R12).Within(1e-10));
            Assert.That(resultFromAngles.R13, Is.EqualTo(resultFromQuaternion.R13).Within(1e-10));

            Assert.That(resultFromAngles.R21, Is.EqualTo(resultFromQuaternion.R21).Within(1e-10));
            Assert.That(resultFromAngles.R22, Is.EqualTo(resultFromQuaternion.R22).Within(1e-10));
            Assert.That(resultFromAngles.R23, Is.EqualTo(resultFromQuaternion.R23).Within(1e-10));

            Assert.That(resultFromAngles.R31, Is.EqualTo(resultFromQuaternion.R31).Within(1e-10));
            Assert.That(resultFromAngles.R32, Is.EqualTo(resultFromQuaternion.R32).Within(1e-10));
            Assert.That(resultFromAngles.R33, Is.EqualTo(resultFromQuaternion.R33).Within(1e-10));
        });
        
    }

    [Test]
    [TestCase(0.0, 0.0, 0.0)]
    [TestCase(30.0, 0.0, 0.0)]
    [TestCase(30.0, 45.0, 60.0)]
    public void ToDirectionCosineMatrix_KrylovAnglesAnglesAndQuaternion_ReturnsCorrectValue(double psi, double theta, double phi)
    {
        // Arrange
        var psiAngle = Angle.FromDeg(psi);
        var thetaAngle = Angle.FromDeg(theta);
        var phiAngle = Angle.FromDeg(phi);

        var q1 = CreateRotationQuaternion(Angle.FromDegToRad(psi), 2);
        var q2 = CreateRotationQuaternion(Angle.FromDegToRad(theta), 3);
        var q3 = CreateRotationQuaternion(Angle.FromDegToRad(phi), 1);

        var expected_result = q1 * q2 * q3;

        var krylov = EulerAngles.CreateKrylov(psiAngle, thetaAngle, phiAngle);

        // Act
        var resultFromAngles = krylov.ToDirectionCosineMatrix();
        var resultFromQuaternion = expected_result.ToDirectionCosineMatrix();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(resultFromAngles.R11, Is.EqualTo(resultFromQuaternion.R11).Within(1e-10));
            Assert.That(resultFromAngles.R12, Is.EqualTo(resultFromQuaternion.R12).Within(1e-10));
            Assert.That(resultFromAngles.R13, Is.EqualTo(resultFromQuaternion.R13).Within(1e-10));

            Assert.That(resultFromAngles.R21, Is.EqualTo(resultFromQuaternion.R21).Within(1e-10));
            Assert.That(resultFromAngles.R22, Is.EqualTo(resultFromQuaternion.R22).Within(1e-10));
            Assert.That(resultFromAngles.R23, Is.EqualTo(resultFromQuaternion.R23).Within(1e-10));

            Assert.That(resultFromAngles.R31, Is.EqualTo(resultFromQuaternion.R31).Within(1e-10));
            Assert.That(resultFromAngles.R32, Is.EqualTo(resultFromQuaternion.R32).Within(1e-10));
            Assert.That(resultFromAngles.R33, Is.EqualTo(resultFromQuaternion.R33).Within(1e-10));
        });

    }
    #endregion

    #region Helpers
    private static Quaternion CreateRotationQuaternion(double alpha, int axisIndex)
    {
        return axisIndex switch
        {
            1 => new Quaternion(Math.Cos(0.5 * alpha), Math.Sin(0.5 * alpha), 0.0, 0.0),
            2 => new Quaternion(Math.Cos(0.5 * alpha), 0.0, Math.Sin(0.5 * alpha), 0.0),
            3 => new Quaternion(Math.Cos(0.5 * alpha), 0.0, 0.0, Math.Sin(0.5 * alpha)),
            _ => throw new NotSupportedException(),
        };
    }
    #endregion
}
