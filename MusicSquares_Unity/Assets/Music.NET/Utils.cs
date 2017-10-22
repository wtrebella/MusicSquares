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
}
