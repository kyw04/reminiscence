using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

public class ItemDatabase : MonoBehaviour
{
    public TextAsset ItemDBTxt;
    public List<Item> _items = new List<Item>();


    private void Awake()
    {
        string[] line = ItemDBTxt.text.Substring(0, ItemDBTxt.text.Length -1).Split('\n');
        for (int i = 0; i < line.Length -1; i++)
        {
            string[] row = line[i].Split('\t');
            Item.ItemPart part = (Item.ItemPart)System.Enum.Parse(typeof(Item.ItemPart), row[3]);

            //이름 레벨 경험치 부위 등급 장착여부 공방 저항x4
            _items.Add(new Item(row[0], int.Parse(row[1]), int.Parse(row[2]), part, int.Parse(row[4]), false,
                                int.Parse(row[6]), int.Parse(row[7]), int.Parse(row[8]), int.Parse(row[9]), int.Parse(row[10])));
        }

        Load();
    }


    public void ReArrange()
    {
        _items = _items.OrderBy(item => item._itemGradeID).ThenBy(item => item._itemLevel).
                ThenBy(item => item._itemName).ToList();
        Save();
    }


    private void Save()
    {
        string serialized = JsonConvert.SerializeObject(_items);
        File.WriteAllText(Application.dataPath + "/Resources/KHS/ItemDataTxt.txt", serialized);
    }


    private void Load()
    {
        string serialized = File.ReadAllText(Application.dataPath + "/Resources/KHS/ItemDataTxt.txt");
        _items = JsonConvert.DeserializeObject<List<Item>>(serialized);
    }
}