using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
	[SerializeField] private AnimationCurve _scaleCurve;
	[SerializeField] private Vector3 _minScale = new Vector3(0.5f, 1.0f, 0.5f);

	private BoxCollider _boxCollider;
	private int _x;
	private int _z;
	private Vector3 _size;
	private MeshRenderer _meshRenderer;
	private bool _isFocused = false;
	private Vector3 _targetScale = Vector3.one;
	private bool _isDirty = false;

	void Awake()
	{
		_meshRenderer = GetComponentInChildren<MeshRenderer>();
		_boxCollider = GetComponentInChildren<BoxCollider>();

		_size = new Vector3
		(
			_boxCollider.size.x * _boxCollider.transform.localScale.x,
			_boxCollider.size.y * _boxCollider.transform.localScale.y,
			_boxCollider.size.z * _boxCollider.transform.localScale.z
		);

		RandomizeColor();
		Metronome.instance.SignalBeatImmediate += OnBeat;
	}

	void OnDestroy()
	{
		if (Metronome.DoesExist())
		{
			Metronome.instance.SignalBeatImmediate -= OnBeat;
		}
	}

	void OnBeat(int beat)
	{

	}

	public void Focus()
	{
		_isFocused = true;
	}

	void Update()
	{
		if (_isDirty)
		{
			transform.localScale = _targetScale;
			_isDirty = false;
		}
	}

	public void Unfocus()
	{
		_isFocused = false;
		transform.localScale = Vector3.one;
	}

	void OnAudioFilterRead(float[] data, int channels)
	{
		if (_isFocused)
		{
			UpdateScaling();
			_isDirty = true;
		}
	}

	void UpdateScaling()
	{
		float beatPercent = Metronome.instance.GetBeatPercent();
		_targetScale = Vector3.Lerp(_minScale, Vector3.one, _scaleCurve.Evaluate(beatPercent));
	}

	void RandomizeColor()
	{
		Color c = GetRandomColor();

		MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
		propBlock.SetColor("_Color", c);
		_meshRenderer.SetPropertyBlock(propBlock);
	}

	Color GetRandomColor()
	{
		Color c = new Color(Random.Range(0.3f, 0.9f), Random.Range(0.3f, 0.9f), Random.Range(0.3f, 0.9f));
		return c;
	}

	public Vector3 GetSize()
	{
		if (_boxCollider != null)
		{
			return _size;
		}
		else
		{
			// hacky: for if a non-instantiated object (for example: prefab) needs to give its size
			_boxCollider = GetComponentInChildren<BoxCollider>();
			_size = new Vector3
				(
					_boxCollider.size.x * _boxCollider.transform.localScale.x,
					_boxCollider.size.y * _boxCollider.transform.localScale.y,
					_boxCollider.size.z * _boxCollider.transform.localScale.z
				);
			return _size;
		}
	}

	public void ResetBlock()
	{
		RandomizeColor();
	}

	public void SetCoord(int x, int z)
	{
		_x = x;
		_z = z;

		name = "Block (" + x + ", " + z + ")";
		Vector3 pos = new Vector3();
		pos.x = _size.x * x;
		pos.y = -_size.y / 2f;
		pos.z = _size.z * z;
		transform.position = pos;
	}

	public int GetX()
	{
		return _x;
	}

	public int GetZ()
	{
		return _z;
	}
}
