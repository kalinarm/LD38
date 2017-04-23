using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LD38
{
	[System.Serializable]
	public class Scores {
		public int cellReviewed = 0;
		public int totalDestroyed = 0;
		public int infectedMiss = 0;
		public int infectedDestroyed = 0;
		public int correctDestroyed = 0;

		public float time = 0f;

		public void reset() {
			cellReviewed = 0;
			totalDestroyed = 0;
			infectedMiss = 0;
			infectedDestroyed = 0;
			correctDestroyed = 0;
			time = 0f;
		}
		public void update(float dt) {
			time += dt;
		}
		public int getGlobalScore() {
			return (int)(cellReviewed * 0.1f + infectedDestroyed * 2 - infectedMiss * 3 - correctDestroyed * 4);
		}
		public void addCellReview() {
			cellReviewed += 1;
		}
		public void addTotalDestroyed() {
			totalDestroyed += 1;
		}
		public void addInfectedMiss() {
			infectedMiss += 1;
		}
		public void addInfectedDestroyed() {
			infectedDestroyed += 1;
		}
		public void addCorrectDestroyed() {
			correctDestroyed += 1;
		}
	}

	public class HumanView : MonoBehaviour
	{
		[System.Serializable]
		public class Config {
			public Color colorSelected = Color.blue;
			public Color colorFuture = Color.gray;

			public Fx onCellSelected = new Fx();
			public Fx onCellSelectedNotInfected = new Fx();
			public Fx onCellSelectedWaiting = new Fx();
			public Fx onInfectedDestroyed = new Fx();
			public Fx onCorrectDestroyed = new Fx();
		}

		[SerializeField] Config config = new Config();

		public RobotsManager robotsManager;
		public BloodManager bloodManager;
		public AbilityManager abilityUser;

		public LayerMask maskBloodPart;
		public LayerMask maskRobot;

		public Queue<BloodPart> waitingsParts = new Queue<BloodPart>();

		public Scores scores = new Scores();

		bool isPausing = false;

		public bool IsPausing {
			get {
				return isPausing;
			}
			set {
				isPausing = value;
			}
		}

		// Use this for initialization
		void Start ()
		{
			App.Instance.evtMgr.AddListener(OnBloodPartDestroyed, new Events.BloodPartDestroyed(null, DESTROY_CAUSE.END_LEVEL));
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (Input.GetKeyDown(KeyCode.Escape)) {
				App.Instance.gameView.IsPausing = !App.Instance.gameView.IsPausing;
			}
			if (isPausing) {
				return;
			}

			scores.update(Time.deltaTime);
			if (Input.GetMouseButtonDown(0)) {
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray,out hit, 100f,maskBloodPart)) {
					BloodPart p = hit.collider.GetComponent<BloodPart>();
					if (p != null) {
						if (p.data.infected) {
							config.onCellSelected.activate(p.transform.position);
						}else {
							config.onCellSelectedNotInfected.activate(p.transform.position);
						}
						onClickOnBloodPart(p);
					}
				}
			}

			captureAbilityPress (KeyCode.Alpha1, 0);
			captureAbilityPress (KeyCode.Alpha2, 1);
			captureAbilityPress (KeyCode.Alpha3, 2);
			captureAbilityPress (KeyCode.Alpha4, 3);

			if (waitingsParts.Count > 0 && getRobotsAvailablesCount()> 0 ) {
				BloodPart p = waitingsParts.Peek();
				if (p != null ){
					if (robotsManager.affectRobotToCollect(p)) {
						p.markAsSelected(config.colorSelected);
						waitingsParts.Dequeue();
					}
				}
			}
		}

		void captureAbilityPress (KeyCode key, int index)
		{
			if (Input.GetKeyDown (key)) {
				if (abilityUser.isAbilityUsable (index)) {
					abilityUser.cast (index);
					Debug.Log("cast ability");
				}
				else {
					//todo add a sound
				}
			}
		}
			
		public void onClickOnBloodPart(BloodPart p) {
			if (p.isMarked) {
				return;
			}
			/*if (robotsManager.affectRobotToCollect(p)) {
				p.markAsSelected(config.colorSelected);
				config.onCellSelected.activate(p.transform.position);
			}else {*/
				waitingsParts.Enqueue(p);
				p.markAsSelected(config.colorFuture);
				config.onCellSelectedWaiting.activate(p.transform.position);
			//}
		}
		

		public int getRobotsAvailablesCount() {
			return robotsManager.getRobotsAvailablesCount();
		}

		#region evt callbacks
		public void OnBloodPartDestroyed(IEvent evt) {
			Events.BloodPartDestroyed casted = evt as Events.BloodPartDestroyed;
			if (casted.cause == DESTROY_CAUSE.END_LEVEL) {
				scores.addCellReview();
				if (casted.part.data.infected) {
					scores.addInfectedMiss();
				}
			}else if (casted.cause == DESTROY_CAUSE.ROBOT_KILLED) {
				scores.addTotalDestroyed();
				if (casted.part.data.infected) {
					scores.addInfectedDestroyed();
					config.onInfectedDestroyed.activate(casted.part.transform.position);
					abilityUser.currentGold += casted.part.data.goldReward;
				}else {
					scores.addCorrectDestroyed();
					config.onCorrectDestroyed.activate(casted.part.transform.position);
				}
			}
		}
		#endregion
	}
}

