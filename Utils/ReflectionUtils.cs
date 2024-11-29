using System;
using System.Reflection;

namespace CSOEngine.Utils {
    public static class ReflectionUtils
    {
        public static void ResetFields(object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
        
            Type type = obj.GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (var field in fields)
            {
                Type fieldType = field.FieldType;
                object defaultValue = fieldType.IsValueType ? Activator.CreateInstance(fieldType) : null;
                field.SetValue(obj, defaultValue);
            }
            var constructor = type.GetConstructor(Type.EmptyTypes);
            if (constructor != null){
                constructor.Invoke(obj, null);
            }
        }
    }
}