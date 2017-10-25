using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
	[SerializeField] private float _radius = 10;

	private Target[] _targets;

	void Awake()
	{
		_targets = GetComponentsInChildren<Target>();
		ArrangeTargets();
	}

	void ArrangeTargets()
	{

		float anglePerTarget = (Mathf.PI * 2) / (_targets.Length + 1);

		for (int i = 0; i < _targets.Length; i++)
		{
			Vector3 pos = Vector3.zero;
			Target target = _targets[i];
			float angle = anglePerTarget * i;

			pos.x = transform.position.x + Mathf.Cos(angle) * _radius;
			pos.y = target.transform.position.y;
			pos.z = transform.position.z + Mathf.Sin(angle) * _radius;

			target.transform.position = pos;

			Vector3 eulers = target.transform.localEulerAngles;
			eulers.y = -angle * Mathf.Rad2Deg;
			target.transform.localEulerAngles = eulers;
		}
	}
}
