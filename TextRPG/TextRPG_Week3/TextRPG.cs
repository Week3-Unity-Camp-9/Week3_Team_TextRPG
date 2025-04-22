using System;
using System.Numerics;
using TextRPG_Week3;

namespace TextRPG_Week3
{
    internal class TextRPG
    {
        static void Main()
        {
            GameSystem gameSystem = new GameSystem();
            BattleSystem battleSystem = new BattleSystem();
            Character player = new Character();
            new TextRPG().Start(gameSystem, battleSystem, player);
        }

        void Start(GameSystem gameSystem, BattleSystem battleSystem, Character player)
        {
            CharacterCustom custom = new CharacterCustom();
            custom.Customizing(player, false);
            Town(gameSystem, battleSystem, player);
        }

        void Town(GameSystem gameSystem, BattleSystem battleSystem, Character player)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n");

                int input = gameSystem.Select(new string[] { "1. 상태 보기", "2. 전투 시작" }, false);

                switch (input)
                {
                    case 1:
                        Status(gameSystem, player);
                        break;
                    case 2:
                        battleSystem.Encounting(gameSystem, player, BattleSystem.BattleMode.Encounter);
                        break;
                    default:
                        continue;
                }
            }
        }

        void Status(GameSystem gameSystem, Character player)
        {
            CharacterCustom custom = new CharacterCustom();
            while (true)
            {
                Console.WriteLine($"상태 보기");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");

                player.DisplayStatus(player);

                int input = gameSystem.Select(new string[] {"1.커스터마이징" }, true);
                switch(input)
                {
                    case 1:
                        custom.Customizing(player, true);
                        break;
                    case 0:
                        return;
                    default:
                        gameSystem.Message("잘못된 입력입니다.");
                        continue;
                }
            }
        }
    }
}
