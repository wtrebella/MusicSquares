using UnityEngine;
using System.Collections;
using System;

public class Metronome : Singleton<Metronome> {
	public Action SignalPhrase;
	public Action SignalMeasure;
	public Action SignalBeat;
	public Action SignalSixteenth;

	[SerializeField] int bpm = 120;
	[SerializeField] private bool resetDownbeatOnClick = false;

	float phraseDur;
	float measureDur;
	float beatDur;
	float sixteenthDur;
	double initTime;
	double elapsedTime = 0;
	int curBeat = 0;
	int curSixteenth = 0;
	int curMeasure = 0;
	int curPhrase = 0;

	void Awake() {
		beatDur = 60f / bpm;
		sixteenthDur = beatDur / 4f;
		measureDur = beatDur * 4f;
		phraseDur = measureDur * 4f;

		initTime = AudioSettings.dspTime;
	}

	void ResetDownbeat()
	{
		initTime = AudioSettings.dspTime;
		elapsedTime = 0;
		curBeat = 0;
		curSixteenth = 0;
		curMeasure = 0;
		curPhrase = 0;
	}

	void Update() {
		if (resetDownbeatOnClick)
		{
			if (Input.GetMouseButtonDown(0))
			{
				ResetDownbeat();
			}
		}

		elapsedTime = AudioSettings.dspTime - initTime;

		int sixteenth = (int)(elapsedTime / sixteenthDur);
		if (sixteenth > curSixteenth) {
			curSixteenth = sixteenth;
			if (SignalSixteenth != null) SignalSixteenth ();
			if (curSixteenth % 4 == 0) {
				curBeat++;
				if (SignalBeat != null) SignalBeat ();

				if (curBeat % 4 == 0) {
					curMeasure++;
					if (SignalMeasure != null) SignalMeasure ();

					if (curMeasure % 4 == 0) {
						curPhrase++;
						if (SignalPhrase != null) SignalPhrase();
					}
				}
			}
		}
	}

	public float GetBeatDur()
	{
		return beatDur;
	}

	public float GetBPM()
	{
		return bpm;
	}

	public float GetPhrasePercent() 
	{
		return ((float)elapsedTime % phraseDur) / phraseDur;
	}
}
