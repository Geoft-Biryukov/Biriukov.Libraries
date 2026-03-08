namespace Biriukov.Orientation.Core
{
    public static class MathAngle
    {
        #region base
        public static double Sin(Angle angle) => Math.Sin(angle.Rad);
        public static double Cos(Angle angle) => Math.Cos(angle.Rad);        
        public static double Tan(Angle angle) => Math.Tan(angle.Rad);
        public static (double Sin, double Cos) SinCos(Angle angle) => Math.SinCos(angle.Rad);
        #endregion                
    }
}
