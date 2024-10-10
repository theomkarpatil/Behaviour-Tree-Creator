// Developed by Sora
//
// Copyright(c) Sora 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Sora.AI.BehaviourTrees
{
    [CreateAssetMenu(fileName = "NewBehaviourTree", menuName = "Sora /BehaviourTree/Tree")]
    public class BehaviourTree : ScriptableObject
    {
        public Node rootNode;
        public ENodeState treeState = ENodeState.Running;
        public List<Node> nodes = new List<Node>();

        public Blackboard blackboard;
        public Transform transform;

        public ENodeState Update()
        {
            if(rootNode.state == ENodeState.Running)
                treeState = rootNode.Update();

            return treeState;
        }

        public Node CreateNode(System.Type type)
        {
            Node node = ScriptableObject.CreateInstance(type) as Node;
            node.name = type.Name;
            node.guid = GUID.Generate().ToString();

            Undo.RecordObject(this, "BehaviourTree:: (CreateNode)");

            nodes.Add(node);

            if(!Application.isPlaying)
                AssetDatabase.AddObjectToAsset(node, this);

            Undo.RecordObject(node, "BehaviourTree:: (CreateNode)");

            AssetDatabase.SaveAssets();

            return node;
        }

        public void DeleteNode(Node node)
        {
            Undo.RecordObject(this, "BehaviourTree:: (CreateNode)");

            nodes.Remove(node);

            //AssetDatabase.RemoveObjectFromAsset(node);
            Undo.DestroyObjectImmediate(node);  
            AssetDatabase.SaveAssets();

            //if (node is RootNode)
            //    Undo.PerformUndo();
        }

        public void AddChild(Node parent, Node child)
        {
            RootNode rootNode = parent as RootNode;
            if (rootNode)
            {
                Undo.RecordObject(rootNode, "BehaviourTree:: (AddChild)");
                
                rootNode.child = child;
                rootNode.child.parentNode = rootNode;

                EditorUtility.SetDirty(rootNode);

                return;
            }

            DecoratorNode decoratorNode = parent as DecoratorNode;
            if (decoratorNode)
            {
                Undo.RecordObject(decoratorNode, "BehaviourTree:: (AddChild)");
                
                decoratorNode.child = child;
                decoratorNode.child.parentNode = decoratorNode;

                EditorUtility.SetDirty(decoratorNode);
                return;
            }

            ControlFlowNode CFNode = parent as ControlFlowNode;
            if (CFNode)
            {
                Undo.RecordObject(CFNode, "BehaviourTree:: (AddChild)");
                
                CFNode.children.Add(child);
                CFNode.children.ForEach(n => n.parentNode = CFNode);

                EditorUtility.SetDirty(CFNode);
                return;
            }
        }

        public void DeleteChild(Node parent, Node child)
        {
            RootNode rootNode = parent as RootNode;
            if (rootNode)
            {
                Undo.RecordObject(rootNode, "BehaviourTree:: (DeleteChild)");
                
                rootNode.child = null;                

                EditorUtility.SetDirty(rootNode);
            }

            DecoratorNode decoratorNode = parent as DecoratorNode;
            if (decoratorNode)
            {
                Undo.RecordObject(decoratorNode, "BehaviourTree:: (DeleteChild)");
                
                decoratorNode.child = null;

                EditorUtility.SetDirty(decoratorNode);
            }
            
            ControlFlowNode CFNode = parent as ControlFlowNode;
            if (CFNode)
            {
                Undo.RecordObject(CFNode, "BehaviourTree:: (DeleteChild)");
                
                CFNode.children.Remove(child);
                
                EditorUtility.SetDirty(CFNode);
            }
        }

        public List<Node> GetChildren(Node parent)
        {
            List<Node> children = new List<Node>();

            RootNode rootNode = parent as RootNode;
            if (rootNode && rootNode.child != null)
            {
                children.Add(rootNode.child);
                return children;
            }

            DecoratorNode decoratorNode = parent as DecoratorNode;
            if (decoratorNode && decoratorNode.child != null)
            {
                children.Add(decoratorNode.child);
                return children;
            }

            ControlFlowNode CFNode = parent as ControlFlowNode;
            if (CFNode)
                return CFNode.children;

            return children;
        }

        public void Traverse(Node root, System.Action<Node> visitor)
        {
            if (root)
            {
                visitor.Invoke(root);
                var children = GetChildren(root);

                children.ForEach((n) => Traverse(n, visitor));
            }
        }

        public BehaviourTree Clone()
        {
            BehaviourTree tree = Instantiate(this);
            tree.rootNode = tree.rootNode.Clone();

            tree.nodes = new List<Node>();

            Traverse(tree.rootNode, (n) =>
            {
                tree.nodes.Add(n);
            });

            return tree;
        }

        public void InitializeNodes()
        {
            Traverse(rootNode, node =>
            {
                node.transform = transform;
                node.blackboard = blackboard;
                node.blackboard.tree = this;
            });
        }
    }
}