using System;
using System.Reflection;
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

        [MenuItem("Assets/Package Creator/Open Window", false, 0)]
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
            Current.name = manifest.DisplayName;
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
                
                _manifest.Name = EditorGUILayout.TextField("Name", _manifest.Name);
                _manifest.Version = EditorGUILayout.TextField("Version", _manifest.Version);
                _manifest.DisplayName = EditorGUILayout.TextField("Display Name", _manifest.DisplayName);
                _manifest.Author = EditorGUILayout.TextField("Author", _manifest.Author);
                _manifest.Unity = EditorGUILayout.TextField("Unity", _manifest.Unity);
                _manifest.Type = EditorGUILayout.TextField("Type", _manifest.Type);
                _manifest.License = EditorGUILayout.TextField("License", _manifest.License);
                _manifest.Homepage = EditorGUILayout.TextField("Homepage", _manifest.Homepage);
                _manifest.Bugs.URL = EditorGUILayout.TextField("Bugs", _manifest.Bugs.URL);
                _manifest.Repository.Type = EditorGUILayout.TextField("Repository Type", _manifest.Repository.Type);
                _manifest.Repository.URL = EditorGUILayout.TextField("Repository URL", _manifest.Repository.URL);

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
            EditorGUILayout.BeginVertical(GUILayout.Height(50));
            _descriptionScroll = EditorGUILayout.BeginScrollView(_descriptionScroll, GUILayout.Height(50));
            _manifest.Description = EditorGUILayout.TextArea(_manifest.Description, GUILayout.Height(50));
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
        
        private void DrawUseCases()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Use Cases");
            if (GUILayout.Button("+", GUILayout.MaxWidth(20)))
            {
                _manifest.UseCases.Add("");
            }
            EditorGUILayout.EndHorizontal();
            for (int i = 0; i < _manifest.UseCases.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                _manifest.UseCases[i] = EditorGUILayout.TextField(_manifest.UseCases[i]);
                if (GUILayout.Button("-", GUILayout.MaxWidth(20)))
                {
                    _manifest.UseCases.RemoveAt(i);
                    EditorGUILayout.EndHorizontal();
                    return;
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        
        private void DrawDirections()
        {
            EditorStyles.textField.wordWrap = true;
            EditorGUILayout.LabelField("Directions");
            EditorGUILayout.BeginVertical(GUILayout.Height(100));
            _directionsScroll = EditorGUILayout.BeginScrollView(_directionsScroll, GUILayout.Height(100));
            _manifest.Directions = EditorGUILayout.TextArea(_manifest.Directions, GUILayout.Height(100));
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
        
        private void DrawKeywords()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Keywords");
            if (GUILayout.Button("+", GUILayout.MaxWidth(20)))
            {
                _manifest.Keywords.Add("");
            }
            EditorGUILayout.EndHorizontal();
            for (int i = 0; i < _manifest.Keywords.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                _manifest.Keywords[i] = EditorGUILayout.TextField(_manifest.Keywords[i]);
                if (GUILayout.Button("-", GUILayout.MaxWidth(20)))
                {
                    _manifest.Keywords.RemoveAt(i);
                    EditorGUILayout.EndHorizontal();
                    return;
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        
        private void DrawSources()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Sources");
            if (GUILayout.Button("+", GUILayout.MaxWidth(20)))
            {
                _manifest.Sources.Add(new PackageResource());
            }
            EditorGUILayout.EndHorizontal();
            for (int i = 0; i < _manifest.Sources.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical();
                _manifest.Sources[i].Type = EditorGUILayout.TextField(_manifest.Sources[i].Type);
                _manifest.Sources[i].URL = EditorGUILayout.TextField(_manifest.Sources[i].URL);
                EditorGUILayout.EndVertical();
                if (GUILayout.Button("-", GUILayout.MaxWidth(20), GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * 2)))
                {
                    _manifest.Sources.RemoveAt(i);
                    EditorGUILayout.EndHorizontal();
                    return;
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }
        }
        
        private void DrawDependencies()
        {
            EditorGUILayout.LabelField("Dependencies");
            foreach (var key in _manifest.Dependencies.Keys)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.TextField(key);
                EditorGUILayout.TextField(_manifest.Dependencies[key], GUILayout.MaxWidth(100));
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();
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
        }
    }
}