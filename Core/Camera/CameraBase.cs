using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Core.Tickable;

namespace Core.Camera
{
    public class CameraBase : TickableObject
    {

        public CameraBase()
        {

        }

        public CameraBase(float fFOV, float fAspectRatio, float fNear, float fFar)
        {
            FOV = fFOV;
            AspectRatio = fAspectRatio;
            Near = fNear;
            Far = fFar;
        }

        public override void Tick(double fDeltaSeconds)
        {

        }

        public Matrix4 View
        {
            get { return ViewMatrix; }
        }

        public Matrix4 Proj
        {
            get { return ProjMatrix; }
        }

        public void UpdateViewMatrix()
        {
            ViewMatrix = Matrix4.LookAt(EyeLocation, LookAtLocation, Vector3.UnitY);
        }

        public void UpdateProjMatrix()
        {
            ProjMatrix = Matrix4.CreatePerspectiveFieldOfView(FOV, AspectRatio, Near, Far);
        }

        public float FOV { get; set; }
        public float Near { get; set; }
        public float Far { get; set; }
        public float AspectRatio { get; set; }

        public Vector3 EyeLocation = new Vector3();
        public Vector3 LookAtLocation = new Vector3();

        protected float FieldOfView;
        protected Matrix4 ProjMatrix = new Matrix4();
        protected Matrix4 ViewMatrix = new Matrix4();
    }
}
