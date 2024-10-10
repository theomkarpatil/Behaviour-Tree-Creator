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
    /// The primary monobehavior that runs all Behaviour Trees in the scene 
    /// </summary>
    public class BehaviourTreeRunner : MonoBehaviour
    {
        public BehaviourTree tree;

        private void Start()
        {
            tree.transform = transform;
            tree = tree.Clone();
            tree.InitializeNodes();
        }

        private void Update()
        {
            tree.Update();
        }
    }
}