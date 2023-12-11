using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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
  
       
        FilterLevelOnePatterns(GameStateManager.Instance.equipedPatterns);

        var a = GetComponentsInChildren<EquipElement>();
        images = new Image[a.Length];
        int count = 0;
        foreach (var s in a)
        {
            images[count] = s.GetComponent<Image>();
            count++;
        }

        //����1 ��Ÿ�� ��
        foreach(var p in FilterLevelOnePatterns(GameStateManager.Instance.patterns))
        {
       
            GetNewPattern(p);
            GameStateManager.Instance.patterns.Remove(p);
        }

        LoadPattern();
    }

    public List<Pattern> FilterLevelOnePatterns(List<Pattern> allPatterns)
    {

        // 'level'�� 1�� 'Pattern' ��ü�鸸 ���͸��մϴ�.
        return allPatterns.Where(p => p.level == 1).ToList();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
