using System;
using System.Collections.Generic;
using System.Linq;
using Core.CustomAttribute;
using Core.Texture;
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
        public abstract void Render();


        protected abstract void PrepareRenderingData();

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

        protected static int ObjectCount = 0;

        // for rendering data
        public int VertexCount
        {
            get => mVertexCount;
        }

        protected int mVertexCount = 0;

        public List<VertexAttributeSemantic> VertexAttributeList
        {
            get => mVertexAttributeList;
        }

        public Dictionary<string, VertexAttributeSemantic> VertexAttributeMap
        {
            get => mVertexAttributeMap;
        }

        public Dictionary<VertexAttributeSemantic, List<float>> FloatVertexAttributes
        {
            get => mFloatVertexAttributes;
        }
        public Dictionary<VertexAttributeSemantic, List<Vector2>> Vector2VertexAttributes
        {
            get => mVector2VertexAttributes;
        }
        public Dictionary<VertexAttributeSemantic, List<Vector3>> Vector3VertexAttributes
        {
            get => mVector3VertexAttributes;
        }
        public Dictionary<VertexAttributeSemantic, List<Vector4>> Vector4VertexAttributes
        {
            get => mVector4VertexAttributes;
        }

        public MaterialBase.MaterialBase Material { get; set; } = null;

        public string MaterialName { get; set; } = string.Empty;

        protected MaterialBase.MaterialBase defaultMaterial = null;

        public virtual IEnumerable<(string, Vector2)> GetVector2Params()
        {
            return Enumerable.Empty<(string, Vector2)>();
        }

        public virtual IEnumerable<(string, Vector3)> GetVector3Params()
        {
            return Enumerable.Empty<(string, Vector3)>();
        }

        public virtual IEnumerable<(string, Vector4)> GetVector4Params()
        {
            return Enumerable.Empty<(string, Vector4)>();
        }

        public virtual IEnumerable<(string, Matrix4)> GetMatrix4Params()
        {
            return Enumerable.Empty<(string, Matrix4)>();
        }

        public virtual IEnumerable<(string, string)> GetTextureParams()
        {
            return Enumerable.Empty<(string, string)>();
        }

        public virtual IEnumerable<(string, bool)> GetBoolParams()
        {
            return Enumerable.Empty<(string, bool)>();
        }

        public virtual IEnumerable<(string, float)> GetFloatParams()
        {
            return Enumerable.Empty <(string, float)>();
        }

        public virtual IEnumerable<(string, int)> GetIntParams()
        {
            return Enumerable.Empty<(string, int)>();
        }

        public List<uint> UIntIndices => mUIntIndices;

        public List<ushort> UShortIndices => mUShortIndices;

        protected List<VertexAttributeSemantic> mVertexAttributeList = new List<VertexAttributeSemantic>();

        protected Dictionary<string, VertexAttributeSemantic> mVertexAttributeMap = new Dictionary<string, VertexAttributeSemantic>();

        protected Dictionary<VertexAttributeSemantic, List<float>> mFloatVertexAttributes = new Dictionary<VertexAttributeSemantic, List<float>>();

        protected Dictionary<VertexAttributeSemantic, List<Vector3>> mVector3VertexAttributes = new Dictionary<VertexAttributeSemantic, List<Vector3>>();

        protected Dictionary<VertexAttributeSemantic, List<Vector2>> mVector2VertexAttributes = new Dictionary<VertexAttributeSemantic, List<Vector2>>();

        protected Dictionary<VertexAttributeSemantic, List<Vector4>> mVector4VertexAttributes = new Dictionary<VertexAttributeSemantic, List<Vector4>>();

        protected List<uint> mUIntIndices = new List<uint>();

        protected List<ushort> mUShortIndices = new List<ushort>();
    }
}