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
            var dir = (LookAtLocation - EyeLocation).Normalized();
            var right = Vector3.Cross(dir, Vector3.UnitY).Normalized();
            var up = Vector3.Cross(right, dir).Normalized();

            Matrix3 test = new Matrix3();
            test.Row0 = right;
            test.Row1 = up;
            test.Row2 = dir;

            test = test * Matrix3.CreateRotationY(OpenTK.MathHelper.DegreesToRadians(180));

            ViewMatrix.Row0 = new Vector4(test.Row0,0);
            ViewMatrix.Row1 = new Vector4(test.Row1, 0);
            ViewMatrix.Row2 = new Vector4(test.Row2, 0);
            ViewMatrix.Row3 = new Vector4(EyeLocation,1);
            ViewMatrix.Invert();
        }

        public override void MoveForward()
        {
            var vDir = (LookAtLocation - EyeLocation).Normalized();

            DestEyeLocation = vDir * MoveAmount + EyeLocation;
        }

        public override void MoveBackward()
        {
            var vDir = -(LookAtLocation - EyeLocation).Normalized();

            DestEyeLocation = vDir * MoveAmount + EyeLocation;
        }

        public override void RotateRight()
        {
            Yaw += OpenTK.MathHelper.DegreesToRadians(3.0f);

            var distance = (float)(LookAtLocation - EyeLocation).Length;

            var current = new Vector4(LookAtLocation, 1.0f);

            var rotmatrix = Matrix4.CreateRotationY(Yaw);
            
            var transmatrix = Matrix4.CreateTranslation(distance, 0, 0);

            var translation = new Vector4(distance, 0, 0, 0) * rotmatrix;

            var length = translation.Length;

            Console.WriteLine(length);

            DestEyeLocation = EyeLocation = LookAtLocation + translation.Xyz;

            Console.WriteLine("Eye {0} {1} {2}", EyeLocation.X, EyeLocation.Y, EyeLocation.Z);
            Console.WriteLine("LookAt {0} {1} {2}", LookAtLocation.X, LookAtLocation.Y, LookAtLocation.Z);
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

        protected float Yaw = 0;
    }    
}
