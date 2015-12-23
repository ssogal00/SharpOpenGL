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
        public OrbitCamera()
        { }

        public OrbitCamera(float fFOV, float fAspectRatio, float fNear, float fFar)            
            : base(fFOV, fAspectRatio, fNear, fFar)
        {            
        }

        public override void Tick(double fDeltaSeconds)
        {
            var vDir = (DestEyeLocation - EyeLocation).Normalized();

            if(vDir.Length > 0.01)
            {
                EyeLocation = vDir * 0.01f + EyeLocation;                
            }

            UpdateViewMatrix();
            UpdateProjMatrix();
        }

        public void MoveForward(float fAmount)
        {
            var vDir = (LookAtLocation - EyeLocation).Normalized();

            DestEyeLocation = vDir * fAmount + EyeLocation;
        }

        public float CameraDistance;

        public Vector3 DestLocation
        {
            get { return DestEyeLocation; }
            set { DestEyeLocation = value;}
        }

        protected Vector3 DestEyeLocation = new Vector3();
    }    
}
