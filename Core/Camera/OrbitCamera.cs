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
        }

        public override void Tick(double fDeltaSeconds)
        {
            var vDir = (DestEyeLocation - EyeLocation).Normalized();

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
            var vDir = (LookAtLocation - EyeLocation).Normalized();

            EyeLocation = vDir * MoveAmount + EyeLocation;
            UpdateViewMatrix();
        }

        public override void MoveBackward()
        {
            var vDir = (LookAtLocation - EyeLocation).Normalized();
            EyeLocation = vDir * (-MoveAmount) + EyeLocation;
            UpdateViewMatrix();
        }

        public override void RotateRight()
        {
            
        }

        public override void RotateLeft()
        {
            
        }

        public float CameraDistance = 10;
        public float DestCameraDistance = 10;

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
    }    
}
