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
    [SerializeField] private GameObject _resesObj;
    [SerializeField] private Text[] _reses;

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

                InventoryItemBtn._canSelect = true;
                if (_targetIDList.Count > 0)
                {
                    _enhanced.text = _beneficiary._itemName;
                    _curLv.text = $"현재 Lv: {_beneficiary._itemLevel} ( {_beneficiary._itemExp} / {Item._requiredExp(_beneficiary)} )";

                    Item after = new Item(_beneficiary._itemType, "", _beneficiary._itemLevel, _beneficiary._itempart,
                                          _beneficiary._itemGradeID, _beneficiary._itemExp + _sumExp);

                    if (after._itemExp >= Item._requiredExp(after))
                    {
                        if (Item.ItemLevelUP(after)._itemLevel < 10)
                        {
                            after = Item.ItemLevelUP(after);
                        }
                        else
                        {
                            after._itemLevel = 10;
                            InventoryItemBtn._canSelect = false;
                        }
                    }
                    _afterLv.text = $"강화 후 Lv: {after._itemLevel} ( {after._itemExp} / {Item._requiredExp(after)} )";
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
                        if (_beneficiary._itemType == Item.ItemType.WEAPON)
                        {
                            _resesObj.SetActive(false);
                            return "공격력";
                        }
                        else
                        {
                            _resesObj.SetActive(true);
                            _reses[0].text = $"화속성:{_beneficiary._itemFireResis} > ??";
                            _reses[1].text = $"화속성:{_beneficiary._itemWaterResis} > ??";
                            _reses[2].text = $"화속성:{_beneficiary._itemAirResis} > ??";
                            _reses[3].text = $"화속성:{_beneficiary._itemEarthResis} > ??";
                            return "방어력";
                        }
                    }
                    _changed.text = _beneficiary._itemName;
                    _atkOrDef.text = $"{atkOrDef()}: {_beneficiary._itemAtkOrDef} > ??";
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
                else _result.text = $"{Item._name_grades[_higherGradeID + 1]} ???";

                break;
            default: break;
        }
    }


    public void IsFinish(bool o)
    {
        _isFinished = o;
    }
}
