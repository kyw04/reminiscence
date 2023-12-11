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
        var aguments = GameStateManager.Instance.equipedAguments;

        
        int count = 0;
        foreach (var a in aguments)
        {
            
            if (images.Length < count - 1)
            {
                Debug.Log("증강체 초과 표시되지 않습니다.");
                break;
            }
            var image = images[count];
            if (a.sprite == null)
            {

            }
            else
            {
                image.sprite = a.sprite;
            }
            image.enabled = true;
            count++;
        }
    }
    public void GetNewAugment(Augment augment)
    {
        if (images.Length >= equipedAugments.Count)
        {
            Debug.Log("증강체 획득 초과");
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
}
