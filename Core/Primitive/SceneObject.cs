using System;
using Core.CustomAttribute;
using OpenTK;

namespace Core.Primitive
{
    public abstract class SceneObject
    {
        [ExposeUI("Translation")]
        public virtual Vector3 Translation { get; set; }

        [ExposeUI] public virtual float Scale { get; set; } = 1.0f;

        [ExposeUI, UIGroup("Rotation")] public virtual float Yaw { get; set; } = 0;

        [ExposeUI, UIGroup("Rotation")] public virtual float Pitch { get; set; } = 0;

        [ExposeUI, UIGroup("Rotation")] public virtual float Roll { get; set; } = 0;

        public virtual OpenTK.Matrix4 ParentMatrix { get; set; } = Matrix4.Identity;

        public virtual OpenTK.Matrix4 LocalMatrix
        {
            get;
        }

        public virtual void Tick(double elapsed) { }

        public abstract void Draw(MaterialBase.MaterialBase material);

        public static EventHandler<EventArgs> OnOpenGLContextCreated;
        

        public SceneObject()
        {
            SceneObjectManager.Get().AddSceneObject(this);
        }
       
        public virtual void OnGLContextCreated(object sender, EventArgs e)
        {
            Initialize();
        }

        public virtual void Initialize()
        {

        }

        protected string ObjectName = "";

        public bool IsReadyToDraw => bReadyToDraw;

        public bool IsVisible => bVisible;

        protected bool bReadyToDraw = false;

        protected bool bVisible = true;
    }
}