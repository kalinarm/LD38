using UnityEngine;
using System.Collections;

namespace LD38
{
	[System.Serializable]
	public class ProjectileData
	{
		public float speed = 20f;
		public float angularSpeed = 5f;
		public Fx fxOnDestroy = new Fx();
	}

	public class Projectile : MonoBehaviour
	{
		public ProjectileData data = new ProjectileData();

		public BloodPart part = null;

		public BloodPart Part {
			get {
				return part;
			}
			set {
				part = value;
			}
		}

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (part == null) {
				autodestroy();
				return;
			}
			float dist = stepGoto(Time.deltaTime, part.transform.position);
			if (dist < 2f) {
				App.Instance.evtMgr.Trigger(new Events.BloodPartDestroyed(part, DESTROY_CAUSE.ROBOT_KILLED));
				part.destroy();
				autodestroy();
			}
		}

		public void autodestroy() {
			data.fxOnDestroy.activate(transform.position);
			GameObject.Destroy(this.gameObject);
		}

		public float stepGoto(float dt, Vector3 dest) {
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dest - transform.position), data.angularSpeed * dt);
			transform.position += transform.forward * data.speed * dt;
			return (dest - transform.position).magnitude;
		}
	}
}

