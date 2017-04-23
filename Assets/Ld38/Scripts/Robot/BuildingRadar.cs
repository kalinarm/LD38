using UnityEngine;
using System.Collections;

namespace LD38
{
	public class BuildingRadar : Building
	{
		ProcessCaller pSpawner;
		// Use this for initialization
		void Start ()
		{
			data.fxOnCreate.activate (transform.position);
			pSpawner = new ProcessCaller(data.tickFreq, tickBuilding);
			App.Instance.gameView.robotsManager.ProcessMgr.Launch(pSpawner);
		}
		public override void OnDestroy ()
		{
			base.OnDestroy();
			if (pSpawner != null & pSpawner.IsRunning) {
				pSpawner.Cancel();
			}
		}
		public override void tickBuilding() {
			BloodPart part = null;
			Collider[] colls = Physics.OverlapSphere(transform.position, data.range, data.maskBloodPart);
			for( int i = 0; i < colls.Length; ++i) {
				BloodPart p = colls[i].GetComponent<BloodPart>();
				if (p != null && p.data.infected && !p.isMarked) {
					part = p;
					break;
				}
			}

			if (part != null) {
				App.Instance.gameView.humanView.onClickOnBloodPart(part);
				data.fxOnScanned.activate(part.transform.position);
			}
			base.tickBuilding();
		}
	}
}

