using System;

namespace TextRPG
{
    class Program
    {
        static void Main(string[] args)
        {
            Character player = new Character();

            // 초기 아이템 (테스트용)
            player.AddItem(new Item
            {
                Name = "무쇠갑옷",
                Type = ItemType.Armor,
                Value = 5,
                Description = "무쇠로 만들어져 튼튼한 갑옷입니다.",
                IsEquipped = true
            });
            player.AddItem(new Item
            {
                Name = "스파르타의 창",
                Type = ItemType.Weapon,
                Value = 7,
                Description = "스파르타의 전사들이 사용했다는 전설의 창입니다.",
                IsEquipped = true
            });

            ShopItem.InitShopItems(); // 상점 목록 초기화

            // 메인 루프
            while (true)
            {
                Console.Clear();
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n");

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("0. 게임 종료");
                Console.ResetColor();

                Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        player.DisplayStatus();
                        Console.ReadKey();
                        break;
                    case "2":
                        player.ShowInventory();
                        break;
                    case "3":
                        ShopItem.OpenShop(player);
                        break;
                    case "0":
                        Console.WriteLine("게임을 종료합니다.");
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("잘못된 입력입니다.");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
