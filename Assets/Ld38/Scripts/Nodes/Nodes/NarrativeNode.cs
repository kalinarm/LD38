using UnityEngine;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LD38
{
	namespace Nodes
	{
		[Node( false, "Story/Base")]
		public class NarrativeNode : Node
		{
			public const string CONNECT_FORWARD_STR = "ActionForward";
			public const string ID = "base";
			public override string GetID { get { return ID; } }
			public float Duration = 1f;

			public override Node Create (Vector2 pos)
			{
				NarrativeNode node = CreateInstance<NarrativeNode> ();

				node.rect = new Rect (pos.x, pos.y, 300, 120);
				node.name = ID;

				node.CreateInput ("Previous Node", CONNECT_FORWARD_STR, NodeSide.Left, 30);
				node.CreateOutput ("Next Node", CONNECT_FORWARD_STR, NodeSide.Right, 30);

				return node;
			}

			protected internal override void NodeGUI ()
			{
				#if UNITY_EDITOR
				GUILayout.Label ("This is a action node");

				GUILayout.BeginHorizontal ();
				GUILayout.BeginVertical ();

				Inputs [0].DisplayLayout ();

				GUILayout.EndVertical ();
				GUILayout.BeginVertical ();

				Outputs [0].DisplayLayout ();

				GUILayout.EndVertical ();
				GUILayout.EndHorizontal ();
				#endif
			}



			public virtual List<NarrativeNode> Next ()
			{
				List<NarrativeNode> l = new List<NarrativeNode> ();
				if (Outputs.Count == 0) {
					return l;
				}
				addAllConnected (l, Outputs [0]);
				return l;
			}

			public virtual NarrativeNode Previous ()
			{
				if (Inputs.Count == 0) {
					return null;
				}
				return Inputs [0].GetNodeAcrossConnection () as NarrativeNode;
			}
			
			public virtual bool isFinished (float timeSinceBegin)
			{
				return timeSinceBegin > Duration;
			}
			
			protected void addAllConnected (List<NarrativeNode> l, NodeOutput output)
			{
				if (output.connections == null) {
					return;
				}
				for (int i = 0; i < output.connections.Count; ++i) {
					if (output.connections [i].body != null) {
						l.Add (output.connections [i].body as NarrativeNode);
					}
				}
			}

			public virtual bool IsNextAvailable ()
			{
				if (Outputs.Count == 0) {
					return false;
				}
				return Outputs [0].GetNodeAcrossConnection () != default(Node);
			}

			public override bool Calculate ()
			{
				if (!allInputsReady ())
					return false;
				return true;
			}

			public virtual void Activate() {
				Debug.Log ("Narrative Node activated");
			}

			public virtual void OnFinish() {
				Debug.Log ("Narrative Node finished");
			}
		}
		//}

		public class ActionForwardType : IConnectionTypeDeclaration
		{
			public string Identifier { get { return "ActionForward"; } }

			public System.Type Type { get { return typeof(float); } }

			public Color Color { get { return Color.green; } }

			public string InKnobTex { get { return "Textures/In_Knob.png"; } }

			public string OutKnobTex { get { return "Textures/Out_Knob.png"; } }
		}
			
	}
}

