using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    public static int PlayerLives = 3;
        public TMP_Text DeathCounterText;
        void Start(){
            UpdateText();
        }
private void OnCollisionEnter2D(Collision2D collision){
    if(collision.gameObject.CompareTag("Enemy")){
        PlayerLives--;
        UpdateText();
    if(PlayerLives>0){
        Debug.Log("player lives left: " + PlayerLives);
        RestartLevel();
    }else{
        Debug.Log("Game Over");
        SceneManager.LoadScene("EndScreen 1");
    }
    }
    }


private void OnTriggerEnter2D(Collider2D collision){
if(collision.gameObject.CompareTag("DeathZone")){
    PlayerLives--;
    if(PlayerLives>0){
        Debug.Log("player lives left: " + PlayerLives);
        RestartLevel();
    }else{
        Debug.Log("Game Over");
        SceneManager.LoadScene("EndScreen 1");
    }
}}

private void RestartLevel(){
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
void UpdateText(){
     DeathCounterText.text = "Lives left: "+ PlayerLives;
}
}

