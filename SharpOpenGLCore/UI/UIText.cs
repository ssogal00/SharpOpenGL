using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using OpenTK.Mathematics;

namespace SharpOpenGLCore.UI
{
    public class UIText : UIBase
    {
        public UIText(string contents)
        {
            mText = contents;
        }

        public override void BuildVertexList(List<UIVertex> vertexList, out bool bChanged)
        {
            var atlas = UIManager.Instance.FontAtlas;

            bChanged = true;

            Vector2i penPosition = new Vector2i(mOrigin.X, mOrigin.Y);

            foreach (var c in mText)
            {
                if (atlas.GlyphMap.ContainsKey(c))
                {
                    var glyph = atlas.GlyphMap[c];
                    
                    var leftTop = new UIVertex();
                    var rightTop = new UIVertex();
                    var leftBtm = new UIVertex();
                    var rightBtm = new UIVertex();
                    
                    leftTop.Position.X = penPosition.X + glyph.BitmapLeft;
                    leftTop.Position.Y = penPosition.Y - glyph.BitmapTop;
                    leftTop.Texcoord.X = glyph.TexCoordLeftX;
                    leftTop.Texcoord.Y = glyph.TexCoordTopY;

                    rightTop.Position.X = penPosition.X + glyph.BitmapLeft + glyph.BitmapWidth;
                    rightTop.Position.Y = penPosition.Y - glyph.BitmapTop;
                    rightTop.Texcoord.X = glyph.TexCoordRightX;
                    rightTop.Texcoord.Y = glyph.TexCoordTopY;

                    leftBtm.Position.X = penPosition.X + glyph.BitmapLeft;
                    leftBtm.Position.Y = penPosition.Y - glyph.BitmapTop + glyph.BitmapHeight;
                    leftBtm.Texcoord.X = glyph.TexCoordLeftX;
                    leftBtm.Texcoord.Y = glyph.TexCoordBottomY;

                    rightBtm.Position.X = penPosition.X + glyph.BitmapLeft + glyph.BitmapWidth;
                    rightBtm.Position.Y = penPosition.Y - glyph.BitmapTop + glyph.BitmapHeight;
                    rightBtm.Texcoord.X = glyph.TexCoordRightX;
                    rightBtm.Texcoord.Y = glyph.TexCoordBottomY;

                    vertexList.Add(leftTop);
                    vertexList.Add(rightTop);
                    vertexList.Add(leftBtm);

                    vertexList.Add(leftBtm);
                    vertexList.Add(rightTop);
                    vertexList.Add(rightBtm);
                }
                else
                {
                    Debug.Assert(false);
                }
            }
        }

        protected Vector2i mOrigin;

        protected string mText;
    }
}
