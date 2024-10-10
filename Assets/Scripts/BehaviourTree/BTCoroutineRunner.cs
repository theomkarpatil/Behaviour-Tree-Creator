// Developed by Sora
//
// Copyright(c) Sora 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using System.Collections;

namespace Sora.AI.BehaviourTrees
{
    public class BTCoroutineRunner : Managers.Singleton<BTCoroutineRunner>
    {
        public void StartBTCoroutine(IEnumerator coroutine)
        {
            StartCoroutine(coroutine);
        }

        public void StopBTCoroutine(IEnumerator coroutine)
        {
            StopCoroutine(coroutine);
        }
    }
}