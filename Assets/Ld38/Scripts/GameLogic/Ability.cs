using UnityEngine;
using System.Collections;

namespace LD38
{
	public enum AbilityState
	{
		READY,
		RELOAD
	}

	[System.Serializable]
	public class AbilityData
	{
		public int goldCost = 10;
		public float timeReload = 5f;

		public Robot createdRobot;
		public Building createdBuilding;
	}

	[CreateAssetMenu]
	public class Ability : ScriptableObject
	{
		public string gamename;
		public Sprite sprite;
		public AbilityData data = new AbilityData();

		public virtual void Activate() {
			if (data.createdRobot != null) {
				App.Instance.gameView.robotsManager.createRobot(data.createdRobot);
			}
			if (data.createdBuilding != null) {
				App.Instance.gameView.robotsManager.createBuilding(data.createdBuilding);
			}
		}

	}
}

