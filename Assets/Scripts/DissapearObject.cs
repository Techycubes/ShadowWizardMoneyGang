using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissapearObject : MonoBehaviour
{
    public static bool CanStart;
public void Disappear()
    {
        
        Destroy(gameObject); // Permanently removes the object from the scene
        CanStart = true;
        Debug.Log("Worked");
    }

    // Example: Destroy after a delay (for testing)
    void Start()
    {
        CanStart = false;
        Debug.Log("abcde");
        Invoke("Disappear", 4f);
         // Disappears after 5 seconds
         
    }
}
