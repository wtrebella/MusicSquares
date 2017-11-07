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

	public static int[] Shuffle(int[] notes)
	{
		for (int i = 0; i < notes.Length; i++) {
			int tmp = notes [i];
			int r = Random.Range (i, notes.Length);
			notes [i] = notes [r];
			notes [r] = tmp;
		}

		return notes;
	}

	public static int[] ExtractRandom(int[] notes, int numToExtract)
	{
		if (numToExtract < 0 || numToExtract > notes.Length)
			Debug.LogError ("can't extract " + numToExtract + " notes from array of length " + notes.Length);

		notes = Shuffle (notes);

		int[] extractedNotes = new int[numToExtract];
		for (int i = 0; i < numToExtract; i++) 
		{
			extractedNotes [i] = notes [i];
		}

		return extractedNotes;
	}
}
