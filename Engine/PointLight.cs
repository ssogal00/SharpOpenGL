using Core.CustomAttribute;
using OpenTK.Mathematics;

namespace Engine.Light
{
    public class PointLight : LightBase
    {
        private static int PointLightCount = 0;
        public PointLight()
        : base("PointLight", PointLightCount++)
        {
            wireframesphere = new WireFrameSphere(3, 5, 5);
        }

        protected override void PrepareRenderingData()
        {
        }

        public override void Render()
        {
            if (wireframesphere != null)
            {
                wireframesphere.Translation = this.Translation;
                wireframesphere.Render();
            }
        }

        private WireFrameSphere wireframesphere = null;


        public override bool IsEditable { get; set; } = false;

        [ExposeUI]
        public Vector2 MinMaxRadius { get; set; } = new Vector2(1, 500);
    }
}
