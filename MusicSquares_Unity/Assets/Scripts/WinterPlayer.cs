using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinterPlayer : MonoBehaviour
{
	private Camera _cam;

	void Awake()
	{
		_cam = GetComponentInChildren<Camera>();
	}
	
	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = new Ray(_cam.transform.position, _cam.transform.forward);
			RaycastHit hit;
			Physics.Raycast(ray, out hit, 10.0f, 1 << LayerMask.NameToLayer("Tree"));
			if (hit.collider != null)
			{
				Fir fir = hit.collider.gameObject.GetComponentInParent<Fir>();
				fir.Hit();
			}
		}
	}
}
