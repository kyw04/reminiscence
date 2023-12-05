using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ItemInfoUI : InventoryFunction, IDragHandler//MonoBehaviour
{
    public int ID;

    [SerializeField] private Text _itemName, _itemLevel, _itemExp, _itemAtkOrDef;
    [SerializeField] private GameObject _itemReses;
    [SerializeField] private Text _itemFireR, _itemWaterR, _itemEarthR, _itemAirR;

    [SerializeField] private RectTransform _canvasRect;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Vector2 _vec;


    protected override void Awake()
    {
        base.Awake();

        Text[] texts = new Text[] { _itemName, _itemLevel, _itemExp, _itemAtkOrDef, _itemFireR, _itemWaterR, _itemEarthR, _itemAirR };
        for (int i = 0; i < texts.Length; i++)  texts[i].GetComponent<Text>();
    }


    public void Set()
    {
        _itemName.text = itemDB._items[ID]._itemName;
        _itemLevel.text = itemDB._items[ID]._itemLevel.ToString() + " Lv";
        _itemExp.text = itemDB._items[ID]._itemExp.ToString() + ", 다음 레벨까지 몇";

        _itemReses.SetActive(itemDB._items[ID]._itemType == Item.ItemType.ARMOR);

        if (itemDB._items[ID]._itemType == Item.ItemType.WEAPON)
        {
            _itemAtkOrDef.text = "공격력: " + itemDB._items[ID]._itemAtkOrDef.ToString();
        }
        else
        {
            _itemAtkOrDef.text = "방어력: " + itemDB._items[ID]._itemAtkOrDef.ToString();

            _itemFireR.text = "화속성 저항: " + itemDB._items[ID]._itemFireResis.ToString();
            _itemWaterR.text = "수속성 저항: " + itemDB._items[ID]._itemWaterResis.ToString();
            _itemAirR.text = "풍속성 저항: " + itemDB._items[ID]._itemAirResis.ToString();
            _itemEarthR.text = "지속성 저항: " + itemDB._items[ID]._itemEarthResis.ToString();
        }
        Move();
    }


    private void Move()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, Input.mousePosition, Camera.main, out Vector2 anchoredPos);
        this.gameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPos + _vec;
        
        if (this.gameObject.GetComponent<RectTransform>().anchoredPosition.y <= -320)
            this.gameObject.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, -this.gameObject.GetComponent<RectTransform>().anchoredPosition.y - 320);
    }


    public void Disappear()
    {
        if (_mode == FunctionMode.NULL) _targetIDList.Clear();
        this.gameObject.SetActive(false);
    }


    public void OnDrag(PointerEventData eventData)
    {
        //Vector2 vec = new Vector2(0, _vec.y);
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, eventData.position, Camera.main, out Vector2 anchoredPos);
        //this.gameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPos + vec;
        this.gameObject.GetComponent<RectTransform>().anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
}
