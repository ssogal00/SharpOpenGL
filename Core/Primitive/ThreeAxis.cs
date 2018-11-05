using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Primitive
{
    public class ThreeAxis : RenderResource
    {
        public ThreeAxis()
        { }
        
        protected Cylinder xAxis = new Cylinder(1.0f, 10.0f, 10);
        protected Cylinder yAxis = new Cylinder(1.0f, 10.0f, 10);
        protected Cylinder zAxis = new Cylinder(1.0f, 10.0f, 10);
    }
}
