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
            var vMove = Vector3.Multiply(LookAtDir, fMoveAmount);
            EyeLocation += vMove;
        }

        public void MoveBackward()
        {
            var vMove = Vector3.Multiply(LookAtDir, fMoveAmount);
            EyeLocation -= vMove;
        }

        public void MoveRight()
        {
            var RightDir = Vector3.Cross(LookAtDir, UpDir);
            var vMove = Vector3.Multiply(RightDir, fMoveAmount);
            EyeLocation += vMove;
        }

        public void MoveLeft()
        {
            var RightDir = Vector3.Cross(LookAtDir, UpDir);
            var vMove = Vector3.Multiply(RightDir, fMoveAmount);
            EyeLocation -= vMove;
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
        }

        public override void Tick(double fDeltaSeconds)
        {
            UpdateViewMatrix();
            UpdateProjMatrix();
        }


        protected float fMoveAmount = 3;
    }
}
