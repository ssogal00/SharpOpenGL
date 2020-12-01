using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Core;

using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace Core.Camera
{
    public class FreeCamera : CameraBase
    {
        public FreeCamera()
            : base(OpenTK.Mathematics.MathHelper.PiOver6, 1.0f, 1.0f, 10000.0f)
        {
            LookAtDir = new Vector3(1, 0, 0);
            EyeLocation = new Vector3(0, 20, 0);
            
            Destination = EyeLocation;
        }

        public FreeCamera(float fFOV, float fAspectRatio, float fNear, float fFar)            
            : base(fFOV, fAspectRatio, fNear, fFar)
        {
            LookAtDir = new Vector3(1, 0, 0);
            EyeLocation = new Vector3(0, 0, 0);
            Destination = EyeLocation;
        }

        public Vector3 GetLookAtDir()
        {
            return m_RotationMatrix.Row2;
        }

        private Vector3 GetMoveDirection()
        {
            
            if (MoveKeyDictionary[Keys.W] && MoveKeyDictionary[Keys.A])
            {
                return (m_RotationMatrix.Row2 + m_RotationMatrix.Row0).Normalized();
            }
            else if (MoveKeyDictionary[Keys.W] && MoveKeyDictionary[Keys.D])
            {
                return (m_RotationMatrix.Row2 - m_RotationMatrix.Row0).Normalized();
            }
            else if (MoveKeyDictionary[Keys.S] && MoveKeyDictionary[Keys.A])
            {
                return (-m_RotationMatrix.Row2 + m_RotationMatrix.Row0).Normalized();
            }
            else if (MoveKeyDictionary[Keys.S] && MoveKeyDictionary[Keys.D])
            {
                return (-m_RotationMatrix.Row2 - m_RotationMatrix.Row0).Normalized();
            }
            else if (MoveKeyDictionary[Keys.W])
            {
                return (m_RotationMatrix.Row2).Normalized();
            }            
            else if(MoveKeyDictionary[Keys.S])
            {
                return -m_RotationMatrix.Row2;
            }
            else if(MoveKeyDictionary[Keys.A])
            {
                return m_RotationMatrix.Row0;
            }
            else if(MoveKeyDictionary[Keys.D])
            {
                return -m_RotationMatrix.Row0;
            }
            else if(MoveKeyDictionary[Keys.Z])
            {
                return Vector3.UnitY;
            }
            else if(MoveKeyDictionary[Keys.X])
            {
                return -Vector3.UnitY;
            }

            return Vector3.UnitX;
        }

        protected void Move()
        {
            var vMoveDir = GetMoveDirection();

            if (Single.IsNaN(vMoveDir.X) || Single.IsNaN(vMoveDir.Y) || Single.IsNaN(vMoveDir.Z))
            {
                Debug.Assert(false);
            }

            var vMove = Vector3.Multiply(vMoveDir, GetMoveAmount());
            Destination = EyeLocation + vMove;
        }
        
        public override void MoveForward()
        {
            var vMoveDir = m_RotationMatrix.Row2;
            var vMove = Vector3.Multiply(vMoveDir, GetMoveAmount());

            Destination = EyeLocation + vMove;
        }

        public override void MoveBackward()
        {
            var vMoveDir = m_RotationMatrix.Row2;
            var vMove = Vector3.Multiply(vMoveDir, GetMoveAmount());
            Destination = EyeLocation - vMove;
        }

        public override void MoveRight()
        {
         
            var RightDir = m_RotationMatrix.Row0;
            var vMove = Vector3.Multiply(RightDir, GetMoveAmount());
            Destination = EyeLocation - vMove;
        }

        public override void MoveLeft()
        {
         
            var RightDir = m_RotationMatrix.Row0;
            var vMove = Vector3.Multiply(RightDir, GetMoveAmount());
            Destination = EyeLocation + vMove;
        }        

        public override void MoveUpward()
        {
         
            var vMove = Vector3.Multiply(UpDir, GetMoveAmount());
            Destination = EyeLocation + vMove;
        }

        public override void MoveDownward()
        {
         
            var vMove = Vector3.Multiply(UpDir, GetMoveAmount());
            Destination = EyeLocation - vMove;
        }

        public override void RotateYaw(float amount)
        {
            Yaw -= amount;
        }

        public override void RotatePitch(float amount)
        {
            Pitch += amount;
        }

        public override void RotateYawRight()
        {
            Yaw -= m_fRotateAmount;
        }

        public override void RotateYawLeft()
        {
            Yaw += m_fRotateAmount;
        }

        public override void RotatePitchUpward()
        {
            Pitch += m_fRotateAmount;
        }

        public override void RotatePitchDownward()
        {
            Pitch -= m_fRotateAmount;
        }

        private void UpdateMoveSpeed()
        {
            TimeSpan span = DateTime.Now - LastKeyStrokeTime;

            if(span.TotalSeconds < 0.1)
            {
                SpeedIndex++;
                SpeedIndex = Math.Min(SpeedIndex, SpeedList.Count - 1);
            }
            LastKeyStrokeTime = DateTime.Now;
        }

        private float GetMoveAmount()
        {
            return 30.0f;
        }

        public override void OnKeyUp(object sender, KeyboardKeyEventArgs e)
        {
            if (IsLocked)
            {
                return;
            }

            if (MoveKeys.Contains(e.Key))
            {
                bMoving = false;
                SpeedIndex = 0;
            }

            if(MoveKeyDictionary.ContainsKey(e.Key))
            {
                MoveKeyDictionary[e.Key] = false;
            }
        }

        public override void OnKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (IsLocked)
            {
                return;
            }

            if (MoveKeys.Contains(e.Key))
            {
                MoveStarted = EyeLocation;
                bMoving = true;
                UpdateMoveSpeed();
            }

            if(MoveKeyDictionary.ContainsKey(e.Key))
            {
                MoveKeyDictionary[e.Key] = true;
                Move();
            }
        }

        public override void Tick(double fDeltaSeconds)
        {
            if(bMoving)
            {
                Elapsed += (float)fDeltaSeconds;

                var vDir = (Destination - MoveStarted);

                if (Single.IsNaN(vDir.X) || Single.IsNaN(vDir.Y) || Single.IsNaN(vDir.Z))
                {
                    Debug.Assert(false);
                }

                vDir.Normalize();
                EyeLocation += vDir * (float)fDeltaSeconds * SpeedList[SpeedIndex];
            }
            else
            {
                Elapsed = 0;
            }

            if((DateTime.Now - LastKeyStrokeTime).TotalSeconds > 1)
            {
                SpeedIndex = 0;
            }

            UpdateViewMatrix();
            UpdateProjMatrix();
        }

        public override void UpdateViewMatrix()
        {
            PrevViewMatrix = ViewMatrix;
            m_RotationMatrix = Matrix3.CreateRotationX(Pitch) * Matrix3.CreateRotationY(Yaw);
            ViewMatrix = Matrix4.LookAt(EyeLocation, EyeLocation + Vector3.Multiply(m_RotationMatrix.Row2, 1.0f), Vector3.UnitY);
        }

        public float Yaw { get; set; } = OpenTK.Mathematics.MathHelper.PiOver2;

        protected float Pitch = 0;

        protected float fMoveAmount = 6;

        
        protected float m_fRotateAmount = OpenTK.Mathematics.MathHelper.DegreesToRadians(7);

        protected Matrix3 m_RotationMatrix = new Matrix3();

        public Vector3 Destination;

        protected Vector3 MoveStarted;

        protected float MoveSeconds = 0.1f;

        protected float Elapsed = 0;

        bool bMoving = false;

        int SpeedIndex = 0;

        protected List<float> SpeedList = new List<float> {32,64,128,256,512,1024};

        
        private List<Keys> MoveKeys = new List<Keys>{ Keys.W, Keys.A, Keys.S, Keys.D, Keys.Z, Keys.X };

        private Dictionary<Keys, bool> MoveKeyDictionary = new Dictionary<Keys, bool>
        {
            {Keys.W, false },
            {Keys.A, false },
            {Keys.S, false },
            {Keys.D, false },
            {Keys.Z, false },
            {Keys.X, false }
        };

        DateTime LastKeyStrokeTime;
    }
}
