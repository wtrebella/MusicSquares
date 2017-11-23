using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioHelm;

public class LevelManager : Singleton<LevelManager>
{
   [SerializeField] private Bassline _bassline;

   void Awake()
   {
      _bassline.StartPlaying ();
   }
}
