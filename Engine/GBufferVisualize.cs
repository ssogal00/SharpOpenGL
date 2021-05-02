using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompiledMaterial.GBufferDump;
using Core;
using Core.Texture;

namespace Engine.PostProcess
{
    public class GBufferVisualize : PostProcessBase
    {
        public GBufferVisualize()
            : base()
        {
            Initialize();
        }

        public enum EVisualizeMode
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
                    eVisualizeMode = EVisualizeMode.EColor;
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

        public override void Render(TextureBase ColorInput, TextureBase NormalInput, TextureBase PositionInput, TextureBase MotionInput)
        {
            Output.ClearColor = Color.Violet;

            PostProcessMaterial = ShaderManager.Instance.GetMaterial<GBufferDump>();

            Output.BindAndExecute(PostProcessMaterial, () =>
            {
                PostProcessMaterial.SetTexture("DiffuseTex",ColorInput);
                PostProcessMaterial.SetTexture("NormalTex", NormalInput);
                PostProcessMaterial.SetTexture("PositionTex", PositionInput);
                PostProcessMaterial.SetTexture("MotionBlurTex", MotionInput);

                PostProcessMaterial.SetUniformVariable("DiffuseDump", eVisualizeMode == EVisualizeMode.EColor);
                PostProcessMaterial.SetUniformVariable("NormalDump", eVisualizeMode == EVisualizeMode.ENormal);
                PostProcessMaterial.SetUniformVariable("PositionDump", eVisualizeMode == EVisualizeMode.EPosition);
                PostProcessMaterial.SetUniformVariable("MetalicDump", eVisualizeMode == EVisualizeMode.EMetalic);
                PostProcessMaterial.SetUniformVariable("RoughnessDump", eVisualizeMode == EVisualizeMode.ERoughness);
                PostProcessMaterial.SetUniformVariable("MotionBlurDump", eVisualizeMode == EVisualizeMode.EMotionBlur);
                
                BlitToScreenSpace();
            });
        }

        public EVisualizeMode CurrentMode
        {
            get => eVisualizeMode;
            set => eVisualizeMode = value;
        }

        protected EVisualizeMode eVisualizeMode = EVisualizeMode.EPosition;

    }
}
