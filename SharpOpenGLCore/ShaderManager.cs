using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Core;
using Core.MaterialBase;
using GLTF.V2;
using SharpOpenGLCore;

namespace SharpOpenGL
{
    public class ShaderManager : Singleton<ShaderManager>
    {
        public void CompileShaders()
        {
            if (File.Exists("./Resources/Shader/MaterialList.xml") == false)
            {
                return;
            }

            if (Directory.Exists("./Resources/Imported/Shader") == false)
            {
                Directory.CreateDirectory("./Resources/Imported/Shader");
            }

            var compiledShaderAssembly = Assembly.Load("CompiledShaderCore");
            var types = compiledShaderAssembly.GetTypes();

            foreach (var t in types)
            {
                if (t.IsSubclassOf(typeof(MaterialBase)))
                {
                    var instance = (MaterialBase)Activator.CreateInstance(t);
                    ShaderMap.Add(t.Name, instance);
                }
            }
        }

        public void PreCompileShaders()
        {
            Debug.Assert(RenderingThread.IsInRenderingThread());

            if(File.Exists("./Resources/Shader/ShaderDefines.json")== false)
            {
                return;
            }

            var json = File.ReadAllText("./Resources/Shader/ShaderDefines.json");
            var result = JsonSerializer.Deserialize<ShaderListToCompile>(json);

            if (result != null)
            {
                foreach(var item in result.ShaderList)
                {
                    var vsCode = File.ReadAllText(item.VertexShaderPath);
                    var fsCode = File.ReadAllText(item.FragmentShaderPath);

                }
            }
        }

        public MaterialBase GetMaterial(string name)
        {
            if (ShaderMap.ContainsKey(name))
            {
                return ShaderMap[name];
            }

            return null;
        }

        public T GetMaterial<T>() where T : MaterialBase
        {
            foreach (var item in ShaderMap)
            {
                if (item.Value is T)
                {
                    return (T) item.Value;
                }
            }

            return default(T);
        }

        private Dictionary<string, MaterialBase> ShaderMap = new Dictionary<string, MaterialBase>();
    }
}
