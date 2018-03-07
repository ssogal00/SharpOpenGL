using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }

        public FreeCamera(float fFOV, float fAspectRatio, float fNear, float fFar)            
            : base(fFOV, fAspectRatio, fNear, fFar)
        {
            LookAtDir = new Vector3(1, 0, 0);
            EyeLocation = new Vector3(5, 5, 5);
        }
        
        public void MoveForward()
        {
            var vMoveDir = m_RotationMatrix.Row2;
            var vMove = Vector3.Multiply(vMoveDir, fMoveAmount);
            EyeLocation += vMove;
        }

        public void MoveBackward()
        {
            var vMoveDir = m_RotationMatrix.Row2;
            var vMove = Vector3.Multiply(vMoveDir, fMoveAmount);
            EyeLocation -= vMove;
        }

        public void MoveRight()
        {
            var RightDir = m_RotationMatrix.Row0;
            var vMove = Vector3.Multiply(RightDir, fMoveAmount);
            EyeLocation -= vMove;
        }

        public void MoveLeft()
        {
            var RightDir = m_RotationMatrix.Row0;
            var vMove = Vector3.Multiply(RightDir, fMoveAmount);
            EyeLocation += vMove;
        }        

        public void MoveUpward()
        {
            var vMove = Vector3.Multiply(UpDir, fMoveAmount);
            EyeLocation += vMove;
        }

        public void MoveDownward()
        {
            var vMove = Vector3.Multiply(UpDir, fMoveAmount);
            EyeLocation -= vMove;
        }

        public void RotateRight()
        {
            m_fYaw -= m_fRotateAmount;
        }

        public void RotateLeft()
        {
            m_fYaw += m_fRotateAmount;
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
            UpdateViewMatrix();
            UpdateProjMatrix();
        }

        public override void UpdateViewMatrix()
        {
            m_RotationMatrix = Matrix3.CreateRotationY(m_fYaw);
            ViewMatrix = Matrix4.LookAt(EyeLocation, EyeLocation + Vector3.Multiply(m_RotationMatrix.Row2, 1.0f), Vector3.UnitY);
        }

        protected float m_fYaw = 0;

        protected float m_fPitch = 0;

        protected float fMoveAmount = 3;

        protected float m_fRotateAmount = OpenTK.MathHelper.DegreesToRadians(3);

        protected Matrix3 m_RotationMatrix = new Matrix3();
    }
}
