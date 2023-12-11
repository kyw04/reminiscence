using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDetail : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform itemDetailTransform;
    public Vector3 detailObjectOffset;
    public bool staticPosition;

    private void Start()
    {
        itemDetailTransform.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!itemDetailTransform.gameObject.activeSelf)
        {
            //Debug.Log(detailObjectOffset);
            if (staticPosition)
                itemDetailTransform.position = detailObjectOffset;
            else
                itemDetailTransform.position = transform.position + detailObjectOffset;

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
