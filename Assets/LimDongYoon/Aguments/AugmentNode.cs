using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class AugmentNode : MonoBehaviour
{
    public Image[] images;
    public List<Augment> equipedAugments;

    // Start is called before the first frame update

    public void GetNewAugment(Augment augment)
    {
        if (images.Length >= equipedAugments.Count)
        {
            Debug.Log("����ü ȹ�� �ʰ�");
        }
        equipedAugments.Add(augment);
        images[equipedAugments.Count - 1].sprite = augment.sprite;
    }
    public void SaveAugments()
    {
        
    }
    void Start()
    {
        images = GetComponentsInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
