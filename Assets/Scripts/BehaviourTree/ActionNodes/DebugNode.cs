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
    public class DebugNode : ActionNode
    {
        public string log;

        protected override void OnInstantiate()
        {

        }

        protected override void OnStart()
        {
            Debug.Log("OnStart: " + log);            
        }

        protected override void OnStop()
        {
            Debug.Log("OnStop: " + log);
        }

        protected override ENodeState OnUpdate()
        {
            Debug.Log("OnUpdate: " + log);

            return ENodeState.Succeeded;
        }
    }
}