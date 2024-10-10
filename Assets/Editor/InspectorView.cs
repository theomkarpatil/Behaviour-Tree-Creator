// Developed by Sora
//
// Copyright(c) Sora 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine;

namespace Sora.AI.Editor
{
    /// <summary>
    /// Editor class to enable an Inspector view in the BehaviourTree Window
    /// </summary>
    public class InspectorView : VisualElement
    {
        private UnityEditor.Editor editor;

        public new class UxmlFactory : UxmlFactory<InspectorView, UxmlTraits> { }

        public InspectorView()
        {

        }

        internal void UpdateSelection(NodeView nodeView)
        {
            Clear();

            UnityEngine.Object.DestroyImmediate(editor);
            editor = UnityEditor.Editor.CreateEditor(nodeView.node);
            IMGUIContainer imgui = new IMGUIContainer(() => 
            { 
                if(editor.target)
                    editor.OnInspectorGUI(); 
            });
            Add(imgui);
        }
    }
}