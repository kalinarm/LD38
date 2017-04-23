using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace LD38
{
	namespace Nodes
	{
	public class ActionProcess : IProcess
	{
		protected ActionManager parent;
		protected NarrativeNode currentNode;
		protected float timeSinceBegin;

		public ActionProcess (ActionManager _parent, NarrativeNode _node)
		{
			parent = _parent;
			currentNode = _node;
		}

		public override void OnBegin ()
		{
			timeSinceBegin = 0f;
			if (currentNode == null) {
				Cancel();
			}
			currentNode.Activate();
			if (currentNode.isFinished(timeSinceBegin)) {
				Terminate();
			}
		}

		public override void OnStep (float dt)
		{
			timeSinceBegin += dt;
			if (currentNode.isFinished(timeSinceBegin)) {
				currentNode.OnFinish();
				Terminate();
			}
		}

		public override void OnTerminate ()
		{
			foreach (NarrativeNode next in currentNode.Next()) {
				if (next != null && next != currentNode) {
					attachProcess(new ActionProcess(parent, next));
				}
			}
		}

		public override void OnCancel ()
		{

		}



	}
}
}

