using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace IronMountain.PackageCreator.Editor
{
    public static class ManifestFinder
    {
        public static PackageManifest FindManifest(Object asset)
        {
            string path = AssetDatabase.GetAssetPath(asset);
            string directory = Path.GetDirectoryName(path);
            List<string> pieces = directory.Split(Path.DirectorySeparatorChar).ToList();
            while (pieces.Count > 0)
            {
                string relativeDirectory = string.Join(Path.DirectorySeparatorChar, pieces);
                string relativeManifestPath = Path.Combine(relativeDirectory, "Package.json");
                TextAsset manifestFile = (TextAsset) AssetDatabase.LoadAssetAtPath(relativeManifestPath, typeof(TextAsset));
                if (manifestFile)
                {
                    PackageManifest manifest = JsonConvert.DeserializeObject<PackageManifest>(manifestFile.text);
                    manifest.TextAsset = manifestFile;
                    manifest.RelativeDirectory = relativeDirectory;
                    manifest.AbsoluteDirectory = Path.Join(Path.GetDirectoryName(Application.dataPath), relativeDirectory);
                    return manifest;
                }
                pieces.RemoveAt(pieces.Count - 1);
            }
            return null;
        }
    }
}