using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited= true)]
    public class ExposeUI : System.Attribute
    {
        public ExposeUI(string description="")
        {
            this.description = description;
        }

        public string Description => description;

        private string description = "";
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
    public class UseSlider : System.Attribute
    {
        public UseSlider(float min, float max)
        {
            this.Min = min;
            this.Max = max;
        }

        public UseSlider()
        {
        }

        public float Min { get; set; } = float.MinValue;
        public float Max { get; set; } = float.MaxValue;
    }


    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
    public class ReadOnly : System.Attribute
    {
        public ReadOnly()
        {
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
    public class UIGroup : System.Attribute
    {
        private string groupName = "";

        public string GroupName => groupName;

        public UIGroup(string givenGroupName)
        {
            groupName = givenGroupName;
        }
    }
}
