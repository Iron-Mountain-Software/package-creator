using System;
using System.Collections.Generic;
using UnityEngine;

namespace IronMountain.PackageCreator.Editor
{
    [Serializable]
    public class PackageSource
    {
        [SerializeField] private string type = "git";
        [SerializeField] private string url;
        
        public string Type => type;
        public string URL => url;
    }
    
    [Serializable]
    public class PackageManifestBugs
    {
        [SerializeField] private string url;
        
        public string URL => url;
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
        [SerializeField] private string description;
        [SerializeField] private List<string> useCases = new ();
        [SerializeField] private string directions;
        [SerializeField] private List<string> keywords = new ();
        [SerializeField] private string author;
        [SerializeField] private string type;
        [SerializeField] private List<PackageSource> sources = new ();
        [SerializeField] private string license;
        [SerializeField] private PackageManifestBugs bugs;
        [SerializeField] private string homepage;

        public string Name => name;
        public string Version => version;
        public string DisplayName => displayName;
        public string Description => description;
        public List<string> UseCases => useCases;
        public string Directions => directions;
        public List<string> Keywords => keywords;
        public string Author => author;
        public string Type => type;
        public List<PackageSource> Sources => sources;
        public string License => license;
        public PackageManifestBugs Bugs => bugs;
        public string Homepage => homepage;

        public string RelativeDirectory { get; set; }
        public string AbsoluteDirectory { get; set; }
    }
}