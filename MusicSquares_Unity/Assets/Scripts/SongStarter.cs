using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongStarter : MonoBehaviour {
	private AudioSource _songSource;

	void Awake()
	{
		_songSource = GetComponent<AudioSource>();

		Metronome.instance.SignalBeatImmediate += OnBeat;
	}

	void OnDestroy()
	{
		if (Metronome.DoesExist())
		{
			Metronome.instance.SignalBeatImmediate -= OnBeat;
		}
	}

	void OnBeat(int beat)
	{
		if (!_songSource.isPlaying)
		{
			_songSource.Play();
		}
	}
}
