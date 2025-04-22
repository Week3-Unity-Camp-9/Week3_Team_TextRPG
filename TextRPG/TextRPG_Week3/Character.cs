using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_Week3
{
    public class Character
    {
        public List<Item> Inventory { get; set; } = new List<Item>();

        public int Level { get; set; } = 1;
        public string Name { get; set; } = "Chad";
        public string Job { get; set; } = "전사";
        public int Attack { get; set; } = 10;
        public int Defense { get; set; } = 5;
        public int Hp { get; set; } = 100;
        public int MaxHp { get; set; } = 100;
        public int Gold { get; set; } = 1500;

        public float TotalAttack => Attack + Inventory.Where(item => item.IsEquipped && item.Type == ItemType.Weapon).Sum(i => i.Value);

        public int TotalDefense => Defense + Inventory.Where(item => item.IsEquipped && item.Type == ItemType.Armor).Sum(i => i.Value);
        // ============================
        // 1. 상태 보기
        // ============================
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

            Console.WriteLine($"Lv. 0{Level}");
            Console.WriteLine($"{Name} ( {Job} )");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"공격력 : {TotalAttack}");
            if (bonusAttack > 0) Console.Write($" (+{bonusAttack})");
            Console.WriteLine();

            Console.Write($"방어력 : {TotalDefense}");
            if (bonusDefense > 0) Console.Write($" (+{bonusDefense})");
            Console.WriteLine();

            Console.WriteLine($"체 력 : {Hp}/{MaxHp}");
            Console.WriteLine($"Gold   : {Gold} G");
            Console.ResetColor();
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
                    Console.WriteLine("보유 중인 아이템이 없습니다.");
                }
                else
                {
                    for (int i = 0; i < Inventory.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {Inventory[i].GetDisplayInfo()}");
                    }

                    Console.WriteLine("\n아이템 번호를 선택하면 장착/해제하거나 포션을 사용할 수 있습니다.");
                    Console.WriteLine("\n0. 나가기");
                    Console.Write(">> ");
                    string input = Console.ReadLine();
                    if (int.TryParse(input, out int selected))
                    {
                        if (selected == 0) return;
                        UseConsumableItem(selected - 1);  // 회복 아이템 먼저 검사
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Console.ReadKey();
                    }
                }
            }
        }

        // ============================
        // 3. 회복 아이템 사용
        // ============================
        public void UseConsumableItem(int index)
        {
            if (index < 0 || index >= Inventory.Count)
            {
                Console.WriteLine("잘못된 입력입니다.");
                Console.ReadKey();
                return;
            }

            Item item = Inventory[index];

            if (item.Type == ItemType.Consumable)
            {
                Hp += item.Value;
                if (Hp > MaxHp) Hp = MaxHp;

                Console.WriteLine($"{item.Name}을(를) 사용하여 체력을 {item.Value} 회복했습니다.");
                Inventory.RemoveAt(index);
                Console.ReadKey();
            }
            else
            {
                ToggleEquipItem(index);  // 무기/방어구는 장착 토글
            }
        }

        // ============================
        // 4. 상점 구매용 아이템 추가
        // ============================
        public void AddItem(Item item)
        {
            Inventory.Add(item);
            Console.WriteLine($"{item.Name}을(를) 인벤토리에 추가했습니다.");
        }
    }
}
