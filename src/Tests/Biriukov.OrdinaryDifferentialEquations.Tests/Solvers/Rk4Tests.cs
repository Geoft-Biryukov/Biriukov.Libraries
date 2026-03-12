using Biriukov.OrdinaryDifferentialEquations.Solvers;
using Biriukov.OrdinaryDifferentialEquations.Solvers.SolverSettings;

namespace Biriukov.OrdinaryDifferentialEquations.Tests.Solvers;

[TestFixture]
public class Rk4Tests
{    
    private InitialValueProblem problemHarm;

    [SetUp]
    public void Setup()
    {
        double initTime = 0;
        var initCond = new StateVector([0.1, 0]);       

        var eqHarm = new HarmonicOscillatorEquation(1);
        problemHarm = new InitialValueProblem(initTime, initCond, eqHarm);
    }

    [Test]
    public void Constructor_ThrowsArgumentNullException()
    {
        // Apply
        // Assert
        Assert.Throws<ArgumentNullException>(() => { new Rk4Solver(null); });
    }

    [Test]
    public void Constructor_CanCreateSolver()
    {
        // Arrange
        var init = new StateVector(2);
        var settings = new Rk4Settings(0.1);

        // Apply
        var solver = new Rk4Solver(settings);

        // Assert
        Assert.That(solver, Is.Not.Null);

    }
   

    [Test]
    [TestCase(0.1, 10.0)]
    [TestCase(0.01, 10.0)]
    [TestCase(0.0234, 10.123)]
    public void Solve_ShouldSolveOdeHarmonic_ReturnSolution(double step, double finalTime)
    {
        // Arrange        
        var settings = new Rk4Settings(step);
        var slnProvider = problemHarm.Equation as IAnalyticalSolutionProvider;

        var slnFinal = slnProvider.GetAnalyticalSolution(finalTime);

        // Apply
        var solver = new Rk4Solver(settings);
        var numSln = solver.Solve(problemHarm, finalTime).ToArray();
        var finalSln = slnProvider.GetAnalyticalSolution(finalTime);

        double eps = Math.Pow(step, 4);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(numSln.Last().Time, Is.EqualTo(finalTime).Within(1e-10));
            Assert.That(numSln.Last().State[0], Is.EqualTo(finalSln[0]).Within(eps));
            Assert.That(numSln.Last().State[1], Is.EqualTo(finalSln[1]).Within(eps));
        });
    }
}
