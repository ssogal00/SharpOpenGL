using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SharpOpenGL.Camera
{
    public class OrbitCamera : CameraBase
    {

        public override void Tick(float fDeltaSeconds)
        {


        }

        protected Vector3 DestEyeLocation;
    }    
}
