using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Object = UnityEngine.Object;
using UnityObject = UnityEngine.Object;

namespace Unity.VisualScripting
{
    internal class DropdownOptions
    {
        internal bool interactable = true;
        internal string label;
        internal string tooltip;
        internal Action callback;
    }

    internal class SplitDropdown
    {
        private bool reset;
        private List<DropdownOptions> listOfOptions;

        internal SplitDropdown(List<DropdownOptions> listOfOptions)
        {
            this.listOfOptions = listOfOptions;
        }

        internal DropdownOptions GetOption(int index)
        {
            return listOfOptions[index];
        }

        internal void Reset()
        {
            reset = true;
        }

        internal bool Draw(Rect area, string label, ref bool toggleState)
        {
            GUI.Box(area, string.Empty, VisualScriptingWelcomeWindow.buttonStyle);

            if (GUI.Button(new Rect(area.x, area.y, area.width - area.height, area.height), label,
                VisualScriptingWelcomeWindow.labelStyleButton))
            {
                toggleState = false;

                return true;
            }

            Rect toggleRect = new Rect(area.x + area.width - area.height - 2, area.y - 1, area.height + 3,
                area.height + 3);

            toggleState = GUI.Toggle(toggleRect, toggleState, "", VisualScriptingWelcomeWindow.labelStyleButton);

            GUI.Label(toggleRect, VisualScriptingWelcomeWindow.dropdownIcon, VisualScriptingWelcomeWindow.titleStyle);

            if (toggleState)
            {
                GUI.BeginGroup(new Rect(area.x, area.y + area.height - 1, area.width, area.height * 2),
                    VisualScriptingWelcomeWindow.buttonStyle);
                Rect areaChild = new Rect(0, 0, VisualScriptingWelcomeWindow.buttonWidth,
                    VisualScriptingWelcomeWindow.buttonHeight);

                GUIContent contentOption;

                foreach (DropdownOptions dropdownOption in listOfOptions)
                {
                    EditorGUI.BeginDisabledGroup(!dropdownOption.interactable);

                    if (dropdownOption.interactable)
                    {
                        contentOption = new GUIContent(dropdownOption.label);
                    }
                    else
                    {
                        contentOption = new GUIContent(dropdownOption.label, dropdownOption.tooltip);
                    }

                    if (GUI.Button(areaChild, contentOption, VisualScriptingWelcomeWindow.labelStyleDropdownOptions))
                    {
                        toggleState = false;

                        dropdownOption.callback();
                    }

                    EditorGUI.EndDisabledGroup();

                    areaChild.y += areaChild.height;
                }

                GUI.EndGroup();
            }

            if (reset)
            {
                toggleState = false;
                reset = false;
            }

            return false;
        }
    }

    internal class VisualScriptingWelcomeWindow : EditorWindow
    {
        internal static GUIContent dropdownIcon;

        private bool toggleOnScript = false;
        private bool toggleOnState = false;

        internal static GUIStyle buttonStyle;
        internal static GUIStyle labelStyleButton;
        internal static GUIStyle labelStyleDropdownOptions;
        internal static GUIStyle titleStyle;

        internal static int titleHeight = 120;
        internal static int buttonWidth = 200;
        internal static int buttonHeight = 22;

        private bool shouldCloseWindow;
        private Vector2 scrollPosition;
        private Rect scrollArea;

        private SplitDropdown splitDropdownScriptGraph;
        private SplitDropdown splitDropdownStateGraph;

        [MenuItem("Window/Visual Scripting/Visual Scripting Graph", false, 3010)]
        private static void ShowWindow()
        {
            VisualScriptingWelcomeWindow window = GetWindow<VisualScriptingWelcomeWindow>();

            window.titleContent = new GUIContent("Visual Scripting");
        }

        private void OnEnable()
        {
            string pathRoot = PathUtility.GetPackageRootPath();

            Object icon = EditorGUIUtility.Load(Path.Combine(pathRoot,
                "Editor/VisualScripting.Welcome/EditorAssetResources/SplitButtonArrow.png"));

            dropdownIcon = new GUIContent(icon as Texture2D);

            scrollArea = new Rect(0, 0, 630, 300);

            toggleOnScript = false;
            toggleOnState = false;
            shouldCloseWindow = false;

            List<DropdownOptions> listOfOptions = new List<DropdownOptions>();
            listOfOptions.Add(new DropdownOptions()
            {
                label = "...on selected game object",
                tooltip = "Please, select a gameObject",
                callback = CreateScriptGraphOnSelectedGameObject
            });
            listOfOptions.Add(new DropdownOptions()
            { label = "...on new game object", callback = CreateScriptGraphOnNewGameObject });

            splitDropdownScriptGraph = new SplitDropdown(listOfOptions);

            listOfOptions = new List<DropdownOptions>();
            listOfOptions.Add(new DropdownOptions()
            {
                label = "...on selected game object",
                tooltip = "Please, select a gameObject",
                callback = CreateStateGraphOnSelectedGameObject
            });
            listOfOptions.Add(new DropdownOptions()
            { label = "...on new game object", callback = CreateStateGraphOnNewGameObject });

            splitDropdownStateGraph = new SplitDropdown(listOfOptions);
        }

        private void OpenGraphAsset(UnityObject unityObject)
        {
            shouldCloseWindow = true;

            ScriptGraphAsset scriptGraphAsset = unityObject as ScriptGraphAsset;

            GraphReference graphReference = null;

            if (scriptGraphAsset != null)
            {
                graphReference = GraphReference.New(scriptGraphAsset, true);
            }
            else
            {
                StateGraphAsset stateGraphAsset = unityObject as StateGraphAsset;

                if (stateGraphAsset != null)
                {
                    graphReference = GraphReference.New(stateGraphAsset, true);
                }
            }

            GraphWindow.OpenActive(graphReference);
        }

        private void OpenGraphFromPath(string path)
        {
            path = path.Replace(Application.dataPath, "Assets");

            UnityObject unityObject = AssetDatabase.LoadAssetAtPath(path, typeof(UnityObject));

            OpenGraphAsset(unityObject);
        }

        private void OpenGraph()
        {
            EditorGUIUtility.ShowObjectPicker<MacroScriptableObject>(null, false, String.Empty,
                EditorGUIUtility.GetControlID(FocusType.Passive));
        }

        private bool CreateScriptGraphAsset(GameObject gameObject = null, bool updateName = false)
        {
            var path = EditorUtility.SaveFilePanelInProject("Save Graph", "New Script Graph", "asset", null);

            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            var macro = (IMacro)ScriptableObject.CreateInstance(typeof(ScriptGraphAsset));
            var macroObject = (UnityObject)macro;
            macro.graph = FlowGraph.WithStartUpdate();

            if (gameObject != null)
            {
                ScriptMachine flowMachine = gameObject.AddComponent<ScriptMachine>();

                flowMachine.nest.macro = (ScriptGraphAsset)macro;
            }

            string filename = Path.GetFileNameWithoutExtension(path);

            if (updateName)
            {
                gameObject.name = filename;
            }

            macroObject.name = filename;

            AssetDatabase.CreateAsset(macroObject, path);

            OpenGraphFromPath(path);

            return true;
        }

        private void CreateScriptGraph()
        {
            Selection.activeGameObject = null;

            if (CreateScriptGraphAsset())
            {
                shouldCloseWindow = true;
            }
        }

        private void CreateScriptGraphOnNewGameObject()
        {
            Selection.activeGameObject = null;

            GameObject newGameObject = new GameObject();

            if (!CreateScriptGraphAsset(newGameObject, true))
            {
                DestroyImmediate(newGameObject);
            }
            else
            {
                shouldCloseWindow = true;
            }
        }

        private void CreateScriptGraphOnSelectedGameObject()
        {
            if (Selection.activeGameObject != null)
            {
                if (CreateScriptGraphAsset(Selection.activeGameObject))
                {
                    shouldCloseWindow = true;
                }
            }
        }

        private bool CreateStateGraphAsset(GameObject gameObject = null, bool updateName = false)
        {
            var path = EditorUtility.SaveFilePanelInProject("Save Graph", "New State Graph", "asset", null);

            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            var macro = (IMacro)ScriptableObject.CreateInstance(typeof(StateGraphAsset));
            var macroObject = (UnityObject)macro;
            macro.graph = StateGraph.WithStart();

            if (gameObject != null)
            {
                StateMachine stateMachine = gameObject.AddComponent<StateMachine>();

                stateMachine.nest.macro = (StateGraphAsset)macro;
            }

            string filename = Path.GetFileNameWithoutExtension(path);

            if (updateName)
            {
                gameObject.name = filename;
            }

            macroObject.name = filename;

            AssetDatabase.CreateAsset(macroObject, path);

            OpenGraphFromPath(path);

            return true;
        }

        private void CreateStateGraph()
        {
            Selection.activeGameObject = null;

            if (CreateStateGraphAsset())
            {
                shouldCloseWindow = true;
            }
        }

        private void CreateStateGraphOnNewGameObject()
        {
            Selection.activeGameObject = null;

            GameObject newGameObject = new GameObject();

            if (!CreateStateGraphAsset(newGameObject, true))
            {
                DestroyImmediate(newGameObject);
            }
            else
            {
                shouldCloseWindow = true;
            }
        }

        private void CreateStateGraphOnSelectedGameObject()
        {
            if (Selection.activeGameObject != null)
            {
                if (CreateStateGraphAsset(Selection.activeGameObject))
                {
                    shouldCloseWindow = true;
                }
            }
        }

        private void CreateStyles()
        {
            if (buttonStyle == null)
            {
                buttonStyle = new GUIStyle("Button");
            }

            if (labelStyleButton == null)
            {
                labelStyleButton = new GUIStyle("Label") { alignment = TextAnchor.MiddleCenter };
            }

            if (labelStyleDropdownOptions == null)
            {
                labelStyleDropdownOptions = new GUIStyle("Label")
                {
                    alignment = TextAnchor.MiddleLeft,
                    padding = new RectOffset(20, 0, 0, 0)
                };
            }

            if (titleStyle == null)
            {
                titleStyle = new GUIStyle("Label") { alignment = TextAnchor.MiddleCenter, fontSize = 20 };
            }
        }

        private void ResetToggle()
        {
            if (Event.current.rawType == EventType.MouseUp)
            {
                if (toggleOnScript)
                {
                    splitDropdownScriptGraph.Reset();

                    Repaint();
                }

                if (toggleOnState)
                {
                    splitDropdownStateGraph.Reset();

                    Repaint();
                }
            }
        }

        private void OpenGraphFromPicker()
        {
            if (Event.current.commandName == "ObjectSelectorUpdated")
            {
                UnityObject selectedObject = EditorGUIUtility.GetObjectPickerObject();

                OpenGraphAsset(selectedObject);
            }
        }

        private void OnGUI()
        {
            CreateStyles();

            ResetToggle();

            DropAreaGUI();

            OpenGraphFromPicker();

            scrollPosition = GUI.BeginScrollView(new Rect(0, 0, position.width, position.height), scrollPosition,
                scrollArea);
            GUILayout.BeginVertical(GUILayout.ExpandHeight(true));

            Vector2 groupSize = new Vector2(scrollArea.width, 280);

            GUI.BeginGroup(new Rect((position.width / 2) - (groupSize.x / 2), (position.height / 2) - (groupSize.y / 2),
                groupSize.x, groupSize.y));

            GUI.Label(new Rect(0, 0, groupSize.x, titleHeight), "Drag and drop a Visual Scripting Graph asset here\nor",
                titleStyle);

            int buttonX = 10;

            if (GUI.Button(new Rect(buttonX, titleHeight, buttonWidth, buttonHeight), "Browse to open a Graph"))
            {
                OpenGraph();
            }

            buttonX += (buttonWidth + 10);

            Rect area = new Rect(buttonX, titleHeight, buttonWidth, buttonHeight);

            splitDropdownScriptGraph.GetOption(0).interactable = Selection.activeGameObject != null;

            if (splitDropdownScriptGraph.Draw(area, "Create new Script Graph", ref toggleOnScript))
            {
                CreateScriptGraph();
            }

            if (toggleOnScript)
            {
                toggleOnState = false;
            }

            buttonX += (buttonWidth + 10);

            area = new Rect(buttonX, titleHeight, buttonWidth, buttonHeight);

            splitDropdownStateGraph.GetOption(0).interactable = Selection.activeGameObject != null;

            if (splitDropdownStateGraph.Draw(area, "Create new State Graph", ref toggleOnState))
            {
                CreateStateGraph();
            }

            if (toggleOnState)
            {
                toggleOnScript = false;
            }

            GUI.EndGroup();
            GUILayout.EndVertical();
            GUI.EndScrollView();

            if (shouldCloseWindow)
            {
                Close();
            }
        }

        private void DropAreaGUI()
        {
            Event currentEvent = Event.current;

            Rect activeArea = GUILayoutUtility.GetRect(0, 0, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            switch (currentEvent.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!activeArea.Contains(currentEvent.mousePosition))
                    {
                        return;
                    }

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (currentEvent.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();

                        foreach (UnityObject draggedObject in DragAndDrop.objectReferences)
                        {
                            ScriptGraphAsset scriptGraphAsset = draggedObject as ScriptGraphAsset;

                            if (scriptGraphAsset != null)
                            {
                                shouldCloseWindow = true;

                                GraphWindow.OpenActive(GraphReference.New(scriptGraphAsset, true));
                                break;
                            }
                        }
                    }

                    break;
            }
        }
    }
}
