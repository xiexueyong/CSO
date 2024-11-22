using System;

namespace CSOEngine.GenerateCode
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false,AllowMultiple = true)]
    public class CSOFasterAttribute : Attribute
    {
        public string Name { get; private set; }
        private readonly Type[] components;

        public CSOFasterAttribute(string objName,params Type[] components)
        {
            Name = objName;
            this.components = components;
        }

        public Type[] Components
        {
            get { return components; }
        }
    }
}