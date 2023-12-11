using UnityEngine;

public class EquipmentUITap : MonoBehaviour
{
    // �̱��� �ν��Ͻ��� ������ ���� �ʵ�
    public static EquipmentUITap Instance { get; private set; }

    void Awake()
    {
        // �̹� �ν��Ͻ��� �����ϴ� ���, �ߺ� ������ �����ϱ� ���� ���� �ν��Ͻ��� �ı��մϴ�.
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // �̱��� �ν��Ͻ��� �����մϴ�.
        Instance = this;

        // �� ��ȯ �� �ı����� �ʵ��� �����մϴ�.
        DontDestroyOnLoad(gameObject);
    }

    // ������ �ڵ�...
}
