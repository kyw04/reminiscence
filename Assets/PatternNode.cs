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
            // 'images' 배열의 길이를 체크하여 인덱스 초과를 방지합니다.
            if (images.Length <= count)
            {
                Debug.Log("증강체 초과 표시되지 않습니다.");
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
                // 여기에 'a.sprite'가 null일 때의 로직을 추가할 수 있습니다.
                // 예: image.enabled = false;
            }

            count++;
        }
    }
    public void GetNewPattern(Pattern pattern)
    {
        if (images.Length >= equipedAugments.Count)
        {
            Debug.Log("패턴 획득 초과");
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

        //레벨1 스타터 팩
        foreach(var p in FilterLevelOnePatterns(GameStateManager.Instance.patterns))
        {
       
            GetNewPattern(p);
            GameStateManager.Instance.patterns.Remove(p);
        }

        LoadPattern();
    }

    public List<Pattern> FilterLevelOnePatterns(List<Pattern> allPatterns)
    {

        // 'level'이 1인 'Pattern' 객체들만 필터링합니다.
        return allPatterns.Where(p => p.level == 1).ToList();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
