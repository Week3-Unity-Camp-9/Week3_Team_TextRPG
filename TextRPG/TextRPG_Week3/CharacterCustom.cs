using static TextRPG_Week3.Character;

namespace TextRPG_Week3
{
    public class CharacterCustom
    {
        public void Customizing(Character player, bool hasName)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (hasName)
            {
                Console.Clear();
                Console.WriteLine("==== 캐릭터 커스터마이징 ====\n");
            }
            else Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.\n");

            // 이름 입력
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("원하시는 이름을 설정해주세요.\n>>");
            Console.ResetColor();
            string name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
                player.Name = name;

            // 직업 선택
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("직업을 선택하세요:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("1. 전사");
            Console.WriteLine("2. 마법사");
            Console.WriteLine("3. 도적");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("원하시는 직업을 골라주세요.\n>>");
            Console.ResetColor();
            string jobInput = Console.ReadLine();
            switch (jobInput)
            {
                case "1":
                    player.Job = PlayerClass.Warrior;
                    player.Attack = 15;
                    player.Defense = 10;
                    player.Hp = 120;
                    player.MaxHp = 120;
                    player.Mp = 50;
                    player.MaxHp = 50;
                    break;
                case "2":
                    player.Job = PlayerClass.Wizard;
                    player.Attack = 20;
                    player.Defense = 5;
                    player.Hp = 80;
                    player.MaxHp = 80;
                    player.Mp = 120;
                    player.MaxHp = 120;
                    break;
                case "3":
                    player.Job = PlayerClass.Thief;
                    player.Attack = 12;
                    player.Defense = 7;
                    player.Hp = 100;
                    player.MaxHp = 100;
                    player.Mp = 100;
                    player.MaxHp = 100;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("잘못된 입력입니다. 기본 직업(전사)로 설정합니다.");
                    player.Job = PlayerClass.Warrior;
                    player.Attack = 15;
                    player.Defense = 10;
                    player.Hp = 120;
                    player.MaxHp = 120;
                    player.Mp = 50;
                    player.MaxHp = 50;
                    break;
            }
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("적용이 완료되었습니다!");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"이름: {player.Name}");
            Console.WriteLine($"직업: {player.Job}");
            Console.WriteLine($"공격력: {player.Attack}");
            Console.WriteLine($"방어력: {player.Defense}");
            Console.WriteLine($"최대 체력: {player.MaxHp}");
            Console.WriteLine($"골드: {player.Gold}");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\n아무 키나 누르면 계속합니다...");
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
        }
    }
}