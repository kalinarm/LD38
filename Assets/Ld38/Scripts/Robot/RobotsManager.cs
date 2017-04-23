using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LD38
{
	public class RobotsManager : MonoBehaviour
	{
		ProcessManager processMgr = new ProcessManager ();
		public Transform anchorRobots;
		public Transform startRobotPosition;
		public RobotBase globalStation;

		[SerializeField] List<Robot> robotsBuildable = new List<Robot>();

		[SerializeField] List<Transform> buildingSpots = new List<Transform>();

		[Header("debug")]
		[SerializeField] List<Robot> robots = new List<Robot>();
		[SerializeField] List<Robot> robotsInGarage = new List<Robot>();
		[SerializeField] List<Building> buildings = new List<Building>();

		public bool isPausing = false;

		public ProcessManager ProcessMgr {
			get {
				return processMgr;
			}
		}

		public List<Robot> Robots {
			get {
				return robots;
			}
		}

		// Use this for initialization
		void Start ()
		{
			for (int i = 0; i < robots.Count; ++i) {
				if (robots[i]==null) {
					continue;
				}
				registerRobot (robots[i]);
			}
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (isPausing) {
				return;
			}

			processMgr.doStep (Time.deltaTime);
		}

		public void onRobotEnterGarage(Robot r) {
			if (r != null && !robotsInGarage.Contains(r)) {
				robotsInGarage.Add(r);
			}
		}

		public void onRobotLeaveGarage(Robot r) {
			robotsInGarage.Remove(r);
			if (--r.data.lifetime <= 0 ) {
				//todo prevent crash when destroying robot
				//destroyRobot(r);
			}
		}

		public int getRobotsAvailablesCount() {
			return robotsInGarage.Count;
		}

		public Robot GetNextAvailable() {
			if (robotsInGarage.Count == 0) {
				return null;
			}
			return robotsInGarage[0];
		}

		void registerRobot (Robot r)
		{
			r.manager = this;
			r.transform.SetParent (anchorRobots);
			if (r.robotBase == null) {
				r.robotBase = globalStation;
			}
			r.processMgr.Launch (new Proc.GotoBase (r));
		}

		public void createRobot(Robot r) {
			Robot robot = GameObject.Instantiate(r);
			registerRobot(robot);
			robot.transform.position = startRobotPosition.position;
			robots.Add(r);
		}

		public void destroyRobot(Robot r) {
			robots.Remove(r);
			robotsInGarage.Remove(r);
			r.data.fxOnDestroy.activate(r.transform.position);
			GameObject.Destroy(r.gameObject);
		}

		void registerBuilding (Building r)
		{
			r.manager = this;
		}

		public bool createBuilding(Building r) {
			Debug.Log("create building");
			//pick position
			Transform spot = null;
			for (int i = 0 ; i < buildingSpots.Count; ++i) {
				if (buildingSpots[i].childCount > 0) { continue;}
				spot = buildingSpots[i];
			}
			if (spot == null ) {
				return false;
			}
			Debug.Log("create building : spot find");
			Building building = ((GameObject)GameObject.Instantiate(r.gameObject)).GetComponent<Building>();
			registerBuilding(building);
			building.transform.SetParent(spot);
			building.transform.ResetLocal();
			return true;
		}

		public void destroyBuilding(Building r) {
			if( r == null) {
				return;
			}
			buildings.Remove(r);
			r.OnDestroy();
			GameObject.Destroy(r.gameObject);
		}

		public bool affectRobotToCollect(BloodPart p) {
			Robot r = GetNextAvailable();
			if (r == null ){ return false;}

			r.CancelAllProcess();
			Proc.GotoDest d = new Proc.GotoDest(r, r.robotBase.exit.position);
			d.attachProcess(new Proc.CatchPart(r, p));
			r.processMgr.Launch(d);
			return true;
		}
	}
}

