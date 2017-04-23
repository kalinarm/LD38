using UnityEngine;
using System.Collections;

namespace LD38
{
	[System.Serializable]
	public class BuildingData
	{
		public float range = 50f;
		public int lifetime = 50;
		public float tickFreq = 5f;
		public Fx fxOnCreate = new Fx ();
		public Fx fxOnDestroy = new Fx ();

		public Fx fxOnScanned = new Fx ();

		public LayerMask maskBloodPart;
	}

	public class Building : MonoBehaviour
	{
		public BuildingData data = new BuildingData ();
		public RobotsManager manager;
		ProcessCaller pSpawner;
		// Use this for initialization
		void Start ()
		{
			data.fxOnCreate.activate (transform.position);
			pSpawner = new ProcessCaller(data.tickFreq, tickBuilding);
			App.Instance.gameView.robotsManager.ProcessMgr.Launch(pSpawner);
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		public virtual void OnDestroy ()
		{
			data.fxOnDestroy.activate (transform.position);
		}

		public virtual void tickBuilding() {
			if (--data.lifetime <= 0) {
				manager.destroyBuilding(this);
				if (pSpawner != null) { pSpawner.Cancel();}
				return;
			}
		}
	}
}

