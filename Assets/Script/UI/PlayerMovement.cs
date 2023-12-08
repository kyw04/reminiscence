using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 5f;
    public Transform playertransform;
    public Transform targetPosition;
    public GameObject Puzzlecanvas;
    public GameObject canvas;
    public bool canvasActivated = false;

    private int currentWaypoint = 0;

    private void Start()
    {
        Puzzlecanvas.SetActive(false);
        canvas.SetActive(false);
    }
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

            if (!canvasActivated && Vector3.Distance(playertransform.position, targetPosition.position) < 0.5f)
            {
                Puzzlecanvas.SetActive(true);
                canvas.SetActive(true);
                canvasActivated = true;
            }
        }
    }
}
