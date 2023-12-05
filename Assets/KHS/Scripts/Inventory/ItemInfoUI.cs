using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ItemInfoUI : InventoryFunction, IDragHandler//MonoBehaviour
{
    public int _infoID;

    [SerializeField] private Text _itemName, _itemLevel, _requiredExp, _itemAtkOrDef;
    [SerializeField] private Text[] _itemReses;
    [SerializeField] private Slider _itemExp;

    [SerializeField] private GameObject _itemAtkOrDefObj;
    [SerializeField] private GameObject _itemResesObj;

    [SerializeField] private RectTransform _canvasRect;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Vector2 _vec;


    protected override void Awake()
    {
        base.Awake();
    }


    private void Update()
    {
        if (_targetIDList.Count == 0 || _targetIDList[0] != _infoID) this.gameObject.SetActive(false);
    }


    public void Set(int ID)
    {
        _infoID = ID;
        Item item = itemDB._items[_infoID];

        _itemName.text = item._itemName;
        _itemLevel.text = item._itemLevel.ToString() + " Lv";
        _itemExp.value = item._itemExp / (float)Item.RequiredExp(item);
        _requiredExp.text = "다음 레벨까지 " +
                            (Item.RequiredExp(item) - item._itemExp).ToString();

        _itemResesObj.SetActive(item._itemPart == Item.ItemPart.GRIMOIRE);
        _itemAtkOrDefObj.SetActive(item._itemPart != Item.ItemPart.GRIMOIRE);

        if (item._itemPart == Item.ItemPart.GRIMOIRE)
        {
            _itemReses[0].text = "화속성 저항: " + item._itemFireResis.ToString();
            _itemReses[1].text = "수속성 저항: " + item._itemWaterResis.ToString();
            _itemReses[2].text = "풍속성 저항: " + item._itemAirResis.ToString();
            _itemReses[3].text = "지속성 저항: " + item._itemEarthResis.ToString();
        }
        else
        {
            if (item._itemPart == Item.ItemPart.STAFF)
                _itemAtkOrDef.text = "공격력: " + item._itemAtkOrDef.ToString();
            else if (item._itemPart == Item.ItemPart.ROBE)
                _itemAtkOrDef.text = "방어력: " + item._itemAtkOrDef.ToString();
        }

        //SetPos();
    }


    public void SetPos()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, Input.mousePosition, Camera.main, out Vector2 anchoredPos);
        this.gameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPos + _vec;

        if (this.gameObject.GetComponent<RectTransform>().anchoredPosition.y <= -250)
            this.gameObject.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, -this.gameObject.GetComponent<RectTransform>().anchoredPosition.y - 250);
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