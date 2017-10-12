using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCam : MonoBehaviour 
{
	[SerializeField] private Block _blockPrefab;
	[SerializeField] private float _smoothTime = 0.15f;

	private Vector3 _smoothVelocity;
	private Vector3 _blockSize;
	private Vector3 _targetPos;

	void Awake()
	{
		FocusCoord.instance.SignalCoordChange += OnFocusCoordChange;
		_blockSize = _blockPrefab.GetSize();
	}

	void OnDestroy()
	{
		if (FocusCoord.DoesExist())
		{
			FocusCoord.instance.SignalCoordChange -= OnFocusCoordChange;
		}
	}

	void Update()
	{
		UpdateCamPos();
	}

	void OnFocusCoordChange(int x, int z)
	{
		_targetPos = GetTargetPosFromCoord(x, z);
	}

	Vector3 GetTargetPosFromCoord(int x, int z)
	{
		Vector3 pos = transform.position;
		pos.x = _blockSize.x * x;
		pos.z = _blockSize.z * z;

		return pos;
	}

	void UpdateCamPos()
	{
		Vector3 curPos = transform.position;
		Vector3 smoothPos = Vector3.SmoothDamp(curPos, _targetPos, ref _smoothVelocity, _smoothTime);
		transform.position = smoothPos;
	}
}
