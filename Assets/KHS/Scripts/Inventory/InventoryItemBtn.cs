using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class InventoryItemBtn : InventoryFunction//MonoBehaviour
//��ũ��Ʈ�̸� ��������
{
    private bool _isSelect;
    static public bool _canSelect = true;

    public int btnID;
    [SerializeField] private ItemInfoUI _infoPanel;

    private Image _image;
    private Color CantTargeted = new Color(0.2f, 0.2f, 0.2f);


    protected override void Awake()
    {
        base.Awake();
        _infoPanel = GameObject.Find("Canvas").transform.Find("ItemInfoUI").GetComponent<ItemInfoUI>();

        _image = GetComponent<Image>();
    }


    private void Update()
    {
        _isSelect = (_targetIDList.Contains(btnID));

        if (_isSelect)
        {
            if ((_mode == FunctionMode.ENHANCEMENT || _mode == FunctionMode.OPTIONCHANGE) && btnID == _targetIDList[0])
                _image.color = Color.cyan;
            else _image.color = Color.gray;
        }
        else if (!_isSelect)
        {
            _image.color = Color.white;

            if (_mode == FunctionMode.ENHANCEMENT)
            {
                if (_targetIDList.Count == 0)
                {
                    if (!CanTargeted(btnID, true)) _image.color = CantTargeted;
                }
            }
            if (_mode == FunctionMode.OPTIONCHANGE)
            {
                if (_targetIDList.Count > 0)
                {
                    if (!CanTargeted(btnID, false)) _image.color = CantTargeted;
                }
            }
            else if (_mode == FunctionMode.SYNTHESIS)
            {
                if (!CanTargeted(btnID, false)) _image.color = CantTargeted;
            }

            if (btnID == itemDB._items.IndexOf(Item.memoryNewItem)) _image.color = Color.yellow;
            //else 
        }
    }


    public void ClickBtn()
    {
        if (_isFinished) _isFinished = false;
        Item.memoryNewItem = null;

        switch (_mode)
        {
            case (FunctionMode.ENHANCEMENT):

                if (_targetIDList.Count == 0) //��ȭ ����̶��
                {
                    if (CanTargeted(btnID, true)) // �Ķ����: ��ȭ��󿩺�
                    {
                        _beneficiary = itemDB._items[btnID];
                        _targetIDList.Add(btnID);
                        //_isSelect = true;
                    }
                }
                else
                {
                    if (_isSelect)
                    {
                        if (btnID != _targetIDList[0] || _targetIDList.Count == 1)
                        {
                            _targetIDList.Remove(btnID);
                            //_isSelect = false;
                        }
                    }
                    else if (CanTargeted(btnID, false) && !_isSelect)
                    {
                        if (_canSelect)
                        _targetIDList.Add(btnID);
                        //_isSelect = true;
                    }
                }
                break;
            case (FunctionMode.OPTIONCHANGE):
                if (_targetIDList.Count == 0) //�ɼǺ��� ����̶��
                {
                    if (CanTargeted(btnID, true))
                    {
                        _beneficiary = itemDB._items[btnID];
                        _targetIDList.Add(btnID);
                        //_isSelect = true;
                    }
                }
                else //�ɼǺ��� �����
                {
                    if (_isSelect) //�̹� �����Ÿ� Ŭ��
                    {
                        if (btnID != _targetIDList[0] || _targetIDList.Count == 1)
                        {
                            _targetIDList.Remove(btnID);
                            //_isSelect = false;
                        }
                    }
                    else if (CanTargeted(btnID, false)) //���ΰ����°Ŵ�
                    {
                        if (_targetIDList.Count < 2) //1����
                        {
                            _targetIDList.Add(btnID);
                            //_isSelect = true;
                        }
                    }
                }
                break;
            case (FunctionMode.SYNTHESIS):
                if (_isSelect) //Ŭ��
                {
                    _targetIDList.Remove(btnID);
                    //_isSelect = false;
                }
                else //���ΰ����°Ŵ�
                {
                    if (_targetIDList.Count < 2) // �ִ� 2������
                    {
                        if (CanTargeted(btnID, false)) //���ǿ� ������(�����̸�)
                        {
                            _targetIDList.Add(btnID);
                            //_isSelect = true;
                        }
                    }
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
    }*/


    private void AppearInfoUI()
    {
        _infoPanel.gameObject.SetActive(true);
        _infoPanel.ID = btnID;
        _infoPanel.Set();
    }
}