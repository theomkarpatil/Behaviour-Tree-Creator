// Developed by Sora
//
// Copyright(c) Sora Arts 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sora.AI.BehaviourTrees
{
    public class LookForTarget : ActionNode
    {
        private BlackBoardEnemy bb;
        [SerializeField] private LayerMask playerMask;

        protected override void OnInstantiate()
        {
            bb = blackboard as BlackBoardEnemy;
        }

        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {

        }

        protected override ENodeState OnUpdate()
        {
            if (Physics.Raycast(transform.position, bb.target.position - transform.position, out RaycastHit info, playerMask) && info.transform.CompareTag("Player"))
            {
                return ENodeState.Succeeded;
            }
            else
            {
                bb.movingTowardsTarget = false;
                return ENodeState.Failed;
            }
        }
    }
}