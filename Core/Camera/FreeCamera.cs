using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Camera
{
    public class FreeCamera : CameraBase
    {
        public FreeCamera()
        { }

        public FreeCamera(float fFOV, float fAspectRatio, float fNear, float fFar)            
            : base(fFOV, fAspectRatio, fNear, fFar)
        {
        }

        public void RotateRight(float fAngle)
        {
            var RotateMatrix = OpenTK.Matrix4.CreateFromAxisAngle(UpDir, fAngle);
            LookAtVector = OpenTK.Vector3.TransformVector(LookAtVector, RotateMatrix);
        }

        public void RotateLeft(float fAngle)
        {
            var RotateMatrix = OpenTK.Matrix4.CreateFromAxisAngle(UpDir, -fAngle);
            LookAtVector = OpenTK.Vector3.TransformVector(LookAtVector, RotateMatrix);
        }

        public void MoveRight()
        {
            
        }

        public void MoveLeft()
        {
        }        

        public void MoveUpward()
        {
        }

        public void MoveDownward()
        {
            
        }



        protected float fMoveAmount = 3;
    }
}
