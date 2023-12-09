using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class InventoryFunction : MonoBehaviour
{
    protected ItemDatabase itemDB;
    protected MakeItem makeItem;

    protected ItemInfoUI itemInfoUI;
    public int _infoID;
    //protected EquipItemInfo eqiupItemInfoUI;

    public enum FunctionMode { ENHANCEMENT, OPTIONCHANGE, SYNTHESIS, EQUIP, NULL };
    static protected FunctionMode _mode = FunctionMode.NULL;
    private GameObject _fnUI;

    static public List<int> _targetIDList = new List<int>();
    static protected Item _beneficiary;
    //static protected List<Item> _synIng;//redient; 이따 아이디리스트지우기
    static protected int _sumExp = 0;
    static protected int _higherGradeID = 0;

    public UnityEvent Finished;
    static protected bool _isFinished = false;


    protected virtual void Awake()
    {
        itemDB = GameObject.Find("ItemDatabase").GetComponent<ItemDatabase>();
        //makeItem = GameObject.Find("TempMakingbtn").GetComponent<MakeItem>();
        makeItem = GameObject.Find("ItemDatabase").GetComponent<MakeItem>();
        itemInfoUI = GameObject.Find("Canvas").transform.Find("ItemInfoUI").GetComponent<ItemInfoUI>();

        _fnUI = GameObject.Find("InvenFunctionUI");
    }

    private void Update()
    {
        if (_mode == FunctionMode.ENHANCEMENT && _targetIDList.Count > 1)
        {
            int sumExp = 0;
            for (int i = 1; i < _targetIDList.Count; i++)
            {
                int ingreGradeId = itemDB._items[_targetIDList[i]]._itemGradeID;
                sumExp += Item.ExpRiseByGrade[ingreGradeId];
            }
            _sumExp = sumExp;
        }
        else _sumExp = 0;

        if (_mode == FunctionMode.SYNTHESIS && _targetIDList.Count > 0)
        {
            _higherGradeID = itemDB._items[_targetIDList[0]]._itemGradeID;
            if (_targetIDList.Count > 1)
            {
                if (_higherGradeID < itemDB._items[_targetIDList[1]]._itemGradeID)
                    _higherGradeID = itemDB._items[_targetIDList[1]]._itemGradeID;
            }
        }
    }


    public void OnMode(string input)//FunctionMode mode)
    {
        FunctionMode.TryParse(input, out FunctionMode mode);
        _mode = mode;

        for (int i = 0; i < _fnUI.transform.childCount - 1; i++)
        {
            GameObject UI = _fnUI.transform.GetChild(i).gameObject;

            if (UI.name.ToUpper() == input) UI.SetActive(true);
            else UI.SetActive(false);
        }

        if (_mode == FunctionMode.EQUIP)
        {
            _targetIDList.Clear();
        }
        else
        {
            if (_targetIDList.Count == 1)
            {
                int btnID = _targetIDList[0];
                if (CanTargeted(btnID, true))
                {
                    _beneficiary = itemDB._items[btnID];
                }
                else _targetIDList.Clear();
            }
        }
    }


    public void Enhancement()
    {
        if (_targetIDList.Count > 1)
        {
            _beneficiary._itemExp += _sumExp;
            _beneficiary = Item.ItemLevelUP(_beneficiary);//, _sumExp);
            Item.memoryNewItem = _beneficiary;

            RemoveIngredients(true);
            _sumExp = 0;

            Finish();

            if (_beneficiary._itemLevel < 10) _targetIDList.Add(itemDB._items.IndexOf(Item.memoryNewItem));
        }
    }


    public void OptionChange()
    {
        if (_targetIDList.Count == 2)
        {
            Item.ItemPart part = _beneficiary._itemPart;
            int gradeID = _beneficiary._itemGradeID;
            int level = _beneficiary._itemLevel;
            int exp = _beneficiary._itemExp;
            bool equip = false;

            if (_beneficiary._onEquip) equip = true;
            RemoveIngredients(false);
            makeItem.Make(Item.PartToPartID(part), gradeID);

            Item.memoryNewItem._itemLevel = level;
            Item.memoryNewItem._itemExp = exp;
            Item.memoryNewItem._onEquip = equip;

            for (int i = 1; i < level; i++)
                Item.memoryNewItem = Item.UpgradeByLevel(Item.memoryNewItem);

            _targetIDList.Add(itemDB._items.IndexOf(Item.memoryNewItem));
            Finish();
        }
    }


    public void Synthesis()
    {
        if (_targetIDList.Count == 2)
        {
            /*
            if (itemDB._items[_targetIDList[0]]._itemGradeID < itemDB._items[_targetIDList[1]]._itemGradeID)
                _higherGradeID = itemDB._items[_targetIDList[1]]._itemGradeID;
            else _higherGradeID = itemDB._items[_targetIDList[0]]._itemGradeID;*/

            RemoveIngredients(false);
            /*
            Item.ItemPart part;
            if (Random.Range(0, 3) == 0) part = Item.ItemPart.STAFF;
            else if (Random.Range(0, 3) == 1) part = Item.ItemPart.GRIMOIRE;
            else part = Item.ItemPart.ROBE; */

            makeItem.Make(Random.Range(0, 3), _higherGradeID + 1);//, true);
            Finish();
        }
    }


    private void RemoveIngredients(bool _remain0)
    {
        if (_remain0) _targetIDList.RemoveAt(0);
        _targetIDList = _targetIDList.OrderBy(i => i).ToList();

        for (int i = _targetIDList.Count -1; i >= 0; i--)
        {
            Item ingre = itemDB._items[_targetIDList[i]];
            //Debug.Log(i + " " + ingre._itemName);
            itemDB._items.Remove(ingre);
        }

        _targetIDList.Clear();
        //Finished.Invoke();
    }


    private void Finish()
    {
        if (itemInfoUI.enabled)
        {
            itemInfoUI.Set(itemDB._items.IndexOf(Item.memoryNewItem));
        }
        Finished.Invoke();

        //if (_targetIDList.Count == 1) _beneficiary = Item.memoryNewItem;
    }


    public bool CanTargeted(int btnID, bool isBenef)//, FunctionMode mode)
    {
        Item target = itemDB._items[btnID];
        switch (_mode)
        {
            case (FunctionMode.ENHANCEMENT):
                if (isBenef)
                {
                    if (target._itemLevel < 10) return true;
                    else return false;
                }
                else return true;
            case (FunctionMode.OPTIONCHANGE):
                if (isBenef) return true;
                else
                {
                    if (_targetIDList.Count < 2)
                    {
                        if (target._itemGradeID <= _beneficiary._itemGradeID) return true;
                        else return false;
                    }
                    else return false;
                }
            case (FunctionMode.SYNTHESIS):
                if (_targetIDList.Count < 2)
                {
                    if (target._itemLevel >= 10 && target._itemGradeID < 3)
                    {
                        if (_targetIDList.Count == 0) return true;
                        else
                        {
                            if (target._itemGradeID == itemDB._items[_targetIDList[0]]._itemGradeID) return true;
                            else return false;
                        }
                    }
                    else return false;
                }
                else return false;
            default: return true;
        }
    }

    public void QuitBtn()
    {
        _targetIDList.Clear();
        _mode = FunctionMode.NULL;
    }
}