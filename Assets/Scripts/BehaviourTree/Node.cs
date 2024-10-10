// Developed by Sora
//
// Copyright(c) Sora 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using UnityEngine;

namespace Sora.AI.BehaviourTrees
{
    /// <summary>
    /// The three main states a Node can exist in
    /// Running: While running, the tree will remain in the current state
    /// Failed: If Failed, the tree will continue to another Node at the same depth
    /// Succeeded: If Succeeded, the next child Node (unless this Node is a child of a ControlFlowNode) or the next state in queue will execute
    /// </summary>
    public enum ENodeState
    {
        Running,
        Failed,
        Succeeded
    }

    /// <summary>
    /// The Base Class for all Behaviour Tree Nodes
    /// </summary>
    public abstract class Node : ScriptableObject
    {
        [HideInInspector] public string guid;
        [HideInInspector] public Vector2 position;
        [HideInInspector] public object nodeView;

        public Node parentNode;

        public ENodeState state = ENodeState.Running;
        public bool hasStarted = false;
        public bool firstPass = true;

        [TextArea] public string description;
        [HideInInspector] public Blackboard blackboard;
        [HideInInspector] public Transform transform;
        
        public ENodeState Update()
        {
            if(firstPass)
            {
                OnInstantiate();
                firstPass = false;
            }

            // OnStart is only called once per iteration when a Node is first called
            if(!hasStarted)
            {
                OnStart();
                hasStarted = true;
            }

            // The Update state can be called multiple times while the Node runs
            state = OnUpdate();

            // When a Node reaches either the Failed of Succeeded state, OnStop() is called
            if (state == ENodeState.Failed || state == ENodeState.Succeeded)
            {
                OnStop();
                hasStarted = false;
            }

            // return the current state
            return state;
        }

        public virtual Node Clone()
        {
            return Instantiate(this);
        }

        /// <summary>
        /// OnStart is only called once when the tree traversal first begins
        /// </summary>
        protected virtual void OnInstantiate()
        {

        }

        /// <summary>
        /// OnStart is only called once per iteration when a Node is first called
        /// </summary>
        protected virtual void OnStart()
        {

        }

        /// <summary>
        /// The Update state can be called multiple times while the Node runs
        /// </summary>
        protected virtual void OnStop()
        {

        }

        /// <summary>
        /// When a Node reaches either the Failed of Succeeded state, OnStop() is called
        /// </summary>
        /// <returns> Returns the current state of the Node </returns>
        protected virtual ENodeState OnUpdate()
        {
            return ENodeState.Succeeded;
        }
    }

    public class Node<T> : Node where T : Blackboard
    {
        public new T blackboard
        {
            get => base.blackboard as T;
            set => base.blackboard = value;
        }
    }
}