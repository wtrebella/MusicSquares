using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
	public static int Sharpen(int note)
	{
		return note + 1;  
	}

	public static int Flatten(int note)
	{
		return note - 1;  
	}

	public static int[] Transpose(int[] notes, int transposition)
	{
		int[] newNotes = new int[notes.Length];

		for (int i = 0; i < notes.Length; i++)
		{
			newNotes[i] = notes[i] + transposition;
		}

		return newNotes;
	}
}
