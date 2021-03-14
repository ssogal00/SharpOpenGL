using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;

namespace SharpOpenGLCore
{
    public class UIVertex
    {
        public UIVertex(Vector2 position)
        {
            Position = position;
        }
        public Vector2 Position;
        public Vector2 Texcoord;
        public float Alpha;
    }

    public class UIBase
    {
        public virtual void Draw()
        {

        }

        protected virtual void BuildVertexList()
        {
            mCachedVertexList.Clear();
            
            var leftTop = new UIVertex(mLeftTop);
            var rifhtTop = new UIVertex(RightTop);

            
        }

        public Vector2 RightTop
        {
            get
            {
                return new Vector2(mLeftTop.X+mWidth, mLeftTop.Y);
            }
        }

        public Vector2 LeftTop => mLeftTop;

        protected Vector2 mLeftTop = Vector2.Zero;

        protected Vector2 mRightBottm;

        protected Vector2 mAnchor;

        protected int mWidth;

        protected int mHeight;

        protected int mZOrder;

        protected float mScale = 1;

        protected List<UIVertex> mCachedVertexList = new List<UIVertex>();
    }

    public class UIText : UIBase
    {
        public string Text = string.Empty;
    }

    // 
    public class Screen2D
    {

    }
}
