using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.MaterialBase;

namespace Engine.Material
{
    public class MaterialParameter
    {
        public MaterialParameter()
        {
        }

        public void SetFloatParam(string name, float value)
        {
            if (floatParamDictionary.ContainsKey(name))
            {
                floatParamDictionary.Add(name, value);
            }
        }

        public void Apply(MaterialBase material)
        {
            // 
            foreach (var each in floatParamDictionary)
            {
                material.SetUniformVarData(each.Key, each.Value);
            }
        }

        private Dictionary<string, float> floatParamDictionary = new Dictionary<string, float>();
    }

    
}
