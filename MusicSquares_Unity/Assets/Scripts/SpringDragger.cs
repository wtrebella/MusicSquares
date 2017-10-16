using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringDragger : MonoBehaviour {
	[SerializeField] private SpringJoint _springJoint;
	[SerializeField] private Camera _cam;
	[SerializeField] private float _dist = 10;
	[SerializeField] private Spinner _spinner;

	private bool _connected = false;

	void Awake()
	{
		_springJoint = GetComponent<SpringJoint>();
	}

	void Update()
	{
		if (_connected)
		{
			Vector3 objPos = _springJoint.connectedBody.transform.position;
			Vector3 objToCam = _cam.transform.position - objPos;
			Vector3 objToCamDir = objToCam.normalized;
			float objToCamDist = objToCam.magnitude;
			float vectorDist = objToCamDist - _dist;
			Vector3 inputPos = Input.mousePosition;
			inputPos.z = vectorDist;
			Vector3 springPos = _cam.ScreenToWorldPoint(inputPos);
			springPos.y = objPos.y;
			transform.position = springPos;

			if (Input.GetMouseButtonUp(0))
			{
				Disconnect();
			}
		}
		else
		{
			if (Input.GetMouseButtonDown(0))
			{
				Vector3 inputPoint = Input.mousePosition;
				Vector3 camToObj = _spinner.transform.position - _cam.transform.position;
				Vector3 camToObjDir = camToObj.normalized;
				float camToObjDist = camToObj.magnitude;
				inputPoint.z = camToObjDist;
				Ray ray = _cam.ScreenPointToRay(inputPoint);
				RaycastHit hit;

				Debug.DrawRay(ray.origin, ray.direction * camToObjDist, Color.red, 0.5f);

				if (Physics.Raycast(ray, out hit, _cam.farClipPlane))
				{
					if (hit.rigidbody != null)
					{
						Connect(hit.rigidbody, hit.point);
					}
				}
			}
		}
	}

	void Connect(Rigidbody connectedBody, Vector3 hitPoint)
	{
		Vector3 localPoint = connectedBody.transform.InverseTransformPoint(hitPoint);
		_springJoint.connectedBody = connectedBody;
		_springJoint.connectedAnchor = localPoint;
		_connected = true;
	}

	void Disconnect()
	{
		_connected = false;
		_springJoint.connectedBody = null;
	}

	void OnDrawGizmos()
	{
		if (_springJoint.connectedBody != null)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.TransformPoint(_springJoint.anchor), _springJoint.connectedBody.transform.TransformPoint(_springJoint.connectedAnchor));
		}
	}
}
