using static TextRPG_Week3.Boss;

namespace TextRPG_Week3
{
    public class Enemy
    {
        public int Level;
        public string Name;
        public int Hp;
        public int Attack;
        public bool IsDead;
        public bool IsBoss;


        public Enemy(int level, string name, int hp, int attack, bool isDead = false)
        {
            Level = (BattleSystem.stage - 1) + level;
            Name = name;
            Hp = (BattleSystem.stage - 1) + hp;
            Attack = (BattleSystem.stage - 1) + attack;
            IsDead = isDead;
        }
    }

    public class Boss : Enemy
    {
        //Public class Enemy 적의 기본 클래스
        //적의 기본 속성값들
        
        //적의 속성값을 받아오는 방식
        
        //public class Boss : Enemy 적의 기본 클래스를 상속받는 클래스
        //Boss에만 있는 속성
        
        //Boss의 속성값을 받아오는 방식

        public string SpecialSkill;
        public Boss(int level, string name, int hp, int attack, string specialSkill) : base(level, name, hp, attack)
        {
            SpecialSkill = specialSkill;
        }

        public void UseSpecialSkill(Character character)
        {
            Console.WriteLine($"{Name}이(가) {SpecialSkill}을(를) 사용했습니다!");
            int healAmount = 0;
            switch (SpecialSkill)
            {

                case "불멸":
                    // 보스의 체력을 회복시키는 효과
                    healAmount = 12;
                    Hp += healAmount;
                    Console.WriteLine($"{Name}의 체력이 {healAmount} 회복되었습니다!");
                    break;

                default:
                    Console.WriteLine("알 수 없는 스킬입니다.");
                    break;
            }

        }
        //UseSpecialSkill함수
        //메세지 출력
        //힐값 초기화
        //특수스킬에 따라
        //-"불멸"일 경우
        //--힐값 12
        //--12회복
        //--회복 메세지 출력
        
        //-나머지일 경우
        //--메세지 출력
    }

}
