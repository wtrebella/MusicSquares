using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chords : MonoBehaviour
{
	public static int[] MajorTriad = new int[] { 0, 4, 7 };
	public static int[] MinorTriad = new int[] { 0, 3, 7 };
	public static int[] AugmentedTriad = new int[] { 0, 4, 8 };
	public static int[] DiminishedTriad = new int[] { 0, 3, 6 };
	public static int[] MajorSixth = new int[] { 0, 4, 7, 9 };
	public static int[] MinorSixth = new int[] { 0, 4, 7, 9 };
	public static int[] DominantSeventh = new int[] { 0, 4, 7, 10 };
	public static int[] MajorSeventh = new int[] { 0, 4, 7, 11 };
	public static int[] MinorSeventh = new int[] { 0, 3, 7, 10 };
	public static int[] AugmentedSeventh = new int[] { 0, 4, 8, 10 };
	public static int[] DiminishedSeventh = new int[] { 0, 3, 6, 9 };
	public static int[] HalfDiminishedSeventh = new int[] { 0, 3, 6, 10 };
	public static int[] MinorMajorSeventh = new int[] { 0, 3, 7, 11 };

	public static int[] MinorNine = new int[] { 0, 3, 7, 10, 14 };
}