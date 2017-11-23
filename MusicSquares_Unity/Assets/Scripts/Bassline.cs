using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioHelm;

public class Bassline : MonoBehaviour
{
   [SerializeField] private int _baseNote = 50;

   private Sampler _sampler;
   private int[] _measure;
   private bool _isPlaying = false;

   void Awake()
   {
      Metronome.instance.SignalBeat += OnBeat;
      Metronome.instance.SignalMeasure += OnMeasure;
      Metronome.instance.SignalPhrase += OnPhrase;
      Metronome.instance.SignalSixteenth += OnSixteenth;

      _sampler = GetComponent<Sampler> ();
//		_measure = new int[]
//		{
//			12, 10, 8, 7, 		// I
//			0, 3, 7, 10,		// I
//			12, 7, 10, 11,		// I
//			12, 14, 15, 16,		// I
//
//			17, 9, 10, 11,		// IV
//			12, 10, 3, 2,		// IV
//			0, 2, 3, 4,			// I
//			5, 6, 7, 6,			// I
//
//			7, 5, 7, 3,			// V
//			5, 3, 10, 11,		// IV
//			12, 0, 3, 1,		// I
//			2, 8, 7, 11			// ii V
//		};

      RhythmicPhrase phrase = new RhythmicPhrase (4, 4);
      for (int i = 0; i < 16; i++) {
         if (Random.value < 0.5f)
            phrase.WriteNote (i / 4, i % 4, 1);
         else
            phrase.WriteRest (i / 4, i % 4, 1);
      }

      NoteInput[] noteInputPhrase = phrase.GetPhrase ();
      _measure = new int[noteInputPhrase.Length];
      for (int i = 0; i < noteInputPhrase.Length; i++) {
         NoteInput input = noteInputPhrase [i];
         if (input == NoteInput.Start)
            _measure [i] = 0;
         else if (input == NoteInput.Rest)
            _measure [i] = -100000;
      }
   }

   void OnDestroy()
   {
      if (Metronome.DoesExist ()) {
         Metronome.instance.SignalBeat -= OnBeat;
         Metronome.instance.SignalMeasure -= OnMeasure;
         Metronome.instance.SignalPhrase -= OnPhrase;
         Metronome.instance.SignalSixteenth -= OnSixteenth;
      }
   }

   void OnBeat(int beat)
   {
		
   }

   public void StartPlaying()
   {
      _isPlaying = true;
   }

   public void StopPlaying()
   {
      _isPlaying = false;
   }

   void OnMeasure(int measure)
   {
		
   }

   void OnPhrase(int phrase)
   {

   }

   void OnSixteenth(int sixteenth)
   {
      if (!_isPlaying)
         return;
      int sixteenthIndex = (sixteenth - 1) % 16;
      int note = _measure [sixteenthIndex] + _baseNote;
      float beatDur = Metronome.instance.GetBeatDur ();
      _sampler.NoteOn (note, beatDur);
   }
}
