using UnityEditor;
using UnityEngine;

namespace IronMountain.PackageCreator.Editor
{
    public class PackageEditorWindow : EditorWindow
    {
        public static PackageEditorWindow Current { get; private set; }

        private PackageManifest _manifest;
        private SerializedObject _serializedManifest;
        
        private Rect _contentSection;
        private Vector2 _contentScroll = Vector2.zero;
        private Vector2 _descriptionScroll = Vector2.zero;
        private Vector2 _directionsScroll = Vector2.zero;

        [MenuItem("Assets/Open Package Creator", false, 0)]
        private static void OpenWindow()
        {
            if (!Selection.activeObject) return;
            OpenWindow(ManifestFinder.FindManifest(Selection.activeObject));
        }

        private void OnFocus()
        {
            if (_manifest == null || _manifest.TextAsset == null) return;
            OpenWindow(ManifestFinder.FindManifest(_manifest.TextAsset));
        }

        private static void OpenWindow(PackageManifest manifest)
        {
            if (manifest == null) return;
            if (!Current) Current = GetWindow<PackageEditorWindow>();
            Current.name = manifest.displayName;
            Current.minSize = new Vector2(525, 700);
            Current._descriptionScroll = Vector2.zero;
            Current._directionsScroll = Vector2.zero;
            Current._manifest = manifest;
            Current.Focus();
        }

        private void DrawLayouts()
        {
            _contentSection.x = 0;
            _contentSection.y = 0;
            _contentSection.width = position.width;
            _contentSection.height = position.height;
        }

        private void OnGUI()
        {
            DrawLayouts();
            GUILayout.BeginArea(_contentSection);
            if (_manifest != null)
            {
                _contentScroll = EditorGUILayout.BeginScrollView(_contentScroll);
                
                EditorStyles.textField.wordWrap = false;
                
                _manifest.name = EditorGUILayout.TextField("Name", _manifest.name);
                _manifest.version = EditorGUILayout.TextField("Version", _manifest.version);
                _manifest.displayName = EditorGUILayout.TextField("Display Name", _manifest.displayName);
                _manifest.author = EditorGUILayout.TextField("Author", _manifest.author);
                _manifest.unity = EditorGUILayout.TextField("Unity", _manifest.unity);
                _manifest.type = EditorGUILayout.TextField("Type", _manifest.type);
                _manifest.license = EditorGUILayout.TextField("License", _manifest.license);
                _manifest.homepage = DrawURLEditor("Homepage", _manifest.homepage);
                _manifest.bugs.url = DrawURLEditor("Bugs", _manifest.bugs.url);
                _manifest.repository.type = EditorGUILayout.TextField("Repository Type", _manifest.repository.type);
                _manifest.repository.url = DrawURLEditor("Repository URL", _manifest.repository.url);

                EditorStyles.textArea.wordWrap = true;
                
                EditorGUILayout.Space();
                DrawDescription();
                EditorGUILayout.Space();
                DrawUseCases();
                EditorGUILayout.Space();
                DrawDirections();
                EditorGUILayout.Space();
                DrawKeywords();
                EditorGUILayout.Space();
                DrawSources();
                EditorGUILayout.Space();
                DrawDependencies();
                EditorGUILayout.Space();
                DrawActionButtons();
                EditorGUILayout.Space();
                EditorGUILayout.EndScrollView();
            }
            else
            {
                GUILayout.Label("No quest selected.");
            }
            GUILayout.EndArea();
        }

        private void DrawDescription()
        {
            EditorStyles.textField.wordWrap = true;
            EditorGUILayout.LabelField("Description");
            EditorGUI.indentLevel++;
            _descriptionScroll = EditorGUILayout.BeginScrollView(_descriptionScroll, GUILayout.Height(50));
            _manifest.description = EditorGUILayout.TextArea(_manifest.description, GUILayout.ExpandHeight(true));
            EditorGUILayout.EndScrollView();
            EditorGUI.indentLevel--;
        }
        
        private void DrawUseCases()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Use Cases");
            if (GUILayout.Button("+", GUILayout.MaxWidth(20)))
            {
                _manifest.useCases.Add("");
            }
            EditorGUILayout.EndHorizontal();
            for (int i = 0; i < _manifest.useCases.Count; i++)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                _manifest.useCases[i] = EditorGUILayout.TextField(_manifest.useCases[i]);
                if (GUILayout.Button("-", GUILayout.MaxWidth(20)))
                {
                    _manifest.useCases.RemoveAt(i);
                    EditorGUILayout.EndHorizontal();
                    return;
                }
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
            }
        }
        
        private void DrawDirections()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Instructions");
            if (GUILayout.Button("+", GUILayout.MaxWidth(20)))
            {
                _manifest.instructions.Add(new Instruction(string.Empty));
            }
            EditorGUILayout.EndHorizontal();
            
            for (int i = 0; i < _manifest.instructions.Count; i++)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                int label = i + 1;
                EditorGUILayout.LabelField(label + ".", GUILayout.Width(33));
                _manifest.instructions[i].text = EditorGUILayout.TextArea(_manifest.instructions[i].text);
                if (GUILayout.Button("+", GUILayout.MaxWidth(20)))
                {
                    _manifest.instructions[i].details.Add(string.Empty);
                }
                if (GUILayout.Button("-", GUILayout.MaxWidth(20)))
                {
                    _manifest.instructions.RemoveAt(i);
                    EditorGUILayout.EndHorizontal();
                    return;
                }
                EditorGUILayout.EndHorizontal();
                
                for (int detail = 0; detail < _manifest.instructions[i].details.Count; detail++)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(string.Empty, GUILayout.Width(35));
                    _manifest.instructions[i].details[detail] = EditorGUILayout.TextArea(_manifest.instructions[i].details[detail]);
                    if (GUILayout.Button("-", GUILayout.MaxWidth(20)))
                    {
                        _manifest.instructions[i].details.RemoveAt(detail);
                        EditorGUILayout.EndHorizontal();
                        return;
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
            }
        }
        
        private void DrawKeywords()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Keywords");
            if (GUILayout.Button("+", GUILayout.MaxWidth(20)))
            {
                _manifest.keywords.Add(string.Empty);
            }
            EditorGUILayout.EndHorizontal();
            for (int i = 0; i < _manifest.keywords.Count; i++)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                _manifest.keywords[i] = EditorGUILayout.TextField(_manifest.keywords[i]);
                if (GUILayout.Button("-", GUILayout.MaxWidth(20)))
                {
                    _manifest.keywords.RemoveAt(i);
                    EditorGUILayout.EndHorizontal();
                    return;
                }
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
            }
        }
        
        private void DrawSources()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Sources");
            if (GUILayout.Button("+", GUILayout.MaxWidth(20)))
            {
                _manifest.sources.Add(new PackageResource());
            }
            EditorGUILayout.EndHorizontal();
            for (int i = 0; i < _manifest.sources.Count; i++)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical();
                _manifest.sources[i].type = EditorGUILayout.TextField(_manifest.sources[i].type);
                _manifest.sources[i].url = DrawURLEditor(string.Empty, _manifest.sources[i].url);
                EditorGUILayout.EndVertical();
                if (GUILayout.Button("-", GUILayout.MaxWidth(20), GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * 2)))
                {
                    _manifest.sources.RemoveAt(i);
                    EditorGUILayout.EndHorizontal();
                    return;
                }
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
        }
        
        private void DrawDependencies()
        {
            EditorGUILayout.LabelField("Dependencies");
            foreach (var key in _manifest.dependencies.Keys)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.TextField(key);
                EditorGUILayout.TextField(_manifest.dependencies[key], GUILayout.MaxWidth(100));
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
        }

        private void DrawActionButtons()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Revert") && _manifest != null)
            {
                GUI.FocusControl(null);
                OpenWindow(ManifestFinder.FindManifest(_manifest.TextAsset));
            }
            if (GUILayout.Button("Apply") && _manifest != null)
            {
                GUI.FocusControl(null);
                _manifest.Save();
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space();
                
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Copy Markdown") && _manifest != null)
            {
                GUI.FocusControl(null);
                EditorGUIUtility.systemCopyBuffer = _manifest.GetMarkdownDocumentation(false);
            }
            if (GUILayout.Button("Copy HTML") && _manifest != null)
            {
                GUI.FocusControl(null);
                EditorGUIUtility.systemCopyBuffer = _manifest.GetHTMLDocumentation(false);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Refresh README") && _manifest != null)
            {
                GUI.FocusControl(null);
                _manifest.GenerateREADME();
            }
            if (GUILayout.Button("Export") && _manifest != null)
            {
                GUI.FocusControl(null);
                _manifest.Export();
            }
            if (GUILayout.Button("Open Package in Finder") && _manifest != null)
            {
                GUI.FocusControl(null);
                EditorUtility.RevealInFinder(_manifest.RelativeDirectory);
            }
            EditorGUILayout.EndHorizontal();
        }

        private string DrawURLEditor(string label, string url)
        {
            EditorGUILayout.BeginHorizontal();
            url = EditorGUILayout.TextField(label, url);
            EditorGUI.BeginDisabledGroup(string.IsNullOrWhiteSpace(url));
            if (GUILayout.Button("Open", GUILayout.Width(50)))
            {
                Application.OpenURL(url);
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
            return url;
        }
    }
}