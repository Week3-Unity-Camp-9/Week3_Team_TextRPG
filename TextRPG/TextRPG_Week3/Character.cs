namespace TextRPG_Week3
{
    public class Character
    {
        public enum PlayerClass
        {
            Warrior,
            Wizard,
            Thief
        }
        public List<Item> Inventory { get; set; } = new List<Item>();

        public int Level { get; set; } = 1;
        public string Name { get; set; } = "이름";
        public PlayerClass Job { get; set; } = PlayerClass.Warrior;
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Hp { get; set; }
        public int MaxHp { get; set; }
        public int Mp { get; set; }
        public int MaxMp { get; set; }
        public int Gold { get; set; } = 1500;
        public int EXP { get; set; } = 0;
        public Character() { }
        public int EquipAttack => Inventory.Where(item => item.IsEquipped && item.Type == ItemType.Weapon).Sum(i => i.Value);
        public int EquipDefense => Inventory.Where(item => item.IsEquipped && item.Type == ItemType.Armor).Sum(i => i.Value);
        public int RequireEXP => (Level == 1 ? 10 : 0) + (Level - 1) * 35;

        public float TotalAttack => Attack + ((Level - 1) * 0.5f) + EquipAttack;

        public int TotalDefense => Defense + ((Level - 1) * 1) + EquipDefense;
        // ============================
        // 1. 상태 보기
        // ============================
        public string GetJob()
        {
            return Job switch
            {
                PlayerClass.Warrior => "전사",
                PlayerClass.Wizard => "마법사",
                PlayerClass.Thief => "도적",
                _ => "???"
            };
        }

        public (string, string) GetSkills()
        {
            return Job switch
            {
                PlayerClass.Warrior => ("강타 - MP 10\n강력한 공격으로 2배의 데미지를 가한다.", "방어 태세 - MP 15\n방어 태세를 취해 받는 데미지를 1/2로 줄입니다."),
                PlayerClass.Wizard => ("에너지볼 - MP 5\n마나를 모은 구체를 날려 1.5배의 데이지를 가한다.", "마나폭발 - MP 20\n몸속의 마나를 폭발시켜 랜덤한 적에게 2~4번 데미지를 가한다.(턴 스킵)"),
                PlayerClass.Thief => ("백스탭 - MP 10\n적의 뒤로 다가가 급소를 찔러 3배의 데미지를 준다.(회피 가능)", "은신 - MP 20\n지형지물에 몸을 숨겨 적들의 공격을 모두 막는다."),
                _ => ("???", "???")
            };
        }

        public void DisplayStatus()
        {
            int bonusAttack = 0;
            int bonusDefense = 0;

            foreach (var item in Inventory)
            {
                if (item.IsEquipped)
                {
                    if (item.Type == ItemType.Weapon)
                    {
                        bonusAttack += item.Value;
                    }
                    else if (item.Type == ItemType.Armor)
                    {
                        bonusDefense += item.Value;
                    }
                }
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("상태 보기");
            Console.ResetColor();
            Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"Lv. 0{Level} ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"경험치 : {EXP}/{RequireEXP}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{Name} ( {GetJob()} )");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"공격력 : {TotalAttack}");
            if (bonusAttack > 0) Console.Write($" (+{bonusAttack})");
            Console.WriteLine();

            Console.Write($"방어력 : {TotalDefense}");
            if (bonusDefense > 0) Console.Write($" (+{bonusDefense})");
            Console.WriteLine();

            Console.WriteLine($"체 력 : {Hp}/{MaxHp}");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"Gold   : {Gold} G");
            Console.ResetColor();
            Console.WriteLine();
        }

        // ============================
        // 2. 아이템 장착 / 해제
        // ============================
        public void ToggleEquipItem(int index)
        {
            if (index < 0 || index >= Inventory.Count)
            {
                Console.WriteLine("잘못된 입력입니다.");
                Console.ReadKey();
                return;
            }

            Item selectedItem = Inventory[index];

            if (selectedItem.Type == ItemType.Weapon || selectedItem.Type == ItemType.Armor)
            {
                // 같은 타입의 기존 장비 해제
                foreach (var item in Inventory)
                {
                    if (item.Type == selectedItem.Type && item != selectedItem)
                        item.IsEquipped = false;
                }

                // 선택 아이템 장착/해제
                selectedItem.IsEquipped = !selectedItem.IsEquipped;
                Console.WriteLine($"{selectedItem.Name} {(selectedItem.IsEquipped ? "장착" : "해제")}했습니다.");
            }
            else
            {
                Console.WriteLine("장착할 수 없는 아이템입니다.");
            }
            Console.ReadKey();
        }

        public void ShowInventory()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("인벤토리");
                Console.ResetColor();

                if (Inventory.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("보유 중인 아이템이 없습니다.");
                }
                else
                {

                    for (int i = 0; i < Inventory.Count; i++)
                    {
                        if (Inventory[i].IsConsumable)
                        {
                            switch (Inventory[i].Type)
                            {
                                case ItemType.HealthPotion:
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    break;
                                case ItemType.ManaPotion:
                                    Console.ForegroundColor = ConsoleColor.Blue;
                                    break;
                            }
                            Console.WriteLine($"{i + 1}. {Inventory[i].GetDisplayInfo()} | {Inventory[i].Count} 개");
                            continue;
                        }
                        switch (Inventory[i].Type)
                        {
                            case ItemType.Weapon:
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                break;
                            case ItemType.Armor:
                                Console.ForegroundColor = ConsoleColor.DarkBlue;
                                break;
                            default:
                                Console.ResetColor();
                                break;
                        }
                        Console.WriteLine($"{i + 1}. {Inventory[i].GetDisplayInfo()}");
                    }
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n아이템 번호를 선택하면 장착/해제하거나 포션을 사용할 수 있습니다.");
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n0. 나가기");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write(">> ");
                Console.ResetColor();
                string input = Console.ReadLine();
                if (int.TryParse(input, out int selected))
                {
                    if (selected == 0) return;
                    else if (selected <= Inventory.Count && Inventory[selected - 1].IsConsumable)
                    {
                        UseConsumableItem(Inventory[selected - 1]);
                        continue;
                    }
                    else if (selected <= Inventory.Count)
                    {
                        ToggleEquipItem(selected - 1);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("잘못된 입력입니다.");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.ReadKey();
                }
            }
        }

        // ============================
        // 3. 회복 아이템 사용
        // ============================
        public void UseConsumableItem(Item potion)
        {
            switch (potion.Type)
            {
                case ItemType.HealthPotion:
                    int originalHp = Hp;
                    Hp += potion.Value;
                    potion.Count--;
                    if (potion.Count == 0) Inventory.Remove(potion);
                    if (Hp > MaxHp) Hp = MaxHp;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{potion.Name}을(를) 사용하여 체력을 {potion.Value} 회복했습니다.");
                    Console.WriteLine($"HP {originalHp} => {Hp}");
                    Console.ResetColor();
                    Console.ReadKey();
                    break;
                case ItemType.ManaPotion:
                    int originalMp = Mp;
                    Mp += potion.Value;
                    potion.Count--;
                    if (potion.Count == 0) Inventory.Remove(potion);
                    if (Mp > MaxMp) Mp = MaxMp;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{potion.Name}을(를) 사용하여 마나를 {potion.Value} 회복했습니다.");
                    Console.WriteLine($"HP {originalMp} => {Mp}");
                    Console.ResetColor();
                    Console.ReadKey();
                    break;
            }
        }

        // ============================
        // 4. 상점 구매용 아이템 추가
        // ============================
        public void AddItem(Item item)
        {
            Inventory.Add(item);

            Inventory = Inventory
            .OrderBy(item => item.Type)
            .ThenBy(item => item.Value)
            .ThenBy(item => item.Name.Length)
            .ToList();

            Console.WriteLine($"{item.Name}을(를) 인벤토리에 추가했습니다.");
        }
    }
}
