using Newtonsoft.Json;

namespace TextRPG_Week3
{
    public class GameData
    {
        public Character Player { get; set; }
        public ShopItem Shop { get; set; }
        public List<Quest> Quests { get; set; }
        public GameData() { }
        public GameData(Character player, ShopItem shop, List<Quest> quests)
        {
            Player = player;
            Shop = shop;
            Quests = quests;
        }
    }
    /*public class GameData
    캐릭터, 상점아이템, 퀘스트리스트를 묶는 클래스
    
    */

    public static class SaveManager
    {
        public static void SaveGame(Character player, ShopItem shop, List<Quest> quests, int slot)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            };
            string saveFilePath = $"save{slot}.json";
            GameData gameData = new GameData(player, shop, quests);
            string json = JsonConvert.SerializeObject(gameData, settings);
            File.WriteAllText(saveFilePath, json);
        }
        /*SaveGame함수(캐릭터클래스, 상점아이템클래스, 퀘스트리스트, 정수값을 매개변수로 받는다.)
        받은 정수 slot을 통해 세이브파일의 이름을 정하고
        새로운 게임데이터 클래스에 받아온 캐릭터, 상정아이템, 퀘스트리스트 매개변수들을 넣어 참조한다.
        Json변환(직렬화) 함수를 활용해서 게임데이터를 문자열로 변환하고
        세이브파일 이름에 변환된 문자열을 넣어 파일을 생성한다.
        */

        public static (Character, ShopItem, List<Quest>) LoadGame(int slot)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            };
            string saveFilePath = $"save{slot}.json";
            try
            {
                string json = File.ReadAllText(saveFilePath);
                GameData gameData = JsonConvert.DeserializeObject<GameData>(json, settings);
                return (gameData.Player, gameData.Shop, gameData.Quests);
            }
            catch
            {
                return (null, null, null);
            }
        }
        /*LoadGame함수(캐릭터, 상점아이템, 퀘스트리스트 클래스를 반환한다) (정수값을 매개변수로 받는다.)
        받은 정수 slot값을 통해 세이브파일 이름을 정하고
        세이브파일에서 문자열을 불러온다.
        Json변환(역직렬화) 함수를 활용하여 불러온 문자열을 게임데이터 형식에 맞게 변환
        불러온 값들을 각각 형태에 맞게 반환한다.
        */
    }

    public static class GameSystem
    {
        public static int Select(string[]? options = null, bool hasExit = true, string zeroSelection = "0.나가기", string question = "원하시는 행동을 입력해 주세요.\n>>")
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (options != null)
            {
                for (int i = 0; i < options.Length; i++)
                {
                    Console.WriteLine($"{options[i]}");
                }
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{(hasExit ? $"\n{zeroSelection}\n" : "\n")}");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write($"{question}");
            Console.ResetColor();

            if (int.TryParse(Console.ReadLine(), out int input) && input >= 0)
            {
                if ((options != null && input >= 1 && input <= options.Length) || (hasExit && input == 0))
                {
                    return input;
                }
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("잘못된 입력입니다.");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkGreen; Console.Write("계속>>"); Console.ResetColor();
            Console.ReadKey();
            return -1;
        }
        /*Select함수
        값을 입력받을때 사용되는 공통된 코드들을 모아놓은 함수로
        정수값을 반환하며
        (문자열 배열 options = 기본값 null,
        불값 hasExit = 기본값 true,
        문자열 zeroSelection = 기본값 "0.나가기",
        문자열 question = 기본값 "원하시는 행동을 입력해 주세요.\n>>") 의 네 가지의 매개변수를 받는다.
        
        */
    }
}
