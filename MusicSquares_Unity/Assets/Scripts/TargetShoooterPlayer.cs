using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetShoooterPlayer : MonoBehaviour
{
	private Gun _gun;
	private Camera _camera;

	void Awake()
	{
		_camera = GetComponentInChildren<Camera>();
		_gun = GetComponentInChildren<Gun>();
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			StartCoroutine("ShootRoutine");
		}
		if (Input.GetMouseButtonUp(0))
		{
			StopCoroutine("ShootRoutine");
		}
	}

	IEnumerator ShootRoutine()
	{
		_gun.Shoot(_camera.transform.forward);

		float t = 0;
		float interval = 0.5f;

		while (true)
		{
			t += Time.deltaTime;
			if (t >= interval)
			{
				t = Random.Range(-interval / 4f, interval / 4f);
				_gun.Shoot(_camera.transform.forward);
			}
			yield return null;
		}
	}
}
