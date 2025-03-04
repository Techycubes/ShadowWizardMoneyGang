using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public static string CarName = "default";

    public void StartGame(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
        Debug.Log("hi");
    }

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