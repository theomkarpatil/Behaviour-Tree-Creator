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
    // Executes node from left to right in sequence
    // EXECUTION STOPS WHEN ANY NODE RETURNS FAILED
    public class SequencerNode : ControlFlowNode
    {
        int currentChild;

        protected override void OnInstantiate()
        {

        }

        protected override void OnStart()
        {
            currentChild = 0;
        }

        protected override void OnStop()
        {

        }

        protected override ENodeState OnUpdate()
        {
            Node child = children[currentChild];

            switch(child.Update())
            {
                case ENodeState.Running:
                    {
                        currentChild++;
                    }
                    break;
                case ENodeState.Failed:
                    return ENodeState.Failed;
                case ENodeState.Succeeded:
                    {
                        currentChild++;
                    }
                        break;
            }

            return currentChild == children.Count ? ENodeState.Succeeded : ENodeState.Running;
        }
    }
}