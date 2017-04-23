#define USE_PROCESS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace LD38
{
	namespace Nodes
	{
		public class ActionManager : MonoBehaviour
		{
			ProcessManager processMgr = new ProcessManager ();

			[SerializeField]
			private NarrativeCanvas canvas = null;

			[SerializeField]
			private RectTransform _canvasObject;

			NarrativeNode currentNode = null;

			public void Awake ()
			{
				if (canvas == null) {
					NodeCanvasSceneSave savedInScene = GameObject.FindObjectOfType<NodeCanvasSceneSave> ();
					if (savedInScene != null) {
						canvas = savedInScene.savedNodeCanvas as NarrativeCanvas;
					}
				}
				if (canvas == null) {
					return;
				}
				StartActionNode startNode = canvas.GetStartNode ();
				/*foreach (NarrativeCanvas nodeCanvas in Resources.LoadAll<NarrativeCanvas>("Saves/"))
					{
						
					}*/
			}

			void Start ()
			{
				if (canvas == null) {
					return;
				}
				Debug.Log ("Start action manager");
				StartActionNode startNode = canvas.GetStartNode ();
				if (startNode != null) {
					ActivateNode (startNode);
				}
			}

			void Update ()
			{
				processMgr.doStep (Time.deltaTime);
			}
				

			#if USE_PROCESS
			void ActivateNode (NarrativeNode _node)
			{
				currentNode = _node;
				ActionProcess action = new ActionProcess (this, currentNode);
				processMgr.Launch (action);
			}
			#else
			void ActivateNode (NarrativeNode _node)
			{
				currentNode = _node;
				StartCoroutine(ActivateNodeRoutine(_node));
			}

			IEnumerator ActivateNodeRoutine (NarrativeNode _node)
			{
				_node.Activate();

				yield return StartCoroutine(waitingRoutine(_node.Duration));

				List<NarrativeNode> nexts = _node.Next ();
				yield return StartCoroutine(ActivateNodeNextsRoutine(nexts));
			}

			IEnumerator ActivateNodeNextsRoutine (List<NarrativeNode> nexts)
			{
				foreach (NarrativeNode next in nexts) {
					if (next != null && next != currentNode) {
						yield return StartCoroutine(ActivateNodeRoutine(next));
					}
				}
			}

			IEnumerator waitingRoutine (float time)
			{
				yield return new WaitForSeconds(time);
			}
			#endif
		}
	}
}