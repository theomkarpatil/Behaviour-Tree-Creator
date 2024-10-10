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
    /// Decorator nodes have exactly one Node as their child and modify how the this child nodes is executed in the tree
    /// </summary>
    public abstract class DecoratorNode : Node
    {
        public Node child;

        public override Node Clone()
        {
            DecoratorNode node = Instantiate(this);
            node.child.parentNode = node;
            node.child = child.Clone();

            return node;
        }
    }
}