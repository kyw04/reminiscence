using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
/*
public class TempMakeItem : MonoBehaviour
{
    private ItemDatabase itemDB;

    private Item_Weapon _newWeapon;
    private Item_Armor _newArmor;

    private Item.itemGrade _grade;
    private Item.itemPart _part;

    private int[] _armorResMin = new int[4] { 0, 6, 11, 16 };
    private int[] _armorResMax = new int[4] { 5, 10, 15, 25 };

    private int _atk;
    private int[] _staffAtkMin = new int[4] { 1, 11, 21, 31 };
    private int[] _staffAtkMax = new int[4] { 10, 20, 30, 50 };
    private int[] _grimoireAtkMin = new int[4] { 0, 6, 11, 16 };
    private int[] _grimoireAtkMax = new int[4] { 5, 10, 15, 25 };

    private bool _hasMax = false;

    private string[] _name_grade = new string[4] {"일반","희귀한","영웅의","전설의"};
    private string[] _name_resType = new string[4] {"화염의","수","풍","지"};
    private string _name;


    void Start()
    {
        itemDB = GameObject.Find("ItemDatabase").GetComponent<ItemDatabase>();
    }


    public void MakeItem()
    {
        string name_grade, name_resType, name_part;

        int gradeID = Random.Range(0, 4);

        if (gradeID == 0)      _grade = Item.itemGrade.NORMAL;
        else if (gradeID == 1) _grade = Item.itemGrade.RARE;
        else if (gradeID == 2) _grade = Item.itemGrade.HEROIC;
        else                    _grade = Item.itemGrade.LEGENDARY;

        name_grade = _name_grade[gradeID];


        int partID = Random.Range(0, 3);

        if (partID == 0)      _part = Item.itemPart.STAFF;
        else if (partID == 1) _part = Item.itemPart.GRIMOIRE;
        else                  _part = Item.itemPart.ROBE;

        if (_part == Item.itemPart.ROBE)
        {
            name_part = "로브";

            List<int> resList = new List<int>();
            for (int i = 0; i < _name_resType.Length; i++)
            {
                resList.Add(Random.Range(_armorResMin[gradeID], _armorResMax[gradeID] + 1));
            }

            //스스로가 멍청해서 너무 힘들다
            int curMax = resList.Max();
            List<int> curMaxIndexList = new List<int>();

            for (int i = 0; i < resList.Count; i++)
            {
                if (i != resList.IndexOf(resList.Max()))
                {
                    if (resList[i] == curMax) curMaxIndexList.Add(i);
                }
                else curMaxIndexList.Add(i);
            }

            if (curMaxIndexList.Count > 1)
            {
                int chooseMax = Random.Range(0, curMaxIndexList.Count);
                //내성 최댓값이 등급 최댓값의 근삿값일 시> 중복항목을 빼고 / 아닐시> 선택된 걸 올리고
                if (curMax <= _armorResMax[gradeID] /2)
                {
                    for (int i = 0; i < curMaxIndexList.Count; i++)
                    {
                        if (i != chooseMax) resList[curMaxIndexList[i]]--;
                    }
                }
                else   resList[curMaxIndexList[chooseMax]]++; //구조 지랄났네ㅆ발...ㅜㅜ
            }
            curMax = resList.Max();

            name_resType = _name_resType[resList.IndexOf(curMax)];

            _hasMax = (curMax == _armorResMax[gradeID]);

            _name = name_grade + " " + name_resType + " " + name_part;
            if (/*curMax == _armorResMax[gradeID]_hasMax) _name += "+";

            _newArmor = new Item_Armor(_name, 0, _part, _grade, resList[0], resList[1], resList[2], resList[3]);
            itemDB._armors.Add(_newArmor);
        }
        else
        {
            switch (_part)
            {
                case (Item.itemPart.STAFF):
                    name_part = "스태프";

                    _atk = Random.Range(_staffAtkMin[gradeID], _staffAtkMax[gradeID] + 1);
                    _hasMax = (_atk == _staffAtkMax[gradeID]);
                    break;
                case (Item.itemPart.GRIMOIRE):
                    name_part = "마도서";
                    _atk = Random.Range(_grimoireAtkMin[gradeID], _grimoireAtkMax[gradeID] + 1);
                    _hasMax = (_atk == _grimoireAtkMax[gradeID]); 
                    break;
                default: name_part = "ㅇㅅㅇ"; break;
            }

            _name = name_grade + " " + name_part;
            if (_hasMax) _name += "+";

            _newWeapon = new Item_Weapon(_name, 0, _part, _grade, _atk);
            itemDB._weapons.Add(_newWeapon);
        }
    }
} */