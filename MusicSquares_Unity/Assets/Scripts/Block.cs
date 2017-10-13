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
	private float _timer = 0;

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
	}

	public void Focus()
	{
		_isFocused = true;
	}

	public void Unfocus()
	{
		_isFocused = false;
		transform.localScale = Vector3.one;
	}

	void Update()
	{
		if (_isFocused)
		{
			UpdateScaling();
		}
	}

	void UpdateScaling()
	{
		_timer += Time.deltaTime;
		if (_timer >= Metronome.instance.GetBeatDur())
		{
			_timer = 0;
		}

		Vector3 scale = Vector3.Lerp(_minScale, Vector3.one, _scaleCurve.Evaluate(_timer));
		transform.localScale = scale;
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
