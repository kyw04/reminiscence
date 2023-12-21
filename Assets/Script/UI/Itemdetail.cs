using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Itemdetail : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform itemDetailTransform;
    public TextMeshProUGUI text;
    public Image itemDetailImage;
    public Image targetImage;
    public Vector3 detailObjectOffset;
    public bool staticPosition;

    private void Start()
    {
        itemDetailTransform.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!targetImage.enabled)
            return;

        if (!itemDetailTransform.gameObject.activeSelf)
        {
            //Debug.Log(detailObjectOffset);    
            if (staticPosition)
                itemDetailTransform.position = detailObjectOffset;
            else
                itemDetailTransform.position = transform.position + detailObjectOffset;

            if (itemDetailImage != null && targetImage != null)
                itemDetailImage.sprite = targetImage.sprite;

            if (text != null)
            {
                Image[] images = GetComponentInParent<AugmentNode>().images;
                int index = -1;
                for (int i = 0; i < images.Length; i++)
                {
                    if (images[i] == targetImage)
                    {
                        index = i;
                        break;
                    }
                }
                Debug.Log(index);
                Debug.Log(images.Length);
                if (index != -1)
                    text.text = GameStateManager.Instance.equipedAguments[index].description;
            }

            itemDetailTransform.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log(eventData.pointerCurrentRaycast.gameObject);
        if (eventData.pointerCurrentRaycast.gameObject == itemDetailTransform.gameObject)
            return;

        itemDetailTransform.gameObject.SetActive(false);
    }
}
