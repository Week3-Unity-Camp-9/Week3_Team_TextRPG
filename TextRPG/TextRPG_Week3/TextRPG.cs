using Newtonsoft.Json;

namespace TextRPG_Week3
{
    static class TextRPG
    {
        static Character player = new Character();
        static ShopItem shop = new ShopItem();
        /*7~9번줄
        캐릭터클래스 player, 상점아이템클래스 shop, 퀘스트클래스quests
        객체를 클래스 전역 변수로 불러온다
        */

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
        /*Main함수
        플레이어의 기본 지금 아이템을 추가해 주고,
        상점의 아이템들을 구성하는 함수를 호출해주고,
        캐릭터의 이름,직업을 정하는 함수를 호출해
        시작화면으로 넘어간다.
        */

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
        /*Town함수
        마을에서 할 수 있는 행동을 출력한 뒤,
        값을 입력받는다.
        입력된 값이
        1번 = 상태보기 함수 호출
        2번 = 플레이어 인벤토리표시 함수
        3번 = 상점 함수에 플레이어 정보를 넘겨주고 호출
        4번 = 퀘스트 열람 함수 호출
        5번 = 던전 입장 함수 호출
        0번 = 종료
        */

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
        /*Status함수 (fromTown이라는 불값을 매개변수로 받기(기본값 true))
        플레이어의 정보를 표시하는 함수를 호출하고
        값을 입력받는다.
        입력된 값이
        1번 = 커스터마이징
        2번 = 저장
        3번 = 불러오기
        0번 = 나가기
        이때 fromTown매개변수가 true라면 기능 사용 불가
        */

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
        /*SaveFileRead함수
        Save와 Load에서 호출되는 함수로
        save{i+1}.json 형식의 파일이 있는지 찾아서
        파일이 있을 경우 슬롯과 저장된 플레이어 이름을 표시
        파일이 없을 경우 슬롯과 비어있음을 표시
        
        */

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
        /*Save함수
        SaveFileRead함수를 불러와 저장 슬롯을 표시하고
        값을 입력받는다.
        0을 입력하면 나간다
        저장할 슬롯은 1~3까지만 가능하고 입력받은 값이 조건에 맞으면
        입력된 값의 슬롯에 정보를 저장한다.
        */

        static void Load()
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
        /*Load함수
        SaveFileRead함수를 불러와 저장 슬롯을 표시하고
        값을 입력받는다.
        0을 입력하면 나간다.
        저장된 슬롯은 1~3까지만 가능하고 입력받은 값이 조건에 맞으면
        입력된 값의 슬롯의 정보를 불러온다.
        다만 저장파일이 비어있을때 불러오면 데이터가 없다고 경고하고 불러오지 않는다.
        */

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
        /*OpenQuest함수 (fromTown이란 불값을 매개변수로 받기(기본값 true))
        퀘스트 목록을 불러와서 표시한다.
        값을 입력받는다.
        0을 입력하면 나간다.
        입력받은 값이 퀘스트 목록의 값과 일치하면 
        SelectQuest함수에 입력받은 번호와 매개변수 fromTown을 전달하여 호출한다.
        
        */

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
        /*SelectQuest함수(정수 input과 불값 fromTown을 매개변수로 받는다.)
        퀘스트 목록을 불러오고
        input값을 1 뺀다(목록의 저장 순서는 0부터 이기 때문에)
        퀘스트 목록의 input 순서의 퀘스트 설명을 표시하고
        
        퀘스트 달성시
        -달성여부 표시
        -fromTown이 false일 경우 기능사용 불가(자동으로 나가기)
        -값을 입력받는다.
        -0번 = 나가기
        -1번 = 보상 받기
        
        퀘스트 미달성시
        -퀘스트를 수주했을 시
        --진행중임을 표시하고 진행률을 표시
        --(퇴치 퀘스트일 경우 몇마리를 잡았는지)
        --(던전 도달 퀘스트일 경우 몇층까지 도달했는지)
        --값을 입력받는다.
        --값이 0이면 나가고 아니면 반복표시
        
        
        -퀘스트를 수주하지 않았을 시
        --수주가능함을 표시하고
        --formTown이 false일 경우 기능 사용 불가
        --값을 입력받는다.
        --0번 = 나가기
        --1번 = 퀘스트 수주
        */

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
        /*EnteringDungeon함수
        값을 입력받는다.(나가기 없음)
        1번 = 상태보기(fromTown: false) = 기능 사용 불가
        2번 = 전투 시작
        3번 = 회복 아이템
        4번 = 퀘스트 열람(fromTown: false) = 기능 사용 불가
        
        회복 아이템 기능
        회복 포션과 마나 포션이 인벤토리에 있을 경우 불러와서 저장
        플레이어의 체력과 마나의 현상태를 표시하고
        값을 입력받는다.
        1번 = 회복 포션 사용
        회복포션이 없으면 포션이 부족하다 알리고 돌아감
        회폭표션이 있다면 회복포션을 매개변수로 소모품사용 함수 호출
        2번 = 마나 포션 사용
        마나포션이 없다면 포션이 부족하다 알리고 돌아감
        마나포션이 있다면 마나포션을 매개변수로 소모품사용 함수 호출
        */
    }
}
