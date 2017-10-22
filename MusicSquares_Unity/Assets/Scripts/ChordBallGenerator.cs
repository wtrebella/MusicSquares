using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helm;

public class ChordBallGenerator : MonoBehaviour
{
	[SerializeField] private ChordBall _chordBallPrefab;
	[SerializeField] private int _key = 48;
	[SerializeField] private int _ballBurstSize = 7;
	[SerializeField] private float _noteDur = 0.3f;

	// m9/13
	private int[] _chord1 = new int[] {
		0,
		7,
		9,
		10,
		14,
		15,
		19,
		0 + 12,
		7 + 12,
		9 + 12,
		10 + 12,
		14 + 12,
		15 + 12,
		19 + 12
	};

	// #m9
	private int[] _chord2 = new int[] {
		-3,
		4,
		7,
		11,
		12,
		16,
		19,
		-3 + 12,
		4 + 12,
		7 + 12,
		11 + 12,
		12 + 12,
		16 + 12,
		19 + 12
	};

	// bm9
	private int[] _chord3 = new int[] {
		-6,
		1,
		4,
		8,
		9,
		13,
		16,
		-6 + 12,
		1 + 12,
		4 + 12,
		8 + 12,
		9 + 12,
		13 + 12,
		16 + 12
	};

	private HelmController _synth;
	private BoxCollider _boxCollider;
	private List<ChordBall> _balls = new List<ChordBall>();
	private int[] _curChord;

	public HelmController GetSynth()
	{
		return _synth;
	}

	void Awake()
	{
		_boxCollider = GetComponent<BoxCollider>();
		_synth = GetComponent<HelmController>();

		RefreshChord();
		GenerateBallBurst();
	}

	void Update()
	{
		if (_balls.Count == 0)
		{
			OnAllBallsDone();
			GenerateBallBurst();
		}
	}

	void GenerateBallBurst()
	{
		for (int i = 0; i < _ballBurstSize; i++)
		{
			GenerateBall();
		}
	}

	void OnAllBallsDone()
	{
		RefreshChord();
	}

	public void OnBallDone(ChordBall ball)
	{
		_balls.Remove(ball);
		Destroy(ball.gameObject);
	}

	public void RefreshNoteFromCurChord(ChordBall ball)
	{
		int note = GetNote();
		ball.SetNote(note, _noteDur);
	}

	void RefreshChord()
	{
		int rand = Random.Range(0, 3);
		if (rand == 0) _curChord = _chord1;
		else if (rand == 1) _curChord = _chord2;
		else if (rand == 2) _curChord = _chord3;
	}

	int GetNote()
	{
		return  _key + _curChord[Random.Range(0, _curChord.Length)];
	}

	void GenerateBall()
	{
		int note = GetNote();

		ChordBall ball = Instantiate(_chordBallPrefab);
		ball.SetGenerator(this);
		ball.SetNote(note, _noteDur);
		ball.transform.position = GetSpawnPos();
		_balls.Add(ball);
	}

	Vector3 GetSpawnPos()
	{
		Vector3 spawnPos = Vector3.zero;
		spawnPos.x = transform.position.x + Random.Range(-_boxCollider.size.x / 2f * transform.localScale.x, _boxCollider.size.x / 2f * transform.localScale.x);
		spawnPos.y = transform.position.y + Random.Range(-_boxCollider.size.y / 2f * transform.localScale.y, _boxCollider.size.y / 2f * transform.localScale.y);
		spawnPos.z = transform.position.z + Random.Range(-_boxCollider.size.z / 2f * transform.localScale.z, _boxCollider.size.z / 2f * transform.localScale.z);
		return spawnPos;
	}
}
