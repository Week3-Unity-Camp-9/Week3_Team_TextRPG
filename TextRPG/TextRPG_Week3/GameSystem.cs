using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_Week3
{
    public class GameSystem
    {
        //선택지가 여러가지 일 때 사용하려고 만들어둔 함수입니다.
        //선택지에 번호까지 붙여서 사용해 주세요.
        //예시 : int input = gameSystem.Select(new string[] {"1. 상태 보기", "2. 전투 시작" }, false); = 선택지 두 개에 나가기가 없는 선택지
        public int Select(string[] options, bool hasExit)
        {
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"{options[i]}\t");
            }
            Console.Write($"{(hasExit ? "\n0.나가기" : "")}\n해당하는 번호를 입력해주세요.\n>>");
            if (int.TryParse(Console.ReadLine(), out int input) && input >= 0 && input <= options.Length)
            {
                if ((input >= 1 && input <= options.Length) || (hasExit && input == 0))
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
