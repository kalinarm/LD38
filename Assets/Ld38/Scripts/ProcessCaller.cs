using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD38
{
	public class ProcessCaller : IProcess
	{
		public delegate void OnTick();
		public OnTick tickDelegate;

		float timeTick;
		float timeSinceBegin = 0f;

		public ProcessCaller(float _timeTick, OnTick _tickDelegate) {
			timeTick = _timeTick;
			tickDelegate = _tickDelegate;
		}

		public override void OnBegin ()
		{
			
		}

		public override void OnStep (float dt)
		{
			timeSinceBegin += dt;
			if( timeSinceBegin >= timeTick) {
				tickDelegate();
				timeSinceBegin = 0f;
			}
		}

		public override void OnTerminate ()
		{
			
		}
	}
}

