using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Texture;

namespace SharpOpenGL.PostProcess
{
    

    public class GBufferVisualize : PostProcessBase
    {
        public GBufferVisualize()
            : base()
        {
        }

        protected enum EVisualizeMode
        {
            EColor,
            ENormal,
            EPosition,
            EMetalic,
            ERoughness,
            EVisualizeModeMax,
        }

        public void ChangeVisualizeMode()
        {
            switch (eVisualizeMode)
            {
                case EVisualizeMode.EColor:
                    eVisualizeMode = EVisualizeMode.ENormal;
                    break;
                case EVisualizeMode.ENormal:
                    eVisualizeMode = EVisualizeMode.EPosition;
                    break;
                case EVisualizeMode.EPosition:
                    eVisualizeMode = EVisualizeMode.EMetalic;
                    break;
                case EVisualizeMode.EMetalic:
                    eVisualizeMode = EVisualizeMode.ERoughness;
                    break;
                case EVisualizeMode.ERoughness:
                    eVisualizeMode = EVisualizeMode.EColor;
                    break;
            }
        }

        public override void OnGLContextCreated(object sender, EventArgs e)
        {
            base.OnGLContextCreated(sender, e);
            PostProcessMaterial = ShaderManager.Get().GetMaterial<GBufferDump.GBufferDump>();
        }

        public override void Render(TextureBase ColorInput, TextureBase NormalInput, TextureBase PositionInput)
        {
            Output.ClearColor = Color.Violet;

            var gbufferVisualize = (GBufferDump.GBufferDump) PostProcessMaterial;

            Output.BindAndExecute(gbufferVisualize, () =>
            {
                gbufferVisualize.DiffuseTex2D = ColorInput;
                gbufferVisualize.NormalTex2D = NormalInput;
                gbufferVisualize.PositionTex2D = PositionInput;

                gbufferVisualize.Dump_DiffuseDump = eVisualizeMode == EVisualizeMode.EColor ? 1 : 0;
                gbufferVisualize.Dump_NormalDump = eVisualizeMode == EVisualizeMode.ENormal ? 1 : 0;
                gbufferVisualize.Dump_PositionDump = eVisualizeMode == EVisualizeMode.EPosition ? 1 : 0;
                gbufferVisualize.Dump_MetalicDump = eVisualizeMode == EVisualizeMode.EMetalic ? 1 : 0;
                gbufferVisualize.Dump_RoughnessDump = eVisualizeMode == EVisualizeMode.ERoughness ? 1 : 0;

                BlitToScreenSpace();
            });
        }

        protected EVisualizeMode eVisualizeMode = EVisualizeMode.EColor;

    }
}
