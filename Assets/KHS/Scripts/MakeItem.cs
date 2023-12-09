using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class MakeItem : MonoBehaviour
{
    private ItemDatabase itemDB;
    private string _nameByGrade, _nameByResType, _nameByPart;

    public UnityEvent CompleteMaking;


    private void Start()
    {
        itemDB = GameObject.Find("ItemDatabase").GetComponent<ItemDatabase>();
    }


    public void MakeTrash() //임시임시요
    {
        Make(Random.Range(0, 3), Random.Range(0, 4));
    }


    public Item Make(int partID, int gradeID)
    {
        Item.ItemPart part = Item.PartIDToPart(partID);

        Item maden;
        bool hasMaxOption = false;

        _nameByGrade = Item.NameByGrades[gradeID];

        if (part == Item.ItemPart.GRIMOIRE)
        {
            _nameByPart = Item.NameByParts[2];

            List<int> reses = new List<int>();
            for (int i = 0; i < Item.NameByResTypes.Length; i++)
                reses.Add(Random.Range(Item.GrimoireResMin[gradeID], Item.GrimoireResMax[gradeID] + 1));

            int curMax = reses.Max();
            List<int> curMaxIndexList = new List<int>();

            for (int i = 0; i < reses.Count; i++)
            {
                if (reses[i] == curMax) curMaxIndexList.Add(i);
            }

            if (curMaxIndexList.Count > 1)
            {
                int chooseMax = Random.Range(0, curMaxIndexList.Count);
                //내성 최댓값이 등급 최댓값의 근삿값일 시> 중복항목을 빼고 / 아닐시> 선택된 걸 올리고
                if (curMax >= Item.GrimoireResMax[gradeID] / 2f)
                {
                    for (int i = 0; i < curMaxIndexList.Count; i++)
                    {
                        if (i != chooseMax) reses[curMaxIndexList[i]]--;
                    }
                }
                else reses[curMaxIndexList[chooseMax]]++;
            }
            curMax = reses.Max();

            _nameByResType = Item.NameByResTypes[reses.IndexOf(curMax)];
            hasMaxOption = (curMax == Item.GrimoireResMax[gradeID]);

            //이름 레벨 경험치 부위 등급 장착여부 공방 저항x4
            maden = new Item(Naming(part, hasMaxOption), 1, 0, part, gradeID, false, 0, reses[0], reses[1], reses[2], reses[3]);
        }
        else
        {
            int stat;
            int[] statMin, statMax = new int[4];

            switch (part)
            {
                case (Item.ItemPart.STAFF):
                    _nameByPart = Item.NameByParts[0];

                    statMin = Item.StaffAtkMin;
                    statMax = Item.StaffAtkMax;
                    break;

                case (Item.ItemPart.ROBE):
                    _nameByPart = Item.NameByParts[1];

                    statMin = Item.RobeDefMin;
                    statMax = Item.RobeDefMax;
                    break;
                default: statMin = null; statMax = null; break;
            }

            stat = Random.Range(statMin[gradeID], statMax[gradeID] + 1);
            hasMaxOption = (stat == statMax[gradeID]);

            maden = new Item(Naming(part, hasMaxOption), 1, 0, part, gradeID, false, stat);
        }
        /*
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

            maden = new Item(Item.ItemType.ARMOR, Naming(part, hasMaxOption), 1, part, gradeID, 0, def, reses[0], reses[1], reses[2], reses[3]);
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

            maden = new Item(Item.ItemType.WEAPON, Naming(part, hasMaxOption), 1, part, gradeID, 0, atk);
        }*/

        
        itemDB._items.Add(maden);
        Item.memoryNewItem = maden;

        CompleteMaking.Invoke();

        return maden;
    }


    private string Naming(Item.ItemPart part, bool hasMaxOption)
    {
        string name = "";

        name += _nameByGrade;
        if (part == Item.ItemPart.GRIMOIRE) name += (" " + _nameByResType);
        name += (" " + _nameByPart);
        if (hasMaxOption) name += "+";

        return name;
    }
}