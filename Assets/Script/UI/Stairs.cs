using UnityEngine;

public class Stairs : MonoBehaviour
{
    public float stepHeight = 0.1f; // �� ����� ����

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // "Player" �±׸� ���� ��ü�� �浹�ϸ�
        {
            CharacterController characterController = other.GetComponent<CharacterController>();
            if (characterController != null)
            {
                // ĳ������ ��ġ�� ����� ���̸�ŭ �÷���
                characterController.Move(Vector3.up * stepHeight);
            }
        }
    }
}

