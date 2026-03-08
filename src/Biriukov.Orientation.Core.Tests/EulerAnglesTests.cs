using Biriukov.Orientation.Core;

namespace Orientation.Core.Tests;

[TestFixture]
public class EulerAnglesTests
{
    [Test]
    public void CanCreateEulerAnglesClassic()
    {
        var psi = Angle.Deg0;
        var theta = Angle.Deg90;
        var phi = Angle.Deg180;

        var a = EulerAngles.CreateClassic(psi, theta, phi);
        Assert.Multiple(() =>
        {
            Assert.That(a.AnglesType, Is.EqualTo(EulerAnglesTypes.Classic));
            Assert.That(a.Psi, Is.EqualTo(psi));
            Assert.That(a.Theta, Is.EqualTo(theta));
            Assert.That(a.Phi, Is.EqualTo(phi));
        }
        );
    }

    [Test]
    public void CanCreateEulerAnglesKrylov()
    {
        var psi = Angle.Deg0;
        var theta = Angle.Deg90;
        var phi = Angle.Deg180;

        var a = EulerAngles.CreateKrylov(psi, theta, phi);
        Assert.Multiple(() =>
        {
            Assert.That(a.AnglesType, Is.EqualTo(EulerAnglesTypes.Krylov));
            Assert.That(a.Psi, Is.EqualTo(psi));
            Assert.That(a.Theta, Is.EqualTo(theta));
            Assert.That(a.Phi, Is.EqualTo(phi));
        }
        );
    }
}
