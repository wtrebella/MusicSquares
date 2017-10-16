using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helm;

public class NoteToPlay
{
	public int note;
	public float delay;

	public NoteToPlay(int inNote, float inLength)
	{
		note = inNote;
		delay = inLength;
	}
}

public class Bassline : MonoBehaviour {
	[SerializeField] private int _baseNote = 50;

	private Sampler _sampler;
	private int[] _measure;
	private bool _isPlaying = false;
	private List<NoteToPlay> _notesToPlay = new List<NoteToPlay>();

	void Awake()
	{
		Metronome.instance.SignalBeat += OnBeat;
		Metronome.instance.SignalMeasure += OnMeasure;
		Metronome.instance.SignalPhrase += OnPhrase;
		Metronome.instance.SignalSixteenth += OnSixteenth;

		_sampler = GetComponent<Sampler>();
		_measure = new int[]
		{
			12, 10, 8, 7, 		// I
			0, 3, 7, 10,		// I
			12, 7, 10, 11,		// I
			12, 14, 15, 16,		// I

			17, 9, 10, 11,		// IV
			12, 10, 3, 2,		// IV
			0, 2, 3, 4,			// I
			5, 6, 7, 6,			// I

			7, 5, 7, 3,			// V
			5, 3, 10, 11,		// IV
			12, 0, 3, 1,		// I
			2, 8, 7, 11			// ii V
		};
	}

	void OnDestroy()
	{
		if (Metronome.DoesExist())
		{
			Metronome.instance.SignalBeat -= OnBeat;
			Metronome.instance.SignalMeasure -= OnMeasure;
			Metronome.instance.SignalPhrase -= OnPhrase;
			Metronome.instance.SignalSixteenth -= OnSixteenth;
		}
	}

	void OnAudioFilterRead(float[] data, int channels)
	{

	}

	void Update()
	{
		if (_notesToPlay.Count > 0)
		{
			for (int i = _notesToPlay.Count - 1; i >= 0; i--)
			{
				NoteToPlay note = _notesToPlay[i];
				StartCoroutine(PlayNoteRoutine(note));
				_notesToPlay.RemoveAt(i);
			}
		}
	}

	void OnBeat(int beat)
	{
		if (!_isPlaying) return;
		int beatIndex = (beat - 1) % 36;
		int note = _measure[beatIndex] + _baseNote;
		PlayNote(note, Metronome.instance.GetBeatDur());
	}

	void PlayNote(int note, float delay)
	{
		NoteToPlay noteToPlay = new NoteToPlay(note, delay);
		_notesToPlay.Add(noteToPlay);
	}

	public void StartPlaying()
	{
		_isPlaying = true;
	}

	public void StopPlaying()
	{
		_isPlaying = false;
	}

	IEnumerator PlayNoteRoutine(NoteToPlay note)
	{
		_sampler.NoteOn(note.note);
		yield return new WaitForSeconds(note.delay);
		_sampler.NoteOff(note.note);
	}

	void OnMeasure(int measure)
	{
		
	}

	void OnPhrase(int phrase)
	{

	}

	void OnSixteenth(int sixteenth)
	{

	}
}
