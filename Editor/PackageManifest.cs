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
        [SerializeField] private string name;
        [SerializeField] private string version;
        
        public string Name
        {
            get => name;
            set => name = value;
        }
        
        public string Version
        {
            get => version;
            set => version = value;
        }
    }
    
    [Serializable]
    public class PackageResource
    {
        [SerializeField] private string type = "git";
        [SerializeField] private string url;
        
        public string Type
        {
            get => type;
            set => type = value;
        }
        
        public string URL
        {
            get => url;
            set => url = value;
        }
    }
        
    [Serializable]
    public class PackageManifestSample
    {
        [SerializeField] private string displayName;
        [SerializeField] private string description;
        [SerializeField] private string path;

        public string DisplayName => displayName;
        public string Description => description;
        public string Path => path;
    }

    [Serializable]
    public class PackageManifest
    {
        [SerializeField] private string name;
        [SerializeField] private string version;
        [SerializeField] private string displayName;
        [SerializeField] private string author;
        [SerializeField] private string unity;
        [SerializeField] private string type;
        [SerializeField] private string license;
        [SerializeField] private string homepage;
        [SerializeField] private PackageResource bugs = new ();
        [SerializeField] private PackageResource repository = new ();
        [SerializeField] private string description;
        [SerializeField] private List<string> useCases = new ();
        [SerializeField] private string directions;
        [SerializeField] private List<string> keywords = new ();
        [SerializeField] private List<PackageResource> sources = new ();
        [SerializeField] private Dictionary<string, string> dependencies = new ();
        
        public string Name
        {
            get => name;
            set => name = value;
        }

        public string Version
        {
            get => version;
            set => version = value;
        }
        
        public string DisplayName
        {
            get => displayName;
            set => displayName = value;
        }
        
        public string Author
        {
            get => author;
            set => author = value;
        }
        
        public string Unity
        {
            get => unity;
            set => unity = value;
        }
        
        public string Type
        {
            get => type;
            set => type = value;
        }
        
        public string License
        {
            get => license;
            set => license = value;
        }
        
        public string Homepage
        {
            get => homepage;
            set => homepage = value;
        }
        
        public PackageResource Bugs => bugs;
        public PackageResource Repository => repository;

        public string Description
        {
            get => description;
            set => description = value;
        }
        
        public List<string> UseCases => useCases;
        
        public string Directions
        {
            get => directions;
            set => directions = value;
        }
        
        public List<string> Keywords => keywords;

        public List<PackageResource> Sources => sources;
        public Dictionary<string, string> Dependencies => dependencies;

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

        public void GenerateREADME()
        {
            Assembly assembly = AssemblyFinder.FindAssembly(name);
            string documentation = PackageDocumentationGenerator.Document(assembly, this, PackageDocumentationGenerator.ExportType.Markdown);
            if (!string.IsNullOrWhiteSpace(AbsoluteDirectory))
            {
                string readmePath = Path.Join(AbsoluteDirectory, "README.md");
                File.WriteAllText(readmePath, documentation);
                AssetDatabase.Refresh();
            }
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