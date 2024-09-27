using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using OpenTK.Mathematics;

namespace Engine.UI
{
    public class UIText : UIBase
    {
        public UIText(string contents, Vector2i origin)
        {
            mText = contents;
            mOrigin = origin;
        }

        public override void BuildVertexList(List<UIVertex> vertexList, out bool bChanged)
        {
            if (!mDirtyMark)
            {
                bChanged = false;
                return;
            }


            bChanged = true;
            mDirtyMark = false;

            Vector2i penPosition = new Vector2i(mOrigin.X, mOrigin.Y);

            
        }

        protected Vector2i mOrigin;

        protected string mText;
    }
}
