using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;

namespace Core.Camera
{
    public class FreeCamera : CameraBase
    {
        public FreeCamera()
            : base(OpenTK.MathHelper.PiOver6, 1.0f, 1.0f, 10000.0f)
        {
            LookAtDir = new Vector3(1, 0, 0);
            EyeLocation = new Vector3(5, 5, 5);
            Destination = EyeLocation;
        }

        public FreeCamera(float fFOV, float fAspectRatio, float fNear, float fFar)            
            : base(fFOV, fAspectRatio, fNear, fFar)
        {
            LookAtDir = new Vector3(1, 0, 0);
            EyeLocation = new Vector3(5, 5, 5);
            Destination = EyeLocation;
        }
        
        public override void MoveForward()
        {
            UpdateMoveKeyStrokeCount();
            var vMoveDir = m_RotationMatrix.Row2;
            var vMove = Vector3.Multiply(vMoveDir, GetMoveAmount());
            Destination = EyeLocation + vMove;
        }

        public override void MoveBackward()
        {
            UpdateMoveKeyStrokeCount();
            var vMoveDir = m_RotationMatrix.Row2;
            var vMove = Vector3.Multiply(vMoveDir, GetMoveAmount());
            Destination = EyeLocation - vMove;
        }

        public override void MoveRight()
        {
            UpdateMoveKeyStrokeCount();
            var RightDir = m_RotationMatrix.Row0;
            var vMove = Vector3.Multiply(RightDir, GetMoveAmount());
            Destination = EyeLocation - vMove;
        }

        public override void MoveLeft()
        {
            UpdateMoveKeyStrokeCount();
            var RightDir = m_RotationMatrix.Row0;
            var vMove = Vector3.Multiply(RightDir, GetMoveAmount());
            Destination = EyeLocation + vMove;
        }        

        public override void MoveUpward()
        {
            UpdateMoveKeyStrokeCount();
            var vMove = Vector3.Multiply(UpDir, GetMoveAmount());
            Destination = EyeLocation + vMove;
        }

        public override void MoveDownward()
        {
            UpdateMoveKeyStrokeCount();
            var vMove = Vector3.Multiply(UpDir, GetMoveAmount());
            Destination = EyeLocation - vMove;
        }

        public void RotateRight()
        {
            Yaw -= m_fRotateAmount;
        }

        public void RotateLeft()
        {
            Yaw += m_fRotateAmount;
        }

        public override void OnKeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.W)
            {
                MoveForward();
            }
            else if(e.KeyCode == Keys.S)
            {
                MoveBackward();
            }
            else if (e.KeyCode == Keys.E)
            {
                RotateRight();
            }
            else if (e.KeyCode == Keys.Q)
            {
                RotateLeft();
            }
        }

        private void UpdateMoveKeyStrokeCount()
        {
            TimeSpan span = DateTime.Now - LastKeyStrokeTime;

            if(span.TotalSeconds < 1)
            {
                MoveAcc++;
                MoveAcc = Math.Min(MoveAcc, 4);
            }

            LastKeyStrokeTime = DateTime.Now;
        }

        private float GetMoveAmount()
        {
            return (float) Math.Exp((double)MoveAcc);
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
            else if(e.Key == OpenTK.Input.Key.D)
            {
                MoveRight();
            }
            else if(e.Key == OpenTK.Input.Key.A)
            {
                MoveLeft();
            }
            else if(e.Key == OpenTK.Input.Key.Z)
            {
                MoveUpward();
            }
            else if(e.Key == OpenTK.Input.Key.X)
            {
                MoveDownward();
            }
            else if(e.Key == OpenTK.Input.Key.E)
            {
                RotateRight();
            }
            else if(e.Key == OpenTK.Input.Key.Q)
            {
                RotateLeft();
            }
        }

        public override void Tick(double fDeltaSeconds)
        {
            if( (Destination - EyeLocation).Length > 1 )
            {
                Elapsed = Math.Max(Elapsed, 0.01f);
                EyeLocation += (Destination - EyeLocation) * (MoveSeconds / Elapsed);
                Elapsed += (float) fDeltaSeconds;
                bMoving = true;
            }
            else
            {
                EyeLocation = Destination;
                bMoving = false;
            }

            if((DateTime.Now - LastKeyStrokeTime).TotalSeconds > 1)
            {
                MoveAcc = 0;
            }

            UpdateViewMatrix();
            UpdateProjMatrix();
        }

        public override void UpdateViewMatrix()
        {
            m_RotationMatrix = Matrix3.CreateRotationY(Yaw);
            ViewMatrix = Matrix4.LookAt(EyeLocation, EyeLocation + Vector3.Multiply(m_RotationMatrix.Row2, 1.0f), Vector3.UnitY);
        }

        protected float Yaw = -90;

        protected float Pitch = 0;

        protected float fMoveAmount = 6;

        protected float m_fRotateAmount = OpenTK.MathHelper.DegreesToRadians(3);

        protected Matrix3 m_RotationMatrix = new Matrix3();

        public Vector3 Destination;

        protected float MoveSeconds = 0.3f;

        protected float Elapsed = 0;

        bool bMoving = false;

        int MoveAcc = 0;

        DateTime LastKeyStrokeTime;
    }
}
