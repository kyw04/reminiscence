using UnityEngine;

public class Stairs : MonoBehaviour
{
    public float stepHeight = 0.1f; // 각 계단의 높이

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // "Player" 태그를 가진 객체와 충돌하면
        {
            CharacterController characterController = other.GetComponent<CharacterController>();
            if (characterController != null)
            {
                // 캐릭터의 위치를 계단의 높이만큼 올려줌
                characterController.Move(Vector3.up * stepHeight);
            }
        }
    }
}

