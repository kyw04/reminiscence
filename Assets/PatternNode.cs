using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternNode : MonoBehaviour
{
    public Image[] images;
    public EquipElement[] equipElements;
    public List<Augment> equipedAugments;
    public Sprite defaultSprite;

    // Start is called before the first frame update
    public void LoadPattern()
    {
        var patterns = GameStateManager.Instance.equipedPatterns;


        int count = 0;
        foreach (var a in patterns)
        {
            // 'images' �迭�� ���̸� üũ�Ͽ� �ε��� �ʰ��� �����մϴ�.
            if (images.Length <= count)
            {
                Debug.Log("����ü �ʰ� ǥ�õ��� �ʽ��ϴ�.");
                break;
            }

            var image = images[count];
            if (a.sprite != null)
            {
                image.sprite = a.sprite;
                image.enabled = true;
            }
            else
            {
                // ���⿡ 'a.sprite'�� null�� ���� ������ �߰��� �� �ֽ��ϴ�.
                // ��: image.enabled = false;
            }

            count++;
        }
    }
    public void GetNewPattern(Pattern pattern)
    {
        if (images.Length >= equipedAugments.Count)
        {
            Debug.Log("���� ȹ�� �ʰ�");
        }
        GameStateManager.Instance.equipedPatterns.Add(pattern);
        LoadPattern();

    }
    public void SaveAugments()
    {

    }
    void Start()
    {
        var a = GetComponentsInChildren<EquipElement>();
        images = new Image[a.Length];
        int count = 0;
        foreach (var s in a)
        {
            images[count] = s.GetComponent<Image>();
            count++;
        }
        LoadPattern();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
