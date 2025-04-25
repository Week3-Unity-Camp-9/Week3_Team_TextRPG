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
    }
}
