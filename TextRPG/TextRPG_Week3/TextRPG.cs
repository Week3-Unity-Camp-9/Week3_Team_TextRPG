using System;

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
            Console.Clear();
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n");

            int input = gameSystem.Select(new string[] {"1. 상태 보기", "2. 전투 시작" }, false);

            switch (input)
            {
                case 1:
                    CharacterCustom();
                    break;
                case 2:
                    //전투시작
                    break;
                default:
                    Start(gameSystem, battleSystem, player);
                    return;
            }
        }
    }
}
