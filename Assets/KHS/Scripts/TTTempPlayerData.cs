using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTTempPlayerData : MonoBehaviour
{
    private static TTTempPlayerData instance = null;


    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    public static TTTempPlayerData Instance
    {
        get
        {
            if (null == instance) return null;
            return instance;
        }
    }


    //public Item[] PlayerEquip = { null, null, null };
}
