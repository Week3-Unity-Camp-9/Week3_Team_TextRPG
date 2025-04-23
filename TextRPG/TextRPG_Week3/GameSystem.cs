using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_Week3
{
    public class GameData
    {
        public Character Player { get; set; }
        public ShopItem Shop { get; set; }
        public GameData() { }
        public GameData(Character player, ShopItem shop)
        {
            Player = player;
            Shop = shop;
        }
    }

    public static class SaveManager
    {
        public static void SaveGame(Character player, ShopItem shop, int slot)
        {
            string saveFilePath = $"save{slot}.json";
            GameData gameData = new GameData(player, shop);
            string json = JsonConvert.SerializeObject(gameData, Formatting.Indented);
            File.WriteAllText(saveFilePath, json);
        }

        public static (Character, ShopItem) LoadGame(int slot)
        {
            string saveFilePath = $"save{slot}.json";

            string json = File.ReadAllText(saveFilePath);
            GameData gameData = JsonConvert.DeserializeObject<GameData>(json);

            return (gameData.Player, gameData.Shop);
        }
    }

    public static class GameSystem
    {
        public static int Select(string[]? options = null, bool hasExit = true, string zeroSelection = "0.나가기", string question = "원하시는 행동을 입력해 주세요.\n>>")
        {
            if (options != null)
            {
                for (int i = 0; i < options.Length; i++)
                {
                    Console.WriteLine($"{options[i]}");
                }
            }
            Console.Write($"{(hasExit ? $"\n{zeroSelection}\n" : "")}");
            Console.WriteLine($"{question}");
            if (int.TryParse(Console.ReadLine(), out int input) && input >= 0 && input <= options.Length)
            {
                if ((options != null && input >= 1 && input <= options.Length) || (hasExit && input == 0))
                {
                    return input;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.ReadKey();
                    return -1;
                }
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                Console.ReadKey();
                return -1;
            }
        }
    }
}
