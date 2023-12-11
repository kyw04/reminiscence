using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Itemdetail : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject ItemdetailObject;
    public Canvas canvas;

    private bool isHovering = false;
    private GameObject currentHoveredObject;
    private bool itemDetailActive = false;

    private void Start()
    {
        ItemdetailObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        currentHoveredObject = eventData.pointerEnter.gameObject;

        if (!itemDetailActive)
        {
            ItemdetailObject.SetActive(true);
            itemDetailActive = true;
        }

        // Disable collider of the object being hovered
        Collider collider = currentHoveredObject.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;

        // Re-enable collider of the object being exited
        if (currentHoveredObject != null)
        {
            Collider collider = currentHoveredObject.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = true;
            }
        }

        ItemdetailObject.SetActive(false);
        itemDetailActive = false;
    }
}
