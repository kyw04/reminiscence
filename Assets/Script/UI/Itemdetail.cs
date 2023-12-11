using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Itemdetail : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject ItemdetailObject;
    public Vector2 offset;

    private void Start()
    {
        ItemdetailObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!ItemdetailObject.activeSelf)
        {
            ItemdetailObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerCurrentRaycast.gameObject);
        if (eventData.pointerCurrentRaycast.gameObject == ItemdetailObject)
        {
            Debug.Log("return");
            return;
        }

        ItemdetailObject.SetActive(false);
    }
}
