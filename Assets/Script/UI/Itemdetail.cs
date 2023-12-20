using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDetail : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform itemDetailTransform;
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
