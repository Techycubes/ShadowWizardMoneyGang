using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroy : MonoBehaviour
{
    public Rigidbody2D rb;
    public float bounce;
private void OnCollisionEnter2D(Collision2D collision){
    if(collision.gameObject.CompareTag("Player")){
        rb.velocity = new Vector2(rb.velocity.x, bounce);
       Destroy(transform.parent.gameObject);
    }
}
}
