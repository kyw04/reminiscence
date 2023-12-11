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

            // �����̴��� ���ο� ���� �ִϸ��̼� ����
            if (direction.magnitude > 0.1f)
            {
                animator.SetBool("IsRun", true); // �̵� ���̸� true ����
                transform.position += direction * moveSpeed * Time.deltaTime; // Translate ��� position ���� ����
            }
            else
            {
                animator.SetBool("IsRun", false); // �̵� ���� �ƴϸ� false ����
            }

            // ��������Ʈ ���� Ȯ�� �� ���� ��������Ʈ�� �̵�
            if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < 0.1f)
            {
                currentWaypoint++;
            }

            // ��ǥ ������ �����ϸ� ����
            if (Vector3.Distance(transform.position, targetPosition.position) < 0.5f)
            {
                animator.SetBool("IsRun", false); // �̵� ���� �ƴϸ� false ����
            }

            // ĵ���� Ȱ��ȭ
            if (!canvasActivated && Vector3.Distance(playertransform.position, targetPosition.position) < 0.5f)
            {
                Puzzlecanvas.SetActive(true);
                canvas.SetActive(true);
                canvasActivated = true;
            }
        }
    }
}
