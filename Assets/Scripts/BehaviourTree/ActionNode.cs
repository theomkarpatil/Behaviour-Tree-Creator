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
    /// Action nodes contain the primary logic of behaviour
    /// </summary>
    public abstract class ActionNode : Node
    {
        
    }

    public abstract class ActionNode<T> : Node where T : Blackboard
    {

    }
}