using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Core.CustomAttribute;
using Core.Texture;
using Engine.Rendering;
using GLTF;
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

        public int ID
        {
            get => mID;
        }

        protected int mID = 0;

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
        public virtual void Render(){}


        protected virtual void PrepareRenderingData(){}

        //
        public virtual void JustDraw() { }

        public static EventHandler<EventArgs> OnOpenGLContextCreated;
        

        public GameObject()
        {
            Name = string.Format("SceneObject_{0}", ObjectCount);
            mID = ObjectCount++;
            SceneObjectManager.Instance.AddSceneObject(this);
        }

        protected GameObject(string name)
        {
            mID = ObjectCount++;
            Name = string.Format("{0}_{1}", name, mID);
            SceneObjectManager.Instance.AddSceneObject(this);
        }
       
        public virtual void Initialize()
        {

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

        protected static int ObjectCount = 0;

        // for rendering data
        public int VertexCount
        {
            get => mVertexCount;
        }

        protected int mVertexCount = 0;

        protected int mMeshSectionCount = 1;
        public int MeshSectionCount
        {
            get
            {
                return mMeshSectionList.Count;
            }
        }

        public string MaterialName { get; set; } = string.Empty;

        protected MaterialBase.MaterialBase defaultMaterial = null;

        public virtual IEnumerable<(string, Vector2)> GetVector2Params(int meshSection)
        {
            return Enumerable.Empty<(string, Vector2)>();
        }
        
        public virtual IEnumerable<(string, Vector3)> GetVector3Params(int meshSection)
        {
            return Enumerable.Empty<(string, Vector3)>();
        }

        public virtual IEnumerable<(string, Vector4)> GetVector4Params(int sectionIndex)
        {
            return Enumerable.Empty<(string, Vector4)>();
        }


        public virtual IEnumerable<(string, Matrix4)> GetMatrix4Params(int sectionIndex)
        {
            return Enumerable.Empty<(string, Matrix4)>();
        }

        public virtual IEnumerable<(string, string)> GetTextureParams(int sectionIndex)
        {
            return Enumerable.Empty<(string, string)>();
        }

        public virtual IEnumerable<(string, bool)> GetBoolParams(int sectionIndex)
        {
            return Enumerable.Empty<(string, bool)>();
        }

        public virtual IEnumerable<(string, float)> GetFloatParams(int sectionIndex)
        {
            return Enumerable.Empty<(string, float)>();
        }

        public virtual IEnumerable<(string, int)> GetIntParams(int sectionIndex)
        {
            return Enumerable.Empty<(string, int)>();
        }

        public List<MeshSection> MeshSectionList => mMeshSectionList;

        protected List<MeshSection> mMeshSectionList = new List<MeshSection>();

    }
}