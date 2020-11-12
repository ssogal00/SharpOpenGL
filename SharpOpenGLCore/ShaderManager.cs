using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.MaterialBase;

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
