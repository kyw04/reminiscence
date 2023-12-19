using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AugmentNode : MonoBehaviour
{
    public Image[] images;
    public EquipElement[] equipElements;
    public List<Augment> equipedAugments;
    public Sprite defaultSprite;

    // Start is called before the first frame update
    public void LoadAugment()
    {
        
        var augments = GameStateManager.Instance.equipedAguments;
        if (augments == null) return;
        int count = 0;
        
        foreach (var a in augments)
        {
            // 'images' 배열의 길이를 체크하여 인덱스 초과를 방지합니다.
            if (count >= images.Length)
            {
                Debug.Log("증강체 초과 표시되지 않습니다.");
                break;
            }

            var image = images[count];
            if (a.sprite != null)
            {
                image.sprite = a.sprite;
                image.enabled = true; // 스프라이트가 있는 경우 이미지를 활성화합니다.
            }
            else
            {
                image.enabled = false; // 스프라이트가 없는 경우 이미지를 비활성화할 수 있습니다.
            }

            count++;
        }
    }
    public void GetNewAugment(Augment augment)
    {
        if (images.Length <= equipedAugments.Count)
        {
            Debug.Log("증강체 획득 초과");
        }

        if (augment.actionType == Augment.ActionType.Continuous)
        {
            var augmentActive = AugmentActive.instance;
            if (augmentActive)
            {
                augmentActive.ActivateEffect(augment);
            }
                
        }

        GameStateManager.Instance.equipedAguments.Add(augment);
        
        
        LoadAugment();
        
    }
    public void SaveAugments()
    {
        
    }
    void Start()
    {
        var a = GetComponentsInChildren<EquipElement>();
        images = new Image[a.Length];
        int count = 0;
        foreach(var s in a)
        {
            images[count] = s.GetComponent<Image>();
            count++;
        }
        LoadAugment();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
