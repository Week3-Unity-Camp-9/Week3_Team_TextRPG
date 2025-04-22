using System;
using System.Collections.Generic;

namespace TextRPG_Week3
{
    public class ShopItem
    {
        public static List<ShopItem> ItemList = new List<ShopItem>();

        public Item ItemData { get; set; }
        public int Price { get; set; }
        public bool IsPurchased { get; set; } = false;

        public string GetDisplayInfo(int index)
        {
            string statText = ItemData.Type switch
            {
                ItemType.Weapon => $"공격력 +{ItemData.Value}",
                ItemType.Armor => $"방어력 +{ItemData.Value}",
                ItemType.Consumable => $"체력 회복 +{ItemData.Value}",
                _ => ""
            };

            string rightText = IsPurchased && ItemData.Type != ItemType.Consumable
                ? "구매완료"
                : $"{Price} G";

            return $"- {index + 1}. {ItemData.Name,-12} | {statText,-14} | {ItemData.Description,-30} | {rightText}";
        }

        public static void InitShopItems()
        {
            ItemList = new List<ShopItem>
            {
                new ShopItem
                {
                    ItemData = new Item
                    (
                        "낡은 검",
                        ItemType.Weapon,
                        5,
                        "오래된 검이지만 쓸만함"
                    ),
                    Price = 300
                },
                new ShopItem
                {
                    ItemData = new Item
                    (
                        "가죽 갑옷",
                        ItemType.Armor,
                        3,
                        "얇은 가죽으로 만들어진 방어구"
                    ),
                    Price = 250
                },
                new ShopItem
                {
                    ItemData = new Item
                    (
                        "회복 포션",
                        ItemType.Consumable,
                        30,
                        "체력을 30 회복합니다"
                    ),
                    Price = 100
                }
            };
        }

        public static void OpenShop(Character player)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("상점");
                Console.ResetColor();

                Console.WriteLine($"보유 골드: {player.Gold} G\n");

                for (int i = 0; i < ItemList.Count; i++)
                {
                    Console.WriteLine(ItemList[i].GetDisplayInfo(i));
                }

                Console.WriteLine("\n아이템 번호를 입력하여 구매하세요. (0: 나가기)");
                Console.Write(">> ");
                string input = Console.ReadLine();

                if (input == "0") break;

                if (int.TryParse(input, out int selected) && selected >= 1 && selected <= ItemList.Count)
                {
                    Buy(player, ItemList[selected - 1]);
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.ReadKey();
                }
            }
        }

        private static void Buy(Character player, ShopItem item)
        {
            if (item.IsPurchased && item.ItemData.Type != ItemType.Consumable)
            {
                Console.WriteLine("이미 구매한 아이템입니다.");
                Console.ReadKey();
                return;
            }

            if (player.Gold < item.Price)
            {
                Console.WriteLine("골드가 부족합니다.");
                Console.ReadKey();
                return;
            }

            player.Gold -= item.Price;
            player.Inventory.Add(new Item
                (
                item.ItemData.Name,
                item.ItemData.Type,
                item.ItemData.Value,
                item.ItemData.Description
                ));

            if (item.ItemData.Type != ItemType.Consumable)
                item.IsPurchased = true;

            Console.WriteLine($"{item.ItemData.Name}을(를) 구매했습니다!");
            Console.ReadKey();
        }
    }
}
