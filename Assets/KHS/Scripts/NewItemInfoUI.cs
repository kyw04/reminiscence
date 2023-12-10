using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NewItemInfoUI : MonoBehaviour //InventoryFunction, IDragHandler
{
    [SerializeField] private Text _itemName, _itemAtkOrDef;
    [SerializeField] private Text[] _itemReses;
    [SerializeField] private Image _itemImage;

    [SerializeField] private GameObject _itemAtkOrDefObj;
    [SerializeField] private GameObject _itemResesObj;


    public void Set()
    {
        Item item = Item.memoryNewItem;

        _itemName.text = item._itemName;
        _itemImage.sprite = Item.SetItemImage(item);
        ///////
        if (item._itemPart != Item.ItemPart.ROBE) _itemImage.transform.rotation = Quaternion.Euler(0, 0, 33);
        else _itemImage.transform.rotation = Quaternion.Euler(0, 0, 0);
        //////
        _itemResesObj.SetActive(item._itemPart == Item.ItemPart.GRIMOIRE);
        _itemAtkOrDefObj.SetActive(item._itemPart != Item.ItemPart.GRIMOIRE);

        if (item._itemPart == Item.ItemPart.GRIMOIRE)
        {
            _itemReses[0].text = "ȭ�Ӽ� ����: " + item._itemFireResis.ToString();
            _itemReses[1].text = "���Ӽ� ����: " + item._itemWaterResis.ToString();
            _itemReses[2].text = "ǳ�Ӽ� ����: " + item._itemAirResis.ToString();
            _itemReses[3].text = "���Ӽ� ����: " + item._itemEarthResis.ToString();
        }
        else
        {
            if (item._itemPart == Item.ItemPart.STAFF)
                _itemAtkOrDef.text = "���ݷ�: " + item._itemAtkOrDef.ToString();
            else if (item._itemPart == Item.ItemPart.ROBE)
                _itemAtkOrDef.text = "����: " + item._itemAtkOrDef.ToString();
        }
    }


    public void Disappear()
    {
        this.gameObject.SetActive(false);
    }
}