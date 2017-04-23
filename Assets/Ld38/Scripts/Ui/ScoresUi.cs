using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace LD38
{
	namespace UI
	{
		
		public class ScoresUi : MonoBehaviour
		{
			[SerializeField] Text tex_cellReviewed;
			[SerializeField] Text tex_totalDestroyed;
			[SerializeField] Text tex_infectedMiss;
			[SerializeField] Text tex_infectedDestroyed;
			[SerializeField] Text tex_correctDestroyed;
			[SerializeField] Text tex_time;
			[SerializeField] Text tex_globalScore;
			// Use this for initialization
			public void refresh (Scores scores)
			{
				setContent(tex_cellReviewed, scores.cellReviewed);
				setContent(tex_totalDestroyed, scores.totalDestroyed);
				setContent(tex_infectedMiss, scores.infectedMiss);
				setContent(tex_infectedDestroyed, scores.infectedDestroyed);
				setContent(tex_correctDestroyed, scores.correctDestroyed);
				setContent(tex_time, scores.time);
				setContent(tex_globalScore, scores.getGlobalScore());
			}

			void setContent(Text comp, int content) {
				if (comp != null) {
					comp.text = content.ToString();
				}
			}
			void setContent(Text comp, float content) {
				if (comp != null) {
					comp.text = content.ToString("##.#");
				}
			}
		}

	}
}

