using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEquipData : MonoBehaviour
{
    static public TempEquipData instance = null;


    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    public List<Item> PlayerEquip = new List<Item>() { null, null, null };

    private void Start()
    {
        ItemDatabase itemDB = GameObject.Find("ItemDatabase").GetComponent<ItemDatabase>();
        int check = 0;

        for (int i = 0; i < itemDB._items.Count; i++)
        {
            Item item = itemDB._items[i];
            if (item._onEquip)
            {
                switch (item._itemPart)
                {
                    case (Item.ItemPart.STAFF):
                        PlayerEquip[0] = item;
                        break;
                    case (Item.ItemPart.ROBE):
                        PlayerEquip[1] = item;
                        break;
                    case (Item.ItemPart.GRIMOIRE):
                        PlayerEquip[2] = item;
                        break;
                    default: break;
                }
                check++;
            }
            if (check == 3) break;
        }
    }

    public EquipmentStat PlayerEquipmentStat;

    public void SetEquipmentStat()
    {
        int atk = 0, def = 0, fireRes = 0, waterRes = 0, airRes = 0, earthRes = 0;

        if (PlayerEquip[0] != null && PlayerEquip[0]._onEquip)
            atk = PlayerEquip[0]._itemAtkOrDef;
        if (PlayerEquip[1] != null && PlayerEquip[1]._onEquip)
            def = PlayerEquip[1]._itemAtkOrDef;
        if (PlayerEquip[2] != null && PlayerEquip[2]._onEquip)
        {
            fireRes = PlayerEquip[2]._itemFireResis;
            waterRes = PlayerEquip[2]._itemWaterResis;
            airRes = PlayerEquip[2]._itemAirResis;
            earthRes = PlayerEquip[2]._itemEarthResis;
        }

        PlayerEquipmentStat = new EquipmentStat(atk, def, fireRes, waterRes, airRes, earthRes);
    }
}
