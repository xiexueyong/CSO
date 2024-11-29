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
    public static class AutoGenerateCSOFasterCode
    {
        // 比较hash生成所有Builder的cos Faster文件
        public static void TryGenerateBuilderReferencedTypes() {
            Stopwatch stopwatch1 = Stopwatch.StartNew();
            var resultList =ResolveBuilderReferencedTypes();
            var commonTypes = new HashSet<Type>();
            var commonViewTypes = new HashSet<Type>();
            bool isGenerated = false;
            int count = 0;
            foreach (var item in resultList) {
                string fastFilePath = GenerateCSOFasterCode.GetFastExtendFilePath(item.Item1);
                if (IsNeedGenerate(fastFilePath,item.Item2,out string hash1)) {
                    GenerateCSOFasterCode.ProcessOne(item.Item1,item.Item2,hash1);
                    count++;
                    Debug.Log($"generate faster file {fastFilePath} finished");
                    isGenerated = true;
                }
                foreach (var type in item.Item2) {
                    commonTypes.Add(type);
                }
                fastFilePath = GenerateCSOFasterViewCode.GetFastExtendFilePath(item.Item1);
                if (IsNeedGenerate(fastFilePath,item.Item3,out string hash2)) {
                    GenerateCSOFasterViewCode.ProcessOne(item.Item1,item.Item3,hash2);
                    count++;
                    Debug.Log($"generate faster file {fastFilePath} finished");
                    isGenerated = true;
                }
                foreach (var type in item.Item3) {
                    commonViewTypes.Add(type);
                }
            }

            var commonAttribute = "Common";
            var commonTypeArr = commonTypes.ToArray();
            var commonViewTypeArr = commonViewTypes.ToArray();
            var commonFastFilePath = GenerateCSOFasterCode.GetFastExtendFilePath(commonAttribute);
            var commonViewFastFilePath = GenerateCSOFasterViewCode.GetFastExtendFilePath(commonAttribute);
            if (IsNeedGenerate(commonFastFilePath,commonTypeArr,out string typeHash1)) {
                GenerateCSOFasterCode.ProcessOne(commonAttribute, commonTypeArr,typeHash1);
                count++;
                Debug.Log($"generate faster file {commonFastFilePath} finished");
                isGenerated = true;
            }
            
            if (IsNeedGenerate(commonViewFastFilePath,commonViewTypeArr,out string typeHash2)) {
                GenerateCSOFasterViewCode.ProcessOne(commonAttribute, commonViewTypeArr,typeHash2);
                count++;
                Debug.Log($"generate faster file {commonViewFastFilePath} finished");
                isGenerated = true;
            }
            
            stopwatch1.Stop();
            Debug.Log($"generate builder referenced checked take time = {stopwatch1.ElapsedMilliseconds}ms,,modify files = {count}");
            if (isGenerated) {
                AssetDatabase.Refresh();
            }
        }

        static bool IsNeedGenerate(string fastFilePath, Type[] components,out string hash) {
            var fileHash = GetFastExtendFileHash(fastFilePath);
            hash = CalAttributeTypeHash(components);
            if (fileHash == hash) {
                return false;
            }
            return true;
        }

        private const  string AppDllName = "App";
        private const  string CsoDllName = "CObject";
        private static Dictionary<string, Assembly> _assemblyDic;
        // 返回收集的所有Builder引用cso组件类型的数组
        public static List<(string,Type[],Type[])> ResolveBuilderReferencedTypes() {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            _assemblyDic = assemblies
                .Where(assembly => assembly.GetName().Name is AppDllName or CsoDllName)
                .ToDictionary(assembly => assembly.GetName().Name, assembly => assembly);
            
            Type builderInterface = _assemblyDic[CsoDllName].GetType("CSOEngine.ICSOBuilder");
            Type viewComponent = _assemblyDic[CsoDllName].GetType("CSOEngine.Component.CSOViewComponent");
            var onBuilderMethodInfo = builderInterface.GetMethods().First(info => info.Name.Contains("OnBuild"));
            Assembly curAssembly = _assemblyDic[AppDllName];
            var result = new List<(string,Type[],Type[])>();
            foreach (var builderType in curAssembly.GetTypes().Where(t => builderInterface.IsAssignableFrom(t))){
                // if (builderType.FullName.ToLower().Contains("blockbuilder"))
                // {
                var list = GetReferencedTypesInMethod(curAssembly.Location,builderType.FullName,onBuilderMethodInfo.Name);
                var compAndAbilityList = new List<Type>();
                var viewList = new List<Type>();
                foreach (var t in list) {
                    if (t.IsSubclassOf(viewComponent)){
                        viewList.Add(t);
                    } else {
                        compAndAbilityList.Add(t);
                    }
                }
                result.Add((builderType.Name.Replace("Builder",""),compAndAbilityList.ToArray(),viewList.ToArray()));
                // }
            }
            _assemblyDic.Clear();
            return result;
        }


        //获取给定类型、方法的所有cos组件类型引用
        private static HashSet<Type> GetReferencedTypesInMethod(string assemblyPath, string typeName, string methodName) {
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
            TraverseMethodILRecursively(methodDef,new HashSet<string>(),referencedTypes,typeDef);
            return referencedTypes;
        }

        // 递归遍历方法调用链，收集cso组件类型引用
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
                        while (seekType != null) {
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

        //沿代码段向前追踪非泛型CSO添加组件的类型
        private static Type TrackAbilityBaseSubclass(IList<Instruction> instructions, int index) {
            while (index >= 0) {
                var instr = instructions[index];
                if (instr.OpCode == OpCodes.Newobj && instr.Operand is IMethod ctorMethod) {
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
        private static string AddViewFuncName = "AddView";
        private static string AddContextFuncName = "AddContext";
        //解析泛型的CSO添加组件类型
        private static bool ResolveCompAndAbility(MethodSpec memberSpec,HashSet<Type> referencedTypes) {
            if (memberSpec.DeclaringType is TypeRef declaringType && (declaringType.ReflectionName == CObjectName)) {
                bool isAbility = memberSpec.Name == AddAbilityFuncName;
                bool isComp = memberSpec.Name == AddCompFuncName;
                bool isView = memberSpec.Name == AddViewFuncName;
                bool isContext = memberSpec.Name == AddContextFuncName;
                var oneArg = memberSpec.GenericInstMethodSig?.GenericArguments?.Count ==1;
                if (oneArg) {
                    if (isAbility || isComp|| isView||isContext) {
                        var trueTypeName = memberSpec.GenericInstMethodSig.GenericArguments[0].FullName;
                        referencedTypes.Add(_assemblyDic[AppDllName].GetType(trueTypeName,false));
                        return true;
                    }
                }
            }

            return false;
        }
        
        private static string CObjectName = "CObject";
        //解析非泛型的CSO添加组件类型
        private static bool ResolveCompAndAbility(MemberRef memberRef,MethodDef method,Instruction instr,HashSet<Type> referencedTypes) { 
            if (memberRef.DeclaringType is TypeRef declaringType && declaringType.ReflectionName == CObjectName) {
                bool isAbility = memberRef.Name == AddAbilityFuncName;
                bool isComp = memberRef.Name == AddCompFuncName;
                bool isView = memberRef.Name == AddViewFuncName;
                bool isContext = memberRef.Name == AddContextFuncName;
                if (isComp||isAbility||isView||isContext) {
                    var type= TrackAbilityBaseSubclass(method.Body.Instructions,method.Body.Instructions.IndexOf(instr) - 1);
                    if (type != null) {
                        referencedTypes.Add(_assemblyDic[AppDllName].GetType(type.FullName,false));
                        return true;
                    }
                }
            }

            return false;
        }

        private static string CalAttributeTypeHash(Type[] referencedTypes) {
            if (referencedTypes==null || referencedTypes.Length ==0) {
                return null;
            }
            StringBuilder typeSb = new StringBuilder();
            foreach (var type in referencedTypes) {
                typeSb.Append(type.FullName);
            }

            byte[] inputBytes = Encoding.UTF8.GetBytes(typeSb.ToString());
            using (MD5 md5 = MD5.Create()) {
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes) {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }

        private static string HashMarker = GenerateCSOFasterCode.HashMarker;
        private static string GetFastExtendFileHash(string fastFilePath) {
            if (File.Exists(fastFilePath)) {
                var lines = File.ReadAllLines(fastFilePath);
                if (lines.Length>1 && lines[1].StartsWith(HashMarker)) {
                    var hash = lines[1].Replace(HashMarker,"").Trim();
                    if (string.IsNullOrEmpty(hash)) {
                        return null;
                    }
                    return hash;
                }
            }

            return null;
        }
    }
}
#endif