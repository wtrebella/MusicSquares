using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helm;

public class SyncSequencerWithMetronome : MonoBehaviour
{
	private Sequencer _sequencer;

	void Awake()
	{
		_sequencer = GetComponent<Sequencer>();
		_sequencer.enabled = false;
	}

	void Start()
	{
		_sequencer.StartSequencerScheduled(Metronome.instance.GetSyncTime());
	}
}
