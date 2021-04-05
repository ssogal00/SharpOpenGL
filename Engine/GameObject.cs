using System;
using System.Collections.Generic;
using Core.CustomAttribute;
using OpenTK;
using OpenTK.Mathematics;

namespace Core.Primitive
{
    
    public abstract class GameObject : IDisposable
    {
        [ExposeUI("ObjectName")]
        public virtual string Name { get; set; }

        [ExposeUI("Translation")]
        public virtual Vector3 Translation { get; set; }

        public int Id = 0;

        public virtual float TranslationY
        {
            get { return Translation.Y; }
            set { Translation = new Vector3(Translation.X, value, Translation.Z);}
        }

        [ExposeUI] public virtual float Scale { get; set; } = 1.0f;

        [ExposeUI, UIGroup("Rotation")] public virtual float Yaw { get; set; } = 0;

        [ExposeUI, UIGroup("Rotation")] public virtual float Pitch { get; set; } = 0;

        [ExposeUI, UIGroup("Rotation")] public virtual float Roll { get; set; } = 0;

        public float Alpha = 0;

        public virtual Matrix4 ParentMatrix { get; set; } = Matrix4.Identity;

        public virtual Matrix4 LocalMatrix
        {
            get
            {
                return Matrix4.CreateScale(Scale) * Matrix4.CreateRotationY(Yaw) * Matrix4.CreateRotationX(Pitch) * Matrix4.CreateTranslation(Translation);
            }
        }   

        public virtual void Dispose()
        {

        }

        public virtual bool IsEditable { get; set; } = true;

        public virtual void Tick(double elapsed) { }

        //Render with default material
        public abstract void Render();

        // 
        public List<T> GetVertexAttribute<T>(int index)
        {
            return null;
        }

        protected abstract void PrepareRenderingData();

        //
        public virtual void JustDraw() { }

        public static EventHandler<EventArgs> OnOpenGLContextCreated;
        

        public GameObject()
        {
            Name = string.Format("SceneObject_{0}", ObjectCount);
            Id = ObjectCount++;
            SceneObjectManager.Instance.AddSceneObject(this);
        }

        protected GameObject(string name, int objectCount)
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

        public virtual List<RenderCommand> GetRenderCommands()
        {
            return null;
        }

        public bool IsReadyToDraw => bReadyToDraw;

        [ExposeUI]
        public virtual bool IsVisible
        {
            get { return bVisible; }
            set { bVisible = value; }
        }

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