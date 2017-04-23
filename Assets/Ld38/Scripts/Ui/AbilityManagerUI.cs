using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

namespace LD38
{
	
	public class AbilityManagerUI : MonoBehaviour
	{
		public class IntEvent : UnityEvent<int>{}

		public AbilityManager user;
		public AbilityUsageUi prefabUsageUi;
		public Text tex_gold;
		public Text tex_robots;
		List<AbilityUsageUi> uis = new List<AbilityUsageUi>();

		void Start() {
			for (int i = 0 ; i < user.abilities.Count ; ++i) {
				AbilityUsageUi ui = GameObject.Instantiate(prefabUsageUi);
				RectTransform rt = ui.gameObject.GetComponent<RectTransform>();
				rt.SetParent(transform);
				rt.setFullSize();
				uis.Add(ui);

				Button but = ui.GetComponentInChildren<Button>();
				if (but != null) {
					int j = i;
					but.onClick.AddListener(delegate { OnAbilityClicked(j);});
				}
			}
		}

		void Update() {
			for (int i = 0 ; i < uis.Count ; ++i) {
				uis[i].refresh(user.usages[i]);
			}
			tex_gold.text = user.currentGold.ToString();
			RobotsManager m = App.Instance.gameView.robotsManager;
			tex_robots.text = m.getRobotsAvailablesCount().ToString() + " / " + m.Robots.Count.ToString();
		}

		public void OnAbilityClicked(int i) {
			if (user.isAbilityUsable (i)) {
				user.cast (i);
				Debug.Log("cast ability");
			}
			else {
				//todo add a sound
			}
		}
	}
}

