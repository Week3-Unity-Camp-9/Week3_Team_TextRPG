using Newtonsoft.Json;
using System;
using System.Numerics;

namespace TextRPG_Week3
{
    static class TextRPG
    {
        static Character player = new Character();
        static ShopItem shop = new ShopItem();
        static void Main()
        {
            player.Inventory.AddRange(new List<Item>
                {
                new Item("무쇠갑옷", ItemType.Armor, 5, "무쇠로 만들어져 튼튼한 갑옷입니다.", true),
                new Item("스파르타의 창", ItemType.Weapon,7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", false),
                new Item("회복 포션", ItemType.Consumable, 30, "체력을 30 회복합니다", false, 3)
                });

            shop.InitShopItems();

            CharacterCustom custom = new CharacterCustom();
            custom.Customizing(player, false);

            Town();
        }

        static void Town()
        {
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
                        Status();
                        break;
                    case "2":
                        player.ShowInventory();
                        break;
                    case "3":
                        shop.OpenShop(player);
                        break;
                    case "4":
                        EnteringDungeon();
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

        static void Status()
        {
            CharacterCustom custom = new CharacterCustom();
            while (true)
            {
                Console.WriteLine("상태 보기");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");

                player.DisplayStatus();

                int input = GameSystem.Select(new string[] { "1.커스터마이징", "2.저장하기", "3.불러오기" });
                switch (input)
                {
                    case 0:
                        return;
                    case 1:
                        custom.Customizing(player, true);
                        break;
                    case 2:
                        Save();
                        break;
                    case 3:
                        Load();
                        break;
                    default:
                        continue;
                }
            }
        }
        static string[] SaveFileRead()
        {
            string[] options = new string[3];
            for (int i = 0; i < 3; i++)
            {
                string path = $"save{i + 1}.json";
                GameData gameData = null;
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    gameData = JsonConvert.DeserializeObject<GameData>(json);
                }

                if (gameData != null && gameData.Player != null)
                {
                    options[i] = $"{i + 1}번 슬롯 [{gameData.Player.Name}]";
                }
                else
                {
                    options[i] = $"{i + 1}번 슬롯 [비어있음]";
                }
            }
            return options;
        }

        static void Save()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("저장할 파일을 선택해 주세요.");
                string[] options = SaveFileRead();
                int selection = GameSystem.Select(options, question: "\n>>");
                Console.WriteLine("\n>>");
                if (selection == 0) return;
                else if (selection > 0 && selection <= options.Length)
                {
                    SaveManager.SaveGame(player, shop, selection);
                    Console.WriteLine($"{selection}파일에 {player.Name}의 정보를 저장했습니다.");
                    Console.ReadKey();
                }
            }
        }

        static void Load()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("불러올 파일을 선택해 주세요.");
                string[] options = SaveFileRead();
                int selection = GameSystem.Select(options, question: "\n>>");
                Console.WriteLine("\n>>");

                Character loadedPlayer = null;
                ShopItem loadedStore = null;
                if (selection > 0 && selection <= options.Length)
                {
                    string saveFilePath = $"save{selection}.json";

                    if (File.Exists(saveFilePath) && new FileInfo(saveFilePath).Length > 0)
                    {
                        (loadedPlayer, loadedStore) = SaveManager.LoadGame(selection);
                        player.Inventory.Clear();
                        shop.ItemList.Clear();

                        player = loadedPlayer;
                        shop = loadedStore;

                        Console.WriteLine($"{loadedPlayer.Name}의 정보를 불러왔습니다.");
                        Console.ReadKey();
                        return;
                    }
                    else
                    {
                        Console.WriteLine("선택한 슬롯은 저장된 데이터가 없습니다.");
                        continue;
                    }
                }
            }
        }

        static void EnteringDungeon()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
                Console.WriteLine("이제 전투를 시작할 수 있습니다.");

                int input = GameSystem.Select(new string[] { "1.상태 보기", $"2.전투 시작(현재 진행 : {BattleSystem.stage})", "3.회복 아이템" }, false);

                switch (input)
                {
                    case 1:
                        Status();
                        break;
                    case 2:
                        BattleSystem.Encounting(player);
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

                            int select = GameSystem.Select(new string[] { "1.사용하기" });
                            switch (select)
                            {
                                case 0:
                                    break;
                                case 1:
                                    if (healingPotion == null)
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
