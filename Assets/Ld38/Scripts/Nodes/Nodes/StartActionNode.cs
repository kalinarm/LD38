using UnityEngine;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LD38
{
	namespace Nodes
	{
		[Node (false, "Story/Start")]
		public class StartActionNode : NarrativeNode
		{
			public new const string ID = "startnode";

			public override string GetID { get { return ID; } }

			public override Node Create (Vector2 pos)
			{
				StartActionNode node = CreateInstance<StartActionNode> ();

				node.rect = new Rect (pos.x, pos.y, 200, 60);
				node.name = "Start Action";

				node.CreateOutput ("Next Node", CONNECT_FORWARD_STR, NodeSide.Right, 30);

				return node;
			}
			#if UNITY_EDITOR
			protected internal override void NodeGUI ()
			{
				GUILayout.Label ("Start Scene Trigger");

				GUILayout.BeginHorizontal ();
				GUILayout.BeginVertical ();

				//Inputs [0].DisplayLayout ();

				GUILayout.EndVertical ();
				GUILayout.BeginVertical ();

				Outputs [0].DisplayLayout ();

				GUILayout.EndVertical ();
				GUILayout.EndHorizontal ();

			}
			#endif
			public override void Activate ()
			{
				Debug.Log ("StartActionNode activated");
			}
			public override void OnFinish() {
				Debug.Log ("StartActionNode finished");
			}
		}
	}
}