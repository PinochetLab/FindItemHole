using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounder : MonoBehaviour
{
	[SerializeField] private List<AudioClip> clips;
	[SerializeField] private AudioSource source;

	public void Play(string name) {
		source.clip = clips.Find(x => x.name == name);
		source.Play();
	}
}
