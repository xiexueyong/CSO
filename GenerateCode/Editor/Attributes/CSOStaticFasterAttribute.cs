using System;

namespace CSOEngine.GenerateCode
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false,AllowMultiple = true)]
    public class CSOStaticFasterAttribute : Attribute
    {
        private readonly Type[] components;

        public CSOStaticFasterAttribute(params Type[] components)
        {
            this.components = components;
        }

        public Type[] Components
        {
            get { return components; }
        }
    }
}