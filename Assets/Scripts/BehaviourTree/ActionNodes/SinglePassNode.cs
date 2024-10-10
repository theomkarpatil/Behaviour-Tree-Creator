// Developed by Sora
//
// Copyright(c) Sora 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using Sora.AI.BehaviourTrees;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sora.AI
{
    public class SinglePassNode : BehaviourTrees.ActionNode
    {
        protected override void OnInstantiate()
        {

        }

        protected override void OnStart()
        {
            
        }

        protected override void OnStop()
        {
            blackboard.tree.DeleteChild(parentNode, this);
        }

        protected override ENodeState OnUpdate()
        {
            return ENodeState.Succeeded;
        }
    }
}