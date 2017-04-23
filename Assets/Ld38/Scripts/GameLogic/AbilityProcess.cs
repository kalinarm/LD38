using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD38
{
	namespace Proc
	{
		public class AbillityProcess : IProcess
		{
			public AbilityManager parent;
			public Ability ability;
			public AbilityState state = AbilityState.READY;

			float timer = 0f;
			public AbillityProcess(AbilityManager _parent, Ability _ability)
			{
				parent = _parent;
				ability = _ability;
			}
			public override void OnStep (float dt)
			{
				if (state == AbilityState.RELOAD) {
					timer -= dt;
					if (timer <= 0f) {
						state = AbilityState.READY;
					}
				}
			}
			public bool isReady() {
				return state == AbilityState.READY;
			}
			public void cast() {
				state = AbilityState.RELOAD;
				timer = ability.data.timeReload;
			}
		}
			

	}
}

