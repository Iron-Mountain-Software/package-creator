using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditorInternal;

namespace IronMountain.PackageCreator.Editor
{
    public class AssemblyFinder
    {
        public static Assembly FindAssembly(AssemblyDefinitionAsset assemblyDefinitionAsset)
        {
            if (assemblyDefinitionAsset == null) return null;
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var testAssembly in assemblies)
            {
                if (testAssembly == null || string.IsNullOrWhiteSpace(testAssembly.FullName)) continue;
                List<string> elements = testAssembly.FullName.Split(' ').ToList();
                for (int i = 0; i < elements.Count; i++) elements[i] = elements[i].Trim(' ', ',');
                if (elements.Contains(assemblyDefinitionAsset.name)) return testAssembly;
            }
            return null;
        }
    }
}