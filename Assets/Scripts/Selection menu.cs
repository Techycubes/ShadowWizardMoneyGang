using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Selectionmenu : MonoBehaviour
{
    public static string CarName = "default";
    public TMP_Text SelectText;
    // Start is called before the first frame update
    public void Select(int i) // i could be ignored if toggling is the goal
    {
        
        if(i==0){
            CarName = "default";
            SelectText.text = "Selected: Default";
        }else if(i==1 && Purchase.is2Purchase == true){
            CarName = "Car2";
            SelectText.text = "Selected: Car2";
        } else if(i==2 && Purchase.is3Purchase == true){
            CarName = "Car3";
            SelectText.text = "Selected: Car3";
        }else {
            CarName = "default";
            SelectText.text = "Selected: Default";
        }
    }
    void Start(){

    }
    void Update(){
        SelectText.text = "Selected: " + CarName;
    }

}
