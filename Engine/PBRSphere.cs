using OpenTK.Mathematics;
using System.Collections.Generic;

namespace Engine
{
    public class PBRSphere : Sphere
    {
        public PBRSphere()
           : base()
        {
            this.instanceCount = 36;
            this.Translation = new Vector3(-100, 0, 0);
            this.Color = new Vector3(1,1,1);
            this.Scale = 2.0f;
            this.IsEditable = true;
            this.MaterialName = "GBufferDraw";
        }

        public PBRSphere(string diffuseTex, string normalTex, string roghnessTex, string metallicTex)
        : this()
        {
            this.diffuseTexPath = diffuseTex;
            this.normalTexPath = normalTex;
            this.roughnessTexPath = roghnessTex;
            this.metallicTexPath = metallicTex;
            this.MaterialName = "GBufferDraw";
        }

        private Matrix4 PrevModel = Matrix4.Identity;

        public override void Tick(double elapsed)
        {
        }

        
        public override void Initialize()
        {
            GenerateVertices();
        }

        public void SetNormalTex(string normalTex)
        {
            bNormalExist = true;
            this.normalTexPath = normalTex;
        }

        public void SetDiffuseTex(string diffuseTex)
        {
            bDiffuseExist = true;
            this.diffuseTexPath = diffuseTex;
        }

        public void SetRoughnessTex(string roughnessTex)
        {
            bRoughnessExist = true;
            this.roughnessTexPath = roughnessTex;
        }


        public void SetMetallicTex(string metallicTex)
        {
            bMetallicExist = true;
            this.metallicTexPath = metallicTex;
        }
        
        public override IEnumerable<(string, Matrix4)> GetMatrix4Params(int index)
        {
            yield return ("View", CameraManager.Instance.CurrentCameraView);
            yield return ("Proj", CameraManager.Instance.CurrentCameraProj);
            yield return ("Model", this.LocalMatrix);
        }

        public override IEnumerable<(string, string)> GetTextureParams(int index)
        {
            if (bMetallicExist)
            {
                yield return ("MetallicTex", metallicTexPath);
            }

            if (bNormalExist)
            {
                yield return ("NormalTex", normalTexPath);
            }
            
            if (bRoughnessExist)
            {
                yield return ("RoughnessTex", roughnessTexPath);
            }

            if (bDiffuseExist)
            {
                yield return ("DiffuseTex", diffuseTexPath);
            }
        }

        

        public override IEnumerable<(string, bool)> GetBoolParams(int index)
        {
            yield return ("MetallicExist", bMetallicExist);
            yield return ("NormalMapExist", bNormalExist);
            yield return ("RoughnessExist", bRoughnessExist);
            yield return ("DiffuseMapExist", bDiffuseExist);
        }

        public override IEnumerable<(string, int)> GetIntParams(int index)
        {
            yield return ("LightChannel", (int) Light.LightChannel.StaticMeshChannel);
        }

        protected int instanceCount = 1;

        private bool bRoughnessExist = false;
        private bool bMetallicExist = false;
        private bool bNormalExist = false;
        private bool bDiffuseExist = false;

        private string normalTexPath = null;
        private string roughnessTexPath = null;
        private string metallicTexPath = null;
        private string diffuseTexPath = null;
    }
}
