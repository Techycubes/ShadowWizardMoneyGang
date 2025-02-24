using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class coincollection : MonoBehaviour
{
    public int coinCollectionCounter = 0;
    public TMP_Text CounterText;
private void OnTriggerEnter2D(Collider2D other){

    if (other.gameObject.CompareTag("coin")){
        Destroy(other.gameObject);
        coinCollectionCounter++;
        UpdateScore();
    }
     if (other.gameObject.CompareTag("portal")){
        Destroy(other.gameObject);
        NextLvl();
    }
         if (other.gameObject.CompareTag("Ending")){
        Destroy(other.gameObject);
        EndScreen();
    }
}

    void UpdateScore(){
        CounterText.text = "Coins: " + coinCollectionCounter;
    }

    void NextLvl(){
        SceneManager.LoadScene("Level2");
}
void EndScreen(){
    SceneManager.LoadScene("EndScreen");
}
}
