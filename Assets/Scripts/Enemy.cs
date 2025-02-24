using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float EnemySpeed = 5.0f;
    public GameObject[]wayPoints;
    private int currentWaypoint;



    // Update is called once per frame
    void Update()
    {
       if (Vector2.Distance(wayPoints[currentWaypoint].transform.position, transform.position) < .1f)
       {
        currentWaypoint++;
        transform.Rotate(0,180f,0);
        if (currentWaypoint >= wayPoints.Length)
        {
            currentWaypoint = 0;
        }

       } 
       transform.position = Vector2.MoveTowards(transform.position, wayPoints[currentWaypoint].transform.position, Time.deltaTime * EnemySpeed);
    }
}
