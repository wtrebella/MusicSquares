using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helm;

public class LevelManager : Singleton<LevelManager>
{
	[SerializeField] private Bassline _bassline;

	void Awake()
	{
		_bassline.StartPlaying();
	}
}
