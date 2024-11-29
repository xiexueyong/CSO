#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using CSOEngine.Component;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using UnityEditor;
using Debug = UnityEngine.Debug;

namespace CSOEngine.GenerateCode.Editor
{
    public static class GenerateCSOFasterViewCode
    {
        
        //生成自定义的Faster，可以获取指定的component 和 ability
        public static void GenerateCustomFasterViewCode()
        {
            // 获取所有CSOFasterAttribute
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes()
                    .Where(t => t.IsDefined(typeof(CSOFasterAttribute), false))
                    .ToArray();
                foreach (var classType in types)
                {
                    var classAttributes = CustomAttributeExtensions
                        .GetCustomAttributes<CSOFasterAttribute>(classType).ToList();
                    foreach (var attribute in classAttributes)
                    {
                        ProcessOne(attribute);
                    }
                }
            }
            //AssetDatabase.Refresh(); 
        }

        //生成通用的Faster，可以获取所有的component 和 ability，缺点是点出的提示太多。推荐使用custom faster
        public static void GenerateCommonFasterViewCode()
        {
            List<Type> components = new List<Type>();
            // 获取所有CSOFasterAttribute
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(CSOViewComponent)))
                    .Where(t => !t.IsAbstract)
                    .ToArray();
                components.AddRange(types);
            }

            ProcessOne("Common", components.ToArray());
        }

        private static void ProcessOne(CSOFasterAttribute classAttribute)
        {
            List<Type> types = new List<Type>();
            foreach (var component in classAttribute.Components)
            {
                if (component.IsSubclassOf(typeof(CSOViewComponent)))
                {
                    types.Add(component);
                }
            }
            if(types.Count>0)
                ProcessOne(classAttribute.Name, types.ToArray());
        }

        public static string GetFastExtendFilePath(string attributeName) {
            string directoryPath = "Assets/Scripts/App/APPCSO/CSOGenerated";
            if (!Directory.Exists(directoryPath)) {
                Directory.CreateDirectory(directoryPath);
            }

            return  $"{directoryPath}/{attributeName}FastViewExtend.cs";
        }

        private static string HashMarker = GenerateCSOFasterCode.HashMarker;
        public static void ProcessOne(string attributeName, Type[] components,string attributeHash = null) {
            string filePath = GetFastExtendFilePath(attributeName);
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("// This file is auto-generated. Do not modify it manually.");
                writer.WriteLine($"{HashMarker} {attributeHash}");
                writer.WriteLine("");
                
                writer.WriteLine("using CSOEngine.Component;");
                writer.WriteLine("using CSOEngine.Object;");
                writer.WriteLine("using CSOEngine.Proxy;");

                GenerateUsing(writer, components); //生成using

                writer.WriteLine("namespace CSOEngine.CSOGenerated"); //namespace start
                writer.WriteLine("{");
                GenerateExtendClass(writer, attributeName, "    "); //生成Extend class
                GenerateFasterClass(writer, attributeName, components, "    "); //生成Faster class
                writer.WriteLine("}"); //namespace end
            }
        }

        private static void GenerateFasterClass(StreamWriter writer, string attributeName, Type[] components,
            string indent)
        {
            string className = $"CSObjectFaster{attributeName}View";
            writer.WriteLine($"{indent}public sealed class {className}:CSOFasterViewBase");
            writer.WriteLine($"{indent}{{");

            // writer.WriteLine($"{indent}{indent}public {className}(CObject cso) : base(cso)");
            // writer.WriteLine($"{indent}{indent}{{");
            // writer.WriteLine($"{indent}{indent}}}");
            //生成get comp方法：
            if (components != null && components.Length > 0)
            {
                //生成 get comp 、 ability
                GenerateGetViewMethod(writer, components);
                // GenerateGetAbilityMethod(writer, components);
            }

            writer.WriteLine($"{indent}}}");
        }

        private static void GenerateGetViewMethod(StreamWriter writer, Type[] components)
        {
            writer.WriteLine();
            foreach (var componentType in components)
            {
                string name = componentType.Name.Replace("Component", "");
                name = name.Replace("Ability", "");
                name = name[0].ToString().ToUpper() + name.Substring(1);
                name = name.Trim();
                string getMethodStr =
                    $"        public {componentType.Name} {name} => GetView<{componentType.Name}>();";
                writer.WriteLine(getMethodStr);
            }
        }

        private static void GenerateGetAbilityMethod(StreamWriter writer, Type[] components)
        {
            writer.WriteLine();
            foreach (var componentType in components)
            {
                if (componentType.IsSubclassOf(typeof(CSOComponent)))
                {
                    //ability
                    string name = componentType.Name.Replace("Component", "");
                    string getMethodStr =
                        $"        public {componentType.Name} {name} => cso.GetAbility<{componentType.Name}>();";
                    writer.WriteLine(getMethodStr);
                }
            }
        }


        private static void GenerateExtendClass(StreamWriter writer, string attributeName, string indent)
        {
            string className = $"CSObjectFaster{attributeName}ViewExtend";
            string fasterName = $"CSObjectFaster{attributeName}View";
            writer.WriteLine($"{indent}public static class {className}");
            writer.WriteLine($"{indent}{{");
            writer.WriteLine($"{indent}{indent}private static {fasterName} faster;");
            
            //generate ICobject Extent
            string fastMethodName = attributeName == "Common" ? "" : attributeName;
            writer.WriteLine($"{indent}{indent}public static {fasterName} Fast{fastMethodName}(this CSOViewComponent view,ICObject iCObject)");
            writer.WriteLine($"{indent}{indent}{{");
            writer.WriteLine($"{indent}{indent}{indent}if (faster == null){{");
            writer.WriteLine($"{indent}{indent}{indent}{indent} faster = new {fasterName}();");
            writer.WriteLine($"{indent}{indent}{indent}}}");
            //===========
            writer.WriteLine($"{indent}{indent}if (iCObject is CSOProxy proxy)");
            writer.WriteLine($"{indent}{indent}{indent}faster.Proxy = proxy;");
            writer.WriteLine($"{indent}{indent}if (iCObject is CObject cso)");
            writer.WriteLine($"{indent}{indent}{indent}faster.cso = cso;");
            //===========
            writer.WriteLine($"{indent}{indent}{indent}return faster;");
            writer.WriteLine($"{indent}{indent}}}");
            
     
            
            
            //generate Default Extent
            writer.WriteLine($"{indent}{indent}public static {fasterName} Fast{fastMethodName}(this CSOViewComponent view)");
            writer.WriteLine($"{indent}{indent}{{");
            writer.WriteLine($"{indent}{indent}{indent}if (faster == null){{");
            writer.WriteLine($"{indent}{indent}{indent}{indent} faster = new {fasterName}();");
            writer.WriteLine($"{indent}{indent}{indent}}}");
            writer.WriteLine($"{indent}{indent}{indent}faster.ViewComponent = view;");
            writer.WriteLine($"{indent}{indent}{indent}return faster;");
            writer.WriteLine($"{indent}{indent}}}");
            
            //end==================
            
            
            writer.WriteLine($"{indent}}}");
        }

        private static void GenerateUsing(StreamWriter writer, Type[] components)
        {
            HashSet<string> ss = new HashSet<string>();
            foreach (var component in components)
            {
                string mamespace = component.Namespace;
                ss.Add(mamespace);
            }

            foreach (var s in ss)
            {
                writer.WriteLine($"using {s};");
            }
        }
    }
}
#endif