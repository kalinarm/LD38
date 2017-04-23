using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD38
{
	namespace Proc
	{
		public class RobotProcess : IProcess
		{
			public Robot robot;

			public RobotProcess (Robot _robot)
			{
				robot = _robot;
			}
			public override void OnStep (float dt)
			{
				if (robot == null) {
					Terminate();
				}
			}
		}
		public class GotoDest : RobotProcess
		{
			Vector3 dest;
			float range = 2f;
			public GotoDest (Robot _robot, Vector3 _dest) : base (_robot)
			{
				dest = _dest;
			}

			public override void OnStep (float dt)
			{
				base.OnStep(dt);
				float distRemaining = robot.stepGoto(dt, dest);
				if (distRemaining <= range) {
					Terminate();
				}
			}
		}
		public class GotoBase : RobotProcess
		{
			RobotBase station;
			float range = 2f;
			bool isPassAtEntrance = false;
			public GotoBase (Robot _robot) : base (_robot)
			{
				station = _robot.robotBase;
			}

			public override void OnStep (float dt)
			{
				base.OnStep(dt);
				if (station == null) {
					Terminate();
				}

				if (isPassAtEntrance) {
					float distRemaining = robot.stepGoto(dt, station.parking.position);
					if (distRemaining <= range) {
						robot.notifyEnterGarage();
						Terminate();
					}
					return;
				}

				float distRemainingEntrance = robot.stepGoto(dt, station.entrance.position);
				if (distRemainingEntrance <= range) {
					isPassAtEntrance = true;
				}
			}
		}

		public class CatchPart : RobotProcess
		{
			BloodPart part;
			float rangeCell = 2f;
			bool transport = false;
			Vector3 offset;
			public CatchPart (Robot _robot, BloodPart _part) : base (_robot)
			{
				robot = _robot;
				part = _part;
			}
			public override void OnBegin ()
			{
				robot.notifyLeaveGarage();
			}
			public override void OnStep (float dt)
			{
				base.OnStep(dt);
				if (part == null) {
					Terminate();
					return;
				}
				if (transport) {
					float distRemainingBase = robot.stepGoto(dt, robot.robotBase.destructor.position);
					Vector3 posPart = robot.transform.TransformPoint(offset);
					part.transform.position = posPart;
					if (distRemainingBase <= rangeCell) {
						App.Instance.evtMgr.Trigger(new Events.BloodPartDestroyed(part, DESTROY_CAUSE.ROBOT_KILLED));
						part.destroy();
						Terminate();
					}
					return;
				}
				float distRemaining = robot.stepGoto(dt, part.transform.position);
				if (distRemaining <= rangeCell) {
					offset = robot.transform.InverseTransformPoint(part.transform.position);
					transport = true;
					part.data.directMove = false;
				}
			}

			public override void OnTerminate ()
			{
				base.OnTerminate ();
				GotoBase p = new GotoBase(robot);
				attachProcess(p);
			}
		}

	}
}

