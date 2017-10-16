using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCam : MonoBehaviour 
{
	[SerializeField] private Block _blockPrefab;
	[SerializeField] private float _distance = 10;
	[SerializeField] private BlockGrid _grid;
	[SerializeField] private AnimationCurve _pumpCurve;
	[SerializeField] private float _maxPumpDist = 3.0f;
	[SerializeField] private float _maxPumpSpin = 1.0f;
	[SerializeField] private float _minDist = 10.0f;
	[SerializeField] private float _minSpin = 0.0f;

	private float _pumpDist = 1.0f;
	private float _pumpSpin = 0.2f;
	private float _spin = 1.0f;
	private Vector3 _smoothVelocity;
	private Vector3 _blockSize;
	private Vector3 _targetPos;
	private float _beatTimer = 0;
	private float _prevBeatTime = 0;

	void Awake()
	{
		FocusCoord.instance.SignalCoordChange += OnFocusCoordChange;
		_blockSize = _blockPrefab.GetSize();
		OnFocusCoordChange(0, 0);
		Metronome.instance.SignalBeat += OnBeat;
	}

	void OnDestroy()
	{
		if (FocusCoord.DoesExist())
		{
			FocusCoord.instance.SignalCoordChange -= OnFocusCoordChange;
		}

		if (Metronome.DoesExist())
		{
			Metronome.instance.SignalBeat -= OnBeat;
		}
	}

	void OnBeat(int beat)
	{
		_beatTimer = 0;
	}

	void Update()
	{
		UpdatePump();
		UpdateCamRotation();
		UpdateCamPos();
	}

	void UpdatePump()
	{
		float beatDur = Metronome.instance.GetBeatDur();

		float deltaTime = (float)AudioSettings.dspTime - _prevBeatTime;

		_beatTimer += deltaTime;

		float beatPercent = _beatTimer / beatDur;
		float pumpDist = _maxPumpDist * _pumpCurve.Evaluate(beatPercent) + _minDist;
		float pumpSpin = _maxPumpSpin * _pumpCurve.Evaluate(beatPercent) + _minSpin;

		_distance = pumpDist;
		_spin = pumpSpin;

		_prevBeatTime = (float)AudioSettings.dspTime;
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
		Vector3 vector = -transform.forward * _distance;
		Gizmos.color = Color.green;
		pos = vector + pos;

		return pos;
	}

	void UpdateCamRotation()
	{
		Quaternion angleRotation = Quaternion.AngleAxis(_spin, Vector3.up);

		transform.rotation = angleRotation * transform.rotation;
	}

	void UpdateCamPos()
	{
		Block focusBlock = _grid.GetFocusBlock();
		Vector3 blockPos = focusBlock.transform.position;
		Vector3 vector = -transform.forward * _distance;
		Vector3 camPos = vector + blockPos;

		transform.position = camPos;

		Debug.DrawRay(blockPos, -transform.forward * _distance + blockPos, Color.green);

//		Vector3 curPos = transform.position;
//		Vector3 smoothPos = Vector3.SmoothDamp(curPos, _targetPos, ref _smoothVelocity, _smoothTime);
//		transform.position = smoothPos;
	}
}
