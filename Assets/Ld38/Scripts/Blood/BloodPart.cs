using UnityEngine;
using System.Collections;

namespace LD38
{
	[System.Serializable]
	public class BloodPartData
	{
		public enum END_MODE
		{
			RESTART,
			DESTROY
		}

		[Header("Global")]
		public float speed = 1f;
		public bool infected = false;
		public END_MODE endMode = END_MODE.DESTROY;
		public int goldReward = 1;

		[Header("Initial")]
		public bool directMove = true;
		public float startAngularVelocity = 0f;

		[Header("Fx")]
		public float perlinFactor = 1f;
		public Fx onInfectedDestroyed = new Fx();
		public Fx onCorrectDestroyed = new Fx();

		public void randomize(float factor) {
			speed = speed + (Random.Range(-1f,1f)* speed * factor);
		}
	}

	public class BloodPart : MonoBehaviour
	{
		public BloodManager parent;
		public BloodPartData data = new BloodPartData();
		public Vector3 mainDirection;
		public bool isMarked = false;
		void Start() {
			data.randomize(0.2f);
		}

		public virtual void doStep(float dt, float factor = 1f) {
			if (!data.directMove) {
				return;
			}
			Vector3 dir = mainDirection * data.speed;
			transform.position += dir * dt * factor;
			transform.position += Vector3.up * dt * data.perlinFactor * (Mathf.PerlinNoise(transform.position.x * 0.1f, transform.position.z)-0.5f);
		}

		public void destroy() {
			if (data.infected) {
				data.onInfectedDestroyed.activate(transform.position);
			}else {
				data.onCorrectDestroyed.activate(transform.position);
			}
			if (parent != null) {
				parent.destroyBloodPart(this);
			}
		}

		public void markAsSelected(Color c) {
			isMarked = true;
			foreach(MeshRenderer r in GetComponentsInChildren<MeshRenderer>()) {
				r.material.color = c;
			}
		}

		public void setInfected() {
			data.infected = true;
			foreach(MeshRenderer r in GetComponentsInChildren<MeshRenderer>()) {
				r.material.color = Color.white * 0.05f;
			}
		}
	}
}

