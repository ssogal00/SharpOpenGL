using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace SharpOpenGL
{
    public class EquirectangleToCubemap
    {
        private OpenTK.Matrix4 CaptureProjection = Matrix4.CreatePerspectiveFieldOfView( MathHelper.DegreesToRadians(90), 1.0f, 0.1f, 10.0f);

        private OpenTK.Matrix4[] CatpureViews =
        {
            Matrix4.LookAt(new Vector3(0,0,0), Vector3.UnitX, Vector3.UnitY ),
            Matrix4.LookAt(new Vector3(0,0,0), -Vector3.UnitX, Vector3.UnitY ),
            Matrix4.LookAt(new Vector3(0,0,0), Vector3.UnitY, Vector3.UnitZ),
            Matrix4.LookAt(new Vector3(0,0,0), -Vector3.UnitY, -Vector3.UnitZ ),
            Matrix4.LookAt(new Vector3(0,0,0), Vector3.UnitZ, -Vector3.UnitY ),
            Matrix4.LookAt(new Vector3(0,0,0), -Vector3.UnitZ, -Vector3.UnitY),
        };

    }
}
