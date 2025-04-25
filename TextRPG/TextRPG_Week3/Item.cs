namespace TextRPG_Week3
{
    public enum ItemType
    {
        Weapon =1,
        Armor,
        HealthPotion,
        ManaPotion
    }
        //아이템 종류 열거형
        //Weapon부터 1
        //Armor = 2
        //HealthPotion = 3
        //ManaPotion = 4
    public class Item
    {
        //public class Item
        //아이템의 속성값들
        
        //아이템 속성값 받는 방식
        public string Name { get; set; }
        public ItemType Type { get; set; }
        public int Value { get; set; } // 공격력, 방어력, 회복량
        public string Description { get; set; }
        public bool IsEquipped { get; set; }
        public int Count { get; set; }
        public bool IsConsumable { get; set; }
        public Item() { }
        public Item(string name, ItemType type, int value, string description, bool isEquipped = false, int count = 1, bool isConsumable = false)
        {
            Name = name;
            Type = type;
            Value = value;
            Description = description;
            IsEquipped = isEquipped;
            Count = count;
            IsConsumable = isConsumable;
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
                ItemType.HealthPotion => $"체력 회복 +{Value}",
                ItemType.ManaPotion => $"마나 회복 + {Value}",
                _ => ""
            };

            return $"- {equippedMark}{Name,-12} | {statText,-14} | {Description}";
        }
        //GetDisplayInfo함수 문자열을 반환
        //무기나 방어구이며 착용중일때[E]표시 아니면 띄어쓰기
        
        //타입에 따라 문자열로 변환
        //무기 = 공격력 + 값
        //방어구 = 방어력 +값
        //회복포션 = 체력회복 + 값
        //마나포션 = 마나회복 + 값
        //그외 = ""
        
        //문자열 반환
    }
}
