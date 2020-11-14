using OpenTK;
using System;
using Core.CustomEvent;

using OpenTK.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Core.Camera
{
    public class OrbitCamera : CameraBase
    {
        public OrbitCamera()
            : base(OpenTK.Mathematics.MathHelper.PiOver6, 1.0f, 1.0f, 10000.0f)
        {
            
            EyeLocation = new Vector3(5, 5, 5);
            DestLocation = new Vector3(5, 5, 5);
            LookAtLocation = new Vector3(0, 0, 0);
        }

        public OrbitCamera(float fFOV, float fAspectRatio, float fNear, float fFar)            
            : base(fFOV, fAspectRatio, fNear, fFar)
        {
            EyeLocation = new Vector3(5, 5, 5);
            DestLocation = new Vector3(5, 5, 5);
            LookAtLocation = new Vector3(0, 0, 0);
        }

        public override void OnKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if(e.Key == Keys.W)
            {
                MoveForward();
            }
            else if(e.Key == Keys.S)
            {
                MoveBackward();
            }
            else if(e.Key == Keys.A)
            {
                RotateYawLeft();
            }
            else if(e.Key == Keys.D)
            {
                RotateYawRight();
            }
            else if(e.Key == Keys.Q)
            {
                RotatePitchDownward();
            }
            else if(e.Key == Keys.E)
            {
                RotatePitchUpward();
            }
        }

        public override void Tick(double fDeltaSeconds)
        {
            var vDir = (DestEyeLocation - EyeLocation);

            if(vDir.Length > 1)
            {
                EyeLocation = vDir * 0.01f + EyeLocation;
            }
            else
            {
                EyeLocation = DestEyeLocation;
            }

            UpdateViewMatrix();
            UpdateProjMatrix();
        }

        public override void UpdateViewMatrix()
        {
            ViewMatrix = Matrix4.LookAt(EyeLocation, LookAtLocation, Vector3.UnitY);
        }

        public override void MoveForward()
        {
            var vDir = (EyeLocation - LookAtLocation).Normalized();

            DistanceToLookAt -= MoveAmount;
            DistanceToLookAt = Math.Max(DistanceToLookAt, 1);

            DestEyeLocation = LookAtLocation + DistanceToLookAt * vDir;
        }

        public override void MoveBackward()
        {
            var vDir = (EyeLocation - LookAtLocation).Normalized();

            DistanceToLookAt += MoveAmount;

            DestEyeLocation = LookAtLocation + DistanceToLookAt * vDir;
        }

        private Vector3 GetDestEyeLocation()
        {
            var current = new Vector4(LookAtLocation, 1.0f);
            var rotmatrix = Matrix4.CreateRotationX(Pitch) * Matrix4.CreateRotationY(Yaw);
            var translation = rotmatrix * new Vector4(DistanceToLookAt, 0, 0, 1);

            return LookAtLocation + translation.Xyz;
        }

        public override void RotateYawRight()
        {

            
            Yaw += OpenTK.Mathematics.MathHelper.DegreesToRadians(3.0f);
            DestEyeLocation = GetDestEyeLocation();
        }

        public override void RotateYawLeft()
        {
            Yaw -= OpenTK.Mathematics.MathHelper.DegreesToRadians(3.0f);
            DestEyeLocation = GetDestEyeLocation();
        }

        public override void RotatePitchDownward()
        {
            Pitch -= OpenTK.Mathematics.MathHelper.DegreesToRadians(3.0f);
            DestEyeLocation = GetDestEyeLocation();
        }

        public override void RotatePitchUpward()
        {
            Pitch += OpenTK.Mathematics.MathHelper.DegreesToRadians(3.0f);
            DestEyeLocation = GetDestEyeLocation();
        }

        protected float MoveAmount = 1.0f;

        public Vector3 DestLocation
        {
            get { return DestEyeLocation; }
            set { DestEyeLocation = value;}
        }

        public override void OnWindowResized(int width, int height)
        {
            var Width = width;
            var Height = height;

            float fAspectRatio = Width / (float)Height;

            AspectRatio = fAspectRatio;
            FOV = OpenTK.Mathematics.MathHelper.PiOver6;
            Near = 1;
            Far = 10000;

            EyeLocation = new Vector3(5, 5, 5);
            DestLocation = new Vector3(5, 5, 5);
            LookAtLocation = new Vector3(0, 0, 0);
        }

        protected Vector3 DestEyeLocation = new Vector3();

        protected float Yaw = 0;
        protected float Pitch = 0;
        
        public void SetDistanceToLookAt(float newValue)
        {
            DistanceToLookAt = newValue;
        }
        protected float DistanceToLookAt = 50.0f;
    }    
}
