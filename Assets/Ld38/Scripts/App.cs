using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace LD38
{
	
	public class App : MonoBehaviour
	{
		public EvtMgr evtMgr = new EvtMgr();
		public GameView gameView;

		public static App Instance;

		void Awake() {
			Instance = this;
		}

		void Update() {
			evtMgr.doStep();
		}

		public void restartLevel() {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}

		public void goToMenu() {
			SceneManager.LoadScene(0);
		}
		public void goToLevel(int levelIndex) {
			SceneManager.LoadScene(levelIndex);
		}
		public void exitApplication() {
			Application.Quit();
		}

		public void setPause(bool pause) {
			if (gameView != null) {
				gameView.IsPausing = pause;
			}
		}
	
	}

}

