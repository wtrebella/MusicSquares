using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helm;

public static class SamplerExtensions 
{
	public static void NoteOn(this Sampler sampler, int note, float length, float velocity = 1.0f)
	{
		sampler.NoteOnScheduled(note, velocity, 0, (double)length);
	}
}
