using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Woods : MonoBehaviour
{
	[SerializeField] private float _size = 50.0f;
	[SerializeField] private float _fill = 0.5f;
	[SerializeField] private float _tileSize = 3.0f;
	[SerializeField] private Fir _firPrefab;

	NoteInput[] _phrase;

	void Awake()
	{
		CreateTrees();
		CreateRhythm ();

		Metronome.instance.SignalEighth += OnEighth;
	}

	void OnEighth(int eighth)
	{
		eighth %= _phrase.Length;
	}

	void CreateRhythm()
	{
		int numBeats = 16;
		int numDivisions = 2;

		RhythmicPhrase rhythmicPhrase = new RhythmicPhrase (numBeats, numDivisions);

		for (int beat = 0; beat < numBeats; beat++) {
			for (int division = 0; division < numDivisions; division++)
			{
				if (Random.value < 0.3) {
					rhythmicPhrase.WriteNote (beat, division, 1);
				} else {
					rhythmicPhrase.WriteRest (beat, division, 1);
				}
			}
		}

		_phrase = rhythmicPhrase.GetPhrase ();
	}

	void CreateTrees()
	{
		int divisions = (int)(_size / _tileSize);
		Vector3 origin = new Vector3(-_size / 2f, 0, -_size / 2f);

		for (int z = 0; z < divisions; z++)
		{
			for (int x = 0; x < divisions; x++)
			{
				if (Random.value > _fill) continue;

				Fir fir = Instantiate(_firPrefab);
				Vector3 treePos = new Vector3(origin.x + _tileSize * x + _tileSize / 2f, origin.y, origin.z + _tileSize * z + _tileSize / 2f);
				treePos.x += Mathf.Lerp(-_tileSize / 2f, _tileSize / 2f, Random.value);
				treePos.z += Mathf.Lerp(-_tileSize / 2f, _tileSize / 2f, Random.value);
				fir.transform.SetParent(transform);
				fir.transform.localPosition = treePos;
			}
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(transform.position + new Vector3(0, 10, 0), new Vector3(_size, 20, _size));
	}
}
