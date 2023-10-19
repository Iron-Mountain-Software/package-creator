# Package Creator
Tools for creating well documented Unity packages.

## Use Cases:
* Instantly document an assembly in Markdown or HTML format.
## Directions for Use:
Right-click an assembly and select Package Creator > Write Documentation. If a package.json file is in a parent folder of your assembly, the documentation writer will use it to write better content.
## Package Mirrors:
[<img src='https://img.itch.zone/aW1nLzEzNzQ2ODg3LnBuZw==/original/npRUfq.png'>](https://github.com/Iron-Mountain-Software/package-creator.git)[<img src='https://img.itch.zone/aW1nLzEzNzQ2ODkyLnBuZw==/original/Fq0ORM.png'>](https://www.npmjs.com/package/com.iron-mountain.package-creator)[<img src='https://img.itch.zone/aW1nLzEzNzQ2ODk4LnBuZw==/original/Rv4m96.png'>](https://iron-mountain.itch.io/package-creator)
## Key Scripts & Components:
1. public class **AssemblyFinder**
1. public static class **ManifestFinder**
1. public static class **MenuItems**
1. public static class **PackageDocumentationGenerator**
1. public class **PackageManifest**
   * Properties: 
      * public String ***Name***  { get; }
      * public String ***Version***  { get; }
      * public String ***DisplayName***  { get; }
      * public String ***Description***  { get; }
      * public List<String> ***UseCases***  { get; }
      * public String ***Directions***  { get; }
      * public List<String> ***Keywords***  { get; }
      * public String ***Author***  { get; }
      * public String ***Type***  { get; }
      * public List<PackageSource> ***Sources***  { get; }
      * public String ***License***  { get; }
      * public PackageManifestBugs ***Bugs***  { get; }
      * public String ***Homepage***  { get; }
      * public String ***RelativeDirectory***  { get; set; }
      * public String ***AbsoluteDirectory***  { get; set; }
1. public class **PackageManifestBugs**
   * Properties: 
      * public String ***URL***  { get; }
1. public class **PackageManifestSample**
   * Properties: 
      * public String ***DisplayName***  { get; }
      * public String ***Description***  { get; }
      * public String ***Path***  { get; }
1. public class **PackageSource**
   * Properties: 
      * public String ***Type***  { get; }
      * public String ***URL***  { get; }
