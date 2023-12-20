using System.Collections;
using UnityEngine;

public class MainScene_door : MonoBehaviour
{
    public Transform player;
    public Transform door1;
    public Transform door2;
    public float triggerDistance = 5f; // 트리거 거리
    public float door1OpenAngle = -225f; // door1이 열릴 각도
    public float door2OpenAngle = 70f;
    public float doorOpenSpeed = 1f;

    public Camera mainCamera; // Reference to the main camera
    public Camera secondCamera;

    private bool door1Opened = false;
    private bool door2Opened = false;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !door1Opened && !door2Opened)
        {
            StartCoroutine(OpenDoors());
            SwitchToSecondCamera();
        }
    }

    IEnumerator OpenDoors()
    {
        /* 
         float currentAngle1 = door1.localRotation.eulerAngles.z;
        float currentAngle2 = door2.localRotation.eulerAngles.z;
         while (!door1Opened || !door2Opened)
        {
            if (!door1Opened && currentAngle1 > door1OpenAngle)
            {
                currentAngle1 -= Time.deltaTime * doorOpenSpeed;
                door1.localRotation = Quaternion.Euler(0f, 0f, currentAngle1);
                if (currentAngle1 <= door1OpenAngle)
                {
                    door1Opened = true;
                }
            }

            if (!door2Opened && currentAngle2 < door2OpenAngle)
            {
                currentAngle2 += Time.deltaTime * doorOpenSpeed;
                door2.localRotation = Quaternion.Euler(0f, 0f, currentAngle2);
                if (currentAngle2 >= door2OpenAngle)
                {
                    door2Opened = true;
                }
            }

            yield return null;
        } */

        while (!door1Opened || !door2Opened)
        {
            //Debug.Log($"door1: {door1Opened}, door2: {door2Opened}");
            if (!door1Opened)
            {
                float currentAngle1 = door1.localRotation.eulerAngles.z;
                //Debug.Log($"Angel1: {currentAngle1}");
                if (currentAngle1 > door1OpenAngle)
                {
                    currentAngle1 -= Time.deltaTime * doorOpenSpeed;
                    door1.localRotation = Quaternion.Euler(0f, 0f, currentAngle1);
                    if (currentAngle1 <= door1OpenAngle)
                    {
                        door1Opened = true;
                        //Debug.Log($"door1 open: {door1Opened}");
                    }
                }
            }

            if (!door2Opened)
            {
                float currentAngle2 = door2.localRotation.eulerAngles.z;
                //Debug.Log($"Angel2: {currentAngle2}");
                if (currentAngle2 > door2OpenAngle)
                {
                    currentAngle2 += Time.deltaTime * doorOpenSpeed;
                    door2.localRotation = Quaternion.Euler(0f, 0f, currentAngle2);
                    if (currentAngle2 <= door2OpenAngle)
                    {
                        door2Opened = true;
                        //Debug.Log($"door2 open: {door2Opened}");
                    }
                }
            }

            yield return null;
        }
    }
    void SwitchToSecondCamera()
    {
        mainCamera.gameObject.SetActive(false);
        secondCamera.gameObject.SetActive(true);
    }

}
