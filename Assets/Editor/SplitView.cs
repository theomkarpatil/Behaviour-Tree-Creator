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
    /// Editor class to enable Split view in the BehaviourTree Window
    /// </summary>
    public class SplitView : TwoPaneSplitView
    {
        public new class UxmlFactory : UxmlFactory<SplitView, UxmlTraits> { }

    }
}