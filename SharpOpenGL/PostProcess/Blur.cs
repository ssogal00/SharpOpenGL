using Core;
using Core.CustomEvent;
using Core.MaterialBase;

using Core.Texture;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using MathHelper = Core.MathHelper;

namespace SharpOpenGL.PostProcess
{
    public class BlurPostProcess : SharpOpenGL.PostProcess.PostProcessBase, IResizable
    {
        public BlurPostProcess()
            : base()
        {
            ResizableManager.Get().AddResizable(this);

        }

        public void OnResize(object sender, ScreenResizeEventArgs e)
        {
        }

        public override void OnGLContextCreated(object sender, EventArgs e)
        {
            base.OnGLContextCreated(sender, e);

            PostProcessMaterial = ShaderManager.Get().GetMaterial<Blur.Blur>();
        }

        public override void Render(TextureBase input)
        {
            Output.BindAndExecute(PostProcessMaterial, () =>
            {
                var blurMaterial = (Blur.Blur) (PostProcessMaterial);

                blurMaterial.Horizontal = false;
                blurMaterial.ColorTex2D = input;
                BlitToScreenSpace();

                blurMaterial.Horizontal = true;
                blurMaterial.ColorTex2D = input;
                BlitToScreenSpace();
            });
        }

        protected List<OpenTK.Vector2> offset = new List<Vector2>();
        protected List<OpenTK.Vector2> weight = new List<Vector2>();
    }
}
