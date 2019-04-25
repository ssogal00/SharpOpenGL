using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Windows;
using Core.CustomAttribute;
using Core.Tickable;
using Core.CustomEvent;

namespace Core.Camera
{
    public class CameraBase : TickableObject
    {
        public CameraBase()
        {

        }

        public bool IsLocked = false;

        public void ToggleLock()
        {
            IsLocked = !IsLocked;
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

        public Matrix4 PrevView
        {
            get { return PrevViewMatrix; }
        }

        public Matrix4 PrevProj
        {
            get { return PrevProjMatrix; }
        }

        public virtual void MoveForward() {}

        public virtual void MoveBackward() {}

        public virtual void MoveRight() {}

        public virtual void MoveLeft() {}

        public virtual void MoveUpward() {}

        public virtual void MoveDownward() {}

        public virtual void RotateYawRight() { }
        public virtual void RotateYawLeft() { }
        public virtual void RotateYaw(float fAmount) {}

        public virtual void RotatePitch(float fAmount) { }
        public virtual void RotatePitchDownward() { }
        public virtual void RotatePitchUpward() { }

        public virtual void UpdateViewMatrix()
        {
            
        }

        public virtual void UpdateProjMatrix()
        {
            PrevProjMatrix = ProjMatrix;
            ProjMatrix = Matrix4.CreatePerspectiveFieldOfView(FOV, AspectRatio, Near, Far);
        }

        [ExposeUI]
        public float FOV { get; set; }
        [ExposeUI]
        public float Near { get; set; }
        [ExposeUI]
        public float Far { get; set; }
        
        public float AspectRatio { get; set; }

        public Vector3 EyeLocation = new Vector3();
        public Vector3 LookAtLocation = new Vector3();
        
        public virtual void OnKeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        { }

        public virtual void OnKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        { }

        public virtual void OnKeyUp(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        { }

        public virtual void OnKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        { }

        public virtual void OnWindowResized(object sender, ScreenResizeEventArgs eventArgs)
        {
            var Width = eventArgs.Width;
            var Height = eventArgs.Height;

            float fAspectRatio = Width / (float)Height;

            AspectRatio = fAspectRatio;            
        }

        protected Vector3 UpDir = new Vector3(0,1,0);
        protected Vector3 LookAtDir = new Vector3(1, 0, 0); 
        
        
        protected Matrix4 ProjMatrix = new Matrix4();
        protected Matrix4 ViewMatrix = new Matrix4();

        protected Matrix4 PrevProjMatrix = new Matrix4();
        protected Matrix4 PrevViewMatrix = new Matrix4();
    }
}
