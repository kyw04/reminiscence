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
    private Animator animator;
    private int currentWaypoint = 0;

    void Start()
    {
        Puzzlecanvas.SetActive(false);
        canvas.SetActive(false);
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (currentWaypoint < waypoints.Length)
        {
            Vector3 direction = waypoints[currentWaypoint].position - transform.position;
            direction.Normalize();

            // 움직이는지 여부에 따라 애니메이션 제어
            if (direction.magnitude > 0.1f)
            {
                animator.SetBool("IsRun", true); // 이동 중이면 true 설정
                transform.position += direction * moveSpeed * Time.deltaTime; // Translate 대신 position 직접 조작
            }
            else
            {
                animator.SetBool("IsRun", false); // 이동 중이 아니면 false 설정
            }

            // 웨이포인트 도달 확인 및 다음 웨이포인트로 이동
            if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < 0.1f)
            {
                currentWaypoint++;
            }

            // 목표 지점에 도달하면 멈춤
            if (Vector3.Distance(transform.position, targetPosition.position) < 0.5f)
            {
                animator.SetBool("IsRun", false); // 이동 중이 아니면 false 설정
            }

            // 캔버스 활성화
            if (!canvasActivated && Vector3.Distance(playertransform.position, targetPosition.position) < 0.5f)
            {
                Puzzlecanvas.SetActive(true);
                canvas.SetActive(true);
                canvasActivated = true;
            }
        }
    }
}
