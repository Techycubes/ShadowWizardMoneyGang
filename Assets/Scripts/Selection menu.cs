using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectionmenu : MonoBehaviour
{
    public static string CarName = "default";
    // Start is called before the first frame update
    public void Select(int i) // i could be ignored if toggling is the goal
    {
        if(i==0){
            CarName = "default";
        }else if(i==1){
            CarName = "Car2";
        } else if(i==2){
            CarName = "Car3";
        }else{
            CarName = "default";
        }
    }
}
