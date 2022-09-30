using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace BehaviourSystem.Editor
{
	public class BehaviourStateEditor<TEntity> : NodeEditor
	{
        public override void OnBodyGUI()
        {
            BehaviourState<TEntity> node = target as BehaviourState<TEntity>;
            if (node == null)
                return;
            
            BehaviourGraph<TEntity> graph = node.graph as BehaviourGraph<TEntity>;
            if (graph == null)
                return;

            DrawStartState(graph, node);
            DrawInputPort();
            DrawActionsList();
            DrawDecisionsList(node);
        }

        private void DrawDecisionsList(BehaviourState<TEntity> node)
        {
            EditorGUILayout.Space();

            GUIStyle s = new GUIStyle(EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Decisions:", s);

            EditorGUILayout.Space();

            if (node.Transitions != null)
            {
                for (int i = 0; i < node.Transitions.Length; i++)
                {
                    BehaviourState<TEntity>.Transition nodeTransition = node.Transitions[i];
                    
                    Horizontal(() =>
                    {
                        Vertical(() =>
                        {
                            if (GUILayout.Button("-", GUILayout.Width(30)))
                            {
                                List<BehaviourState<TEntity>.Transition> vector =
                                    new List<BehaviourState<TEntity>.Transition>(node.Transitions);
                                if (node.HasPort(nodeTransition.TruePortName))
                                {
                                    node.RemoveInstancePort(nodeTransition.TruePortName);
                                }

                                if (node.HasPort(nodeTransition.FalsePortName))
                                {
                                    node.RemoveInstancePort(nodeTransition.FalsePortName);
                                }

                                vector.RemoveAt(i);
                                node.Transitions = vector.ToArray();
                            }

                            if (i > 0 && GUILayout.Button("↑", GUILayout.Width(30)))
                            {
                                BehaviourState<TEntity>.Transition selectedElement = node.Transitions[i];
                                node.Transitions[i] = node.Transitions[i - 1];
                                node.Transitions[i - 1] = selectedElement;
                            }
                            
                            if (i < (node.Transitions.Length -1) && GUILayout.Button("↓", GUILayout.Width(30)))
                            {
                                BehaviourState<TEntity>.Transition selectedElement = node.Transitions[i];
                                node.Transitions[i] = node.Transitions[i + 1];
                                node.Transitions[i + 1] = selectedElement;
                            }
                        });

                        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Transitions").GetArrayElementAtIndex(i).FindPropertyRelative("Decisions"));
                    });

                    s = new GUIStyle(EditorStyles.boldLabel)
                    {
                        normal = {textColor = Color.yellow}
                    };

                    Horizontal(() =>
                    {
                        EditorGUILayout.Space();
                        
                        EditorGUILayout.LabelField(
                            nodeTransition.TrueState == null ? "RemainInState" : nodeTransition.TrueState.name, s);
                        NodeEditorGUILayout.PortField(new GUIContent("TRUE"),
                            target.GetOutputPort(nodeTransition.TruePortName), GUILayout.Width(40));
                    });
                    
                    EditorGUILayout.Space();
                    
                    Horizontal(() =>
                    {
                        EditorGUILayout.Space();
                        
                        EditorGUILayout.LabelField(
                            nodeTransition.FalseState == null ? "RemainInState" : nodeTransition.FalseState.name, s);
                        NodeEditorGUILayout.PortField(new GUIContent("FALSE"),
                            target.GetOutputPort(nodeTransition.FalsePortName), GUILayout.Width(40));
                    });
                    
                    EditorGUILayout.Space();
                }
            }

            EditorGUILayout.Space();
            
            Horizontal(() =>
            {
                EditorGUILayout.Space();
                
                if (GUILayout.Button("+", GUILayout.Width(30)))
                {
                    if (node.Transitions == null)
                    {
                        node.Transitions = new BehaviourState<TEntity>.Transition[0];
                    }
                    
                    List<BehaviourState<TEntity>.Transition> vector = new List<BehaviourState<TEntity>.Transition>(node.Transitions);
                    BehaviourState<TEntity>.Transition trans = new BehaviourState<TEntity>.Transition();
                    NodePort newport = node.AddInstanceOutput(typeof(BehaviourState<TEntity>.Connection), Node.ConnectionType.Override);
                    trans.TruePortName = newport.fieldName;
                    newport = node.AddInstanceOutput(typeof(BehaviourState<TEntity>.Connection), Node.ConnectionType.Override);
                    trans.FalsePortName = newport.fieldName;
                    vector.Add(trans);
    
                    node.Transitions = vector.ToArray();
                }
            });
        }

        private void DrawActionsList()
        {
            EditorGUILayout.Space();

            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Actions"));
        }

        private void DrawInputPort()
        {
            EditorGUILayout.Space();

            NodeEditorGUILayout.PortField(target.GetInputPort("Enter"), GUILayout.Width(100));
        }

        private void DrawStartState(BehaviourGraph<TEntity> graph, BehaviourState<TEntity> node)
        {
            bool isStartNode = graph.BeginState == node;
            GUIStyle s;

            if (isStartNode)
            {
                s = new GUIStyle(EditorStyles.boldLabel)
                {
                    normal = {textColor = Color.green},
                    fontSize = 15,
                    fixedHeight = 25
                };

                EditorGUILayout.LabelField("This is Start NODE", s);
            }
            else if (graph.BeginState == null)
            {
                s = new GUIStyle(EditorStyles.boldLabel)
                {
                    normal = {textColor = Color.red},
                    fontSize = 15,
                    fixedHeight = 25
                };

                EditorGUILayout.LabelField("Please select START NODE", s);
                if (GUILayout.Button("Set as start"))
                    graph.BeginState = node;
            }
            else
            {
                if (GUILayout.Button("Set as start"))
                    graph.BeginState = node;
            }
        }

        private void Horizontal(Action action)
        {
            EditorGUILayout.BeginHorizontal();
            action();
            EditorGUILayout.EndHorizontal();
        }

        private void Vertical(Action action)
        {
            EditorGUILayout.BeginVertical();
            action();
            EditorGUILayout.EndVertical();
        }
	}
}
