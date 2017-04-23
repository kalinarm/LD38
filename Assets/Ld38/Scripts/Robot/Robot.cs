using UnityEngine;
using System.Collections;

namespace LD38
{
	[System.Serializable]
	public class RobotData
	{
		public float speed = 5f;
		public float angularSpeed = 5f;
		public int lifetime = 50;

		public Fx fxOnDestroy = new Fx();
	}

	public class Robot : MonoBehaviour
	{
		public RobotData data = new RobotData ();
		public RobotsManager manager;
		public RobotBase robotBase;

		public ProcessManager processMgr = new ProcessManager();

		public float stepGoto(float dt, Vector3 dest) {
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dest - transform.position), data.angularSpeed * dt);
			float factor = Mathf.Clamp( 90f -  Vector3.Angle(transform.forward, dest - transform.position), 0.1f, 1f );
			transform.position += transform.forward * data.speed * dt * factor;
			return (dest - transform.position).magnitude;
		}

		public void notifyEnterGarage() {
			manager.onRobotEnterGarage(this);
		}
		public void notifyLeaveGarage() {
			manager.onRobotLeaveGarage(this);
		}

		void Update() {
			processMgr.doStep(Time.deltaTime);
		}
		public void CancelAllProcess() {
			processMgr.deleteAllProcess();
		}
	}

}

