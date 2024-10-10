// Developed by Sora
//
// Copyright(c) Sora 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using System;
using UnityEngine;
using Sora.AI.BehaviourTrees;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Sora.AI.Editor
{
    /// <summary>
    /// Editor class that creates Nodes inside Graphs
    /// </summary>
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        public BehaviourTrees.Node node;

        public int int1;
        public int int2;

        public Port inputPort;
        public Port outputPort;

        public Action<NodeView> onNodeSelected;

        /// <summary>
        /// Creates individual nodes in the editor for a Node specified by the node parameter
        /// </summary>
        /// <param name="node"> The current node and it's properties that need to be shown on the editor </param>
        public NodeView(BehaviourTrees.Node node, string behaviorTreeName = "") : base("Assets/Editor/NodeView.UXML")
        {
            this.node = node;

            if (this.node is RootNode)
                this.title = behaviorTreeName + ": " + node.name;
            else
                this.title = node.name;
            this.viewDataKey = node.guid;
            node.nodeView = this;

            style.left = node.position.x;
            style.top = node.position.y;

            CreateInputPorts();
            CreateOutputPorts();
            SetupClasses();

            Label descriptionLabel = this.Q<Label>("description");
            descriptionLabel.bindingPath = "description";
            descriptionLabel.Bind(new SerializedObject(node));
        }

        /// <summary>
        /// Saves the position whenever a node is moved 
        /// </summary>
        /// <param name="newPos"></param>
        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);

            Undo.RecordObject(node, "BehaviorTree:: (Reset Position)");

            node.position.x = newPos.x;
            node.position.y = newPos.y;

            EditorUtility.SetDirty(node);
        }

        /// <summary>
        /// Creates input ports for each Node
        /// </summary>
        private void CreateInputPorts()
        {
            // Rootnode is the only Node without an input node
            if(node is not RootNode)
                inputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));

            if (inputPort != null)
            {
                inputPort.portName = "";
                inputPort.style.flexDirection = FlexDirection.Row;
                inputPort.style.paddingLeft = 10.0f;

                var connectorBox = inputPort.Q("connector");
                if (connectorBox != null)
                {
                    connectorBox.style.width = 16;
                    connectorBox.style.height = 16;
                }

                var connectorCap = inputPort.Q("cap");

                if (connectorCap != null)
                {
                    connectorCap.style.width = 6;
                    connectorCap.style.height = 6;
                }

                inputContainer.Add(inputPort);
            }

        }

        /// <summary>
        /// Creates output ports for each Node
        /// </summary>
        private void CreateOutputPorts()
        {
            if (node is RootNode)
            {
                outputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
            }
            if (node is ControlFlowNode)
            {
                outputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
            }
            else if (node is DecoratorNode)
            {
                outputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
            }

            if (outputPort != null)
            {
                outputPort.portName = "";
                outputPort.style.flexDirection = FlexDirection.RowReverse;
                outputPort.style.bottom = -2.5f;
                outputPort.style.paddingRight = 16.0f;

                var connectorBox = outputPort.Q("connector");
                if (connectorBox != null)
                {
                    connectorBox.style.width = 16;
                    connectorBox.style.height = 16;
                }

                var connectorCap = outputPort.Q("cap");

                if (connectorCap != null)
                {
                    connectorCap.style.width = 6;
                    connectorCap.style.height = 6;
                }

                outputContainer.Add(outputPort);
            }
        }

        public override void OnSelected()
        {
            base.OnSelected();

            if (onNodeSelected != null)
                onNodeSelected.Invoke(this);            
        }

        private void SetupClasses()
        {
            if (node is RootNode)
            {
                AddToClassList("root");

            }
            else if (node is ControlFlowNode)
            {
                AddToClassList("controlFlow");
            }
            else if (node is DecoratorNode)
            {
                AddToClassList("decorator");
            }
            else if (node is ActionNode)
            {
                AddToClassList("action");
            }
            else if (node is CoroutineNode)
            {
                AddToClassList("coroutine");
            }
        }

        public void SortChildren()
        {
            ControlFlowNode _node = node as ControlFlowNode;
            if(_node)
            {
                _node.children.Sort(SortByHorizontalPosition);
            }
        }

        private int SortByHorizontalPosition(BehaviourTrees.Node left, BehaviourTrees.Node right)
        {
            return left.position.x < right.position.x ? -1 : 1;
        }

        public void UpdateState()
        {
            if (Application.isPlaying)
            {
                RemoveFromClassList("running");
                RemoveFromClassList("failed");
                RemoveFromClassList("succeeded");

                switch (node.state)
                {
                    case ENodeState.Running:
                        if(node.hasStarted)
                            AddToClassList("running");
                        break;
                    case ENodeState.Failed:
                        AddToClassList("failed");
                        break;
                    case ENodeState.Succeeded:
                        AddToClassList("succeeded");
                        break;
                }
            }
        }

        
    }
}