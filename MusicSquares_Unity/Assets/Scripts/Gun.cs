using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
	public void Shoot(Vector3 dir)
	{
		Ray ray = new Ray(transform.position, dir);
		RaycastHit hit;
		Physics.Raycast(ray, out hit, float.MaxValue, 1 << LayerMask.NameToLayer("Target"));
		Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green, 0.2f);
		if (hit.collider)
		{
			Target target = hit.collider.GetComponent<Target>();
			target.Hit();
		}
	}
}
