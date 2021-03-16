using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Core.Buffer;
using Core.Primitive;
using Vector2 = OpenTK.Mathematics.Vector2;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SharpOpenGL;

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
        
        [FieldOffset(0)]
        public Vector2 Position; // 8byte
        [FieldOffset(8)]
        public Vector2 Texcoord;
        [FieldOffset(16)]
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

        public virtual void BuildVertexList(List<UIVertex> vertexList)
        {
            if (mDirtyMark)
            {
                mCachedVertexList.Clear();
                Vector2 anchoredPos = GetAnchorPosition(mAnchorPos);

                var leftTop = new UIVertex(anchoredPos + mLeftTop, Vector2.Zero);
                var rightTop = new UIVertex(anchoredPos + RightTop, new Vector2(1, 0));
                var leftBtm = new UIVertex(anchoredPos + LeftBottom, new Vector2(0, 1));
                var rightBtm = new UIVertex(anchoredPos + mRightBottom, new Vector2(1, 1));

                mCachedVertexList.Add(leftTop);
                mCachedVertexList.Add(rightTop);
                mCachedVertexList.Add(leftBtm);

                mCachedVertexList.Add(leftBtm);
                mCachedVertexList.Add(rightTop);
                mCachedVertexList.Add(rightBtm);

                mDirtyMark = false;
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
    public class UIScreen : UIBase
    {
        public int Width = 1920;
        public int Height = 1080;

        protected List<UIBase> mChildList=new List<UIBase>();

        protected List<UIVertex> mVertexList = new List<UIVertex>();

        protected AOSVertexBuffer<UIVertex> mVertexBuffer;


        public void BuildVertexList()
        {
            foreach (var item in mChildList)
            {
                item.BuildVertexList(mVertexList);
            }
        }

        protected void PrepareRenderingData()
        {
            if (mVertexBuffer == null)
            {
                mVertexBuffer = new AOSVertexBuffer<UIVertex>();
            }

            var vertices = mVertexList.ToArray();

            mVertexBuffer.BufferData(ref vertices);
        }

        public void Render()
        {
            mVertexBuffer.Bind();

            

            mVertexBuffer.Unbind();
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
