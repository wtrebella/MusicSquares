using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Metronome : Singleton<Metronome>
{
	public IntDelegate SignalPhraseImmediate;
	public IntDelegate SignalMeasureImmediate;
	public IntDelegate SignalBeatImmediate;
	public IntDelegate SignalEighthImmediate;
	public IntDelegate SignalSixteenthImmediate;

	public IntDelegate SignalPhrase;
	public IntDelegate SignalMeasure;
	public IntDelegate SignalBeat;
	public IntDelegate SignalEighth;
	public IntDelegate SignalSixteenth;

	public GenericEventDelegate SignalMetronomeStart;

	[SerializeField] private int _bpm = 120;
	[SerializeField] private float _preroll = 2.0f;

	private float _firstDownbeatTime;
	private float _phraseDur;
	private float _measureDur;
	private float _beatDur;
	private float _eighthDur;
	private float _sixteenthDur;
	private double _syncTime;
	private double _elapsedTime = 0;
	private double _lastBeatTime = 0;
	private int _curBeat = 0;
	private int _curSixteenth = 0;
	private int _curEighth = 0;
	private int _curMeasure = 0;
	private int _curPhrase = 0;
	private bool _hasStarted = false;
	private bool _signalPhrase = false;
	private bool _signalMeasure = false;
	private bool _signalBeat = false;
	private bool _signalEighth = false;
	private bool _signalSixteenth = false;
	private bool _hasPlayedFirstDownbeat = false;
	private double _prevDSPTime = 0;

	void Awake()
	{
		_beatDur = 60f / _bpm;
		_sixteenthDur = _beatDur / 4f;
		_eighthDur = _beatDur / 2f;
		_measureDur = _beatDur * 4f;
		_phraseDur = _measureDur * 4f;
		StartMetronome();
	}

	void SetDownbeat(float downbeat)
	{
		_syncTime = downbeat;
		_hasPlayedFirstDownbeat = false;
		_elapsedTime = 0;
		_curBeat = 0;
		_curSixteenth = 0;
		_curEighth = 0;
		_curMeasure = 0;
		_curPhrase = 0;
		_lastBeatTime = _syncTime - _beatDur;
		_prevDSPTime = AudioSettings.dspTime;
	}

	void OnFirstDownbeat()
	{
		_hasPlayedFirstDownbeat = true;

		if (SignalBeatImmediate != null) SignalBeatImmediate(1);
		if (SignalEighthImmediate != null) SignalEighthImmediate(1);
		if (SignalSixteenthImmediate != null) SignalSixteenthImmediate(1);
		if (SignalMeasureImmediate != null) SignalMeasureImmediate(1);
		if (SignalPhraseImmediate != null) SignalPhraseImmediate(1);

		_signalBeat = true;
		_signalSixteenth = true;
		_signalEighth = true;
		_signalMeasure = true;
		_signalPhrase = true;
	}

	void Update()
	{
		if (!_hasStarted) return;

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
		if (_signalEighth)
		{
			_signalEighth = false;
			if (SignalEighth != null) SignalEighth(_curEighth);
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
		if (!_hasStarted) return;

		if (!_hasPlayedFirstDownbeat)
		{
			if (_prevDSPTime < _syncTime && AudioSettings.dspTime >= _syncTime)
			{
				OnFirstDownbeat();
			}
			else
			{
				_prevDSPTime = AudioSettings.dspTime;
				return;
			}
		}

		_elapsedTime = AudioSettings.dspTime - _syncTime;

		int sixteenth = (int)(_elapsedTime / _sixteenthDur);
		int eighth = (int)(_elapsedTime / _eighthDur);
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
			if (SignalMeasureImmediate != null) SignalMeasureImmediate(_curMeasure);
		}

		if (beat > _curBeat)
		{
			_lastBeatTime = _elapsedTime;
			_curBeat = beat;
			_signalBeat = true;
			if (SignalBeatImmediate != null) SignalBeatImmediate(_curBeat);
		}

		if (eighth > _curEighth)
		{
			_curEighth = eighth;
			_signalEighth = true;
			if (SignalEighthImmediate != null) SignalEighthImmediate(_curEighth);
		}

		if (sixteenth > _curSixteenth)
		{
			_curSixteenth = sixteenth;
			_signalSixteenth = true;
			if (SignalSixteenthImmediate != null) SignalSixteenthImmediate(_curSixteenth);
		}
	}

	public double GetSyncTime()
	{
		return _syncTime;
	}

	private void StartMetronome()
	{		
		SetDownbeat((float)AudioSettings.dspTime + _preroll);
		_hasStarted = true;
		if (SignalMetronomeStart != null) SignalMetronomeStart();
	}

	public float GetCurBeatPercent()
	{
		float timeSinceLastBeat = (float)(_elapsedTime - _lastBeatTime);
		float percent = timeSinceLastBeat / _beatDur;
		return percent;
	}

	public float GetBeatPercent(int beat)
	{
		if (beat < _curBeat) return 0;
		else if (beat > _curBeat) return 1;
		else return GetCurBeatPercent();
	}

	public float GetSixteenthDur()
	{
		return _sixteenthDur;
	}

	public float GetEighthDur()
	{
		return _eighthDur;
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
