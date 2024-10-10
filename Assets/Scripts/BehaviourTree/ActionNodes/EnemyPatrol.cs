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
    
    public class EnemyPatrol : ActionNode
    {
        [SerializeField] private Vector3[] wayPoints;
        [SerializeField] private float movementSpeed;
        private BlackBoardEnemy bb;
        private int index;

        protected override void OnInstantiate()
        {
            index = 0;
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
            if (wayPoints.Length == 0 || bb.movingTowardsTarget)
                return ENodeState.Failed;

            if (!Mathf.Approximately(Vector3.Distance(transform.position, wayPoints[index % wayPoints.Length]), 0.0f))
            {
                transform.position = Vector3.MoveTowards(transform.position, wayPoints[index % wayPoints.Length], movementSpeed * Time.deltaTime);
                return ENodeState.Running;
            }

            index++;
            return ENodeState.Running;
        }
    }
}