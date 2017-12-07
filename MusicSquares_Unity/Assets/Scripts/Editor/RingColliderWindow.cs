using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RingColliderWindow : EditorWindow {
	private GameObject _parentObj;
	private int _numColliders = 6;
	private float _ringRadius = 2.0f;
	private float _colliderRadius = 0.5f;
	private CapsuleCollider[] colliders;

	[MenuItem("Window/Ring Collider Window")]
	static void Init()
	{
		RingColliderWindow window = ScriptableObject.CreateInstance<RingColliderWindow> ();
		window.ShowUtility ();
	}

	void OnGUI()
	{
		EditorGUI.BeginChangeCheck ();
		_parentObj = (GameObject)EditorGUILayout.ObjectField ("Parent Object", _parentObj, typeof(GameObject), true);
		_numColliders = EditorGUILayout.IntField ("Num Colliders", _numColliders);
		_ringRadius = EditorGUILayout.FloatField ("Ring Radius", _ringRadius);
		_colliderRadius = EditorGUILayout.FloatField ("Collider Radius", _colliderRadius);

		if (EditorGUI.EndChangeCheck ()) {
			if (_parentObj != null) {
				RefreshNumColliders ();
				ArrangeColliders ();
				ResizeColliders ();
				RotateColliders ();
			}
		}
	}

	void RefreshNumColliders()
	{
		if (colliders == null) {
			colliders = _parentObj.GetComponentsInChildren<CapsuleCollider> ();
		}

		if (colliders.Length == _numColliders)
			return;

		if (colliders.Length < _numColliders) {
			int numToAdd = _numColliders - colliders.Length;

			for (int i = 0; i < numToAdd; i++) {
				GameObject colliderObj = new GameObject ("Collider", typeof(CapsuleCollider));
				colliderObj.transform.SetParent (_parentObj.transform);
				colliderObj.transform.localPosition = Vector3.zero;
				colliderObj.transform.localRotation = Quaternion.identity;
			}
		} else {
			for (int i = colliders.Length - 1; i >= _numColliders; i--) {
				CapsuleCollider collider = colliders [i];
				DestroyImmediate (collider.gameObject);
			}
		}

		colliders = _parentObj.GetComponentsInChildren<CapsuleCollider> ();
	}

	void ArrangeColliders()
	{
		float angle = 360.0f / _numColliders * Mathf.Deg2Rad;
		float innerRadius = _ringRadius * Mathf.Cos (Mathf.PI / _numColliders);

		for (int i = 0; i < _numColliders; i++) {
			float x = Mathf.Cos (angle * i) * innerRadius;
			float y = Mathf.Sin (angle * i) * innerRadius;
			Vector3 pos = new Vector3 (x, y, 0);
			CapsuleCollider collider = colliders [i];
			collider.transform.localPosition = pos;
		}
	}

	void ResizeColliders()
	{
		float heightMinusCaps = 2.0f * _ringRadius * Mathf.Sin (Mathf.PI / _numColliders);
		float capHeight = _colliderRadius;
		float height = heightMinusCaps + capHeight * 2;

		for (int i = 0; i < colliders.Length; i++) {
			CapsuleCollider collider = colliders [i];
			collider.height = height;
			collider.radius = _colliderRadius;
		}
	}

	void RotateColliders()
	{
		float angle = 360.0f / _numColliders;

		for (int i = 0; i < colliders.Length; i++) {
			CapsuleCollider collider = colliders [i];
			Vector3 eulers = new Vector3 (0, 0, angle * i);
			collider.transform.localEulerAngles = eulers;
		}
	}
}
