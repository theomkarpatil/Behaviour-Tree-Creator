// Developed by Sora
//
// Copyright(c) Sora 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using Sora.AI.BehaviourTrees;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Sora.AI.Editor
{
    /// <summary>
    /// Editor class to that modifies how the entire Graph displays individual nodes based on Behaviour Tree's requirement
    /// </summary>
    public class BehaviourTreeView : GraphView
    {   
        public new class UxmlFactory : UxmlFactory<BehaviourTreeView, UxmlTraits> { }
        private BehaviourTree behaviourTree;

        public Action<NodeView> onNodeSelected;

        public BehaviourTreeView()
        {
            Insert(0, new GridBackground());

            // Add basic graphView related modifiers
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            // Adding stylesheet
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviourTreeEditor.uss");
            styleSheets.Add(styleSheet);

            Undo.undoRedoPerformed += OnUndoRedo;
        }

        private void OnUndoRedo()
        {
            PopulateView(behaviourTree);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// Clear out the graph view and populate with the new selected behaviour tree
        /// </summary>
        /// <param name="tree"></param>
        internal void PopulateView(BehaviourTree tree)
        {
            behaviourTree = tree;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            if(tree.rootNode == null)
            {
                tree.rootNode = tree.CreateNode(typeof(RootNode)) as RootNode;
                EditorUtility.SetDirty(tree);
                AssetDatabase.SaveAssets();
            }

            tree.nodes.ForEach(n => CreateNodeView(n, null));
            tree.nodes.ForEach(n =>
            {
                var children = tree.GetChildren(n);
                children.ForEach(c =>
                {
                    NodeView parentView = GetNodeByGuid(n.guid) as NodeView;
                    NodeView childView = GetNodeByGuid(c.guid) as NodeView;

                    Edge edge = parentView.outputPort.ConnectTo(childView.inputPort);
                    AddElement(edge);
                }); 
            });
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null)
            {
                graphViewChange.elementsToRemove.ForEach(element =>
                {
                    NodeView nodeview = element as NodeView;
                    if (nodeview != null)
                        behaviourTree.DeleteNode(nodeview.node);

                    Edge edge = element as Edge;
                    if(edge != null)
                    {
                        NodeView parentView = edge.output.node as NodeView;
                        NodeView childView = edge.input.node as NodeView;

                        behaviourTree.DeleteChild(parentView.node, childView.node);
                    }
                });
            }

            if (graphViewChange.edgesToCreate != null)
            {
                graphViewChange.edgesToCreate.ForEach(edge =>
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;

                    behaviourTree.AddChild(parentView.node, childView.node);
                });
            }

            if(graphViewChange.movedElements != null)
            {
                nodes.ForEach((n) =>
                {
                    NodeView view = n as NodeView;
                    view.SortChildren();
                });
            }

            return graphViewChange;
        }

        /// <summary>
        /// Add nodes to trees
        /// </summary>
        /// <param name="node"></param>
        void CreateNodeView(BehaviourTrees.Node node, object data)
        {
            NodeView nodeView = new NodeView(node, behaviourTree.name);

            if (data != null)
            {
                Rect rect = new Rect();
                Vector2[] mousePos = data as Vector2[];
                rect.x = mousePos[0].x;
                rect.y = mousePos[0].y;
                nodeView.SetPosition(rect);
            }

            nodeView.onNodeSelected = onNodeSelected;
            Focus();

            AddElement(nodeView);
        }

        /// <summary>
        /// Modifies what we see when we right click inside the graph view
        /// </summary>
        /// <param name="evt"></param>
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            Vector2 graphViewMousePosition = viewTransform.matrix.inverse.MultiplyPoint(evt.localMousePosition);

            var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"{type.BaseType.Name} / {type.Name}", (a) => CreateNode(type, graphViewMousePosition));
            }
            evt.menu.AppendSeparator();            
            
            types = TypeCache.GetTypesDerivedFrom<ControlFlowNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"{type.BaseType.Name} / {type.Name}", (a) => CreateNode(type, graphViewMousePosition));
            }
            evt.menu.AppendSeparator();

            types = TypeCache.GetTypesDerivedFrom<ActionNode>();
            foreach(var type in types)
            {
                evt.menu.AppendAction($"{type.BaseType.Name} / {type.Name}", (a) => CreateNode(type, graphViewMousePosition));
            }
            evt.menu.AppendSeparator();

            types = TypeCache.GetTypesDerivedFrom<CoroutineNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"{type.BaseType.Name} / {type.Name}", (a) => CreateNode(type, graphViewMousePosition));
            }
        }

        /// <summary>
        /// Creates nodes inside the graph
        /// </summary>
        /// <param name="type"></param>
        void CreateNode(System.Type type, Vector2 mousePos)
        {
            BehaviourTrees.Node node = behaviourTree.CreateNode(type);

            Vector2[] pos = new Vector2[1];
            pos[0] = mousePos;
            CreateNodeView(node, pos);
        }

        public void UpdateNodeState()
        {
            nodes.ForEach(n =>
            {
                NodeView nodeView = n as NodeView;
                nodeView.UpdateState();
            });
        }
    }
}