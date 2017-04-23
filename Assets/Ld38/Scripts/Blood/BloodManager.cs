using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LD38
{
	[System.Serializable]
	public class BloodPartProduction
	{
		public BloodPart prefab;
		public int count = 10;
		[Range(0f,1f)]
		public float infectedRateMin = 0.05f;
		public float timeOffset = 0f;
	}

	[System.Serializable]
	public class BloodManagerData
	{
		public Vector3 bloodVelocity = Vector3.right;
		public AnimationCurve speedCurve = new AnimationCurve();
		public float speedCurvePeriod = 1f;
		public List<BloodPartProduction> prods = new List<BloodPartProduction>();
	}

	public class BloodManager : MonoBehaviour
	{
		[Header("Blood")]
		public BloodManagerData data = new BloodManagerData();

		[Header("Creation")]
		[SerializeField] Transform createPos = null;
		[SerializeField] float rangeCreation = 10f;
		[SerializeField] float maxRange = 100f;
		[SerializeField] Transform anchorParts = null;

		[Header("Game")]
		public bool isPausing = false;

		List<BloodPart> currentParts = new List<BloodPart>();

		ProcessManager processMgr = new ProcessManager();
		float time = 0f;
		// Use this for initialization
		void Start ()
		{
			tickProduction ();
			ProcessCaller pSpawner = new ProcessCaller(2f, tickProduction);
			processMgr.Launch(pSpawner);
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (isPausing) {
				return;
			}

			processMgr.doStep(Time.deltaTime);

			float dt = Time.deltaTime;
			time+= dt;
			if (time > data.speedCurvePeriod) {
				time = 0f;
			}
			float factor = 1.0f + data.speedCurve.Evaluate(time / data.speedCurvePeriod);
			for(int i = 0; i < currentParts.Count; ++i) {
				currentParts[i].doStep(dt, factor);

				if (currentParts[i].transform.position.x > maxRange) {
					onPartAtEnd (currentParts [i]);
				}
			}
		}

		#region core

		void tickProduction ()
		{
			for (int i = 0; i < data.prods.Count; ++i) {
				if (data.prods [i].timeOffset > time) {
					continue;
				}
				for (int j = 0; j < data.prods [i].count; ++j) {
					create (data.prods [i]);
				}
			}
		}

		void create(BloodPartProduction prod) {
			if (prod == null) {
				return;
			}
			BloodPart p = GameObject.Instantiate(prod.prefab);
			p.name = prod.prefab.name;
			p.transform.SetParent(anchorParts);
			p.transform.position = createPos.position + rangeCreation * Random.insideUnitSphere;
			p.transform.rotation = Quaternion.Euler(Random.Range(-180f,180f),Random.Range(-180f,180f),Random.Range(-180f,180f));
			p.mainDirection = data.bloodVelocity;
			p.parent = this;
			currentParts.Add(p);

			Rigidbody r = p.GetComponent<Rigidbody>();
			if (r != null) {
				r.velocity = data.bloodVelocity * (1f + (Random.Range(-1f,1f) * 0.2f * data.bloodVelocity.magnitude));
				r.angularVelocity = new Vector3(
					Random.Range(0f, p.data.startAngularVelocity),
					Random.Range(0f, p.data.startAngularVelocity),
					Random.Range(0f, p.data.startAngularVelocity));
			}

			//infedcted
			if (prod.infectedRateMin > Random.Range(0f,1f)) {
				p.setInfected();
			}
		}

		void onPartAtEnd (BloodPart p)
		{
			switch (p.data.endMode) {
			case BloodPartData.END_MODE.DESTROY:
				App.Instance.evtMgr.Trigger(new Events.BloodPartDestroyed(p, DESTROY_CAUSE.END_LEVEL));
				destroyBloodPart(p);
				break;
			case BloodPartData.END_MODE.RESTART:
				p.transform.position = createPos.position + rangeCreation * Random.insideUnitSphere;
				break;
			}
		}

		public void destroyBloodPart(BloodPart p) {
			currentParts.Remove(p);
			GameObject.Destroy (p.gameObject);
		}
		#endregion
	}
}

