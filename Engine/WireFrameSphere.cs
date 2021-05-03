using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompiledMaterial.GBufferPNC;
using Core.Primitive;
using OpenTK;
using Core;
using Core.MaterialBase;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using CameraTransform = Core.MaterialBase.CameraTransform;

namespace Engine
{
    public class WireFrameSphere : PBRSphere
    {
        public WireFrameSphere()
        :base()
        {
            this.Scale = 10.0f;
            this.MaterialName = "GeometryWireframeMaterial2";
            this.mMeshSectionList[0].MaterialName= "GeometryWireframeMaterial2";
        }

        public override bool IsEditable { get; set; } = false;

        public WireFrameSphere(float r, int stack, int sector)
        {
            Radius = r;
            StackCount = stack;
            SectorCount = sector;
            Initialize();
        }

        public override IEnumerable<(string, Vector4)> GetVector4Params(int index)
        {
            yield return ("WireframeColor", mColor);
        }

        public override IEnumerable<(string, float)> GetFloatParams(int index)
        {
            yield return ("Width", 2.0f);
        }

        public override IEnumerable<(string, Matrix4)> GetMatrix4Params(int index)
        {
            yield return ("View", CameraManager.Instance.CurrentCameraView);
            yield return ("Proj", CameraManager.Instance.CurrentCameraProj);
            yield return ("Model", this.LocalMatrix);
            yield return ("ViewportMatrix", CameraManager.Instance.CurrentViewport);
        }

        private Vector4 mColor = new Vector4(0,0,0,1);
    }
}
