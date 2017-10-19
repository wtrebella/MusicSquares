using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Metronome : Singleton<Metronome> {
	public IntDelegate SignalPhraseImmediate;
	public IntDelegate SignalMeasureImmediate;
	public IntDelegate SignalBeatImmediate;
	public IntDelegate SignalSixteenthImmediate;

	public IntDelegate SignalPhrase;
	public IntDelegate SignalMeasure;
	public IntDelegate SignalBeat;
	public IntDelegate SignalSixteenth;

	[SerializeField] private int _bpm = 120;
	[SerializeField] private int _preroll = 2;

	private float _phraseDur;
	private float _measureDur;
	private float _beatDur;
	private float _sixteenthDur;
	private double _initTime;
	private double _elapsedTime = 0;
	private double _lastBeatTime = 0;
	private int _curBeat = 0;
	private int _curSixteenth = 0;
	private int _curMeasure = 0;
	private int _curPhrase = 0;
	private bool _hasStarted = false;
	private bool _signalPhrase = false;
	private bool _signalMeasure = false;
	private bool _signalBeat = false;
	private bool _signalSixteenth = false;

	void Awake() {
		_beatDur = 60f / _bpm;
		_sixteenthDur = _beatDur / 4f;
		_measureDur = _beatDur * 4f;
		_phraseDur = _measureDur * 4f;
	}

	void ResetDownbeat()
	{
		_initTime = AudioSettings.dspTime;
		_elapsedTime = 0;
		_curBeat = 0;
		_curSixteenth = 0;
		_curMeasure = 0;
		_curPhrase = 0;
		_lastBeatTime = 0;
	}

	void Update()
	{
		if (_signalPhrase)
		{
			_signalPhrase = false;
			if (SignalPhrase != null) SignalPhrase(_curPhrase);
		}
		if (_signalMeasure)
		{
			_signalMeasure = false;
			if (SignalMeasure != null) SignalMeasure(_curMeasure);
		}
		if (_signalBeat)
		{
			_signalBeat = false;
			if (SignalBeat != null) SignalBeat(_curBeat);
		}
		if (_signalSixteenth)
		{
			_signalSixteenth = false;
			if (SignalSixteenth != null) SignalSixteenth(_curSixteenth);
		}
	}

	void OnAudioFilterRead(float[] data, int channels)
	{
		if (!_hasStarted)
		{
			int preroll = (int)(AudioSettings.dspTime / _beatDur);
			if (preroll >= _preroll)
			{
				StartMetronome();
			}
			else
			{
				return;
			}
		}

		_elapsedTime = AudioSettings.dspTime - _initTime;

		int sixteenth = (int)(_elapsedTime / _sixteenthDur);
		int beat = (int)(_elapsedTime / _beatDur);
		int measure = (int)(_elapsedTime / _measureDur);
		int phrase = (int)(_elapsedTime / _phraseDur);

		if (phrase > _curPhrase)
		{
			_curPhrase = phrase;
			_signalPhrase = true;
			if (SignalPhraseImmediate != null) SignalPhraseImmediate(_curPhrase);
		}

		if (measure > _curMeasure)
		{
			_curMeasure = measure;
			_signalMeasure = true;
			if (SignalMeasureImmediate != null) SignalMeasureImmediate (_curMeasure);
		}

		if (beat > _curBeat)
		{
			_lastBeatTime = _elapsedTime;
			_curBeat = beat;
			_signalBeat = true;
			if (SignalBeatImmediate != null) SignalBeatImmediate (_curBeat);
		}

		if (sixteenth > _curSixteenth)
		{
			_curSixteenth = sixteenth;
			_signalSixteenth = true;
			if (SignalSixteenthImmediate != null) SignalSixteenthImmediate (_curSixteenth);
		}
	}

	private void StartMetronome()
	{
		_initTime = AudioSettings.dspTime;
		_hasStarted = true;
	}

	public float GetBeatPercent()
	{
		float timeSinceLastBeat = (float)(_elapsedTime - _lastBeatTime);
		float percent = timeSinceLastBeat / _beatDur;
		return percent;
	}

	public float GetSixteenthDur()
	{
		return _sixteenthDur;
	}

	public float GetBeatDur()
	{
		return _beatDur;
	}

	public float GetMeasureDur()
	{
		return _measureDur;
	}

	public float GetPhraseDur()
	{
		return _phraseDur;
	}

	public int GetCurSixteenth()
	{
		return _curSixteenth;
	}

	public int GetCurBeat()
	{
		return _curBeat;
	}

	public int GetCurMeasure()
	{
		return _curMeasure;
	}

	public int GetCurPhrase()
	{
		return _curPhrase;
	}

	public float GetBPM()
	{
		return _bpm;
	}

	public float GetPhrasePercent() 
	{
		return ((float)_elapsedTime % _phraseDur) / _phraseDur;
	}
}
