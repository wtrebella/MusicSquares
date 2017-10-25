using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helm;

public class Target : MonoBehaviour
{
	[SerializeField] private int _note;
	[SerializeField] private int _key = 48;
	[SerializeField] private float _scaleMultiplier = 0.5f;
	[SerializeField] private float _scaleSpeed = 10;
	[SerializeField] private AnimationCurve _scaleCurve;

	private HelmController _synth;
	private float _scaleAlpha = 2;
	private Vector3 _initialScale;
	private Vector3 _targetScale;

	void Awake()
	{
		_synth = GetComponent<HelmController>();
		_initialScale = transform.localScale;
		_targetScale = _initialScale * _scaleMultiplier;
	}

	public void Hit()
	{
		_synth.NoteOn(_key + _note, 1, 0.15f);
		_scaleAlpha = 0;
	}

	void Update()
	{
		if (_scaleAlpha < 2)
		{
			Vector3 scale = Vector3.Lerp(_initialScale, _targetScale, _scaleCurve.Evaluate(_scaleAlpha));
			transform.localScale = scale;
			_scaleAlpha += Time.deltaTime * _scaleSpeed;
		}
		else
		{
			transform.localScale = _initialScale;
		}

		Quaternion angleBy = Quaternion.AngleAxis(-50 * Time.deltaTime, transform.up);
		Quaternion rot = angleBy * transform.localRotation;
		transform.localRotation = rot;
	}
}
