namespace Biriukov.Orientation.Core
{    
    /// <summary>
    /// Struct for Euler angles 
    /// </summary>
    public readonly struct EulerAngles
    {
        private EulerAngles(Angle psi, Angle theta, Angle phi, EulerAnglesTypes type)
        {
            Psi = psi;
            Theta = theta;
            Phi = phi;
            AnglesType = type;
        }

        /// <summary>
        /// Create a classical Euler Angles sequence
        /// </summary>
        /// <param name="psi">precession</param>
        /// <param name="theta">nutation</param>
        /// <param name="phi">spin (intrinsic rotation)</param>
        /// <returns>Classical Euler angles sequence</returns>
        public static EulerAngles CreateClassic(Angle psi, Angle theta, Angle phi)
            => new(psi, theta, phi, EulerAnglesTypes.Classic);

        /// <summary>
        /// Create a Krylov Angles
        /// </summary>
        /// <param name="psi">yaw</param>
        /// <param name="theta">pitch</param>
        /// <param name="phi">roll</param>
        /// <returns>Krylov angles</returns>
        public static EulerAngles CreateKrylov(Angle psi, Angle theta, Angle phi)
            => new(psi, theta, phi, EulerAnglesTypes.Krylov);

        /// <summary>
        /// Precession or yaw
        /// </summary>
        public Angle Psi { get; }

        /// <summary>
        /// Nutation or pitch
        /// </summary>
        public Angle Theta { get; }

        /// <summary>
        /// Spin (intrinsic rotation) or roll
        /// </summary>
        public Angle Phi { get; }

        /// <summary>
        /// Type of Euler angles sequence
        /// </summary>
        public EulerAnglesTypes AnglesType { get; }
    }

    public enum EulerAnglesTypes
    {
        /// <summary>
        /// Classic Euler angles (precession, nutation, intrensic rotation
        /// </summary>
        Classic,

        /// <summary>
        /// Krylov angles (yaw, pitch, roll)
        /// </summary>
        Krylov
    }
}
