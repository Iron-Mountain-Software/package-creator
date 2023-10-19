using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace IronMountain.PackageCreator.Editor
{
    public static class MenuItems
    {
        [MenuItem("Assets/Package Creator/Copy Markdown", true)]
        static bool ValidateCopyMarkdownDocumentation()
        {
            return Selection.activeObject is AssemblyDefinitionAsset;
        }

        [MenuItem("Assets/Package Creator/Copy Markdown", false, 0)]
        private static void CopyMarkdownDocumentation(MenuCommand menuCommand)
        {
            if (Selection.activeObject is not AssemblyDefinitionAsset assemblyDefinitionAsset) return;
            Assembly assembly = AssemblyFinder.FindAssembly(assemblyDefinitionAsset);
            PackageManifest manifest = ManifestFinder.FindManifest(assemblyDefinitionAsset);
            string documentation = PackageDocumentationGenerator.Document(assembly, manifest, PackageDocumentationGenerator.ExportType.Markdown);
            EditorGUIUtility.systemCopyBuffer = documentation;
        }

        [MenuItem("Assets/Package Creator/Copy HTML", true)]
        static bool ValidateCreateHTMLDocumentation()
        {
            return Selection.activeObject is AssemblyDefinitionAsset;
        }
    
        [MenuItem("Assets/Package Creator/Copy HTML", false, 0)]
        private static void CreateHTMLDocumentation(MenuCommand menuCommand)
        {
            if (Selection.activeObject is not AssemblyDefinitionAsset assemblyDefinitionAsset) return;
            Assembly assembly = AssemblyFinder.FindAssembly(assemblyDefinitionAsset);
            PackageManifest manifest = ManifestFinder.FindManifest(assemblyDefinitionAsset);
            string documentation = PackageDocumentationGenerator.Document(assembly, manifest, PackageDocumentationGenerator.ExportType.HTML);
            EditorGUIUtility.systemCopyBuffer = documentation;
        }
    }
}