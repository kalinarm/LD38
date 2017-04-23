using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace LD38
{
	
	public class AbilityUsageUi : MonoBehaviour
	{
		public Image img_ability;
		public Image img_gold;
		public Text tex_name;
		public Text tex_goldCost;

		public void refresh(Proc.AbillityProcess p) {
			img_ability.color = p.isReady() ? Color.white : Color.white * 0.2f;
			img_ability.sprite = p.ability.sprite;
			tex_name.text = p.ability.gamename;
			if (tex_goldCost != null) {
				tex_goldCost.text = p.ability.data.goldCost.ToString();
			}
		}
	}
}

