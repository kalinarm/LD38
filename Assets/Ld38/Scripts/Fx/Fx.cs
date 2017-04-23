using UnityEngine;
using System.Collections;

namespace LD38
{
	[System.Serializable]
	public class Fx
	{
		public GameObject prefab;
		public AudioClip sound;
		[Range(0f,1f)]
		public float audioVolume = 1f;

		public void activate(Vector3 pos) {
			if (prefab != null) {
				GameObject obj = GameObject.Instantiate(prefab);
				obj.transform.position = pos;
			}
			if (sound != null) {
				play2DSound(sound, pos, audioVolume);
			}
		}

		public static void play3DSound(AudioClip clip, Vector3 pos, float volume) {
			GameObject obj = new GameObject("sound");
			AudioSource audio = obj.AddComponent<AudioSource>();
			obj.transform.position = pos;
			audio.clip = clip;
			audio.spatialBlend = 1f;
			audio.spatialize = true;
			audio.minDistance = 10f;
			audio.maxDistance = 250;
			audio.volume = volume;
			audio.Play();
			Temporary temp = obj.AddComponent<Temporary>();
			temp.duration = clip.length + 0.5f;
		}
		public static void play2DSound(AudioClip clip, Vector3 pos, float volume) {
			GameObject obj = new GameObject("sound");
			AudioSource audio = obj.AddComponent<AudioSource>();
			obj.transform.position = pos;
			audio.clip = clip;
			audio.playOnAwake = false;
			audio.spatialBlend = 0f;
			audio.spatialize = false;
			audio.volume = volume;
			audio.Play();
			Temporary temp = obj.AddComponent<Temporary>();
			temp.duration = clip.length + 0.5f;
		}

	}
}

