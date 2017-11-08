using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
	[SerializeField] private float _closedY = 0;
	[SerializeField] private float _openY = -10;
	[SerializeField] private float _openDur = 3.0f;

	private bool _isOpen = true;

	void Awake()
	{
		Vector3 pos = transform.localPosition;
		pos.y = _closedY;
		transform.localPosition = pos;
	}

	public bool IsOpen()
	{
		return _isOpen;
	}

	public void Open()
	{
		_isOpen = true;
		transform.DOLocalMoveY(_openY, _openDur).SetEase(Ease.InOutSine);
	}

	public void Close()
	{
		_isOpen = false;
		transform.DOLocalMoveY(_closedY, _openDur).SetEase(Ease.InOutSine);
	}
}
