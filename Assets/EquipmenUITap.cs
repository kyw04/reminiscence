using UnityEngine;

public class EquipmentUITap : MonoBehaviour
{
    // 싱글톤 인스턴스를 보관할 정적 필드
    public static EquipmentUITap Instance { get; private set; }

    void Awake()
    {
        // 이미 인스턴스가 존재하는 경우, 중복 생성을 방지하기 위해 현재 인스턴스를 파괴합니다.
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // 싱글톤 인스턴스를 설정합니다.
        Instance = this;

        // 씬 전환 시 파괴되지 않도록 설정합니다.
        DontDestroyOnLoad(gameObject);
    }

    // 나머지 코드...
}
