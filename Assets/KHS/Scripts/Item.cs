using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[System.Serializable]
public class Item
{
    //public enum ItemType { WEAPON, ARMOR }
    public enum ItemPart { STAFF, ROBE, GRIMOIRE, NULL }

    public string _itemName;
    public int _itemLevel;
    public int _itemExp; //업그레이드 게이지
    public ItemPart _itemPart;
    public int _itemGradeID; //0:일반 1:희귀 2:영웅 3:전설
    public bool _onEquip; //장착여부

    public int _itemAtkOrDef;

    public int _itemFireResis;
    public int _itemWaterResis;
    public int _itemAirResis;
    public int _itemEarthResis;

    static public Item memoryNewItem = null;


    public Item(string name, int level, int exp, ItemPart part, int itemGradeID, bool onEquip = false,
                int atkOrDef = 0, int fireRes = 0, int waterRes = 0, int airRes = 0, int earthRes = 0)
    {
        _itemName = name;
        _itemLevel = level;
        _itemExp = exp;
        _itemPart = part;
        _itemGradeID = itemGradeID;

        _onEquip = onEquip;

        _itemAtkOrDef = atkOrDef;
        _itemFireResis = fireRes;
        _itemWaterResis = waterRes;
        _itemAirResis = airRes;
        _itemEarthResis = earthRes;
    }


    static public int[] ExpRiseByGrade = new int[4] { 100, 250, 500, 1000 };

    static public int[] NormalExpRequired = new int[10] { 200, 400, 600, 800, 1000, 1200, 1400, 1600, 1800, 0 };
    static public int[] RareExpRequired = new int[10] { 1000, 1400, 1800, 2200, 2600, 3000, 3400, 3800, 4200, 0 };
    static public int[] HeroicExpRequired = new int[10] { 4000, 4600, 5200, 5800, 6400, 7000, 7600, 8200, 8800, 0 };
    static public int[] LegendaryExpRequired = new int[10] { 8000, 8800, 9600, 10400, 11200, 12000, 12800, 13600, 14400, 0 };

    static public int RequiredExp(Item item)
    {
        if (item._itemLevel < 10)
        {
            switch (item._itemGradeID)
            {
                case (0): return NormalExpRequired[item._itemLevel -1];
                case (1): return RareExpRequired[item._itemLevel -1];
                case (2): return HeroicExpRequired[item._itemLevel -1];
                case (3): return LegendaryExpRequired[item._itemLevel -1];
                default: return 0;
            }
        }
        else return 0;
    }

    //등급별로
    static public int[] StaffAtkMin = new int[4] { 1, 11, 21, 31 };
    static public int[] StaffAtkMax = new int[4] { 10, 20, 30, 50 };

    static public int[] RobeDefMin = new int[4] { 0, 6, 11, 16 };
    static public int[] RobeDefMax = new int[4] { 5, 10, 15, 25 };

    static public int[] GrimoireResMin = new int[4] { 0, 6, 11, 16 };
    static public int[] GrimoireResMax = new int[4] { 5, 10, 15, 25 };

    static public string[] NameByGrades = new string[4] { "일반", "희귀", "영웅", "전설" };
    static public string[] NameByResTypes = new string[4] { "화염의", "파도의", "폭풍의", "대지의" };
    static public string[] NameByParts = new string[3] { "스태프", "로브", "마도서" };


    static public Item ItemLevelUP(Item item)//, int curExp)
    {
        while (item._itemLevel < 10)
        {
            if (item._itemExp >= RequiredExp(item))
            {
                item._itemExp -= RequiredExp(item);
                item._itemLevel += 1;
                item = UpgradeByLevel(item);
            }
            else break;
        }
        if (item._itemLevel == 10) item._itemExp = 0;

        return item;
    }


    static public Item UpgradeByLevel(Item item)
    {
        if (item._itemPart != ItemPart.GRIMOIRE)
        {
            item._itemAtkOrDef += 1;
        }
        else
        {
            item._itemFireResis += 1;
            item._itemWaterResis += 1;
            item._itemAirResis += 1;
            item._itemEarthResis += 1;
        }

        return item;
    }


    static public int PartToPartID(Item.ItemPart part)
    {
        if (part == Item.ItemPart.STAFF) return 0;
        else if (part == Item.ItemPart.ROBE) return 1;
        else return 2;
    }


    static public Item.ItemPart PartIDToPart(int ID)
    {
        if (ID == 0) return Item.ItemPart.STAFF;
        else if (ID == 1) return Item.ItemPart.ROBE;
        else return Item.ItemPart.GRIMOIRE;
    }


    static public int duddjfmfTMwk(Item item)
    {
        List<int> reses = new List<int>() { item._itemFireResis, item._itemWaterResis, item._itemAirResis, item._itemEarthResis };

        return reses.IndexOf(reses.Max());
    }


    static public Sprite SetItemImage(Item item)
    {
        if (item._itemPart == Item.ItemPart.GRIMOIRE)
            return Resources.Load<Sprite>("Sprites/obj" + Item.PartToPartID(item._itemPart) + Item.duddjfmfTMwk(item));
        else return Resources.Load<Sprite>("Sprites/obj" + Item.PartToPartID(item._itemPart));
    }
}