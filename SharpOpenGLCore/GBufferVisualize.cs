using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompiledMaterial.GBufferDump;
using Core;
using Core.Texture;

namespace SharpOpenGL.PostProcess
{
    public class GBufferVisualize : PostProcessBase
    {
        public GBufferVisualize()
            : base()
        {
            Initialize();
        }

        protected enum EVisualizeMode
        {
            EColor,
            ENormal,
            EPosition,
            EMetalic,
            ERoughness,
            EMotionBlur,
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
                    eVisualizeMode = EVisualizeMode.EMotionBlur;
                    break;
                case EVisualizeMode.EMotionBlur:
                    eVisualizeMode = EVisualizeMode.EColor;
                    break;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            PostProcessMaterial = ShaderManager.Get().GetMaterial<GBufferDump>();
        }

        public override void Render(TextureBase ColorInput, TextureBase NormalInput, TextureBase PositionInput, TextureBase MotionInput)
        {
            Output.ClearColor = Color.Violet;

            var gbufferVisualize = (GBufferDump) PostProcessMaterial;

            Output.BindAndExecute(gbufferVisualize, () =>
            {
                gbufferVisualize.DiffuseTex2D = ColorInput;
                gbufferVisualize.NormalTex2D = NormalInput;
                gbufferVisualize.PositionTex2D = PositionInput;
                gbufferVisualize.MotionBlurTex2D = MotionInput;

                gbufferVisualize.Dump_DiffuseDump = eVisualizeMode == EVisualizeMode.EColor;
                gbufferVisualize.Dump_NormalDump = eVisualizeMode == EVisualizeMode.ENormal;
                gbufferVisualize.Dump_PositionDump = eVisualizeMode == EVisualizeMode.EPosition;
                gbufferVisualize.Dump_MetalicDump = eVisualizeMode == EVisualizeMode.EMetalic;
                gbufferVisualize.Dump_RoughnessDump = eVisualizeMode == EVisualizeMode.ERoughness;
                gbufferVisualize.Dump_MotionBlurDump = eVisualizeMode == EVisualizeMode.EMotionBlur;

                BlitToScreenSpace();
            });
        }

        protected EVisualizeMode eVisualizeMode = EVisualizeMode.EColor;

    }
}
