using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Core.Primitive
{
    public class Sphere : RenderResource
    {
        public Sphere(float radius, int stackcount, int sectorcount)
        {
            Debug.Assert(radius > 0 && StackCount > 0 && SectorCount > 0);
            Radius = radius;
            StackCount = stackcount;
            SectorCount = sectorcount;
        }

        protected void GenerateVertices()
        {

        }

        protected float Radius = 10.0f;
        protected int StackCount = 10;
        protected int SectorCount = 10;

    }
}
