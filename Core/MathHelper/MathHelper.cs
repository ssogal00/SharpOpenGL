using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Core.MathHelper
{
    public class MathHelper
    {
        public static double GaussianDistribution(double fX, double fY, double fRho)
        {
            double g = 1.0 / Math.Sqrt(( 2.0f * OpenTK.MathHelper.Pi * fRho * fRho ));
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

            return (1.0f / Math.Sqrt(2.0f * OpenTK.MathHelper.Pi * std_deviation * std_deviation))
                * Math.Exp((-((x - mean) * (x - mean))) / (2.0f * std_deviation * std_deviation));
        }
    }
}
