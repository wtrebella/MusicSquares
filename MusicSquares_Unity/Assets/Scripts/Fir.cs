using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Helm;

public class Fir : MonoBehaviour
{
	[SerializeField] private Transform _top;
	[SerializeField] private Transform _trunk;
	[SerializeField] private ParticleSystem _snow;

	[SerializeField] private Vector2 _widthScaleMinMax = new Vector2(0.5f, 1.5f);
	[SerializeField] private Vector2 _heightScaleMinMax = new Vector2(0.5f, 2.0f);
	[SerializeField] private Vector2 _trunkWidthMinMax = new Vector2(0.5f, 2.5f);
	[SerializeField] private Vector2 _trunkHeightMinMax = new Vector2(1.0f, 3.0f);

	[SerializeField] private int _key = 48;

	private HelmController _synth;
	private int[] _chord;

	void Awake()
	{
		_synth = GetComponent<HelmController>();

		float val = Random.value;
		if (val < 0.333f)
		{
			_chord = Chords.MajorSeventh;
		}
		else if (val < 0.667f)
		{
			_chord = Utils.Transpose(Chords.MajorSeventh, 5);
		}
		else
		{
			_chord = Utils.Transpose(Chords.MinorNine, 9);
		}

		Randomize();
	}

	void Start()
	{
		ParticleSystem.ShapeModule shape = _snow.shape;
		float b = shape.length * Mathf.Tan (shape.angle * Mathf.Deg2Rad);
		float h = shape.length * _top.transform.localScale.y;
//		Debug.Log (shape.angle + ", " + (Mathf.Atan2 (shape.length, b) * Mathf.Rad2Deg));
		float angle = 90 - Mathf.Atan2 (h, b) * Mathf.Rad2Deg;
//		Debug.Log (b + ", " + h + ", " + angle);
		shape.angle = angle;
		shape.length = h;
	}

	void Update()
	{
		
	}

	public void Hit()
	{
		Shake();
		PlayChord();
		PlaySnow ();
	}

	void PlaySnow()
	{
		_snow.Play ();
	}

	void Randomize()
	{
		float topWidthScale = Random.Range(_widthScaleMinMax.x, _widthScaleMinMax.y);
		float topHeightScale = Random.Range(_heightScaleMinMax.x, _heightScaleMinMax.y);
		float trunkWidthScale = Random.Range(_trunkWidthMinMax.x, _trunkWidthMinMax.y);
		float trunkHeight = Random.Range(_trunkHeightMinMax.x, _trunkHeightMinMax.y);

		_top.transform.localScale = new Vector3(topWidthScale, topHeightScale, topWidthScale);
		_trunk.transform.localScale = new Vector3(trunkWidthScale, _trunk.transform.localScale.y, trunkWidthScale);

		Vector3 topPos = _top.transform.localPosition;
		topPos.y = trunkHeight;
		_top.transform.localPosition = topPos;

		float angle = Random.Range(0, 360);
		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up) * transform.localRotation;
		transform.localRotation = rotation;
	}

	void Shake()
	{
		_top.transform.DOShakePosition(0.3f, 0.2f, 20);
	}

	void PlayChord()
	{
		for (int i = 0; i < _chord.Length; i++)
		{
			int note = _key + _chord[i];
			_synth.NoteOn(note, 1, Metronome.instance.GetEighthDur());
		}
	}
}
