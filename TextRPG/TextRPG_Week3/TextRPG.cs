using Newtonsoft.Json;
using System;
using System.Numerics;

namespace TextRPG_Week3
{
    static class TextRPG
    {
        static Character player = new Character();
        static ShopItem shop = new ShopItem();
        static List<Quest> quests = QuestManager.Quests;
        static void Main()
        {
            player.Inventory.AddRange(new List<Item>
                {
                new Item("스파르타의 창", ItemType.Weapon,7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", false),
                new Item("무쇠갑옷", ItemType.Armor, 5, "무쇠로 만들어져 튼튼한 갑옷입니다.", true),
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
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("마을");
                Console.ResetColor();
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("4. 퀘스트 열람");
                Console.WriteLine("5. 던전 입장");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("0. 게임 종료");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
                Console.ResetColor();
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
                        OpenQuest();
                        break;
                    case "5":
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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("저장할 파일을 선택해 주세요.");
                string[] options = SaveFileRead();
                int selection = GameSystem.Select(options, question: "\n>>");
                Console.WriteLine("\n>>");
                if (selection == 0) return;
                else if (selection > 0 && selection <= options.Length)
                {
                    SaveManager.SaveGame(player, shop, quests, selection);
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
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("불러올 파일을 선택해 주세요.");
                string[] options = SaveFileRead();
                int selection = GameSystem.Select(options, question: "\n>>");
                Console.WriteLine("\n>>");

                Character loadedPlayer = null;
                ShopItem loadedStore = null;
                List<Quest> loadedQuests = null;
                if(selection == 0)
                {
                    return;
                }
                else if (selection > 0 && selection <= options.Length)
                {
                    string saveFilePath = $"save{selection}.json";

                    if (File.Exists(saveFilePath) && new FileInfo(saveFilePath).Length > 0)
                    {
                        (loadedPlayer, loadedStore, loadedQuests) = SaveManager.LoadGame(selection);
                        player.Inventory.Clear();
                        shop.ItemList.Clear();
                        QuestManager.Quests.Clear();

                        player = loadedPlayer;
                        shop = loadedStore;
                        quests = loadedQuests;

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

        static void OpenQuest()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("퀘스트 열람");
                Console.ResetColor();
                Console.WriteLine("퀘스트 목록에서 조건과 달성여부를 확인할 수 있습니다.\n");

                List<Quest> quest = QuestManager.Quests;
                string[] options = new string[quest.Count];

                for (int i = 0; i < quest.Count; i++)
                {
                    Console.ResetColor();
                    if (quest[i].IsClear) Console.ForegroundColor = ConsoleColor.DarkYellow;
                    options[i] = $"{i + 1}. {quest[i].QuestName}";
                }

                int input = GameSystem.Select(options, question: "열람하고 싶은 퀘스트를 선택해 주세요\n>>");

                if (input == 0) return;
                else if (input > 0 && input <= quest.Count)
                {
                    input--;
                    while (true)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("퀘스트 열람\n");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"{quest[input].QuestDescription}");
                        Console.Write($"달성 상황 : ");
                        Console.ResetColor();

                        if (quest[input].IsClear)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("달성\n");

                            int selection = GameSystem.Select(new string[] { "1.보상 받기" });
                            switch (selection)
                            {
                                case 0:
                                    break;
                                case 1:
                                    quest[input].IsClear = false;
                                    player.Gold += quest[input].Reword;
                                    quest[input].ClearCount++;
                                    Console.WriteLine($"퀘스트 달성! 보상으로 {quest[input].Reword} Gold를 받았습니다!");
                                    Console.ReadKey();
                                    break;
                                default:
                                    continue;
                            }
                            break;
                        }
                        else
                        {
                            Console.ResetColor();
                            Console.WriteLine("미달성\n");
                            int selection = GameSystem.Select();
                            if (selection == 0) break;
                        }
                    }
                }
            }
        }

        static void EnteringDungeon()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("던전 입장");
                Console.ResetColor();
                Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
                Console.WriteLine("이제 전투를 시작할 수 있습니다.\n");

                int input = GameSystem.Select(new string[] { "1.상태 보기", $"2.전투 시작(현재 진행 : {BattleSystem.stage})", "3.회복 아이템" }, false);

                switch (input)
                {
                    case 1:
                        Status();
                        break;
                    case 2:
                        BattleSystem.Encounting(player);
                        if (BattleSystem.lose) return;
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
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("회복");
                            Console.ResetColor();
                            Console.WriteLine($"포션을 사용하면 체력을 30 회복 할 수 있습니다. (남은 포션 : {healingPotionCount})\n");

                            int select = GameSystem.Select(new string[] { "1.사용하기" });
                            switch (select)
                            {
                                case 0:
                                    break;
                                case 1:
                                    if (healingPotion == null)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("포션이 부족합니다.");
                                        Console.ReadKey();
                                        continue;
                                    }
                                    player.UseConsumableItem(healingPotion);
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
