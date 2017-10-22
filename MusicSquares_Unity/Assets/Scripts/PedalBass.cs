using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helm;

public class PedalBass : MonoBehaviour
{
	[SerializeField] private int _pedalTone = 36;

	private HelmController _synth;
	private bool _playThisBeat = true;

	void Awake()
	{
		_synth = GetComponent<HelmController>();
		Metronome.instance.SignalBeat += OnBeat;
	}

	void OnDestroy()
	{
		if (Metronome.DoesExist())
		{
			Metronome.instance.SignalBeat -= OnBeat;
		}
	}

	void OnBeat(int beat)
	{
		if (_playThisBeat)
		{
			_synth.NoteOn(_pedalTone, 1, Metronome.instance.GetBeatDur() - 0.01f);
		}

		_playThisBeat = !_playThisBeat;
	}
}
