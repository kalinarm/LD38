using UnityEngine;
using System.Collections;

namespace LD38 {
	public enum DESTROY_CAUSE
	{
		END_LEVEL,
		ROBOT_KILLED
	}
	namespace Events
	{
		public class BloodPartDestroyed : IEvent
		{
			public BloodPart part;
			public DESTROY_CAUSE cause;
			public BloodPartDestroyed(BloodPart p, DESTROY_CAUSE c) {
				part = p;
				cause = c;
			}
			public override string getKey ()
			{
				return "BloodPartDestroyed";
			}
		}


	}
}


