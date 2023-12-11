using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Events;

public class InvenItemBtn : InventoryFunction
{
    private bool _isSelect;
    static public bool _canSelect = true;

    public int btnID;

    private Image _image;
    private Image _frame;
    private Color _cantTarget = new Color(0.2f, 0.2f, 0.2f);


    protected override void Awake()
    {
        base.Awake();

        _image = GetComponent<Image>();
        _frame = this.transform.Find("Frame").GetComponent<Image>();
    }


    private void Update()
    {
        _isSelect = (_targetIDList.Contains(btnID));

        if (!_isSelect)
        {
            _image.color = Color.white;
            _frame.color = Color.white;

            if (_mode == FunctionMode.ENHANCEMENT)
            {
                if (_targetIDList.Count == 0)
                {
                    if (!CanTargeted(btnID, true)) { _image.color = _cantTarget; _frame.color = _cantTarget; }
                }
                else
                {
                    if (!CanTargeted(btnID, false)) { _image.color = _cantTarget; _frame.color = _cantTarget; }
                }
            }
            else if (_mode == FunctionMode.OPTIONCHANGE)
            {
                if (_targetIDList.Count > 0)
                {
                    if (!CanTargeted(btnID, false)) { _image.color = _cantTarget; _frame.color = _cantTarget; }
                }
            }
            else if (_mode == FunctionMode.SYNTHESIS)
            {
                if (!CanTargeted(btnID, false)) { _image.color = _cantTarget; _frame.color = _cantTarget; }
            }

            if (btnID == itemDB._items.IndexOf(Item.memoryNewItem)) _image.color = Color.yellow;
        }
        else if (_isSelect)
        {
            if ((_mode == FunctionMode.ENHANCEMENT || _mode == FunctionMode.OPTIONCHANGE) && btnID == _targetIDList[0])
                _image.color = Color.cyan;
            else { if (_mode != FunctionMode.EQUIP) {
                    _image.color = Color.gray; _frame.color = Color.gray;   }
            }
        }

        gameObject.transform.Find("OnEquipMark").gameObject.SetActive(itemDB._items[btnID]._onEquip);
    }


    public void ClickBtn()
    {
        if (_isFinished) _isFinished = false;
        Item.memoryNewItem = null;

        switch (_mode)
        {
            case (FunctionMode.ENHANCEMENT):

                if (_targetIDList.Count == 0) //강화 대상이라면
                {
                    if (CanTargeted(btnID, true)) // 파라미터: 강화대상여부
                    {
                        _beneficiary = itemDB._items[btnID];
                        _targetIDList.Add(btnID);
                    }
                }
                else
                {
                    if (_isSelect)
                    {
                        if (btnID != _targetIDList[0] || _targetIDList.Count == 1)
                        {
                            _targetIDList.Remove(btnID);
                        }
                    }
                    else if (CanTargeted(btnID, false) && !_isSelect)
                    {
                        if (_canSelect) _targetIDList.Add(btnID);
                    }
                }
                break;
            case (FunctionMode.OPTIONCHANGE):
                if (_targetIDList.Count == 0) //옵션변경 대상이라면
                {
                    if (CanTargeted(btnID, true))
                    {
                        _beneficiary = itemDB._items[btnID];
                        _targetIDList.Add(btnID);
                    }
                }
                else //옵션변경 재료라면
                {
                    if (_isSelect) //이미 고른거면 클취
                    {
                        if (btnID != _targetIDList[0] || _targetIDList.Count == 1)
                        {
                            _targetIDList.Remove(btnID);
                        }
                    }
                    else if (CanTargeted(btnID, false)) //새로고르는거는
                    {
                        _beneficiary = itemDB._items[_targetIDList[0]];
                        _targetIDList.Add(btnID);
                    }
                }
                break;
            case (FunctionMode.SYNTHESIS):
                if (_isSelect) //클취
                {
                    _targetIDList.Remove(btnID);
                }
                else //새로고르는거는
                {
                    if (CanTargeted(btnID, false))
                    {
                        _targetIDList.Add(btnID);
                    }
                }
                break;
            case (FunctionMode.EQUIP):
                Item item = itemDB._items[btnID];

                int part = Item.PartToPartID(item._itemPart);

                if (item._onEquip)
                {
                    TempEquipData.instance.PlayerEquip[part] = null;
                    item._onEquip = false;
                }
                else
                {
                    if (TempEquipData.instance.PlayerEquip[part] != null && TempEquipData.instance.PlayerEquip[part]._onEquip)
                    {
                        TempEquipData.instance.PlayerEquip[part]._onEquip = false;
                    }
                    TempEquipData.instance.PlayerEquip[part] = item;
                    item._onEquip = true;
                }

                TempEquipData.instance.SetEquipmentStat();
                break;
            case (FunctionMode.NULL):
                _targetIDList.Clear();
                _targetIDList.Add(btnID);
                AppearInfoUI();
                break;
        }
    }


    private void AppearInfoUI()
    {
        itemInfoUI.gameObject.SetActive(true);
        //_infoPanel.ID = btnID;
        itemInfoUI.Set(btnID);
        itemInfoUI.SetPos();
    }
}