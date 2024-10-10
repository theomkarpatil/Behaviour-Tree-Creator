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

    public class MoveTowardsTarget : ActionNode
    {
        [SerializeField] private float movementSpeed;
        private BlackBoardEnemy bb;

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
            if (!Mathf.Approximately(Vector3.Distance(transform.position, bb.target.position), 0.0f))
            {
                transform.position = Vector3.MoveTowards(transform.position, bb.target.position, movementSpeed * Time.deltaTime);

                bb.movingTowardsTarget = true;
                return ENodeState.Running;
            }
                return ENodeState.Succeeded;
        }
    }
}