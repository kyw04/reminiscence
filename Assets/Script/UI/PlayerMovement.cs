using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 5f;

    private int currentWaypoint = 0;

    void Update()
    {
        if (currentWaypoint < waypoints.Length)
        {
            
            Vector3 direction = waypoints[currentWaypoint].position - transform.position;
            direction.Normalize();

            
            transform.Translate(direction * moveSpeed * Time.deltaTime);

            
            if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < 0.1f)
            {
                currentWaypoint++;
            }
        }
    }
}
