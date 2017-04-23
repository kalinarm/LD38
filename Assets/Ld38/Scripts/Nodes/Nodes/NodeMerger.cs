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
	[Node (false, "Story/Merger")]
	public class NodeMerger : NarrativeNode 
	{
		public new const string ID = "merger";
		public override string GetID { get { return ID; } }

		public override Node Create (Vector2 pos) 
		{
			NodeMerger node = CreateInstance<NodeMerger> ();

			node.rect = new Rect (pos.x, pos.y, 130, 160);
			node.name = "Merger";

			node.CreateOutput("Next Node", "ActionForward", NodeSide.Right, 30);

			node.CreateInput("Previous Node","ActionForward", NodeSide.Left, 30);
			node.CreateInput("Previous Node","ActionForward", NodeSide.Left, 30);

			return node;
		}
		#if UNITY_EDITOR
		protected internal override void NodeGUI () 
		{
			GUILayout.BeginHorizontal ();

			GUILayout.BeginVertical ();
			Outputs [0].DisplayLayout ();
			GUILayout.EndVertical ();

			GUILayout.EndHorizontal ();


			for (int i = 0; i < Inputs.Count; ++i) {
				Inputs[i].DisplayLayout();
			}
			if (GUILayout.Button("Add Input")) {
				CreateInput("Previous Node","ActionForward", NodeSide.Left, 30);
			}
			if (Inputs.Count > 3 && GUILayout.Button("Remove Last Input")) {
				int i = Inputs.Count - 1;
				Inputs[i].RemoveConnection();
				DestroyImmediate(Inputs[i]);
				Inputs.RemoveAt(i);
			}

		}
		#endif
		public override void Activate() {
			Debug.Log ("NodeMerger activated");
		}
	}
}
}