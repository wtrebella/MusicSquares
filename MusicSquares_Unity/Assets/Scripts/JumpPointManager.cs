using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPointManager : Singleton<JumpPointManager>
{
	private JumpPoint[] _jumpPoints;

	void Awake()
	{
		_jumpPoints = GetComponentsInChildren<JumpPoint>();
	}

	public JumpPoint GetRandomJumpPoint()
	{
		return _jumpPoints[Random.Range(0, _jumpPoints.Length)];
	}
}
