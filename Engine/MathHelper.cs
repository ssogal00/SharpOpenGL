using System;
using OpenTK;
using OpenTK.Mathematics;


namespace Core
{
    public static class MathHelper
    {
        public static double GaussianDistribution(double fX, double fY, double fRho)
        {
            double g = 1.0 / Math.Sqrt(( 2.0f * OpenTK.Mathematics.MathHelper.Pi * fRho * fRho ));
            g *= Math.Exp(-(fX * fX + fY * fY) / (2 * fRho * fRho));
            return g;
        }
        public static double ComputeGaussianValue(float x, float mean, float std_deviation)
        {

            // The gaussian equation is defined as such:
            /*    
              -(x - mean)^2
              -------------
              1.0               2*std_dev^2
              f(x,mean,std_dev) = -------------------- * e^
              sqrt(2*pi*std_dev^2)

             */

            return (1.0f / Math.Sqrt(2.0f * OpenTK.Mathematics.MathHelper.Pi * std_deviation * std_deviation))
                * Math.Exp((-((x - mean) * (x - mean))) / (2.0f * std_deviation * std_deviation));
        }


        public static Vector3 UniformHemisphere()
        {
            Vector3 result = new Vector3(0);

            float x1 = (float) random.NextDouble();
            float x2 = (float) random.NextDouble();

            float s = (float) Math.Sqrt(1.0f - x1 * x1);
            result.X = (float) Math.Cos(OpenTK.Mathematics.MathHelper.TwoPi * x2) * s;
            result.Y = (float) Math.Sin(OpenTK.Mathematics.MathHelper.TwoPi * x2) * s;
            result.Z = x1;

            return result;
        }

        public static Vector2 UniformCircle()
        {
            var result = new Vector2(0);

            float x = (float) random.NextDouble();
            result.X = (float) Math.Cos(OpenTK.Mathematics.MathHelper.TwoPi * x);
            result.Y = (float) Math.Sin(OpenTK.Mathematics.MathHelper.TwoPi * x);

            return result;
        }

        public static Random random = new Random();

        public static Vector3 Forward => new Vector3(0, 0, 1);
        public static Vector3 ForwardRight => (new Vector3(1, 0, 1)).Normalized();
        public static Vector3 ForwardLeft => (new Vector3(-1, 0, 1)).Normalized();

    }
}
