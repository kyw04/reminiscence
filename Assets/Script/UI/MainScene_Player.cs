using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.XR;

public class MainScene_Player : MonoBehaviour
{
    [Header("Reference Fidlds")]
    public LayerMask camRaycastMask;
    public CharacterController controller;
    public Transform cameraTransform;
    public Transform cameraPivot;

    [Header("Numeric Fields")]
    public float controllerLookSommthTime;
    public float lookSmoothTime;
    public float cameraDistance;
    [Space]
    public Vector3 pivotOffsest;
    public Vector2 xRotationLimits;

    public float gravity;
    public float sensitivity;
    public float speed = 5f;
    public float AutomoveSpeed = 5f;
    public float distanceToMove;

    private float distanceMoved = 0f;
    private bool shouldMove = false;
    private bool Run = false;

    private float yVelocity;
    private float yAngleOffset;
    private float currentControllerYRotation;
    private float targetControllerYRotation;
    private float controllerYRotationVeloctiy;

    private float currentCameraXRotation;
    private float currentCameraYRotation;
    private float xRotationVelocity;
    private float yRotationVelocity;
    private float targetYRotation;
    private float targetXRotation;

    private Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        setCameraLook();
        setControllerLook();
        moveController();
        movePivot();
        move();

        if (shouldMove)
        {
            // 일정 방향으로 이동
            transform.Translate(Vector3.forward * AutomoveSpeed * Time.deltaTime);
            distanceToMove -= AutomoveSpeed * Time.deltaTime;

            // 이동할 거리를 다 이동하면 이동 중단
            if (distanceToMove <= 0)
            {
                shouldMove = false;
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StartGround")) // 콜라이더의 태그에 따라 변경 가능
        {
            shouldMove = true;
        }
    }

    void move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 moveDirection = new Vector3(horizontalInput, 0.0f, 0.0f);
        controller.Move(moveDirection * speed * Time.deltaTime);
    }
    void movePivot()
    {
        cameraPivot.position = pivotOffsest + controller.transform.position;
    }

    void moveController()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        if(x * x + y * y > 0)
        {
            controller.Move(controller.transform.forward * speed * Time.deltaTime);
        }

        yVelocity = controller.isGrounded ? 0f : yVelocity - gravity * Time.deltaTime;
        yVelocity -= gravity * Time.deltaTime;
        controller.Move(Vector3.up * yVelocity * Time.deltaTime);
    }

    // Update is called once per frame
    void setControllerLook()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        if (x * x + y * y > 0)
        {
            targetControllerYRotation = Mathf.Atan2(x, y) * Mathf.Rad2Deg;
            targetControllerYRotation -= yAngleOffset;
            animator.SetBool("IsRun", true);
        }
        else
        {
            animator.SetBool("IsRun", false);
        }
        currentControllerYRotation = Mathf.SmoothDampAngle(currentControllerYRotation, targetControllerYRotation, ref controllerYRotationVeloctiy, controllerLookSommthTime);
        controller.transform.eulerAngles = new Vector3(0f, currentControllerYRotation, 0f);
    }

    void setCameraLook()
    {
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            targetYRotation += mouseX * sensitivity;
            targetXRotation += mouseY * sensitivity;
            targetXRotation = Mathf.Clamp(targetXRotation, xRotationLimits.x, xRotationLimits.y);
        }

        currentCameraXRotation = Mathf.SmoothDampAngle(currentCameraXRotation, targetXRotation, ref xRotationVelocity, lookSmoothTime);
        currentCameraYRotation = Mathf.SmoothDampAngle(currentCameraYRotation, targetYRotation, ref yRotationVelocity, lookSmoothTime);
        cameraPivot.eulerAngles = new Vector3(currentCameraXRotation, currentCameraYRotation, 0f);

        Ray camRay = new Ray(cameraPivot.position, -cameraPivot.forward);
        float maxDistance = cameraDistance;
        if(Physics.SphereCast(camRay, 0.25f, out RaycastHit hitInfo, cameraDistance, camRaycastMask))
        {
            maxDistance = (hitInfo.point - cameraPivot.position).magnitude - 0.25f;
        }

        cameraTransform.localPosition = Vector3.forward * -(maxDistance - 0.1f);
        yAngleOffset = Mathf.Atan2(cameraPivot.forward.z, cameraPivot.forward.y) * Mathf.Rad2Deg - 90f;
    }
}
