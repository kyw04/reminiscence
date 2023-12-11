using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenFunctionUI : InventoryFunction // MonoBehaviour
{
    [SerializeField] private Text _modeText;

    [Header("Enhancement")]
    [SerializeField] private Text _enhanced;
    [SerializeField] private Image _enhancedImage;
    [SerializeField] private GameObject _enhancedImageObj;
    [SerializeField] private Text _curLv;
    [SerializeField] private Text _afterLv;

    [Header("OptionChange")]
    [SerializeField] private Text _changed;
    [SerializeField] private Image _changedImage;
    [SerializeField] private GameObject _changedImageObj;
    [SerializeField] private Text _atkOrDef;
    [SerializeField] private Text[] _reses;
    [SerializeField] private GameObject _resesObj;

    [Header("Synthesis")]
    [SerializeField] private Text[] _ingredients;
    [SerializeField] private Image[] _ingredientsImage;
    [SerializeField] private GameObject[] _ingredientsImageObj;
    [SerializeField] private Text  _result;

    [Header("Equip")]
    [SerializeField] private Text _staffName;
    [SerializeField] private GameObject _staffImageObj;
    [SerializeField] private Text _staffAtk;
    [SerializeField] private Text _robeName;
    [SerializeField] private GameObject _robeImageObj;
    [SerializeField] private Text _robeDef;
    [SerializeField] private Text _grimoireName;
    [SerializeField] private Image _grimoireImage;
    [SerializeField] private GameObject _grimoireImageObj;
    [SerializeField] private Text[] _grimoireReses;


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
                    _enhancedImageObj.SetActive(true);
                    _enhancedImage.sprite = Item.SetItemImage(_beneficiary);
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
                    _enhanced.text = "???";
                    _enhancedImageObj.SetActive(false);
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
                    _changedImageObj.SetActive(true);
                    _changedImage.sprite = ChangedSprite();
                    _atkOrDef.text = $"{atkOrDef()}: {_beneficiary._itemAtkOrDef} > {ChangedText("atkOrDef")}";
                }
                else
                {
                    _changed.text = "???";
                    _changedImageObj.SetActive(false);
                    _atkOrDef.gameObject.SetActive(false);
                    _resesObj.SetActive(false);
                }
                break;
            case (FunctionMode.SYNTHESIS):
                _modeText.text = "아이템 합성";

                for (int i = 0; i < 2; i++)
                {
                    if (_targetIDList.Count >= i + 1)
                    {
                        _ingredients[i].text = itemDB._items[_targetIDList[i]]._itemName;
                        _ingredientsImageObj[i].SetActive(true);
                        _ingredientsImage[i].sprite = Item.SetItemImage(itemDB._items[_targetIDList[i]]);
                    }
                    else
                    {
                        _ingredients[i].text = "???";
                        _ingredientsImageObj[i].SetActive(false);
                    }
                }

                if (_targetIDList.Count == 0) _result.text = "???";
                else _result.text = $"결과:\n{Item.NameByGrades[_higherGradeID + 1]} ???";

                break;
            case (FunctionMode.EQUIP):
                _modeText.text = "아이템 장착";

                if (TempEquipData.instance.PlayerEquip[0] != null && TempEquipData.instance.PlayerEquip[0]._onEquip)
                {
                    _staffName.text = TempEquipData.instance.PlayerEquip[0]._itemName;
                    _staffImageObj.SetActive(true);
                    _staffAtk.text = $"공격력: {TempEquipData.instance.PlayerEquip[0]._itemAtkOrDef}";
                }
                else
                {
                    _staffName.text = "?? 스태프";
                    _staffImageObj.SetActive(false);
                    _staffAtk.text = "공격력: ??";
                }

                if (TempEquipData.instance.PlayerEquip[1] != null && TempEquipData.instance.PlayerEquip[1]._onEquip)
                {
                    _robeName.text = TempEquipData.instance.PlayerEquip[1]._itemName;
                    _robeImageObj.SetActive(true);
                    _robeDef.text = $"방어력: {TempEquipData.instance.PlayerEquip[1]._itemAtkOrDef}";
                }
                else
                {
                    _robeName.text = "?? 로브";
                    _robeImageObj.SetActive(false);
                   _robeDef.text = "방어력: ??";
                }


                if (TempEquipData.instance.PlayerEquip[2] != null && TempEquipData.instance.PlayerEquip[2]._onEquip)
                {
                    _grimoireName.text = TempEquipData.instance.PlayerEquip[2]._itemName;
                    _grimoireImageObj.SetActive(true);
                    _grimoireImage.sprite = Item.SetItemImage(TempEquipData.instance.PlayerEquip[2]);
                    _grimoireReses[0].text = $"화속성 내성: {TempEquipData.instance.PlayerEquip[2]._itemFireResis.ToString()}";
                    _grimoireReses[1].text = $"수속성 내성: {TempEquipData.instance.PlayerEquip[2]._itemWaterResis.ToString()}";
                    _grimoireReses[2].text = $"풍속성 내성: {TempEquipData.instance.PlayerEquip[2]._itemAirResis.ToString()}";
                    _grimoireReses[3].text = $"지속성 내성: {TempEquipData.instance.PlayerEquip[2]._itemEarthResis.ToString()}";
                }
                else
                {
                    _grimoireName.text = "?? ??의 마도서";
                    _grimoireImageObj.SetActive(false);
                    _grimoireReses[0].text = "화속성 내성: ??";
                    _grimoireReses[1].text = "수속성 내성: ??";
                    _grimoireReses[2].text = "풍속성 내성: ??";
                    _grimoireReses[3].text = "지속성 내성: ??";
                }


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

    private Sprite ChangedSprite()
    {
        if (_isFinished) return Item.SetItemImage(Item.memoryNewItem);
        else return Item.SetItemImage(_beneficiary);
    }
}
