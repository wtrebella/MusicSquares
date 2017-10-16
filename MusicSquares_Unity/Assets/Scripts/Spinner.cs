using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour {
	[SerializeField] private AudioSource _source;
	[SerializeField] private float _approxMaxAngularSpeed = 8;

	private Rigidbody _rigidbody;

	void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}

	void Update()
	{
		
	}

	void FixedUpdate()
	{
		float angularSpeed = Mathf.Abs(_rigidbody.angularVelocity.magnitude);
		float percent = Mathf.Min(_approxMaxAngularSpeed, angularSpeed / _approxMaxAngularSpeed);
		_source.volume = percent;
	}
}
