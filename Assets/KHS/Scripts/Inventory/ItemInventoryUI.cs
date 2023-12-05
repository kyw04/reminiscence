using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventoryUI : MonoBehaviour
{
    private ItemDatabase itemDB;
    private int _itemCnt;

    [SerializeField] private GameObject _itemIconBase;


    private void Start()
    {
        Init();
    }


    private void Init()
    {
        itemDB = GameObject.Find("ItemDatabase").GetComponent<ItemDatabase>();
        _itemCnt = itemDB._items.Count;

        for (int _invenCnt = 0; _invenCnt < _itemCnt; _invenCnt++)
        {
            GameObject _maden = Instantiate(_itemIconBase);
            Text _itemName = _maden.transform.Find("ItemName").GetComponent<Text>();
            _itemName.text = itemDB._items[_invenCnt]._itemName;

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
