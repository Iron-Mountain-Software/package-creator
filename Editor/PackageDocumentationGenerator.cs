using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditorInternal;

namespace IronMountain.PackageCreator.Editor
{
    public static class PackageDocumentationGenerator
    {
        public enum ExportType { Markdown, HTML };
        
        private static string _readmePath;
        private static ExportType _exportType;

        private static string BoldStart => _exportType == ExportType.Markdown ? "**" : "<b>";
        private static string BoldEnd => _exportType == ExportType.Markdown ? "**" : "</b>";
        private static string ItalicsStart => _exportType == ExportType.Markdown ? "*" : "<i>";
        private static string ItalicsEnd => _exportType == ExportType.Markdown ? "*" : "</i>";
        private static string BoldItalicsStart => _exportType == ExportType.Markdown ? "***" : "<b><i>";
        private static string BoldItalicsEnd => _exportType == ExportType.Markdown ? "***" : "</i></b>";
        private static string H1Start => _exportType == ExportType.Markdown ? "# " : "<h1>";
        private static string H1End => _exportType == ExportType.Markdown ? string.Empty : "</h1>";
        private static string H2Start => _exportType == ExportType.Markdown ? "## " : "<h2>";
        private static string H2End => _exportType == ExportType.Markdown ? string.Empty : "</h2>";
        private static string H3Start => _exportType == ExportType.Markdown ? "### " : "<h3>";
        private static string H3End => _exportType == ExportType.Markdown ? string.Empty : "</h3>";
        private static string ListIndent => _exportType == ExportType.Markdown ? "   " : string.Empty;
        private static string OrderedListStart => _exportType == ExportType.Markdown ? string.Empty : "<ol>";
        private static string OrderedListEnd => _exportType == ExportType.Markdown ? string.Empty : "</ol>";
        private static string OrderedListItemStart => _exportType == ExportType.Markdown ? "1. " : "<li>";
        private static string OrderedListItemEnd => _exportType == ExportType.Markdown ? string.Empty : "</li>";
        private static string UnorderedListStart => _exportType == ExportType.Markdown ? string.Empty : "<ul>";
        private static string UnorderedListEnd => _exportType == ExportType.Markdown ? string.Empty : "</ul>";
        private static string UnorderedListItemStart => _exportType == ExportType.Markdown ? "* " : "<li>";
        private static string UnorderedListItemEnd => _exportType == ExportType.Markdown ? string.Empty : "</li>";

        public static string Document(Assembly assembly, PackageManifest manifest, ExportType exportType)
        {
            if (assembly == null) return string.Empty;
            _exportType = exportType;

            StringBuilder documentation = new StringBuilder();
        
            Dictionary<string, List<Type>> folders = new Dictionary<string, List<Type>>();

            foreach (Type type in assembly.GetTypes())
            {
                if (!char.IsLetter(type.Name[0]) || type.Namespace == null) continue;
                if (folders.ContainsKey(type.Namespace)) folders[type.Namespace].Add(type);
                else folders.Add(type.Namespace, new List<Type> { type });
            }
        
            var keys = folders.Keys.ToList();
            keys.Sort();
            string root = FindRoot(keys);
            
            if (manifest != null)
            {
                documentation.AppendLine(H1Start + manifest.DisplayName + H1End);
                documentation.AppendLine(manifest.Description);
                documentation.AppendLine();

                if (manifest.UseCases.Count > 0)
                {
                    documentation.AppendLine(H2Start + "Use Cases:" + H2End);
                    documentation.Append(UnorderedListStart);
                    foreach (string useCase in manifest.UseCases)
                    {
                        documentation.AppendLine(UnorderedListItemStart + useCase + UnorderedListItemEnd);
                    }
                    documentation.Append(UnorderedListEnd);
                }

                if (!string.IsNullOrWhiteSpace(manifest.Directions))
                {
                    documentation.AppendLine(H2Start + "Directions for Use:" + H2End);
                    documentation.AppendLine(manifest.Directions);
                }

                if (manifest.Sources.Count > 0)
                {
                    documentation.AppendLine(H2Start + "Package Mirrors:" + H2End);
                    foreach (var source in manifest.Sources)
                    {
                        documentation.Append(GetSourceImage(source));
                    }
                    documentation.AppendLine();
                }
            }

            documentation.AppendLine(H2Start + "Key Scripts & Components:" + H2End);

            foreach (var key in keys)
            {
                string folderName = GetFolder(root, key);
                if (!string.IsNullOrWhiteSpace(folderName))
                {
                    documentation.AppendLine(H3Start + folderName + H3End);
                }
                folders[key].Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
                documentation.Append(OrderedListStart);
                foreach (var type in folders[key])
                {
                    if (!type.IsPublic) continue;
                
                    documentation.AppendLine(OrderedListItemStart + GetMainInformation(type) + OrderedListItemEnd);

                    documentation.Append(UnorderedListStart);
                
                    List<string> actions = GetActions(type);
                    if (actions.Count > 0)
                    {
                        documentation.AppendLine(ListIndent + UnorderedListItemStart + "Actions: " + UnorderedListItemEnd);
                        foreach (string action in actions)
                        {
                            documentation.Append(UnorderedListStart);
                            documentation.AppendLine(ListIndent + ListIndent + UnorderedListItemStart + action + UnorderedListItemEnd);
                            documentation.Append(UnorderedListEnd);
                        }
                    }
                
                    List<string> properties = GetProperties(type);
                    if (properties.Count > 0)
                    {
                        documentation.AppendLine(ListIndent + UnorderedListItemStart + "Properties: " + UnorderedListItemEnd);
                        foreach (string property in properties)
                        {
                            documentation.Append(UnorderedListStart);
                            documentation.AppendLine(ListIndent + ListIndent + UnorderedListItemStart + property + UnorderedListItemEnd);
                            documentation.Append(UnorderedListEnd);
                        }
                    }
                
                    List<string> methods = GetMethods(type);
                    if (methods.Count > 0)
                    {
                        documentation.AppendLine(ListIndent + UnorderedListItemStart + "Methods: " + UnorderedListItemEnd);
                        foreach (string method in methods)
                        {
                            documentation.Append(UnorderedListStart);
                            documentation.AppendLine(ListIndent + ListIndent + UnorderedListItemStart + method + UnorderedListItemEnd);
                            documentation.Append(UnorderedListEnd);
                        }
                    }
                
                    documentation.Append(UnorderedListEnd);
                }
                documentation.Append(OrderedListEnd);
            }

            return documentation.ToString();
        }

        public static string GetSourceImage(PackageSource source)
        {
            if (source == null) return string.Empty;
            StringBuilder result = new StringBuilder();
            string imageURL = GetImageURL(source.Type);
            if (_exportType == ExportType.HTML)
            {
                result.Append("<a href='" + source.URL + "' target='_blank'>");
                result.Append("<img src='" + imageURL + "' alt='" + source.Type + "' title='" + source.Type + "'>");
                result.Append("</a>");
            }
            else if (_exportType == ExportType.Markdown)
            {
                result.Append("[<img src='" + imageURL + "'>]");
                result.Append("(" + source.URL + ")");
            }
            return result.ToString();
        }

        public static string GetImageURL(string type)
        {
            type = type.ToLower();
            return type switch
            {
                "git" or "github" or "github.com" => "https://img.itch.zone/aW1nLzEzNzQ2ODg3LnBuZw==/original/npRUfq.png",
                "npm" => "https://img.itch.zone/aW1nLzEzNzQ2ODkyLnBuZw==/original/Fq0ORM.png",
                "itch" or "itch.io" => "https://img.itch.zone/aW1nLzEzNzQ2ODk4LnBuZw==/original/Rv4m96.png",
                _ => string.Empty
            };
        }

        public static string GetMainInformation(Type type)
        {
            string accessModifier = type.IsPublic ? "public" : "private";
            string entityType = string.Empty;
            if (type.IsEnum) entityType = "enum";
            else if (type.IsInterface) entityType = "interface";
            else if (type.IsValueType && !type.IsPrimitive) entityType = "struct";
            else if (type.IsAbstract)
            {
                entityType = type.IsSealed ? "static class" : "abstract class";
            }
            else if (type.IsClass) entityType = "class";
            string baseType = type.BaseType != null ? type.BaseType.Name : string.Empty;
            if (baseType == "ValueType") baseType = string.Empty;
            else if (baseType == "Object") baseType = string.Empty;
            string extension = ! string.IsNullOrWhiteSpace(baseType) ? " : " + baseType : string.Empty;
            return accessModifier + " " + entityType + " " + BoldStart + type.Name + BoldEnd + extension;
        }

        private static List<string> GetActions(Type type)
        {
            List<string> actions = new List<string>();
            EventInfo[] eventInfos = type.GetEvents(BindingFlags.Public | BindingFlags.Instance);
            foreach (var eventInfo in eventInfos)
            {
                if (eventInfo.DeclaringType != type) continue;
                string returnType = string.Empty;
                if (!eventInfo.GetAddMethod().IsPublic && !eventInfo.GetRemoveMethod().IsPublic) continue;
                string attributes = string.Empty;
                foreach (CustomAttributeData attribute in eventInfo.CustomAttributes)
                {
                    if (attributes.Length == 0) attributes += '<';
                    attributes += attribute.AttributeType.Name;
                }
                if (attributes.Length > 0) attributes += '>';
                actions.Add("public event Action" + attributes + " " + BoldItalicsStart + eventInfo.Name + BoldItalicsEnd + " " + returnType);
            }
            return actions;
        }
    
        private static List<string> GetProperties(Type type)
        {
            List<string> properties = new List<string>();
            PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var propertyInfo in propertyInfos)
            {
                if (propertyInfo.DeclaringType != type) continue;
                MethodInfo getter = propertyInfo.GetGetMethod();
                MethodInfo setter = propertyInfo.GetSetMethod();
                if (!(getter != null && getter.IsPublic || setter != null && setter.IsPublic)) continue;
                string returnType = GetPropertyType(propertyInfo);
                string accessors = " {";
                if (getter != null && getter.IsPublic) accessors += " get;";
                if (setter != null && setter.IsPublic) accessors += " set;";
                accessors += " }";
                properties.Add("public " + returnType + " " + BoldItalicsStart + propertyInfo.Name + BoldItalicsEnd + " " + accessors);
            }
            return properties;
        }

        public static string GetPropertyType(PropertyInfo property)
        {
            if (property == null) return string.Empty;
            if (property.PropertyType.Name == "Void") return "void";
            if (property.PropertyType.Name == "Single") return "float";
            if (property.PropertyType.IsGenericType &&
                property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
            {
                string arguments = string.Empty;
                foreach (Type argument in property.PropertyType.GetGenericArguments())
                {
                    if (arguments.Length > 0) arguments += ", ";
                    arguments += argument.Name;
                }
                return "List<" + arguments + ">";
            }
            return property.PropertyType.Name;
        }

        private static List<string> GetMethods(Type type)
        {
            List<string> methods = new List<string>();
            MethodInfo[] methodInfos = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            foreach (var methodInfo in methodInfos)
            {
                if (methodInfo.DeclaringType != type
                    || !methodInfo.IsPublic
                    || methodInfo.Name.Contains("add_")
                    || methodInfo.Name.Contains("remove_")
                    || methodInfo.Name.Contains("get_")
                    || methodInfo.Name.Contains("set_")) continue;
                string method = string.Empty;
                method += methodInfo.IsPublic ? "public" : "private";
                if (methodInfo.IsStatic) method += " static";
                if (methodInfo.IsAbstract) method += " abstract";
                else if (methodInfo.DeclaringType != methodInfo.GetBaseDefinition().DeclaringType) method += " override";
                else if (methodInfo.IsVirtual) method += " virtual";
                string returnType = methodInfo.ReturnType.Name;
                if (returnType == "Void") returnType = "void";
                else if (returnType == "Single") returnType = "float";
                string parameters = string.Empty;
                foreach (var parameterInfo in methodInfo.GetParameters())
                {
                    if (parameters.Length > 0) parameters += ", ";
                    string typeName = parameterInfo.ParameterType.Name;
                    if (typeName == "Single") typeName = "float";
                    parameters += typeName + " " + parameterInfo.Name;
                }
                method += " " + returnType + " " + BoldItalicsStart + methodInfo.Name + BoldItalicsEnd + "(" + parameters + ")";
                methods.Add(method);
            }
            return methods;
        }

        private static string GetFolder(string root, string path)
        {
            string raw = root.Length <= path.Length
                ? path.Substring(root.Length)
                : string.Empty;
            if (raw.Length >= 1 && raw[0] == '.') raw = raw.Substring(1);
            raw = AddSpaces(raw);
            return raw;
        }
    
        public static string AddSpaces(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                    newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }
    
        public static String FindRoot(List<string> input) 
        { 
            // Determine size of the array 
            int n = input.Count; 
 
            // Take first word from array as reference 
            String s = input[0]; 
            int len = s.Length; 
 
            String res = ""; 
 
            for (int i = 0; i < len; i++) 
            { 
                for (int j = i + 1; j <= len; j++)
                { 
 
                    // generating all possible substrings 
                    // of our reference string arr[0] i.e s 
                    String stem = s.Substring(i, j-i); 
                    int k = 1; 
                    for (k = 1; k < n; k++) 
 
                        // Check if the generated stem is 
                        // common to all words 
                        if (!input[k].Contains(stem)) 
                            break; 
                 
                    // If current substring is present in 
                    // all strings and its length is greater 
                    // than current result 
                    if (k == n && res.Length < stem.Length) 
                        res = stem; 
                } 
            } 
 
            return res; 
        } 
    }
}
