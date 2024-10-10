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
    public abstract class CoroutineNode : Node
    {
        protected IEnumerator coroutineToRun;
        protected bool isRunning;

        protected override void OnInstantiate()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnStart()
        {
            isRunning = false;
            coroutineToRun = CoroutineToRun();
        }

        protected override void OnStop()
        {
            if (coroutineToRun != null)
            {
                BTCoroutineRunner.instance.StopBTCoroutine(coroutineToRun);
                coroutineToRun = null;
            }

            isRunning = false;
        }

        /// <summary>
        /// We want OnUpdate to run only once for a Coroutine Node. 
        /// The actual update happens from the CoroutineWrapper
        /// </summary>
        /// <returns></returns>
        protected override ENodeState OnUpdate()
        {
            if (!isRunning)
            {
                isRunning = true;
                BTCoroutineRunner.instance.StartBTCoroutine(CoroutineWrapper());
            }

            return state;
        }

        private IEnumerator CoroutineWrapper()
        {
            yield return BTCoroutineRunner.instance.StartCoroutine(coroutineToRun);
            if (state == ENodeState.Running)
            {
                state = ENodeState.Succeeded;
            }
        }

        /// <summary>
        /// The method that needs to be overridden with actual coroutine logic for the behaviour
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerator CoroutineToRun();
    }
}