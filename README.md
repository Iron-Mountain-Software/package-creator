# Package Creator
*Version: 1.0.5*
## Description: 
Tools for creating well documented Unity packages.
## Use Cases: 
* Instantly document an assembly in Markdown or HTML format.
## Dependencies: 
* com.unity.nuget.newtonsoft-json (3.1.0)
## Directions for Use: 
Right-click an assembly and select Package Creator > Write Documentation. If a package.json file is in a parent folder of your assembly, the documentation writer will use it to write better content.
## Package Mirrors: 
[<img src='https://img.itch.zone/aW1nLzEzNzQ2ODg3LnBuZw==/original/npRUfq.png'>](https://github.com/Iron-Mountain-Software/package-creator.git)[<img src='https://img.itch.zone/aW1nLzEzNzQ2ODkyLnBuZw==/original/Fq0ORM.png'>](https://www.npmjs.com/package/com.iron-mountain.package-creator)[<img src='https://img.itch.zone/aW1nLzEzNzQ2ODk4LnBuZw==/original/Rv4m96.png'>](https://iron-mountain.itch.io/package-creator)
---
## Key Scripts & Components: 
1. public class **AssemblyFinder**
1. public static class **ManifestFinder**
1. public class **PackageDependency**
   * Properties: 
      * public String ***Name***  { get; set; }
      * public String ***Version***  { get; set; }
1. public static class **PackageDocumentationGenerator**
1. public class **PackageEditorWindow** : EditorWindow
1. public class **PackageManifest**
   * Properties: 
      * public String ***Name***  { get; set; }
      * public String ***Version***  { get; set; }
      * public String ***DisplayName***  { get; set; }
      * public String ***Author***  { get; set; }
      * public String ***Unity***  { get; set; }
      * public String ***Type***  { get; set; }
      * public String ***License***  { get; set; }
      * public String ***Homepage***  { get; set; }
      * public PackageResource ***Bugs***  { get; }
      * public PackageResource ***Repository***  { get; }
      * public String ***Description***  { get; set; }
      * public List<String> ***UseCases***  { get; }
      * public String ***Directions***  { get; set; }
      * public List<String> ***Keywords***  { get; }
      * public List<PackageResource> ***Sources***  { get; }
      * public Dictionary`2 ***Dependencies***  { get; }
      * public TextAsset ***TextAsset***  { get; set; }
      * public String ***RelativeDirectory***  { get; set; }
      * public String ***AbsoluteDirectory***  { get; set; }
      * public String ***AbsolutePath***  { get; }
   * Methods: 
      * public void ***Save***()
      * public String ***GetHTMLDocumentation***(Boolean includeTitle)
      * public String ***GetMarkdownDocumentation***(Boolean includeTitle)
      * public void ***GenerateREADME***()
      * public void ***Export***()
1. public class **PackageManifestSample**
   * Properties: 
      * public String ***DisplayName***  { get; }
      * public String ***Description***  { get; }
      * public String ***Path***  { get; }
1. public class **PackageResource**
   * Properties: 
      * public String ***Type***  { get; set; }
      * public String ***URL***  { get; set; }
