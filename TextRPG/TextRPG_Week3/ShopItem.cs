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
                // 무기
                new ShopItem
                {
                    ItemData = new Item("낡은 검", ItemType.Weapon, 5, "오래된 검이지만 쓸만함"),
                    Price = 300
                },
                new ShopItem
                {
                    ItemData = new Item("검", ItemType.Weapon, 7, "잘 제련된 양산형 검"),
                    Price = 500
                },
                new ShopItem
                {
                    ItemData = new Item("강철 검", ItemType.Weapon, 10, "강철로 제련한 튼튼한 검"),
                    Price = 700
                },
                new ShopItem
                {
                    ItemData = new Item("낡은 스파르타의 창", ItemType.Weapon, 20, "오래된 스파르타의 창"),
                    Price = 1000
                },
                new ShopItem
                {
                    ItemData = new Item("스파르타의 창", ItemType.Weapon, 30, "강인한 스파르타 인들의 무기"),
                    Price = 1500
                },
                new ShopItem
                {
                    ItemData = new Item("스파르타의 혼", ItemType.Weapon, 2500, "고대 스파르타 영웅들의 혼이 담긴 에고웨폰"),
                    Price = 10000
                },

                // 방어구
                new ShopItem
                {
                    ItemData = new Item("가죽 갑옷", ItemType.Armor, 3, "얇은 가죽으로 만들어진 기본 방어구입니다."),
                    Price = 250
                },
                new ShopItem
                {
                    ItemData = new Item("강화 가죽 갑옷", ItemType.Armor, 5, "추가 패딩으로 보호력이 향상된 가죽 방어구입니다."),
                    Price = 500
                },
                new ShopItem
                {
                    ItemData = new Item("청동 갑옷", ItemType.Armor, 7, "기초 금속으로 제작된 무난한 방어구입니다."),
                    Price = 700
                },
                new ShopItem
                {
                    ItemData = new Item("무쇠 갑옷", ItemType.Armor, 10, "무쇠로 제작되어 튼튼하고 신뢰할 수 있는 방어구입니다."),
                    Price = 1000
                },
                new ShopItem
                {
                    ItemData = new Item("사슬 갑옷", ItemType.Armor, 15, "금속 사슬로 엮어 찌르기에 강한 방어구입니다."),
                    Price = 1300
                },
                new ShopItem
                {
                    ItemData = new Item("강철 갑옷", ItemType.Armor, 20, "강철로 만들어진 중갑. 뛰어난 내구성을 자랑합니다."),
                    Price = 1700
                },
                new ShopItem
                {
                    ItemData = new Item("중갑 기사단 갑옷", ItemType.Armor, 30, "중갑 기사단이 착용하던 신뢰도 높은 갑옷입니다."),
                    Price = 2000
                },
                new ShopItem
                {
                    ItemData = new Item("마법 금속 갑옷", ItemType.Armor, 40, "마법이 부여된 금속 방어구로 물리와 마법을 함께 방어합니다."),
                    Price = 2500
                },
                new ShopItem
                {
                    ItemData = new Item("용비늘 갑옷", ItemType.Armor, 50, "전설 속 용의 비늘을 정제해 만든 강력한 방어구입니다."),
                    Price = 3000
                },
                new ShopItem
                {
                    ItemData = new Item("스파르타 갑옷", ItemType.Armor, 2500, "스파르타 최정예 전사들이 착용했던 전설적인 갑옷입니다."),
                    Price = 10000
                },

                // 소비 아이템
                new ShopItem
                {
                    ItemData = new Item("회복 포션", ItemType.Consumable, 30, "체력을 30 회복합니다"),
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
            player.Inventory.Add(new Item(
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
