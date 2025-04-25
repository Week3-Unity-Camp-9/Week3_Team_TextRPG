using Newtonsoft.Json;

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
                new Item("스파르타의 창", ItemType.Weapon,7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", false),
                new Item("무쇠갑옷", ItemType.Armor, 5, "무쇠로 만들어져 튼튼한 갑옷입니다.", true),
                new Item("회복 포션", ItemType.HealthPotion, 30, "체력을 30 회복합니다", false, 3, true),
                new Item("마나 포션", ItemType.ManaPotion, 30, "마나를 30 회복합니다", false, 3, true)
                });

            shop.InitShopItems();

            CharacterCustom custom = new CharacterCustom();
            custom.Customizing(player, false);

            Town();
        }
        //Main함수
        //플레이어의 기본 지급 아이템을 설정.
        //상점의 아이템 구성.
        //캐릭터의 이름, 직업을 정하는 함수를 호출.
        //이후 시작화면으로 넘어감

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
                        Console.ForegroundColor = ConsoleColor.DarkGreen; Console.Write("계속>>"); Console.ResetColor();
                        Console.ReadKey();
                        break;
                }
            }
        }
        //Town함수
        //마을에서 할 수 있는 행동을 출력한 뒤,
        //값을 입력받음
        //1번 = 상태보기 함수 호출
        //2번 = 플레이어 인벤토리표시 함수
        //3번 = 상점 함수에 플레이어 정보를 넘겨주고 호출
        //4번 = 퀘스트 열람 함수 호출
        //5번 = 던전 입장 함수 호출
        //0번 = 종료

        static void Status(bool fromTown = true)
        {
            CharacterCustom custom = new CharacterCustom();
            while (true)
            {
                player.DisplayStatus();
                if (!fromTown)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("던전에서는 기능을 사용할 수 없습니다.");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.ForegroundColor = ConsoleColor.DarkGreen; Console.Write("계속>>"); Console.ResetColor();
                    Console.ReadKey();
                    break;
                }
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
        //Status함수 플레이어의 정보를 표시하는 함수를 호출하고
        //값을 입력받는다.
        //입력된 값.
        //1번 = 커스터마이징
        //2번 = 저장
        //3번 = 불러오기
        //0번 = 나가기

        static string[] SaveFileRead()
        {
            string[] options = new string[3];
            for (int i = 0; i < 3; i++)
            {
                string path = $"save{i + 1}.json";
                GameData gameData = null;
                if (File.Exists(path))
                {
                    try
                    {
                        string json = File.ReadAllText(path);
                        gameData = JsonConvert.DeserializeObject<GameData>(json);
                    }
                    catch
                    {
                        options[i] = $"{i + 1}번 슬롯 [불러오기 실패!]";
                        gameData = null;
                        continue;
                    }
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

                if (selection == 0) return;
                else if (selection > 0 && selection <= options.Length)
                {
                    SaveManager.SaveGame(player, shop, QuestManager.Quests, selection);
                    Console.WriteLine($"{selection}파일에 {player.Name}의 정보를 저장했습니다.");
                    Console.ForegroundColor = ConsoleColor.DarkGreen; Console.Write("계속>>"); Console.ResetColor();
                    Console.ReadKey();
                }
            }
        }
        static void Load()
<<<<<<< HEAD
        //SaveFileRead함수

        //Save함수

        //Load함수
        //게임 플레이 데이터를 저장하고 불러들이는데 사용된 함수이다.
=======
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("불러올 파일을 선택해 주세요.");

                string[] options = SaveFileRead();
                int selection = GameSystem.Select(options, question: "\n>>");

                Character loadedPlayer = null;
                ShopItem loadedStore = null;
                List<Quest> loadedQuests = null;

                if (selection == 0)
                {
                    return;
                }
                else if (selection > 0 && selection <= options.Length)
                {
                    string saveFilePath = $"save{selection}.json";

                    if (File.Exists(saveFilePath) && new FileInfo(saveFilePath).Length > 0)
                    {
                        (loadedPlayer, loadedStore, loadedQuests) = SaveManager.LoadGame(selection);
                        if (loadedPlayer == null || loadedStore == null || loadedQuests == null)
                        {
                            Console.WriteLine("\n불러오기에 실패했습니다.");
                            Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("계속>>"); Console.ResetColor();
                            Console.ReadKey();
                            continue;
                        }

                        player.Inventory.Clear();
                        shop.ItemList.Clear();
                        QuestManager.Quests.Clear();

                        player = loadedPlayer;
                        shop = loadedStore;
                        QuestManager.Quests = loadedQuests;

                        Console.WriteLine($"{loadedPlayer.Name}의 정보를 불러왔습니다.");
                        Console.ForegroundColor = ConsoleColor.DarkGreen; Console.Write("계속>>"); Console.ResetColor();
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
>>>>>>> 6d3085d7e3e9886d7c9dbda1b9d956d4ab3e826f

        static void OpenQuest(bool fromTown = true)
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
                    options[i] = $"{i + 1}.{quest[i].QuestName} : {(quest[i].IsAccept ? (quest[i].IsClear ? "달성!" : "진행중") : "수주가능")}";
                }

                int input = GameSystem.Select(options, question: "열람하고 싶은 퀘스트를 선택해 주세요\n>>");
                if (input == 0) return;
                else if (input > 0 && input <= quest.Count) SelectQuest(input - 1, fromTown);
            }
        }
        //OpenQuest함수
        //퀘스트 목록을 불러와서 표시한다.

        static void SelectQuest(int input, bool fromTown)
        {
            Quest quest = QuestManager.Quests[input];

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("퀘스트 열람\n");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{quest.QuestDescription}");
                Console.Write($"달성 상황 : ");

                switch (quest.IsClear)
                {
                    //퀘스트가 달성 상태일때
                    case true:
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("달성\n");
                        if (!fromTown)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("보상 수령은 마을에서만 가능합니다.");
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.ForegroundColor = ConsoleColor.DarkGreen; Console.Write("계속>>"); Console.ResetColor();
                            Console.ReadKey();
                            break;
                        }

                        int selection = GameSystem.Select(new string[] { "1.보상 받기" });
                        switch (selection)
                        {
                            case 0:
                                break;
                            case 1:
                                quest.IsClear = false;
                                quest.IsAccept = false;
                                player.Gold += quest.Reword;
                                quest.ClearCount++;
                                switch (quest)
                                {
                                    case DefeatQuest defeatQuest:
                                        defeatQuest.DefeatCount = 0;
                                        break;
                                    case DungeonQuest dungeonQuest:
                                        dungeonQuest.ReachedStage = 0;
                                        break;
                                }
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine($"퀘스트 달성! 보상으로 {quest.Reword} Gold를 받았습니다!");
                                Console.ForegroundColor = ConsoleColor.DarkGreen; Console.Write("계속>>"); Console.ResetColor();
                                Console.ReadKey();
                                continue;
                            default:
                                continue;
                        }
                        break;
                    //퀘스트가 달성 상태가 아닐때
                    case false:
                        switch (quest.IsAccept)
                        {
                            //퀘스트가 진행중 일때
                            case true:
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write("진행중\n진행률 : ");
                                switch (quest)
                                {
                                    //처치 퀘스트일때
                                    case DefeatQuest defeatQuest:
                                        Console.WriteLine($"{defeatQuest.DefeatCount}/{defeatQuest.RequiredDefeatCount} 마리");
                                        break;
                                    //층 도달 퀘스트일때
                                    case DungeonQuest dungeonQuest:
                                        Console.WriteLine($"{dungeonQuest.ReachedStage}/{dungeonQuest.RequiredStage} 층");
                                        break;
                                }
                                int select = GameSystem.Select();
                                if (select == 0) break;
                                continue;
                            //퀘스트가 진행중이 아닐때
                            case false:
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.WriteLine("수주가능\n");
                                if (!fromTown)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("퀘스트 수주는 마을에서만 가능합니다.");
                                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                                    Console.ForegroundColor = ConsoleColor.DarkGreen; Console.Write("계속>>"); Console.ResetColor();
                                    Console.ReadKey();
                                    break;
                                }

                                int select1 = GameSystem.Select(new string[] { "1.퀘스트 수주" });
                                if (select1 == 0) break;
                                else if(select1 == 1)
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine($"{quest.QuestName}을(를) 수주했습니다.");

                                    quest.IsAccept = true;
                                    Console.ForegroundColor = ConsoleColor.DarkGreen; Console.Write("계속>>"); Console.ResetColor();
                                    Console.ReadKey();
                                    continue;
                                }
                                continue;
                        }
                        break;
                }
                break;
            }
        }
        //SelectQuest함수
        //퀘스트를 관리하는 함수이다.
        
        //퀘스트 달성시
        //-달성여부 표시
        //-달성할 경우 보상 받기
        
        //퀘스트 미달성시
        //-퀘스트를 수주했을 시
        //--진행중임을 표시하고 진행률을 표시
        
        //-퀘스트를 수주하지 않았을 시
        //--수주가능함을 표시하고 퀘스트를 수주한다.        

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

                int input = GameSystem.Select(new string[] { "1.상태 보기", $"2.전투 시작(현재 진행 : {BattleSystem.stage})", "3.회복 아이템", "4.퀘스트 열람" }, false);

                switch (input)
                {
                    case 1:
                        Status(false);
                        break;
                    case 2:
                        BattleSystem.Encounting(player);
                        if (BattleSystem.lose) return;
                        break;
                    case 3:
                        while (true)
                        {
                            Item healingPotion = player.Inventory.FirstOrDefault(item => item.Type == ItemType.HealthPotion);
                            int healingPotionCount = 0;
                            if (healingPotion != null) healingPotionCount = healingPotion.Count;
                            Item manaPotion = player.Inventory.FirstOrDefault(item => item.Type == ItemType.ManaPotion);
                            int manaPotionCount = 0;
                            if (manaPotion != null) manaPotionCount = manaPotion.Count;

                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("회복");
                            Console.ResetColor();
                            Console.WriteLine($"포션을 사용하면 체력이나 마나를 30 회복 할 수 있습니다. (회복 포션 : {healingPotionCount}개, 마나 포션 : {manaPotionCount}개)");
                            Console.WriteLine($"Hp : {player.Hp}/{player.MaxHp}\nMp : {player.Mp}/{player.MaxMp}\n");

                            int select = GameSystem.Select(new string[] { "1.회복 포션 사용", "2.마나 포션 사용" });
                            switch (select)
                            {
                                case 0:
                                    break;
                                case 1:
                                    if (healingPotion == null)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("포션이 부족합니다.");
                                        Console.ForegroundColor = ConsoleColor.DarkGreen; Console.Write("계속>>"); Console.ResetColor();
                                        Console.ReadKey();
                                        continue;
                                    }
                                    player.UseConsumableItem(healingPotion);
                                    continue;
                                case 2:
                                    if (manaPotion == null)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("포션이 부족합니다.");
                                        Console.ForegroundColor = ConsoleColor.DarkGreen; Console.Write("계속>>"); Console.ResetColor();
                                        Console.ReadKey();
                                        continue;
                                    }
                                    player.UseConsumableItem(manaPotion);
                                    continue;
                                default:
                                    continue;
                            }
                            break;
                        }
                        break;
                    case 4:
                        OpenQuest(false);
                        break;
                    default:
                        break;
                }
            }
        }
        //EnteringDungeon함수
        //던전의 진행상황을 나타내는 함수이다.
        //던전 진행 중 플레이어의 상태를 확인하거나 전투 전 부족한 체력을 채우는 등의 일을 진행한다.
        
        //1번 = 상태보기(fromTown: false) = 기능 사용 불가
        //2번 = 전투 시작
        //3번 = 회복 아이템
        //4번 = 퀘스트 열람(fromTown: false) = 기능 사용 불가
        
        //회복 아이템 기능
        //회복 포션과 마나 포션이 인벤토리에 있을 경우 불러와서 저장
        //플레이어의 체력과 마나의 현상태를 표시하고
        //값을 입력받는다.
        //1번 = 회복 포션 사용
        //회복포션이 없으면 포션이 부족하다 알리고 돌아감
        //회폭표션이 있다면 회복포션을 매개변수로 소모품사용 함수 호출
        //2번 = 마나 포션 사용
        //마나포션이 없다면 포션이 부족하다 알리고 돌아감
        //마나포션이 있다면 마나포션을 매개변수로 소모품사용 함수 호출
    }
}
