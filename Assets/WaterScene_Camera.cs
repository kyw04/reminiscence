using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaterScene_Camera : MonoBehaviour
{
    public Transform[] waypoints; 
    public float speed = 5f;
    public Transform target;
    private int currentWaypointIndex = 0;
    public Camera mainCamera;
    public GameObject Puzzlecanvas;
    public GameObject canvas;

    void Start()
    {
        if (Puzzlecanvas != null) Puzzlecanvas.SetActive(false);
        if (canvas != null) canvas.SetActive(false);
    }
    void Update()
    {
        transform.LookAt(target.position);
        if (currentWaypointIndex < waypoints.Length)
        {
            // 현재 지점으로 이동
            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, speed * Time.deltaTime);

            // 현재 지점에 도착하면 다음 지점으로 이동
            if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
            {
                currentWaypointIndex++;
            }
        }
        else
        {
            transform.position = mainCamera.transform.position;
            transform.rotation = mainCamera.transform.rotation;
            SwitchToMainCamera();
        }

    }

    void SwitchToMainCamera()
    {
        Camera.main.enabled = true;
        gameObject.SetActive(false);
        if (Puzzlecanvas != null) Puzzlecanvas.SetActive(false);
        if (canvas != null) canvas.SetActive(false);
    }
}
