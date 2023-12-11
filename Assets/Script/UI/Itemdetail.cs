using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemDetail : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform itemDetailTransform;
    public Image imageBig;
    public Image imageSmall;
    public Vector3 detailObjectOffset;
    public bool staticPosition;

    private void Start()
    {
        itemDetailTransform.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
        if (!itemDetailTransform.gameObject.activeSelf)
        {
            //Debug.Log(detailObjectOffset);
            if (staticPosition)
                itemDetailTransform.position = detailObjectOffset;
            else
                itemDetailTransform.position = transform.position + detailObjectOffset;

            if (imageBig != null && imageSmall != null)
            {
                imageBig.sprite = imageSmall.sprite;
            }

            itemDetailTransform.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log(eventData.pointerCurrentRaycast.gameObject);
        if (eventData.pointerCurrentRaycast.gameObject == itemDetailTransform.gameObject)
        {
            Debug.Log("return");
            return;
        }

        itemDetailTransform.gameObject.SetActive(false);
    }
}
