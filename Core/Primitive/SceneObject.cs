using System;
using Core.CustomAttribute;
using OpenTK;

namespace Core.Primitive
{
    public abstract class SceneObject
    {
        [ExposeUI("ObjectName")]
        public virtual string Name { get; set; }

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

        public virtual bool IsEditable { get; set; } = true;

        public virtual void Tick(double elapsed) { }

        //Draw with default material
        public abstract void Draw();

        //
        public virtual void JustDraw() { }

        public static EventHandler<EventArgs> OnOpenGLContextCreated;
        

        public SceneObject()
        {
            Name = string.Format("SceneObject_{0}", ObjectCount);
            SceneObjectManager.Get().AddSceneObject(this);
            ObjectCount++;
        }

        protected SceneObject(string name, int objectCount)
        {
            Name = string.Format("{0}_{1}", name, objectCount);
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

        public void SetVisible(bool flag)
        {
            bVisible = flag;
        }

        protected bool bReadyToDraw = false;

        protected bool bVisible = true;

        protected MaterialBase.MaterialBase defaultMaterial = null;

        protected static int ObjectCount = 0;
    }
}