
using UnityEngine;


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
        // Implement movement in both horizontal and vertical directions
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0.0f, verticalInput) * speed * Time.deltaTime;
        controller.Move(moveDirection);
    }
    void movePivot()
    {
        cameraPivot.position = pivotOffsest + controller.transform.position;
    }

    void moveController()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0.0f, verticalInput) * speed * Time.deltaTime;
        controller.Move(moveDirection);

        // Apply gravity
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
            float mouseX = Input.GetAxis("Mouse X") * sensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

            targetYRotation += mouseX;
            targetXRotation -= mouseY;
            targetXRotation = Mathf.Clamp(targetXRotation, xRotationLimits.x, xRotationLimits.y);
        }

        currentCameraXRotation = Mathf.SmoothDamp(currentCameraXRotation, targetXRotation, ref xRotationVelocity, lookSmoothTime);
        currentCameraYRotation = Mathf.SmoothDamp(currentCameraYRotation, targetYRotation, ref yRotationVelocity, lookSmoothTime);

        cameraPivot.rotation = Quaternion.Euler(currentCameraXRotation, currentCameraYRotation, 0f);

        RaycastHit hitInfo;
        Ray camRay = new Ray(cameraPivot.position, -cameraPivot.forward);

        if (Physics.Raycast(camRay, out hitInfo, cameraDistance, camRaycastMask))
        {
            cameraTransform.position = hitInfo.point;
        }
        else
        {
            cameraTransform.localPosition = new Vector3(0f, 0f, -cameraDistance);
        }
    }
}
