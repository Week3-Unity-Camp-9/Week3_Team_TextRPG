using System;

namespace TextRPG_Week3
{
    public class CharacterCustom
    {
        public void Customizing(Character character, bool hasName)
        {
            if (hasName)
            {
                Console.Clear();
                Console.WriteLine("==== 캐릭터 커스터마이징 ====");
            }
            else Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");

            // 이름 입력
            Console.Write("원하시는 이름을 설정해주세요.\n>>");
            string name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
                character.Name = name;

            // 직업 선택
            Console.WriteLine("직업을 선택하세요:");
            Console.WriteLine("1. 전사");
            Console.WriteLine("2. 마법사");
            Console.WriteLine("3. 도적");
            Console.Write("번호 입력: ");
            string jobInput = Console.ReadLine();
            switch (jobInput)
            {

                case "1":
                    character.Job = "전사";
                    character.Attack = 15;
                    character.Defense = 10;
                    character.Health = 120;
                    break;
                case "2":
                    character.Job = "마법사";
                    character.Attack = 20;
                    character.Defense = 5;
                    character.Health = 80;
                    break;
                case "3":
                    character.Job = "도적";
                    character.Attack = 12;
                    character.Defense = 7;
                    character.Health = 100;
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다. 기본 직업(전사)로 설정합니다.");
                    character.Job = "전사";
                    character.Attack = 15;
                    character.Defense = 10;
                    character.Health = 120;
                    break;
            }

            Console.WriteLine("\n적용이 완료되었습니다!");
            Console.WriteLine($"이름: {character.Name}");
            Console.WriteLine($"직업: {character.Job}");
            Console.WriteLine($"공격력: {character.Attack}");
            Console.WriteLine($"방어력: {character.Defense}");
            Console.WriteLine($"체력: {character.Health}");
            Console.WriteLine($"골드: {character.Gold}");
            Console.WriteLine("\n아무 키나 누르면 계속합니다...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}