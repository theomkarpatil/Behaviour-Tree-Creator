// Developed by Sora
//
// Copyright(c) Sora Arts 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Sora.AI.BehaviourTrees;
using UnityEditor.Callbacks;

namespace Sora.AI.Editor
{
    public class BehaviourTreeEditor : EditorWindow
    {
        private BehaviourTreeView treeView;
        private InspectorView inspectorView;
        private IMGUIContainer blackboardView;

        SerializedObject blackboardObject;
        SerializedProperty blackboardProperty;

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChange;
            EditorApplication.playModeStateChanged += OnPlayModeStateChange;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChange;            
        }

        private void OnPlayModeStateChange(PlayModeStateChange change)
        {
            switch (change)
            {
                case PlayModeStateChange.EnteredEditMode:
                    OnSelectionChange();
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    OnSelectionChange();
                    break;
            }
        }

        /// <summary>
        /// Adds the Behaviour Tree to the Menu
        /// </summary>
        [MenuItem("Sora/BehaviourTreeEditor")]
        public static void OpenBTWindow()
        {
            BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
            wnd.titleContent = new GUIContent("BehaviourTreeEditor");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/BehaviourTreeEditor.uxml");
            visualTree.CloneTree(root);

            // Adding stylesheet
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviourTreeEditor.uss");
            root.styleSheets.Add(styleSheet);

            treeView = root.Q<BehaviourTreeView>();
            inspectorView = root.Q<InspectorView>();
            blackboardView = root.Q<IMGUIContainer>();
            

            treeView.onNodeSelected = OnNodeSelectionChanged;

            OnSelectionChange();
        }

        /// <summary>
        /// Called when a new Behaviour Tree is selected
        /// </summary>
        private void OnSelectionChange()
        {
            BehaviourTree tree = Selection.activeObject as BehaviourTree;

            if(!tree && Selection.activeGameObject != null)
            {
                BehaviourTreeRunner btRunner = Selection.activeGameObject.GetComponent<BehaviourTreeRunner>();

                if (btRunner)
                    tree = btRunner.tree;
            }

            if (Application.isPlaying && tree)
                treeView.PopulateView(tree);
            else if(tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
                treeView.PopulateView(tree);


            if (tree != null && tree.blackboard != null)
            {
                blackboardObject = new SerializedObject(tree.blackboard);
                blackboardProperty = blackboardObject.GetIterator();

                blackboardView.onGUIHandler = () =>
                {
                    blackboardObject.Update();

                    SerializedProperty property = blackboardObject.GetIterator();
                    property.NextVisible(true);

                    while (property.NextVisible(false))
                    {
                        EditorGUILayout.PropertyField(property, true);
                    }

                    blackboardObject.ApplyModifiedProperties();
                };
            }

            

            BehaviourTrees.Node node = Selection.activeObject as BehaviourTrees.Node;
            if(node)
            {
                NodeView nodeView = node.nodeView as NodeView;
                inspectorView.UpdateSelection(nodeView);

                treeView.ClearSelection();
                treeView.AddToSelection(nodeView);
                treeView.FrameSelection();
            }
        }

        /// <summary>
        /// adds details about the Node to the inspector view
        /// </summary>
        /// <param name="nodeView"></param>
        private void OnNodeSelectionChanged(NodeView nodeView)
        {
            inspectorView.UpdateSelection(nodeView);
        }

        /// <summary>
        /// Opens the behaviourTree Scriptable object asset using double clicks
        /// </summary>
        /// <returns></returns>
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            if(Selection.activeObject is BehaviourTree)
            {
                OpenBTWindow();
                return true;
            }

            return false;
        }

        private void OnInspectorUpdate()
        {
            treeView?.UpdateNodeState();
        }
    }
}