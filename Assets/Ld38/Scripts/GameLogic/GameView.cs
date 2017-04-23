using UnityEngine;
using System.Collections;

namespace LD38
{
	
	public class GameView : MonoBehaviour
	{
		public Gameplay gameplay;
		public RobotsManager robotsManager;
		public BloodManager bloodManager;
		public HumanView humanView;

		bool isPausing = false;
		public bool isGameFinish = false;

		public bool IsPausing {
			get {
				return isPausing;
			}
			set {
				isPausing = value;
				humanView.IsPausing = value;
				bloodManager.isPausing = value;
				robotsManager.isPausing = value;
			}
		}

		void StartGame() {
			humanView.scores.reset();
			IsPausing = false;
		}
		void EndGame() {
			IsPausing = true;
			isGameFinish = true;
		}

		void Start() {
			StartGame();
		}
		void Update ()
		{
			if (isPausing) {
				return;
			}
			if (humanView.scores.time >= gameplay.timeMax) {
				EndGame();
			}
		}

	}
}

