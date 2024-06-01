# Package Creator
*Version: 1.0.6*
## Description: 
Tools for creating well documented Unity packages.
## Use Cases: 
* Instantly document an assembly in Markdown or HTML format.
## Dependencies: 
* com.unity.nuget.newtonsoft-json (3.1.0)
## Directions for Use: 
1. In Unity, right click any file within a package and select "Open Package Creator". 
1. Use the Package Editor Window to edit package details.
1. Click Apply.
## Package Mirrors: 
[<img src='https://img.itch.zone/aW1nLzEzNzQ2ODg3LnBuZw==/original/npRUfq.png'>](https://github.com/Iron-Mountain-Software/package-creator.git)[<img src='https://img.itch.zone/aW1nLzEzNzQ2ODkyLnBuZw==/original/Fq0ORM.png'>](https://www.npmjs.com/package/com.iron-mountain.package-creator)[<img src='https://img.itch.zone/aW1nLzEzNzQ2ODk4LnBuZw==/original/Rv4m96.png'>](https://iron-mountain.itch.io/package-creator)
---
## Key Scripts & Components: 
1. public class **AssemblyFinder**
1. public class **Instruction**
1. public static class **ManifestFinder**
1. public class **PackageDependency**
1. public static class **PackageDocumentationGenerator**
1. public class **PackageEditorWindow** : EditorWindow
1. public class **PackageManifest**
   * Properties: 
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
1. public class **PackageResource**
