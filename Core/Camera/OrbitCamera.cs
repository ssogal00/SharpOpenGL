using OpenTK;
using System;
using Core.CustomEvent;
using System.Windows.Forms;
using OpenTK.Input;

namespace Core.Camera
{
    public class OrbitCamera : CameraBase
    {
        public OrbitCamera()
            : base(OpenTK.MathHelper.PiOver6, 1.0f, 1.0f, 10000.0f)
        { }

        public OrbitCamera(float fFOV, float fAspectRatio, float fNear, float fFar)            
            : base(fFOV, fAspectRatio, fNear, fFar)
        {            
        }

        public override void OnKeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if(e.Key == OpenTK.Input.Key.W)
            {
                MoveForward();
            }
            else if(e.Key == OpenTK.Input.Key.S)
            {
                MoveBackward();
            }
            else if(e.Key == OpenTK.Input.Key.A)
            {
                RotateLeft();
            }
            else if(e.Key == OpenTK.Input.Key.D)
            {
                RotateRight();
            }
            else if(e.Key == Key.Q)
            {
                RotatePitchDownward();
            }
            else if(e.Key == Key.E)
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

        public override void RotateRight()
        {
            Yaw += OpenTK.MathHelper.DegreesToRadians(3.0f);
            DestEyeLocation = GetDestEyeLocation();
        }

        public override void RotateLeft()
        {
            Yaw -= OpenTK.MathHelper.DegreesToRadians(3.0f);
            DestEyeLocation = GetDestEyeLocation();
        }

        public override void RotatePitchDownward()
        {
            Pitch -= OpenTK.MathHelper.DegreesToRadians(3.0f);
            DestEyeLocation = GetDestEyeLocation();
        }

        public override void RotatePitchUpward()
        {
            Pitch += OpenTK.MathHelper.DegreesToRadians(3.0f);
            DestEyeLocation = GetDestEyeLocation();
        }

        protected float MoveAmount = 1.0f;

        public Vector3 DestLocation
        {
            get { return DestEyeLocation; }
            set { DestEyeLocation = value;}
        }

        public override void OnWindowResized(object sender, ScreenResizeEventArgs eventArgs)
        {
            var Width = eventArgs.Width;
            var Height = eventArgs.Height;

            float fAspectRatio = Width / (float)Height;

            AspectRatio = fAspectRatio;
            FOV = OpenTK.MathHelper.PiOver6;
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
