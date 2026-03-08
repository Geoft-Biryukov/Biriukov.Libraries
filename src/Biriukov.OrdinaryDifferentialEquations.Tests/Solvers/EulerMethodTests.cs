using Biriukov.OrdinaryDifferentialEquations.Solvers;
using Biriukov.OrdinaryDifferentialEquations.Solvers.SolverSettings;

namespace Biriukov.OrdinaryDifferentialEquations.Tests.Solvers;

[TestFixture]
public class EulerMethodTests
{
    private InitialValueProblem problemHarmonic;

    [SetUp]
    public void Setup()
    {
        double initTime = 0;
        var initCond = new StateVector([0.1, 0]);

        var eqnsHarm = new HarmonicOscillatorEquation(1);
        problemHarmonic = new InitialValueProblem(initTime, initCond, eqnsHarm);
    }

    [Test]
    public void Constructor_ThrowsArgumentNullException()
    {
        // Apply
        // Assert
        Assert.Throws<ArgumentNullException>(() => {new EulerMethodSolver(null); });     
    }

    [Test]
    public void Constructor_CanCreateSolver()
    {
        // Arrange
        var init = new StateVector(2);        
        var settings = new EulerMethodSettings(0.1);        

        // Apply
        var solver = new EulerMethodSolver(settings);

        // Assert
        Assert.That(solver, Is.Not.Null);

    }
    
    [Test]
    [TestCase(0.1, 10.0)]
    [TestCase(0.01, 10.0)]
    [TestCase(0.0123, 10.324)]
    public void Solve_ShouldSolveOdeHarmonic_ReturnSolution(double step, double finalTime)
    {
        // Arrange        
        var settings = new EulerMethodSettings(step);
        var slnProvider = problemHarmonic.Equation as IAnalyticalSolutionProvider;

        var slnFinal = slnProvider.GetAnalyticalSolution(finalTime);

        // Apply
        var solver = new EulerMethodSolver(settings);
        var numSln = solver.Solve(problemHarmonic, finalTime).ToArray();

        var finalSln = slnProvider.GetAnalyticalSolution(finalTime);

        double eps = Math.Pow(step, 1);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(numSln.Last().Time, Is.EqualTo(finalTime).Within(1e-10));
            Assert.That(numSln.Last().State[0], Is.EqualTo(finalSln[0]).Within(eps));
            Assert.That(numSln.Last().State[1], Is.EqualTo(finalSln[1]).Within(eps));
        });


    }
}
