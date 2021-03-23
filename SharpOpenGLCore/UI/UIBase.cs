using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Markup;
using CompiledMaterial.FontRenderMaterial;
using Core;
using Core.Buffer;
using Core.Primitive;
using Core.Texture;
using Core.VertexCustomAttribute;
using Vector2 = OpenTK.Mathematics.Vector2;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SharpOpenGL;
using SharpOpenGLCore.UI;

namespace SharpOpenGLCore
{
    [StructLayout(LayoutKind.Explicit, Size = 32)]
    public struct UIVertex : IGenericVertexAttribute
    {
        public UIVertex(Vector2 position)
        {
            Position = position;
            Texcoord = Vector2.Zero;
            Color = Vector4.UnitX;
        }

        public UIVertex(Vector2 position, Vector2 texcoord)
        {
            Position = position;
            Texcoord = texcoord;
            Color = Vector4.One;
        }

        public UIVertex(Vector2 position, Vector2 texcoord, Vector4 color)
        {
            Position = position;
            Texcoord = texcoord;
            Color = color;
        }

        public void VertexAttributeBinding()
        {
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 32, new IntPtr(0));

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 32, new IntPtr(8));

            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 32, new IntPtr(16));
        }

        public void VertexAttributeBinding(int index)
        {
            throw new NotImplementedException();
        }

        [FieldOffset(0), ComponentCount(2), ComponentType(VertexAttribPointerType.Float)]
        public Vector2 Position; // 8byte
        [FieldOffset(8), ComponentCount(2), ComponentType(VertexAttribPointerType.Float)]
        public Vector2 Texcoord;
        [FieldOffset(16), ComponentCount(4), ComponentType(VertexAttribPointerType.Float)]
        public Vector4 Color; // including alpha

        
    }

    public enum EAnchorPosition
    {
        LeftTop,
        LeftCenter,
        LeftBottom,
        CenterTop,
        CenterCenter,
        CenterBottom,
        RightTop,
        RightCenter,
        RightBottom,
    }

    public class UIBase
    {
        public virtual void Draw()
        {

        }

        public virtual void BuildVertexList(List<UIVertex> vertexList, out bool bChanged)
        {
            if (mDirtyMark)
            {
                mCachedVertexList.Clear();
                Vector2 anchoredPos = GetAnchorPosition(mAnchorPos);

                var leftTop = new UIVertex(anchoredPos + LeftTop, Vector2.Zero, new Vector4(1,0,0,1));
                var rightTop = new UIVertex(anchoredPos + RightTop, new Vector2(1, 0), new Vector4(1, 0, 0, 1));
                var leftBtm = new UIVertex(anchoredPos + LeftBottom, new Vector2(0, 1), new Vector4(1, 0, 0, 0));
                var rightBtm = new UIVertex(anchoredPos + RightBottom, new Vector2(1, 1), new Vector4(1, 0, 0, 0));

                mCachedVertexList.Add(leftTop);
                mCachedVertexList.Add(rightTop);
                mCachedVertexList.Add(leftBtm);

                mCachedVertexList.Add(leftBtm);
                mCachedVertexList.Add(rightTop);
                mCachedVertexList.Add(rightBtm);

                mDirtyMark = false;
                bChanged = true;
            }
            else
            {
                bChanged = false;
            }

            vertexList.AddRange(mCachedVertexList);
        }


        public Vector2 GetAnchorPosition(EAnchorPosition anchorType)
        {
            switch (anchorType)
            {

            }

            return Vector2.Zero;
        }
        

        public List<UIVertex> GetVertexList()
        {
            return mCachedVertexList;
        }

        public Vector2 RightTop
        {
            get { return new Vector2(mLeftTop.X + mWidth, mLeftTop.Y); }
        }

        public Vector2 LeftBottom
        {
            get { return new Vector2(mLeftTop.X, mLeftTop.Y + mHeight);}
        }

        public Vector2 RightBottom
        {
            get { return new Vector2(mLeftTop.X + mWidth, mLeftTop.Y+ mHeight);}
        }
        

        protected UIBase mParent = null;

        protected List<UIBase> mChildList = new List<UIBase>();

        public Vector2 LeftTop => mLeftTop;

        protected Vector2 mLeftTop = Vector2.Zero;

        protected Vector2 mRightBottom;

        protected Vector2 mAnchor;

        protected EAnchorPosition mAnchorPos = EAnchorPosition.LeftTop;

        protected int mWidth;

        protected int mHeight;

        protected int mZOrder=0;

        protected float mScale = 1;

        protected bool mDirtyMark = true;

        protected List<UIVertex> mCachedVertexList = new List<UIVertex>();

    }

    public class UIText : UIBase
    {
        public string Text = string.Empty;

        public UIText(byte[] fontData)
        {
            mFontData = fontData;
        }

        private byte[] mFontData = null;
    }

    public class UIBox : UIBase
    {
        public UIBox(Vector2 leftTop, int width, int height)
        {
            mLeftTop = leftTop;
            mWidth = width;
            mHeight = height;
        }
    }

    // top most ui 
    public class UIScreen : UIBase, IResizable
    {
        public int Width = 640;
        public int Height = 360;

        protected List<UIVertex> mVertexList = new List<UIVertex>();

        protected DrawableBase<UIVertex> mDrawable;

        protected Texture2D mTexture;

        public UIScreen()
        {
            ResizableManager.Get().AddResizable(this);
        }

        public void AddChild(UIBase child)
        {
            mChildList.Add(child);
        }

        public void OnResize(int newWidth, int newHeight)
        {
            Width = newWidth;
            Height = newHeight;
        }

        private void BuildVertexList()
        {
            bool bNeedVertexBufferUpdate = false;
            foreach (var item in mChildList)
            {
                bool bChanged = false;
                item.BuildVertexList(mVertexList, out bChanged);
                if (bChanged)
                {
                    bNeedVertexBufferUpdate |= bChanged;
                }
            }

            if (bNeedVertexBufferUpdate)
            {
                PrepareRenderingData();
            }
        }

        protected void PrepareRenderingData()
        {
            if (mDrawable == null)
            {
                mDrawable = new DrawableBase<UIVertex>();
            }

            if (mTexture == null)
            {
                mTexture = new Texture2D();
                mTexture.LoadFromMemory(UIManager.Instance.FontData, 128, 128, PixelInternalFormat.Rgba, PixelFormat.Red);
            }

            var vertices = mVertexList.ToArray();

            mDrawable.SetupVertexData(ref vertices);
        }

        //
        public void Render()
        {
            // build vertex list
            BuildVertexList();

            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.AlphaTest);
            GL.Disable(EnableCap.DepthTest);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            
            var mtl = ShaderManager.Instance.GetMaterial<FontRenderMaterial>();
            mtl.Bind();
            mtl.ScreenSize = new Vector2(Width, Height);

            mtl.FontTexture2D = mTexture;
            
            mDrawable.DrawArrays(PrimitiveType.Triangles);
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.AlphaTest);
            GL.Enable(EnableCap.DepthTest);
            mtl.Unbind();
        }

        // Screen coordinate to shader position
        Vector2 ToShaderPosition(Vector2 position)
        {
            // 0 ----------------------- (Width,0)
            // | 
            // |
            // |
            // (0,Height)--------------- (Width,Height)

            // (-1,-1) ------------ (1,-1)
            // | 
            // |
            // |
            // (-1, 1) ------------ (1, 1)

            float fX = ((position.X - Width * 0.5f) * 2) / (float)Width;
            float fY = ((position.Y - Height * 0.5f) * 2) / (float) Height;

            return new Vector2(fX, fY);
        }
    }


}
