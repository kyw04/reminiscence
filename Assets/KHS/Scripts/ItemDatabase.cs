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
        //타입 이름 레벨 부위 등급 뭐더라경험치? 공격력/ 화수풍지

        string[] line = ItemDBTxt.text.Substring(0, ItemDBTxt.text.Length -1).Split('\n');
        for (int i = 0; i < line.Length -1; i++)
        {
            string[] row = line[i].Split('\t');
            Item.ItemType type = (Item.ItemType)System.Enum.Parse(typeof(Item.ItemType), row[0]);
            Item.ItemPart part = (Item.ItemPart)System.Enum.Parse(typeof(Item.ItemPart), row[3]);

            _items.Add(new Item(type, row[1], int.Parse(row[2]), part, int.Parse(row[4]), int.Parse(row[5]),
                                int.Parse(row[6]), int.Parse(row[7]), int.Parse(row[8]), int.Parse(row[9]), int.Parse(row[10]), false));
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