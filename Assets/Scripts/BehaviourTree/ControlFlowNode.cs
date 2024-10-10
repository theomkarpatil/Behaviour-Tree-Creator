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
    /// Allows for controlling how the Nodes are traversed through in a Tree
    /// </summary>
    public abstract class ControlFlowNode : Node
    {
        public List<Node> children = new List<Node>();

        public override Node Clone()
        {
            ControlFlowNode node = Instantiate(this);
            node.children.ForEach(n => n.parentNode = node);
            node.children =  children.ConvertAll(child => child.Clone());

            return node;
        }
    }
}