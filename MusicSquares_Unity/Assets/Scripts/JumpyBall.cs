using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioHelm;

public class JumpPointBeatPair
{
   public JumpPointBeat point0;
   public JumpPointBeat point1;

   public JumpPointBeatPair (JumpPointBeat inPoint0, JumpPointBeat inPoint1)
   {
      point0 = inPoint0;
      point1 = inPoint1;
   }
}

public class JumpPointBeat
{
   public JumpPoint jumpPoint;
   public int beat;

   public JumpPointBeat (JumpPoint inJumpPoint, int inBeat)
   {
      jumpPoint = inJumpPoint;
      beat = inBeat;
   }
}

public class JumpyBall : MonoBehaviour
{
   [SerializeField] private AnimationCurve _jumpTween;
   [SerializeField] private int _key = 48;

   private HelmController _synth;
   private JumpPointBeatPair _beatPair;
   private Renderer _renderer;
   private int[] _notes;

   void Awake()
   {
      _synth = GameObject.Find ("Jumpy Ball Synth").GetComponent<HelmController> ();
      _notes = Scales.MinorPentatonic;
      _renderer = GetComponent<Renderer> ();
      InitColor ();
      Metronome.instance.SignalBeat += OnBeat;
   }

   void InitColor()
   {
      Color c = new Color (Random.Range (0.3f, 1.0f), Random.Range (0.3f, 1.0f), Random.Range (0.3f, 1.0f));
      MaterialPropertyBlock propBlock = new MaterialPropertyBlock ();
      propBlock.SetColor ("_EmissionColor", c);
      _renderer.SetPropertyBlock (propBlock);
   }

   void Start()
   {
      JumpPoint firstPoint = JumpPointManager.instance.GetRandomJumpPoint ();
      JumpPoint secondPoint = JumpPointManager.instance.GetRandomJumpPoint ();
      JumpPointBeat firstBeatPoint = new JumpPointBeat (firstPoint, -1);
      JumpPointBeat secondBeatPoint = new JumpPointBeat (secondPoint, 0);
      _beatPair = new JumpPointBeatPair (firstBeatPoint, secondBeatPoint);
   }

   void OnDestroy()
   {
      if (Metronome.DoesExist ()) {
         Metronome.instance.SignalBeat -= OnBeat;
      }
   }

   void OnBeat(int beat)
   {
      GetNewBeatPoint ();
      PlayNote ();
   }

   void PlayNote()
   {
      int note = _key + _notes [Random.Range (0, _notes.Length)];
      if (!_synth.IsNoteOn (note)) {
         _synth.NoteOn (note, 1, Metronome.instance.GetBeatDur () / 2f);
      }
   }

   void GetNewBeatPoint()
   {
      JumpPoint newJumpPoint = null;

      while (newJumpPoint == null || newJumpPoint == _beatPair.point1.jumpPoint) {
         newJumpPoint = JumpPointManager.instance.GetRandomJumpPoint ();
      }
      _beatPair.point0 = _beatPair.point1;
      _beatPair.point1 = new JumpPointBeat (newJumpPoint, _beatPair.point0.beat + 1);
   }

   void FixedUpdate()
   {
      int curBeat = Metronome.instance.GetCurBeat ();
      if (_beatPair.point0.beat != curBeat)
         return;

      float beatPercent = Metronome.instance.GetCurBeatPercent ();

      Vector3 pos = Vector3.Lerp (_beatPair.point0.jumpPoint.transform.position, _beatPair.point1.jumpPoint.transform.position, _jumpTween.Evaluate (beatPercent));
      transform.position = pos;
   }
}
