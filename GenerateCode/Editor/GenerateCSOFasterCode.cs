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
    public static class GenerateCSOFasterCode
    {
        
        //生成自定义的Faster，可以获取指定的component 和 ability
        public static void GenerateCustomFasterCode()
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
        
        public static void GenerateBuilderReferencedTypes() {
            var resultList =ResolveBuilderReferencedTypes();
            var commonTypes = new HashSet<Type>();
            foreach (var item in resultList) {
                ProcessOne(item.Item1,item.Item2);
                foreach (var type in item.Item2) {
                    commonTypes.Add(type);
                }
            }
            ProcessOne("Common", commonTypes.ToArray());
            AssetDatabase.Refresh();
        }

        private const  string AppDllName = "App";
        private const  string CsoDllName = "CObject";
        private static Dictionary<string, Assembly> _assemblyDic;
        public static List<(string,Type[])> ResolveBuilderReferencedTypes() {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            _assemblyDic = new Dictionary<string, Assembly>();
            foreach (Assembly assembly in assemblies)
            {
                var assemblyName = assembly.GetName().Name;
                if (assemblyName.Equals(AppDllName)||
                    assemblyName.Equals(CsoDllName)) {
                    _assemblyDic.Add(assemblyName,assembly);
                }
            }

            Type builderInterface = _assemblyDic[CsoDllName].GetType("CSOEngine.Group.ICSOBuilder");
            var onBuilderMethodInfo = builderInterface.GetMethods().First(info => info.Name.Contains("OnBuild"));
            Assembly curAssembly = _assemblyDic[AppDllName];
            var result = new List<(string,Type[])>();
            foreach (var builderType in curAssembly.GetTypes()) {
                if (builderInterface.IsAssignableFrom(builderType)) {
                    var list = GetReferencedTypesInMethod(curAssembly.Location,builderType.FullName,onBuilderMethodInfo.Name);
                    result.Add((builderType.Name.Replace("Builder",""),list.ToArray()));
                    Debug.Log($"ResolveBuilder--{builderType.FullName},\n{string.Join(",\n",list)}");
                }
            }
            _assemblyDic.Clear();
            return result;
        }


        private static HashSet<Type> GetReferencedTypesInMethod(string assemblyPath, string typeName, string methodName)
        {
            var referencedTypes = new HashSet<Type>();
            using var module = ModuleDefMD.Load(assemblyPath);
            var typeDef = module.Find(typeName,isReflectionName: true);
            if (typeDef == null) {
                var matchingTypes = module.Types
                    .Where(type => type.Name.String.Contains(typeName))
                    .ToList();
                typeDef = matchingTypes.Any() ? module.Find(matchingTypes[0].FullName, isReflectionName: true)  : null;
            }

            var methodDef = typeDef.FindMethod(methodName);
            if (methodDef == null) {
                var matchingMethods = typeDef.Methods
                    .Where(type => type.Name.String.Contains(methodName))
                    .ToList();
                methodDef = matchingMethods.Any() ? typeDef.FindMethod(matchingMethods[0].FullName)  : null;
            }
                
            TraverseMethodILRecursively(methodDef,new HashSet<string>(),referencedTypes,typeDef);
            return referencedTypes;
        }

        private static void TraverseMethodILRecursively(MethodDef method, HashSet<string> visitedMethods, HashSet<Type> referencedTypes,TypeDef callingType,int depth = 0) {
            if (method == null || visitedMethods.Contains(method.FullName))
                return;
            if (depth>3) {
                return;
            }
            
            visitedMethods.Add(method.FullName);
            foreach (var instr in method.Body.Instructions) {
                TypeDef currentType = callingType;
                if (instr.OpCode == OpCodes.Call || instr.OpCode == OpCodes.Callvirt) {
                    var targetMethod = instr.Operand as IMethod;
                    if (instr.OpCode == OpCodes.Callvirt) {
                        var seekType = currentType;
                        while (seekType != null)
                        {
                            var overridMethod = seekType.Methods.FirstOrDefault(
                                m => m.Name == targetMethod?.Name && m.MethodSig.ToString().Equals(targetMethod?.MethodSig.ToString())
                                );
                            if (overridMethod != null) {
                                targetMethod = overridMethod;
                                currentType = seekType;
                                break;
                            }
                            seekType = seekType.BaseType?.ResolveTypeDef();
                        }
                    }
                    if (targetMethod is  MethodDef methodDef) {
                        if (methodDef.HasGenericParameters && instr.Operand is MethodSpec memberSpec ) {
                            if (ResolveCompAndAbility(memberSpec,referencedTypes)) {
                                continue;
                            }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
                        }
                        if (methodDef.HasBody) {
                            TraverseMethodILRecursively(methodDef, visitedMethods, referencedTypes,currentType,depth + 1);
                        }
                    }

                    if (targetMethod is  MethodSpec methodSpec) {
                        if (ResolveCompAndAbility(methodSpec,referencedTypes)) {
                            continue;
                        }
                    }
                    
                    if (targetMethod is  MemberRef memberRef) {
                        ResolveCompAndAbility(memberRef,method,instr,referencedTypes);
                    }
                }
            }
        }

        //代码段向前追踪非泛型CSO添加组件的类型
        private static Type TrackAbilityBaseSubclass(IList<Instruction> instructions, int index)
        {
            while (index >= 0)
            {
                var instr = instructions[index];
                if (instr.OpCode == OpCodes.Newobj && instr.Operand is IMethod ctorMethod)
                {
                    var declarType = _assemblyDic[AppDllName].GetType(ctorMethod.DeclaringType.FullName,false);
                    var retType = IsCsoComponentSubClass(declarType);
                    if (retType != null) {
                        return retType;
                    }
                }
                
                if (instr.OpCode == OpCodes.Call || instr.OpCode == OpCodes.Callvirt) {
                    if (instr.Operand is MethodDef methodDef) {
                       var retType = _assemblyDic[AppDllName].GetType(methodDef.MethodSig.RetType.FullName,false);
                       var baseType = _assemblyDic[CsoDllName].GetType(CSOBaseComponentName);
                       if (retType != null &&retType.IsSubclassOf(baseType)) {
                           return retType;
                       }
                    }
                }

                index--;
            }
            return null;
        }

        private static string CSOBaseComponentName = "CSOEngine.Component.CSOBaseComponent";
        private static Type IsCsoComponentSubClass(Type type) {
            var retType = _assemblyDic[AppDllName].GetType(type.FullName,false);
            var baseType = _assemblyDic[CsoDllName].GetType(CSOBaseComponentName);
            if (retType != null &&retType.IsSubclassOf(baseType)) {
                return retType;
            }

            return null;
        }

        private static string AddAbilityFuncName = "AddAbility";
        private static string AddCompFuncName = "AddComp";
        //解析泛型的CSO添加组件类型
        private static bool ResolveCompAndAbility(MethodSpec memberSpec,HashSet<Type> referencedTypes) {
            if (memberSpec.DeclaringType is TypeRef declaringType && (declaringType.ReflectionName == CSObjectName||declaringType.ReflectionName == CObjectName)) {
                bool isAbility = memberSpec.Name == AddAbilityFuncName;
                bool isComp = memberSpec.Name == AddCompFuncName;
                var oneArg = memberSpec.GenericInstMethodSig?.GenericArguments?.Count ==1;
                if (oneArg) {
                    if (isAbility || isComp) {
                        var trueTypeName = memberSpec.GenericInstMethodSig.GenericArguments[0].FullName;
                        referencedTypes.Add(_assemblyDic[AppDllName].GetType(trueTypeName,false));
                        return true;
                    }
                }
            }

            return false;
        }

        private static string CSObjectName = "CObject";
        private static string CObjectName = "CObject";
        //解析非泛型的CSO添加组件类型
        private static bool ResolveCompAndAbility(MemberRef memberRef,MethodDef method,Instruction instr,HashSet<Type> referencedTypes) { 
            if (memberRef.DeclaringType is TypeRef declaringType && (declaringType.ReflectionName == CSObjectName||declaringType.ReflectionName == CObjectName)) {
                
                bool isAbility = memberRef.Name == AddAbilityFuncName;
                bool isComp = memberRef.Name == AddCompFuncName;
                // 获取传递给 AddAbility 的参数
                if (isComp||isAbility) {
                    var type= TrackAbilityBaseSubclass(method.Body.Instructions,method.Body.Instructions.IndexOf(instr) - 1);
                    if (type != null) {
                        referencedTypes.Add(_assemblyDic[AppDllName].GetType(type.FullName,false));
                        return true;
                    }
                }
            }

            return false;
        }

        //生成通用的Faster，可以获取所有的component 和 ability，缺点是点出的提示太多。推荐使用custom faster
        public static void GenerateCommonFasterCode()
        {
            List<Type> components = new List<Type>();
            // 获取所有CSOFasterAttribute
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(CSOAbilityComponent)) ||
                                t.IsSubclassOf(typeof(CSODirtyComponent)) || t.IsSubclassOf(typeof(UniqKeyComponent)))
                    .Where(t => !t.IsAbstract)
                    .ToArray();
                components.AddRange(types);
            }

            ProcessOne("Common", components.ToArray());
        }

        private static void ProcessOne(CSOFasterAttribute classAttribute)
        {
            ProcessOne(classAttribute.Name, classAttribute.Components);
        }


        private static void ProcessOne(string attributeName, Type[] components)
        {
            string directoryPath = "Assets/Scripts/App/APPCSO/CSOGenerated";
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string filePath = $"{directoryPath}/{attributeName}FastExtend.cs";
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("// This file is auto-generated. Do not modify it manually.");
                writer.WriteLine("");
                writer.WriteLine("using CSOEngine.Object;");
                writer.WriteLine("using App.APPCSO.CSOGenerated;");

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
            string className = $"CSObjectFaster{attributeName}";
            writer.WriteLine($"{indent}public sealed class {className}:CSObjectFasterBase");
            writer.WriteLine($"{indent}{{");

            // writer.WriteLine($"{indent}{indent}public {className}(CObject cso) : base(cso)");
            // writer.WriteLine($"{indent}{indent}{{");
            // writer.WriteLine($"{indent}{indent}}}");
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
                    string getMethodStr =
                        $"        public {componentType.Name} {name} => cso.GetComp<{componentType.Name}>();";
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
                    string getMethodStr =
                        $"        public {componentType.Name} {name} => cso.GetAbility<{componentType.Name}>();";
                    writer.WriteLine(getMethodStr);
                }
            }
        }


        private static void GenerateExtendClass(StreamWriter writer, string attributeName, string indent)
        {
            string className = $"CSObjectFaster{attributeName}Extend";
            string fasterName = $"CSObjectFaster{attributeName}";
            writer.WriteLine($"{indent}public static class {className}");
            writer.WriteLine($"{indent}{{");
            writer.WriteLine($"{indent}{indent}private static {fasterName} faster;");

            if (attributeName == "Common")
            {
                writer.WriteLine($"{indent}{indent}public static {fasterName} Fast(this CObject cso)");
            }
            else
            {
                writer.WriteLine($"{indent}{indent}public static {fasterName} Fast{attributeName}(this CObject cso)");
            }

            writer.WriteLine($"{indent}{indent}{{");
            writer.WriteLine($"{indent}{indent}{indent}if (faster == null){{");
            writer.WriteLine($"{indent}{indent}{indent}{indent} faster = new {fasterName}();");
            writer.WriteLine($"{indent}{indent}{indent}}}");
            writer.WriteLine($"{indent}{indent}{indent}faster.cso = cso;");
            writer.WriteLine($"{indent}{indent}{indent}return faster;");
            writer.WriteLine($"{indent}{indent}}}");
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