using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentStat : MonoBehaviour
{
    public int _atk, _def, _fireRes, _waterRes, _airRes, _earthRes;

    public EquipmentStat(int atk, int def, int fireRes, int waterRes, int airRes, int earthRes)
    {
        _atk = atk;
        _def = def;
        _fireRes = fireRes;
        _waterRes = waterRes;
        _airRes = airRes;
        _earthRes = earthRes;
    }
}
