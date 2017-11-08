using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NoteInput
{
	Start,
	Hold,
	End,
	Rest
}

public class RhythmicPhrase
{
	private int _beats;
	private int _divisionsPerBeat;
	private int _totalDivisions;
	private List<NoteInput> _phrase;

	public RhythmicPhrase(int beats, int divisionsPerBeat)
	{
		_beats = beats;
		_divisionsPerBeat = divisionsPerBeat;
		_totalDivisions = _beats * _divisionsPerBeat;
		_phrase = new List<NoteInput>(_totalDivisions);

		for (int i = 0; i < _totalDivisions; i++)
		{
			_phrase.Add(NoteInput.Rest);
		}
	}

	public void WriteNote(int beatStart, int beatDivisionStart, int lengthInDivisions)
	{
		Write(NoteInput.Start, beatStart, beatDivisionStart, lengthInDivisions);
	}

	public void WriteRest(int beatStart, int beatDivisionStart, int lengthInDivisions)
	{
		Write(NoteInput.Rest, beatStart, beatDivisionStart, lengthInDivisions);
	}

	void Write(NoteInput noteInput, int beatStart, int beatDivisionStart, int lengthInDivisions)
	{
		if (beatStart >= _beats) Debug.LogError("invalid beat start: " + beatStart);
		if (beatDivisionStart >= _divisionsPerBeat) Debug.LogError("invalid beat divisionStart: " + beatDivisionStart);
		if (lengthInDivisions < 1) Debug.LogError("invalid length: " + lengthInDivisions);
		if (noteInput == NoteInput.End) Debug.LogError("can't write end of note on its own");
		if (noteInput == NoteInput.Hold) Debug.LogError("can't write held note on its own");

		int start = beatStart + beatDivisionStart;
		int end = start + lengthInDivisions;

		if (end >= _phrase.Count) Debug.LogError("length extends past end of phrase. start: " + start + ", end: " + end + ", length: " + lengthInDivisions + ", totalDivisions: " + _totalDivisions);

		if (noteInput == NoteInput.Start)
		{
			_phrase[start] = NoteInput.Start;

			for (int i = start + 1; i < end - 1; i++)
			{
				_phrase[i] = NoteInput.Hold;
			}

			_phrase[end] = NoteInput.End;
		}
		else if (noteInput == NoteInput.Rest)
		{
			for (int i = start; i < end; i++)
			{
				_phrase[i] = NoteInput.Rest;
			}
		}
	}

	public NoteInput[] GetPhrase()
	{
		return _phrase.ToArray();
	}
}
