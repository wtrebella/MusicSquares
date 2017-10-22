using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scales : MonoBehaviour
{
   public static int[] Major = new int[] { 0, 2, 4, 5, 7, 9, 11 };
   
   public static int[] NaturalMinor = new int[] { 0, 2, 3, 5, 7, 8, 10 };
   public static int[] HarmonicMinor = new int[] { 0, 2, 3, 5, 7, 8, 11 };
   public static int[] MelodicMinor = new int[] { 0, 2, 3, 5, 7, 9, 11 };
   
   public static int[] MajorPentatonic = new int[] { 0, 2, 3, 7, 9 };
   public static int[] MinorPentatonic = new int[] { 0, 3, 5, 7, 10 };
   
   public static int[] Blues = new int[] { 0, 3, 5, 6, 7, 10 };
}
