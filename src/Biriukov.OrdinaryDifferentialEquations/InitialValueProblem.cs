namespace Biriukov.OrdinaryDifferentialEquations
{
    public class InitialValueProblem
    {
        public InitialValueProblem(double initialTime, StateVector initialStateVector, IDifferentialEquation equation)
        {
            InitialTime = initialTime;
            InitialStateVector = initialStateVector ?? throw new ArgumentNullException(nameof(initialStateVector)); 
            Equation = equation ?? throw new ArgumentNullException(nameof(equation));
        }

        public double InitialTime { get; init; }
        public StateVector InitialStateVector { get; init; }
        public IDifferentialEquation Equation { get; init; }
    }
}
