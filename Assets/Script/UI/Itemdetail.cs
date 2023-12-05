using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Itemdetail : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject ItemdetailObject;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ItemdetailObject.SetActive(true);
        Debug.Log("Open");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemdetailObject.SetActive(false);
    }
}
