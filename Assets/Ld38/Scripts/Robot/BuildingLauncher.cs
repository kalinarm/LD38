using UnityEngine;
using System.Collections;

namespace LD38
{
	public class BuildingLauncher : Building
	{
		ProcessCaller pSpawner;
		public Transform lauchMissilePos;
		public Projectile projectilePrefab;
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
				LaunchProjectil(projectilePrefab, part);
				data.fxOnScanned.activate(part.transform.position);
			}
			base.tickBuilding();
		}

		void LaunchProjectil(Projectile prefab, BloodPart target) {
			Projectile p = GameObject.Instantiate(prefab);
			p.Part = target;
			p.transform.position = lauchMissilePos.position;
			target.markAsSelected(Color.cyan);
		}
	}
}

