using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipItemInfo : InventoryFunction
{
    [SerializeField] int _partID = 0;
    private int _itemID;


    private void Update()
    {
        if (TempEquipData.instance.PlayerEquip[_partID] != null && TempEquipData.instance.PlayerEquip[_partID]._onEquip)
            _itemID = itemDB._items.IndexOf(TempEquipData.instance.PlayerEquip[_partID]);
    }


    public void AppearInfoUI()
    {
        if (TempEquipData.instance.PlayerEquip[_partID] != null && TempEquipData.instance.PlayerEquip[_partID]._onEquip)
        {
            _targetIDList.Clear();
            _targetIDList.Add(_itemID);

            itemInfoUI.gameObject.SetActive(true);
            itemInfoUI.Set(_itemID);
            itemInfoUI.SetPos();
        }
    }
}
