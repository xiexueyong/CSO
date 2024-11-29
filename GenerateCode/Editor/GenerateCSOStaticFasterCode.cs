#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CSOEngine.Component;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using UnityEditor;
using UnityEngine;

namespace CSOEngine.GenerateCode.Editor
{
    public static class GenerateCSOStaticFasterCode
    {
        
        //生成自定义的Faster，转为静态类生成
        public static void GenerateStaticFasterCode()
        {
            // 获取所有CSOFasterAttribute
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes()
                    .Where(t => t.IsDefined(typeof(CSOStaticFasterAttribute), false))
                    .ToArray();
                foreach (var classType in types)
                {
                    var classAttributes = CustomAttributeExtensions
                        .GetCustomAttributes<CSOStaticFasterAttribute>(classType).ToList();
                    foreach (var attribute in classAttributes)
                    {
                        ProcessOne(classType.Name,attribute.Components);
                    }
                }
            }

        }
        private static void ProcessOne(string className, Type[] components)
        {
            string directoryPath = "Assets/Scripts/App/APPCSO/CSOGenerated";
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string filePath = $"{directoryPath}/{className}Extend.cs";
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("// This file is auto-generated. Do not modify it manually.");
                writer.WriteLine("");
                // writer.WriteLine("using _cso.Object;");

                GenerateUsing(writer, components); //生成using

                // writer.WriteLine("namespace CSOEngine.CSOGenerated"); //namespace start
                // writer.WriteLine("{");
                GenerateExtendClass(writer, className,components, ""); //生成Extend class
                // writer.WriteLine("}"); //namespace end
            }
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
        private static void GenerateExtendClass(StreamWriter writer, string className,Type[] components, string indent)
        {
            writer.WriteLine($"{indent}public static partial class {className}");
            writer.WriteLine($"{indent}{{");

            //生成get comp方法：
            if (components != null && components.Length > 0)
            {
                //生成 get comp 、 ability
                GenerateGetCompMethod(writer, components);
                GenerateGetAbilityMethod(writer, components);
            }
            
            
            
            writer.WriteLine($"{indent}}}");
        }
        
        private static void GenerateGetCompMethod(StreamWriter writer, Type[] components)
        {
            writer.WriteLine();
            foreach (var componentType in components)
            {
                if (!componentType.IsSubclassOf(typeof(CSOComponent)))
                {
                    string name = componentType.Name.Replace("Component", "");
                    name = name[0].ToString().ToUpper() + name.Substring(1);
                    name = name.Trim();
                    string getMethodStr =
                        $"    public static {componentType.Name} {name} => _cso.GetComp<{componentType.Name}>();";
                    writer.WriteLine(getMethodStr);
                }
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
                    name = name[0].ToString().ToUpper() + name.Substring(1);
                    name = name.Trim();
                    string getMethodStr =
                        $"    public static {componentType.Name} {name} => _cso.GetAbility<{componentType.Name}>();";
                    writer.WriteLine(getMethodStr);
                }
            }
        }
    }
}
#endif