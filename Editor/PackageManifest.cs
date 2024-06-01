using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEditor;
using UnityEngine;

namespace IronMountain.PackageCreator.Editor
{
    [Serializable]
    public class PackageDependency
    {
        [SerializeField] public string name;
        [SerializeField] public string version;
    }
    
    [Serializable]
    public class PackageResource
    {
        [SerializeField] public string type = "git";
        [SerializeField] public string url;
    }
        
    [Serializable]
    public class PackageManifestSample
    {
        [SerializeField] public string displayName;
        [SerializeField] public string description;
        [SerializeField] public string path;
    }
    
    [Serializable]
    public class Instruction
    {
        [SerializeField] public string text;
        [SerializeField] public List<string> details = new();

        public Instruction(string text)
        {
            this.text = text;
        }
    }

    [Serializable]
    public class PackageManifest
    {
        [SerializeField] public string name;
        [SerializeField] public string version;
        [SerializeField] public string displayName;
        [SerializeField] public string author;
        [SerializeField] public string unity;
        [SerializeField] public string type;
        [SerializeField] public string license;
        [SerializeField] public string homepage;
        [SerializeField] public PackageResource bugs = new ();
        [SerializeField] public PackageResource repository = new ();
        [SerializeField] public string description;
        [SerializeField] public List<string> useCases = new ();
        [SerializeField] public List<Instruction> instructions = new ();
        [SerializeField] public List<string> keywords = new ();
        [SerializeField] public List<PackageResource> sources = new ();
        [SerializeField] public Dictionary<string, string> dependencies = new ();
        
        [JsonIgnore]
        public TextAsset TextAsset { get; set; }
        
        [JsonIgnore]
        public string RelativeDirectory { get; set; }
        
        [JsonIgnore]
        public string AbsoluteDirectory { get; set; }
        
        [JsonIgnore]
        public string AbsolutePath => Path.Combine(AbsoluteDirectory, "package.json");

        public void Save()
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            string json = JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });
            
            File.WriteAllText(AbsolutePath, json);
            AssetDatabase.Refresh();
        }

        public string GetHTMLDocumentation(bool includeTitle)
        {
            Assembly assembly = AssemblyFinder.FindAssembly(name);
            return PackageDocumentationGenerator.Document(assembly, this, PackageDocumentationGenerator.ExportType.HTML, includeTitle);
        }
        
        public string GetMarkdownDocumentation(bool includeTitle)
        {
            Assembly assembly = AssemblyFinder.FindAssembly(name);
            return PackageDocumentationGenerator.Document(assembly, this, PackageDocumentationGenerator.ExportType.Markdown, includeTitle);
        }

        public void GenerateREADME()
        {
            if (string.IsNullOrWhiteSpace(AbsoluteDirectory)) return;
            string readmePath = Path.Join(AbsoluteDirectory, "README.md");
            File.WriteAllText(readmePath, GetMarkdownDocumentation(true));
            AssetDatabase.Refresh();
        }

        public void Export()
        {
            string exportPath = Path.Combine(Application.dataPath, name + ".unitypackage");
            AssetDatabase.ExportPackage(
                RelativeDirectory,
                exportPath,
                ExportPackageOptions.Recurse);
            AssetDatabase.Refresh();
            EditorUtility.RevealInFinder(exportPath);
        }
    }
}