using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class MakeItem : MonoBehaviour
{
    private ItemDatabase itemDB;

    //private Item_Weapon _newWeapon;
    //private Item_Armor _newArmor;

    /*private int[] _staffAtkMin = new int[4] { 1, 11, 21, 31 };
    private int[] _staffAtkMax = new int[4] { 10, 20, 30, 50 };
    private int[] _grimoireAtkMin = new int[4] { 0, 6, 11, 16 };
    private int[] _grimoireAtkMax = new int[4] { 5, 10, 15, 25 };

    private int[] _armorDefMin = new int[4] { 1, 6, 11, 16 };
    private int[] _armorDefMax = new int[4] { 5, 10, 15, 25 };

    private int[] _armorResMin = new int[4] { 0, 6, 11, 16 };
    private int[] _armorResMax = new int[4] { 5, 10, 15, 25 };
    
    private string[] _name_grades = new string[4] { "일반", "희귀", "영웅", "전설" };
    private string[] _name_resTypes = new string[4] { "화염의", "파도의", "폭풍의", "대지의" };
    private string[] _name_parts = new string[3] { "스태프", "마도서", "로브" };*/

    private string _name_grade, _name_resType, _name_part;
    public UnityEvent CompleteMaking;


    private void Start()
    {
        itemDB = GameObject.Find("ItemDatabase").GetComponent<ItemDatabase>();
    }


    public void MakeTrash() //임시임시요
    {
        Item.ItemPart part;
        if (Random.Range(0, 3) == 0)      part = Item.ItemPart.STAFF;
        else if (Random.Range(0, 3) == 1) part = Item.ItemPart.GRIMOIRE;
        else                              part = Item.ItemPart.ROBE;
        Make(part, Random.Range(0, 4));
    }

    /*
    public void ItemOptionChange(Item.ItemPart part, int gradeID)
    {
        Make(part, gradeID);
    }

    
    public void ItemSynthesis(int gradeID)
    {
        Item.ItemPart part;
        if (Random.Range(0, 3) == 0)      part = Item.ItemPart.STAFF;
        else if (Random.Range(0, 3) == 1) part = Item.ItemPart.GRIMOIRE;
        else                              part = Item.ItemPart.ROBE;

        Make(part, gradeID + 1);
    }*/


    public void Make(Item.ItemPart part, int gradeID)
    {
        Item maden;
        bool hasMax = false;

        _name_grade = Item._name_grades[gradeID];

        if (part == Item.ItemPart.ROBE)
        {
            _name_part = Item._name_parts[2];

            int def;
            def = Random.Range(Item._armorDefMin[gradeID], Item._armorDefMax[gradeID] + 1);

            List<int> reses = new List<int>();
            for (int i = 0; i < Item._name_resTypes.Length; i++)
            {
                reses.Add(Random.Range(Item._armorResMin[gradeID], Item._armorResMax[gradeID] + 1));
            }

            int curMax = reses.Max();
            List<int> curMaxIndexList = new List<int>();

            for (int i = 0; i < reses.Count; i++)
            {
                if (i != reses.IndexOf(reses.Max()))
                {
                    if (reses[i] == curMax) curMaxIndexList.Add(i);
                }
                else curMaxIndexList.Add(i);
            }

            if (curMaxIndexList.Count > 1)
            {
                int chooseMax = Random.Range(0, curMaxIndexList.Count);
                //내성 최댓값이 등급 최댓값의 근삿값일 시> 중복항목을 빼고 / 아닐시> 선택된 걸 올리고
                if (curMax <= Item._armorResMax[gradeID] / 2)
                {
                    for (int i = 0; i < curMaxIndexList.Count; i++)
                    {
                        if (i != chooseMax) reses[curMaxIndexList[i]]--;
                    }
                }
                else reses[curMaxIndexList[chooseMax]]++; //구조 지랄났네...ㅜㅜ
            }
            curMax = reses.Max();

            _name_resType = Item._name_resTypes[reses.IndexOf(curMax)];
            hasMax = (curMax == Item._armorResMax[gradeID]);

            maden = new Item(Item.ItemType.ARMOR, Naming(part, hasMax), 1, part, gradeID, 0, def, reses[0], reses[1], reses[2], reses[3]);
            //itemDB._items.Add(new Item(Item.ItemType.ARMOR, Naming(part, hasMax), 0, part, gradeID, 0, def, reses[0], reses[1], reses[2], reses[3]));
        }
        else
        {
            int atk;
            int[] atkMin, atkMax = new int[4];

            switch (part)
            {
                case (Item.ItemPart.STAFF):
                    _name_part = Item._name_parts[0];

                    atkMin = Item._staffAtkMin;
                    atkMax = Item._staffAtkMax;
                    //_atk = Random.Range(_staffAtkMin[gradeID], _staffAtkMax[gradeID] + 1);
                    //_hasMax = (_atk == _staffAtkMax[gradeID]);
                    break;

                case (Item.ItemPart.GRIMOIRE):
                    _name_part = Item._name_parts[1];

                    atkMin = Item._grimoireAtkMin;
                    atkMax = Item._grimoireAtkMax;
                    //_atk = Random.Range(_grimoireAtkMin[gradeID], _grimoireAtkMax[gradeID] + 1);
                    //_hasMax = (_atk == _grimoireAtkMax[gradeID]);
                    break;
                default: atkMin = null; atkMax = null; break;
            }
            atk = Random.Range(atkMin[gradeID], atkMax[gradeID] + 1);
            hasMax = (atk == atkMax[gradeID]);

            maden = new Item(Item.ItemType.WEAPON, Naming(part, hasMax), 1, part, gradeID, 0, atk);
        }
        itemDB._items.Add(maden);
        Item.memoryNewItem = maden;

        CompleteMaking.Invoke();
    }


    private string Naming(Item.ItemPart part, bool hasMax)
    {
        string name = "";

        name += _name_grade;
        if (part == Item.ItemPart.ROBE && hasMax) name += (" " + _name_resType);
        name += (" " + _name_part);
        if (hasMax) name += "+";

        return name;
    }
}