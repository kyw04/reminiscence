using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentStat : MonoBehaviour
{
    public int _atk, _def, _fireRes, _waterRes, _airRes, _earthRes;

    public EquipmentStat(int atk = 0, int def = 0, int fireRes = 0, int waterRes = 0, int airRes = 0, int earthRes = 0)
    {
        _atk = atk;
        _def = def;
        _fireRes = fireRes;
        _waterRes = waterRes;
        _airRes = airRes;
        _earthRes = earthRes;
    }
}
