using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectionmenu : MonoBehaviour
{
    string CarName;
    // Start is called before the first frame update
    public void Select(int i) // i could be ignored if toggling is the goal
    {
        if (CarName == "default")
        {
            CarName = "Car2";
            Debug.Log("Selected Car2");
        }
        else
        {
            CarName = "default";
            Debug.Log("Selected default");
        }
    }
}
