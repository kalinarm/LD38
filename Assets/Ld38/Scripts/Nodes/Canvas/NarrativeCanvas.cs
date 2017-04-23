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

	[NodeCanvasType("Narrative Canvas")]
	public class NarrativeCanvas : NodeCanvas
	{
			[SerializeField]
			private List<StartActionNode> _lstActionStartNodes = new List<StartActionNode>();

			private Dictionary<int, NarrativeNode> _lstActiveActions = new Dictionary<int, NarrativeNode>();

			public StartActionNode GetStartNode()
			{
				return _lstActionStartNodes.Count > 0 ? _lstActionStartNodes[0] : null;
			}

			public override void BeforeSavingCanvas()
			{
				this._lstActionStartNodes.Clear();

				foreach (Node node in this.nodes)
				{
					if (node is StartActionNode)
					{
						_lstActionStartNodes.Add(node as StartActionNode);
					}
				}
			}
	}
}
}

