using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helm;

public class ChordBall : MonoBehaviour
{
	[SerializeField] private float _initialForceStrength = 1.0f;

	private ChordBallGenerator _generator;
	private Rigidbody _rigidbody;
	private HelmController _synth;
	private int _note = 96;
	private float _noteDur = 0.3f;

	void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
		AddInitialForce();
	}

	void AddInitialForce()
	{
		Vector3 force = Random.insideUnitSphere * _initialForceStrength;
		_rigidbody.AddForce(force * _rigidbody.mass, ForceMode.Impulse);
	}

	public void SetNote(int note, float noteDur)
	{
		_note = note;
		_noteDur = noteDur;
	}

	public void SetGenerator(ChordBallGenerator generator)
	{
		_generator = generator;
	}

	void OnCollisionEnter(Collision collision)
	{
		if (!collision.collider.CompareTag("Platform")) return;

		float beatDur = Metronome.instance.GetBeatDur();
		float velocityMagnitude = collision.relativeVelocity.magnitude;

		float velocityAlpha = Mathf.InverseLerp(0, collision.relativeVelocity.magnitude, velocityMagnitude);
		_generator.GetSynth().NoteOn(_note, velocityAlpha, _noteDur);
		_generator.RefreshNoteFromCurChord(this);
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.CompareTag("OffCourseTrigger"))
		{
			_generator.OnBallDone(this);
		}
	}
}
