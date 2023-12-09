/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Events;

public class InvenItemBtn : InventoryFunction
{
    private bool _isSelect;
    static public bool _canSelect = true;

    public int btnID;
    [SerializeField] private ItemInfoUI _infoPanel;

    private Image _image;
    private Color _cantTarget = new Color(0.2f, 0.2f, 0.2f);


    protected override void Awake()
    {
        base.Awake();
        _infoPanel = GameObject.Find("Canvas").transform.Find("ItemInfoUI").GetComponent<ItemInfoUI>();

        _image = GetComponent<Image>();
    }


    private void Update()
    {
        _isSelect = (_targetIDList.Contains(btnID));

        if (!_isSelect)
        {
            if (itemDB._items[btnID]._onEquip) _image.color = Color.red;
            else _image.color = Color.white;

            if (_mode == FunctionMode.ENHANCEMENT)
            {
                if (_targetIDList.Count == 0)
                {
                    if (!CanTargeted(btnID, true)) _image.color = _cantTarget;
                }
            }
            else if (_mode == FunctionMode.OPTIONCHANGE)
            {
                if (_targetIDList.Count > 0)
                {
                    if (!CanTargeted(btnID, false)) _image.color = _cantTarget;
                }
            }
            else if (_mode == FunctionMode.SYNTHESIS)
            {
                if (!CanTargeted(btnID, false)) _image.color = _cantTarget;
            }

            if (btnID == itemDB._items.IndexOf(Item.memoryNewItem)) _image.color = Color.yellow;
        }
        else if (_isSelect)
        {
            if ((_mode == FunctionMode.ENHANCEMENT || _mode == FunctionMode.OPTIONCHANGE) && btnID == _targetIDList[0])
                _image.color = Color.cyan;
            else _image.color = Color.gray;
        }
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
                        //if (_targetIDList.Count < 2) //1나만
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
                    //if (_targetIDList.Count < 2) // 최대 2개까지
                    if (CanTargeted(btnID, false)) //조건에 맞으면(만렙이면)
                    {
                        _targetIDList.Add(btnID);
                    }
                }
                break;
            case (FunctionMode.EQUIP):
                Item item = itemDB._items[btnID];

                int part = 0;
                switch (item._itemPart)
                {
                    case (Item.ItemPart.STAFF): part = 0; break;
                    case (Item.ItemPart.ROBE): part = 1; break;
                    case (Item.ItemPart.GRIMOIRE): part = 2; break;
                }

                Debug.Log(TTTempPlayerData.instance.PlayerEquip[part]);
                if (item._onEquip)
                {
                    TTTempPlayerData.instance.PlayerEquip[part] = null;
                    item._onEquip = false;
                }
                else
                {
                    if (TTTempPlayerData.instance.PlayerEquip[part] != null)
                    {
                        Debug.Log(TTTempPlayerData.instance.PlayerEquip[part]);
                        TTTempPlayerData.instance.PlayerEquip[part]._onEquip = false;
                    }
                    TTTempPlayerData.instance.PlayerEquip[part] = item;
                    item._onEquip = true;
                }
                break;
            case (FunctionMode.NULL):
                if (_targetIDList.Count != 0) _targetIDList[0] = btnID;
                else _targetIDList.Add(btnID);
                AppearInfoUI();
                break;
        }
    }

    /*
    public bool CanTargeted(bool isIngredient)//, FunctionMode mode)
    {
        Item target = itemDB._items[btnID];
        switch (_mode)
        {
            case (FunctionMode.ENHANCEMENT):
                if (!isIngredient)
                {
                    if (target._itemLevel < 10) return true;
                    else return false;
                }
                else return true;
            case (FunctionMode.OPTIONCHANGE):
                if (!isIngredient) return true;
                else
                {
                    if (target._itemGradeID <= _beneficiary._itemGradeID) return true;
                    else return false;
                }
            case (FunctionMode.SYNTHESIS):
                if (target._itemLevel >= 10 && target._itemGradeID < 3) return true;
                else return false;

            default: return true;
        }
    }


    private void AppearInfoUI()
    {
        _infoPanel.gameObject.SetActive(true);
        //_infoPanel.ID = btnID;
        _infoPanel.Set(btnID);
        _infoPanel.SetPos();
    }
}*/