using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace LD38
{
	namespace UI
	{
		public class HumanViewUi : MonoBehaviour
		{
			public HumanView humanView;
			public ScoresUi scores = null;
			public Text tex_gameMessage;
			public Text tex_robotsAvailable;
			public GameObject endGamePanel;
			public GameObject pausePanel;

			void Start() {
				endGamePanel.SetActive(false);
			}
			void Update() {
				refresh();
			}

			public void refresh() {
				if (humanView == null) {
					return;
				}

				pausePanel.SetActive(humanView.IsPausing);

				if (scores != null) {
					scores.refresh(humanView.scores);
				}
				setContent(tex_robotsAvailable, humanView.getRobotsAvailablesCount().ToString());
				if (!endGamePanel.activeSelf && humanView.IsPausing && App.Instance.gameView.isGameFinish) {
					endGamePanel.SetActive(true);
					UI.ScoresUi ui = endGamePanel.GetComponentInChildren<UI.ScoresUi>();
					if ( ui != null) {
						ui.refresh(humanView.scores);
					}
				}
			}

			void setContent(Text comp, string content) {
				if (comp != null) {
					comp.text = content;
				}
			}

			public void setMessage(string text) {
				setContent(tex_gameMessage, text);
			}
			public void clearMessage() {
				setContent(tex_gameMessage, string.Empty);
			}

		}
	}
}

