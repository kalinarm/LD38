using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LD38
{
	public class AbilityManager : MonoBehaviour
	{
		
		public int currentGold = 100;
		public List<Ability> abilities = new List<Ability>();
		public List<Proc.AbillityProcess> usages = new List<Proc.AbillityProcess>();
		ProcessManager processMgr = new ProcessManager();

		void Awake() {
			usages.Clear();
			for (int i = 0; i < abilities.Count; ++i) {
				Proc.AbillityProcess p = new Proc.AbillityProcess(this, abilities[i]);
				processMgr.Launch(p);
				usages.Add(p);
			}
		}

		void Update() {
			processMgr.doStep(Time.deltaTime);
		}

		public void cast(int i) {
			if (!isAbilityUsable(i)) {
				return;
			}
			currentGold -= abilities[i].data.goldCost;
			usages[i].cast();
			abilities[i].Activate();
		}

		public bool isAbilityUsable(int i) {
			if (i<0 || i >= abilities.Count) {
				return false;
			}
			return (abilities[i].data.goldCost <= currentGold && usages[i].isReady());
		}
	}
}

