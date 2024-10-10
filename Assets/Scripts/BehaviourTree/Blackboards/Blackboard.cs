// Developed by Sora
//
// Copyright(c) Sora 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sora.AI.BehaviourTrees
{
    /// <summary>
    /// The blackboard acts as a common interface between various methods and classes for a single behaviour tree
    /// Blackboard methods can be created for each BehaviourTree with it's own required shared variables
    /// This blackboard and it's variables can be accessed in the BehaviourTree editor as well as in individual nodes
    /// </summary>
    [System.Serializable]
    public class Blackboard : ScriptableObject
    {
        public BehaviourTree tree;
    }
}