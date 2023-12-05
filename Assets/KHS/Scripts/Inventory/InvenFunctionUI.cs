using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenFunctionUI : InventoryFunction // MonoBehaviour
{
    [SerializeField] private Text _modeText;

    [Header("Enhancement")]
    [SerializeField] private Text _enhanced;
    [SerializeField] private Text _curLv;
    [SerializeField] private Text _afterLv;

    [Header("OptionChange")]
    [SerializeField] private Text _changed;
    [SerializeField] private Text _atkOrDef;
    [SerializeField] private Text[] _reses;
    //[SerializeField] private GameObject _atkOrDefObj;
    [SerializeField] private GameObject _resesObj;

    [Header("Synthesis")]
    [SerializeField] private Text[] _ingredients;
    [SerializeField] private Text  _result;

    [Header("Equip")]
    [SerializeField] private Text _staffName;
    [SerializeField] private Text _staffAtk;
    [SerializeField] private Text _grimoireName;
    [SerializeField] private Text _grimoireAtk;
    [SerializeField] private Text _robeName;
    [SerializeField] private Text _robeDef;
    [SerializeField] private Text[] _robeRes;


    protected override void Awake()
    {
        base.Awake();
    }


    private void Update()
    {
        switch (_mode)
        {
            case (FunctionMode.ENHANCEMENT):
                _modeText.text = "아이템 강화";

                InvenItemBtn._canSelect = true;
                if (_targetIDList.Count > 0)
                {
                    _enhanced.text = _beneficiary._itemName;
                    _curLv.text = $"현재 Lv: {_beneficiary._itemLevel} ( {_beneficiary._itemExp} / {Item.RequiredExp(_beneficiary)} )";

                    Item after = new Item("", _beneficiary._itemLevel, _beneficiary._itemExp + _sumExp, Item.ItemPart.NULL, _beneficiary._itemGradeID);

                    if (after._itemExp >= Item.RequiredExp(after))
                    {
                        if (Item.ItemLevelUP(after)._itemLevel < 10)
                        {
                            after = Item.ItemLevelUP(after);
                        }
                        else
                        {
                            after._itemLevel = 10;
                            InvenItemBtn._canSelect = false;
                        }
                    }
                    _afterLv.text = $"강화 후 Lv: {after._itemLevel} ( {after._itemExp} / {Item.RequiredExp(after)} )";
                }
                else
                {
                    _enhanced.text = null;
                    _curLv.text = "현재 Lv: ??";
                    _afterLv.text = "강화 후 Lv: ??";
                }
                break;
            case (FunctionMode.OPTIONCHANGE):
                _modeText.text = "아이템 옵션 변경";

                if (_targetIDList.Count > 0)
                {
                    string atkOrDef()
                    {
                        if (_beneficiary._itemPart == Item.ItemPart.GRIMOIRE)
                        {
                            _resesObj.SetActive(true);
                            _atkOrDef.gameObject.SetActive(false);

                            _reses[0].text = $"화속성:{_beneficiary._itemFireResis} > {ChangedText("fireRes")}";
                            _reses[1].text = $"수속성:{_beneficiary._itemWaterResis} > {ChangedText("waterRes")}";
                            _reses[2].text = $"풍속성:{_beneficiary._itemAirResis} > {ChangedText("airRes")}";
                            _reses[3].text = $"지속성:{_beneficiary._itemEarthResis} > {ChangedText("earthRes")}";

                            return null;
                        }
                        else
                        {
                            _resesObj.SetActive(false);
                            _atkOrDef.gameObject.SetActive(true);

                            if (_beneficiary._itemPart == Item.ItemPart.STAFF) return "공격력";
                            else return "방어력";
                        }
                    }
                    _changed.text = ChangedText("name");
                    _atkOrDef.text = $"{atkOrDef()}: {_beneficiary._itemAtkOrDef} > {ChangedText("atkOrDef")}";
                }
                else
                {
                    _changed.text = null;
                    _atkOrDef.gameObject.SetActive(false);
                    _resesObj.SetActive(false);
                }
                break;
            case (FunctionMode.SYNTHESIS):
                _modeText.text = "아이템 합성";

                for (int i = 0; i < 2; i++)
                {
                    if (_targetIDList.Count >= i + 1) _ingredients[i].text = itemDB._items[_targetIDList[i]]._itemName;
                    else _ingredients[i].text = "ㅇㅅㅇ";
                }

                if (_targetIDList.Count == 0) _result.text = "???";
                else _result.text = $"{Item.NameByGrades[_higherGradeID + 1]} ???";

                break;
            default: break;
        }
    }


    public void IsFinish(bool o)
    {
        _isFinished = o;
    }


    private string ChangedText(string stat)
    {
        if (stat == "name")
        {
            if (_isFinished) return Item.memoryNewItem._itemName;
            else return _beneficiary._itemName;
        }
        else
        {
            if (_isFinished)
            {
                switch (stat)
                {
                    case ("atkOrDef"):
                        return Item.memoryNewItem._itemAtkOrDef.ToString();
                    case ("fireRes"):
                        return Item.memoryNewItem._itemFireResis.ToString();
                    case ("waterRes"):
                        return Item.memoryNewItem._itemWaterResis.ToString();
                    case ("airRes"):
                        return Item.memoryNewItem._itemAirResis.ToString();
                    case ("earthRes"):
                        return Item.memoryNewItem._itemEarthResis.ToString();
                    default: return null;
                }
            }
            else return "??";
        }
    }
}
