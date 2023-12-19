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
            // 'images' �迭�� ���̸� üũ�Ͽ� �ε��� �ʰ��� �����մϴ�.
            if (count >= images.Length)
            {
                Debug.Log("����ü �ʰ� ǥ�õ��� �ʽ��ϴ�.");
                break;
            }

            var image = images[count];
            if (a.sprite != null)
            {
                image.sprite = a.sprite;
                image.enabled = true; // ��������Ʈ�� �ִ� ��� �̹����� Ȱ��ȭ�մϴ�.
            }
            else
            {
                image.enabled = false; // ��������Ʈ�� ���� ��� �̹����� ��Ȱ��ȭ�� �� �ֽ��ϴ�.
            }

            count++;
        }
    }
    public void GetNewAugment(Augment augment)
    {
        if (images.Length <= equipedAugments.Count)
        {
            Debug.Log("����ü ȹ�� �ʰ�");
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
