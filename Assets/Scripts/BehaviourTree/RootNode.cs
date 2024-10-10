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
    public class RootNode : Node
    {
        public Node child;

        protected override void OnInstantiate()
        {

        }

        protected override void OnStart()
        {
        
        }

        protected override void OnStop()
        {

        }

        protected override ENodeState OnUpdate()
        {
            return child.Update();
        }

        public override Node Clone()
        {
            RootNode node = Instantiate(this);
            node.child = child.Clone();

            return node;
        }
    }
}