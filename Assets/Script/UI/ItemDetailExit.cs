using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDetailExit : MonoBehaviour, IPointerExitHandler
{
    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.SetActive(false);
    }
}