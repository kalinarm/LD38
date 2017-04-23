using UnityEngine;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;


namespace LD38
{
	namespace Nodes
	{
		[Node (false, "Story/Delay")]
		public class NodeTimer : NarrativeNode
		{

			public new const string ID = "delay";
			public override string GetID { get { return ID; } }

			public override Node Create (Vector2 pos)
			{
				NodeTimer node = CreateInstance<NodeTimer> ();

				node.rect = new Rect (pos.x, pos.y, 200, 100);
				node.name = "Delay";

				node.CreateInput ("Previous Node", "ActionForward", NodeSide.Left, 30);
				node.CreateOutput ("Next Node", "ActionForward", NodeSide.Right, 30);

				return node;
			}
			#if UNITY_EDITOR
			protected internal override void NodeGUI ()
			{
				GUILayout.Label ("This is an delay Node");

				GUILayout.BeginHorizontal ();
				GUILayout.BeginVertical ();

				Inputs [0].DisplayLayout ();

				GUILayout.EndVertical ();
				GUILayout.BeginVertical ();

				Outputs [0].DisplayLayout ();

				GUILayout.EndVertical ();
				GUILayout.EndHorizontal ();

				Duration = RTEditorGUI.FloatField (new GUIContent ("Delay", "delay in second before continue"), Duration);
			}
			#endif
	
			public override void Activate ()
			{
				Debug.Log ("timer activate");
			}
			public override void OnFinish() {
				Debug.Log ("Timer finished");
			}

			public override bool isFinished (float timeSinceBegin)
			{
				//Debug.Log ("isFinished " + timeSinceBegin + " / "+ Duration);
				return timeSinceBegin > Duration;
			}

		}
	}
}