[System.Serializable]
public class Item
{
    public enum ItemType { WEAPON, ARMOR }
    public enum ItemPart { STAFF, GRIMOIRE, ROBE, NULL }
    //public enum itemGrade { NORMAL, RARE, HEROIC, LEGENDARY }

    public ItemType _itemType;

    public string _itemName;
    public int _itemLevel;
    public ItemPart _itempart;
    public int _itemGradeID; //0~3
    public int _itemExp; //업그레이드 게이지

    public int _itemAtkOrDef;

    public int _itemFireResis;
    public int _itemWaterResis;
    public int _itemAirResis;
    public int _itemEarthResis;

    public bool _onEquip;

    static public Item memoryNewItem = null;


    public Item(ItemType type, string name, int level, ItemPart part, int itemGradeID, int exp,
                int atkOrDef = 0, int fireRes = 0, int waterRes = 0, int airRes = 0, int earthRes = 0, bool onEquip = false)
    {
        _itemType = type;

        _itemName = name;
        _itemLevel = level;
        _itempart = part;
        _itemGradeID = itemGradeID;
        _itemExp = exp;

        _itemAtkOrDef = atkOrDef;

        _itemFireResis = fireRes;
        _itemWaterResis = waterRes;
        _itemAirResis = airRes;
        _itemEarthResis = earthRes;

        _onEquip = onEquip;
    }

    //일반 희귀 영웅 전설
    static public int[] _expRiseByGrade = new int[4] { 100, 250, 500, 1000 };

    // ㅜㅜㅜㅜㅜㅜㅜㅜㅜㅜㅜㅜㅜㅜㅜㅜㅜㅜㅜㅜㅜㅜㅜㅜㅜㅜ
    static public int[] _normalExpRequired = new int[10] { 400, 400, 400, 400, 400, 400, 400, 400, 400, 0 };
    static public int[] _rareExpRequired = new int[10] { 400, 400, 400, 400, 400, 400, 400, 400, 400, 0 };
    static public int[] _heroicExpRequired = new int[10] { 400, 400, 400, 400, 400, 400, 400, 400, 400, 0 };
    static public int[] _legendaryExpRequired = new int[10] { 400, 400, 400, 400, 400, 400, 400, 400, 400, 0 };
    /*static public int[] _expRequired(int gradeID) //아마도 안쓹듯..
    {
        switch (gradeID)
        {
            case (0): return _normalExpRequired;
            case (1): return _rareExpRequired;
            case (2): return  _heroicExpRequired;
            case (3): return _legendaryExpRequired;
            default: return null;
        }
    }*/

    static public int _requiredExp(Item item)
    {
        if (item._itemLevel < 10)
        {
            switch (item._itemGradeID)
            {
                case (0): return _normalExpRequired[item._itemLevel -1];
                case (1): return _rareExpRequired[item._itemLevel -1];
                case (2): return _heroicExpRequired[item._itemLevel -1];
                case (3): return _legendaryExpRequired[item._itemLevel -1];
                default: return 0;
            }
        }
        else return 0;
    }

    static public int[] _staffAtkMin = new int[4] { 1, 11, 21, 31 };
    static public int[] _staffAtkMax = new int[4] { 10, 20, 30, 50 };
    static public int[] _grimoireAtkMin = new int[4] { 0, 6, 11, 16 };
    static public int[] _grimoireAtkMax = new int[4] { 5, 10, 15, 25 };

    static public int[] _armorDefMin = new int[4] { 1, 6, 11, 16 };
    static public int[] _armorDefMax = new int[4] { 5, 10, 15, 25 };
    static public int[] _armorResMin = new int[4] { 0, 6, 11, 16 };
    static public int[] _armorResMax = new int[4] { 5, 10, 15, 25 };

    static public string[] _name_grades = new string[4] { "일반", "희귀", "영웅", "전설" };
    static public string[] _name_resTypes = new string[4] { "화염의", "파도의", "폭풍의", "대지의" };
    static public string[] _name_parts = new string[3] { "스태프", "마도서", "로브" };


    static public Item ItemLevelUP(Item item)//, int curExp)
    {
        while (item._itemLevel < 10)
        {
            if (item._itemExp >= _requiredExp(item))
            {
                item._itemExp -= _requiredExp(item);
                item._itemLevel += 1;
            }
            else break;
        }
        if (item._itemLevel == 10) item._itemExp = 0;

        return item;
    }
}