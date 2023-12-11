using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventoryUI : MonoBehaviour
{
    private ItemDatabase itemDB;
    private int _itemCnt;

    [SerializeField] private GameObject _itemIconBase;

    private Item detectChange = null;


    private void Start()
    {
        Init();
        detectChange = Item.memoryNewItem;
    }


    private void Update()
    {
        if (Item.memoryNewItem != detectChange)
        {
            Rearrnage();
            detectChange = Item.memoryNewItem;
        }
    }


    private void Init()
    {
        itemDB = GameObject.Find("ItemDatabase").GetComponent<ItemDatabase>();
        _itemCnt = itemDB._items.Count;

        for (int _invenCnt = 0; _invenCnt < _itemCnt; _invenCnt++)
        {
            Item item = itemDB._items[_invenCnt];
            GameObject _maden = Instantiate(_itemIconBase);

            Image _itemObj = _maden.transform.transform.Find("Obj").GetComponent<Image>();
            _itemObj.sprite = Item.SetItemImage(item);
            ///////
            if (item._itemPart != Item.ItemPart.ROBE) _itemObj.transform.rotation = Quaternion.Euler(0, 0, 33);
            //////

            Image _itemGradeFrame = _maden.transform.Find("Frame").GetComponent<Image>();
            _itemGradeFrame.sprite = Resources.Load<Sprite>("Sprites/itemFrame/frame" + item._itemGradeID);
            //Text _itemName = _maden.transform.Find("ItemName").GetComponent<Text>();
            //_itemName.text = item._itemName;

            _maden.transform.SetParent(this.gameObject.transform, false);
            _maden.gameObject.GetComponent<InvenItemBtn>().btnID = _invenCnt;
        }
    }


    public void Rearrnage()
    {
        Transform[] childList = this.GetComponentsInChildren<Transform>();
        for (int i = 1; i < childList.Length; i++)
        {
            if (childList[i] != transform) Destroy(childList[i].gameObject);
        } //이게맞나............. 이 무식한방법이...
        Init();
    }
}
