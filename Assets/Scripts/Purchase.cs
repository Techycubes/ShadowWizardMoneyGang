using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Purchase : MonoBehaviour
{
    public static bool is2Purchase = false;
    public static bool is3Purchase = false;
    public void Select(string car) // i could be ignored if toggling is the goal
    {
        if (car == "Car2" && !is2Purchase)
        {
            is2Purchase = true;
        }
        if (car == "Car3" && !is3Purchase )
        {
            is3Purchase = true;
        }
    }
}
