namespace TextRPG_Week3
{
    public enum ItemType
    {
        Weapon,
        Armor,
        Consumable   // 회복 아이템 추가됨
    }

    public class Item
    {
        public string Name { get; set; } = "";
        public ItemType Type { get; set; }
        public int Value { get; set; } // 공격력, 방어력, 회복량
        public string Description { get; set; } = "";
        public bool IsEquipped { get; set; } = false;
        public int Count { get; set; } = 1;
        public Item(string name, ItemType type, int value, string description, bool isEquipped = false, int count = 1)
        {
            Name = name;
            Type = type;
            Value = value;
            Description = description;
            IsEquipped = isEquipped;
            Count = count;
        }

        public string GetDisplayInfo()
        {
            string equippedMark = Type == ItemType.Weapon || Type == ItemType.Armor
                ? IsEquipped ? "[E]" : "   "
                : "   ";

            string statText = Type switch
            {
                ItemType.Weapon => $"공격력 +{Value}",
                ItemType.Armor => $"방어력 +{Value}",
                ItemType.Consumable => $"체력 회복 +{Value}",
                _ => ""
            };

            return $"- {equippedMark}{Name,-12} | {statText,-14} | {Description}";
        }
    }
}
