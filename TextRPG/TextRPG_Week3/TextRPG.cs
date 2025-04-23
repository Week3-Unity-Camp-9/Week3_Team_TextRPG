using System;
using System.Numerics;

namespace TextRPG_Week3
{
    internal class TextRPG
    {
        static void Main()
        {
            GameSystem gameSystem = new GameSystem();
            BattleSystem battleSystem = new BattleSystem();
            Character player = new Character();
            player.Inventory.AddRange(new List<Item>
                {
                new Item("무쇠갑옷", ItemType.Armor, 5, "무쇠로 만들어져 튼튼한 갑옷입니다.", true),
                new Item("스파르타의 창", ItemType.Weapon,7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", false),
                new Item("회복 포션", ItemType.Consumable, 30, "체력을 30 회복합니다", false, 3)
                });

            ShopItem.InitShopItems();

            CharacterCustom custom = new CharacterCustom();
            custom.Customizing(player, false);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n");

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("4. 전투 시작");
                Console.WriteLine("0. 게임 종료");
                Console.ResetColor();

                Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Status(gameSystem, player);
                        break;
                    case "2":
                        player.ShowInventory();
                        break;
                    case "3":
                        ShopItem.OpenShop(player);
                        break;
                    case "4":
                        EnteringDungeon(gameSystem, battleSystem,player);
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

        static void Status(GameSystem gameSystem, Character player)
        {
            CharacterCustom custom = new CharacterCustom();
            while (true)
            {
                Console.WriteLine("상태 보기");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");

                player.DisplayStatus();

                int input = gameSystem.Select(new string[] {"1.커스터마이징" }, true);
                switch(input)
                {
                    case 1:
                        custom.Customizing(player, true);
                        break;
                    case 0:
                        return;
                    default:
                        continue;
                }
            }
        }

        static void EnteringDungeon(GameSystem gameSystem, BattleSystem battleSystem,Character player)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
                Console.WriteLine("이제 전투를 시작할 수 있습니다.");

                int input = gameSystem.Select(new string[] { "1.상태 보기", $"2.전투 시작(현재 진행 : {BattleSystem.stage})", "3.회복 아이템" }, false);
                switch (input)
                {
                    case 1:
                        Status(gameSystem, player);
                        break;
                    case 2:
                        battleSystem.Encounting(player);
                        continue;
                    case 3:
                        while (true)
                        {
                            Item healingPotion = player.Inventory.FirstOrDefault(item => item.Name == "회복 포션");
                            int healingPotionCount = 0;
                            if (healingPotion != null)
                            {
                                healingPotionCount = healingPotion.Count;
                            }

                            Console.Clear();
                            Console.WriteLine("회복");
                            Console.WriteLine($"포션을 사용하면 체력을 30 회복 할 수 있습니다. (남은 포션 : {healingPotionCount})\n");

                            int select = gameSystem.Select(new string[] { "1.사용하기" }, true);
                            switch (select)
                            {
                                case 0:
                                    break;
                                case 1:
                                    if(healingPotion == null)
                                    {
                                        Console.WriteLine("포션이 부족합니다.");
                                        Console.ReadKey();
                                        continue;
                                    }

                                    healingPotion.Count--;
                                    if (healingPotion.Count == 0) player.Inventory.Remove(healingPotion);

                                    int originalHp = player.Hp;
                                    player.Hp += healingPotion.Value;
                                    if (player.Hp > player.MaxHp) player.Hp = player.MaxHp;

                                    Console.WriteLine($"{healingPotion.Name}을(를) 사용하여 체력을 {healingPotion.Value} 회복했습니다.");
                                    Console.WriteLine($"HP {originalHp} => {player.Hp}");
                                    Console.ReadKey();
                                    continue;
                                default:
                                    continue;
                            }
                            break;
                        }
                        break;
                    default:
                        continue;
                }
            }
        }
    }
}
