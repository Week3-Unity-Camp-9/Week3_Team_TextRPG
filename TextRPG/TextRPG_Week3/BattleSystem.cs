using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TextRPG_Week3; // 현재 프로젝트의 네임스페이스 참조

namespace TextRPG_Week3
{
    public static class BattleSystem // 전투 시스템 클래스 정의
    {
        static public int stage = 1;

        static Random random = new Random();

        static List<Enemy> enemyList = new List<Enemy>
        {
            new Enemy(1, "미니언", 10, 4),
            new Enemy(2, "공허충", 5, 8),
            new Enemy(4, "대포미니언", 20, 7)
        };

        static List<Enemy> appearEnemies = new List<Enemy>();

        static int originalHp;
        static bool lose;

        public static void Encounting(Character player)
        {
            originalHp = player.Hp; // 전투 시작 전 플레이어의 현재 HP를 저장
            lose = false;
            appearEnemies.Clear(); // 이전 전투에서 등장했던 적 목록을 초기화

            int enemyCount = random.Next(1 + (stage / 3), 5); // 1마리에서 4마리 사이의 적 개수를 랜덤으로 결정

            if(stage % 10 == 0) // 10의 배수 스테이지마다 보스 등장
            {
                appearEnemies.Clear();
                appearEnemies.Add(new Boss(5, "내셔 남작", 85, 15, "불멸")); // 보스 몬스터 추가
            }
            else
            {
                for (int i = 0; i < enemyCount; i++) // 결정된 적의 수만큼 반복
                {
                    int randomIndex = random.Next(enemyList.Count); // enemyList에서 랜덤한 인덱스 선택
                    Enemy selectedEnemy = enemyList[randomIndex]; // 선택된 인덱스에 해당하는 적 정보 가져오기
                    appearEnemies.Add(new Enemy(selectedEnemy.Level, selectedEnemy.Name, selectedEnemy.Hp, selectedEnemy.Attack)); // 새로운 Enemy 객체를 생성하여 appearEnemies 리스트에 추가 (기존 적 데이터 복사)
                }
            }

            Battle(player); // 전투 메서드 호출, 초기 상태는 적 조우 상태
        }

        static void Battle(Character player)
        {
            while (true)
            {
                if (appearEnemies.All(enemy => enemy.IsDead))
                {
                    BattleWin(player);
                    return;
                }

                ShowEnemies(player);
                SelectTarget(player);
                EnemyAttack(appearEnemies, player);
                if (lose) return;
            }
        }

        // 적 표시
        static void ShowEnemies(Character player)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Battle!!\n");

                foreach (var enemy in appearEnemies)
                {
                    if (enemy.IsDead) Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"Lv.{enemy.Level} {enemy.Name} HP {(enemy.IsDead ? "Dead" : enemy.Hp.ToString())}");
                    Console.ResetColor();
                }

                Console.WriteLine("\n[내정보]");
                Console.WriteLine($"Lv.{player.Level}  {player.Name} ({player.Job})");
                Console.WriteLine($"HP {player.Hp}/{player.MaxHp}\n");

                Console.WriteLine("1.공격");
                Console.Write("해당하는 번호를 입력해주세요.\n>>");

                if (int.TryParse(Console.ReadLine(), out int input) && input == 1) return;
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.ReadKey();
                    continue;
                }
            }
        }

        static void SelectTarget(Character player)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Battle!!\n");
                for (int i = 0; i < appearEnemies.Count; i++)
                {
                    if (appearEnemies[i].IsDead)
                        Console.ForegroundColor = ConsoleColor.DarkGray;

                    Console.WriteLine($"{i + 1}. Lv.{appearEnemies[i].Level} {appearEnemies[i].Name} HP {(appearEnemies[i].IsDead ? "Dead" : appearEnemies[i].Hp.ToString())}");
                    Console.ResetColor();
                }
                Console.WriteLine("\n[내정보]");
                Console.WriteLine($"Lv.{player.Level}  {player.Name} ({player.Job})");
                Console.WriteLine($"HP {player.Hp}/{player.MaxHp}");
                Console.WriteLine("\n0.취소\n");
                Console.Write("대상을 선택해 주세요.\n>>");

                if (int.TryParse(Console.ReadLine(), out int input))
                {
                    if (input == 0)
                    {
                        Console.WriteLine("행동을 취소하고 턴을 넘깁니다.");
                        Console.ReadKey();
                        return;
                    }

                    if (input >= 1 && input <= appearEnemies.Count)
                    {
                        var target = appearEnemies[input - 1];
                        if (target.IsDead)
                        {
                            Console.WriteLine($"{target.Name}은 이미 죽었습니다.");
                            Console.ReadKey();
                            continue;
                        }
                        PlayerAttack(input, player);
                        return;
                    }
                }
                Console.WriteLine("잘못된 입력입니다.");
                Console.ReadKey();
                continue;
            }
        }

        static void PlayerAttack(int select, Character player)
        {
            Enemy selectedEnemy = appearEnemies[select - 1]; // 선택된 적 객체 가져오기
            int Damage = (int)player.TotalAttack; // 플레이어의 총 공격력을 기본 데미지로 설정

            bool critical = (random.Next(1, 101) < 15); // 15% 확률로 치명타 발생 여부 결정
            if (critical) Damage = (int)(player.TotalAttack * 1.6f); // 치명타 시 데미지 1.6배 증가

            bool hit = (random.Next(1, 101) > 10); // 90% 확률로 공격 성공 여부 결정
            if (!hit) Damage = 0; // 공격 실패 시 데미지는 0

            selectedEnemy.Hp -= Damage; // 선택된 적의 HP에서 데미지 감소
            if (selectedEnemy.Hp <= 0) selectedEnemy.IsDead = true; // 적의 HP가 0 이하가 되면 IsDead 상태를 true로 변경

            Console.Clear();
            Console.WriteLine("Battle!!\n");

            Console.WriteLine($"{player.Name} 의 공격!");
            Console.Write($"Lv.{selectedEnemy.Level} {selectedEnemy.Name}을(를) ");
            if (hit)
            {
                Console.WriteLine($"맞췄습니다. [데미지 : {Damage}]" + $"{(critical ? "- 치명타 공격!!" : "")}" + "\n");
                Console.WriteLine($"Lv.{selectedEnemy.Level} {selectedEnemy.Name}");
                Console.WriteLine($"HP {selectedEnemy.Hp + Damage} => {(selectedEnemy.IsDead ? "Dead" : selectedEnemy.Hp)}");
            }
            else Console.WriteLine("공격했지만 아무일도 일어나지 않았습니다.\n");

            Console.Write("\n아무 키나 눌러서 계속\n>>");
            Console.ReadKey();
        }

        static void EnemyAttack(List<Enemy> enemies, Character player)
        {
            if (enemies.All(enemy => enemy.IsDead)) return;
            Console.Clear();
            Console.WriteLine("Battle!!\n");
            int originalHp = player.Hp;
            for (int i = 0; i < enemies.Count; i++) // 등장한 모든 적에 대해 반복
            {
                if (enemies[i].IsDead) continue; // 이미 죽은 적은 공격하지 않음

                int Damage = enemies[i].Attack; // 현재 적의 공격력을 데미지로 설정

                bool critical = (random.Next(1, 101) < 15); // 15% 확률로 치명타 발생 여부 결정
                if (critical) Damage = (int)(enemies[i].Attack * 1.6f); // 치명타 시 데미지 1.6배 증가

                bool hit = (random.Next(1, 101) > 10); // 90% 확률로 공격 성공 여부 결정
                if (!hit) Damage = 0; // 공격 실패 시 데미지는 0

                player.Hp -= Damage; // 플레이어의 HP에서 데미지 감소

                Console.WriteLine($"{enemies[i].Name} 의 공격!");
                Console.Write($"Lv.{player.Level} {player.Name}을(를)");
                if (hit)
                {
                    Console.WriteLine($"맞췄습니다. [데미지 : {Damage}]" + $"{(critical ? "- 치명타 공격!!" : "")}" + "\n");
                }
                else Console.WriteLine("공격했지만 아무일도 일어나지 않았습니다.\n");
            }
            Console.WriteLine($"Lv.{player.Level} {player.Name}");
            Console.WriteLine($"HP {originalHp} => {player.Hp}");
            Console.Write("\n아무 키나 눌러서 계속\n>>");
            Console.ReadKey();
            if (player.Hp <= 0) // 플레이어의 HP가 0 이하이면
            {
                BattleLose(player); // 전투 결과 처리 (패배)
                return; // 메서드 종료
            }
            foreach (var appearEnemy in appearEnemies)
            {
                if (appearEnemy is Boss boss)
                {
                    boss.UseSpecialSkill(player);
                    Console.ReadKey();
                }
            }
        }

        static void BattleWin(Character player)
        {
            int heal = (int)(player.MaxHp / 10);
            player.Hp += heal;
            if (player.Hp > player.MaxHp) player.Hp = player.MaxHp;
            int originalEXP = player.EXP;
            for (int i = 0; i < appearEnemies.Count; i++)
            {
                player.EXP += appearEnemies[i].Level;
            }
            int gold = (stage * 500) + (appearEnemies.Count * 100);
            player.Gold += gold;
            stage++;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Battle!! - Result\n");
                Console.WriteLine("Victory");
                Console.WriteLine($"\n던전에서 몬스터 {appearEnemies.Count}마리를 잡았습니다.");
                Console.WriteLine($"체력을 {heal}만큼 회복합니다.");
                Console.WriteLine($"Lv.{player.Level} {player.Name}");
                Console.WriteLine($"HP {player.Hp - heal} => {player.Hp}");
                Console.WriteLine($"EXP {originalEXP} => {player.EXP}");
                Console.WriteLine("[획득 보상]");
                Console.WriteLine($"{gold} Gold");
                Console.WriteLine($"{stage} 스테이지 클리어!");

                Console.Write("\n0.다음\n>>");

                if (int.TryParse(Console.ReadLine(), out int input))
                {
                    if (input == 0)
                    {
                        while (true)
                        {
                            if (player.EXP >= player.RequireEXP)
                            {
                                player.EXP -= player.RequireEXP;
                                player.Level++;
                                heal = (int)(player.MaxHp / 5);
                                player.Hp += heal;
                                if (player.Hp > player.MaxHp) player.Hp = player.MaxHp;
                                Console.Clear();
                                Console.WriteLine("레벨업!");
                                Console.WriteLine($"레벨이 {player.Level - 1}에서 {player.Level}이 되었습니다!");
                                Console.WriteLine($"공격력 : {player.TotalAttack - 0.5f}{(player.EquipAttack != 0 ? $"(+{player.EquipAttack})" : "")} => {player.TotalAttack}{(player.EquipAttack != 0 ? $"(+{player.EquipAttack})" : "")}");
                                Console.WriteLine($"방어력 : {player.TotalDefense - 1}{(player.EquipDefense != 0 ? $"(+{player.EquipDefense})" : "")} => {player.TotalDefense}{(player.EquipDefense != 0 ? $"(+{player.EquipDefense})" : "")}");
                                Console.WriteLine($"체력을 {heal}만큼 회복합니다!");
                                Console.WriteLine($"{player.Hp - heal} => {player.Hp}");
                                Console.WriteLine("\n아무 키나 누르면 계속합니다...");
                                Console.ReadKey();
                            }
                            else break;
                        }
                        break;
                    }
                    else
                    {
                        Console.Write("잘못된 입력입니다.");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
                else
                {
                    Console.Write("잘못된 입력입니다.");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            return;
        }

        static void BattleLose(Character player)
        {
            string[] lastStats = new string[] { player.Level.ToString(), player.TotalAttack.ToString(), player.TotalDefense.ToString(), player.MaxHp.ToString() };
            int originalHp = player.Hp;
            int gold = (int)((player.Level * 500));
            player.Gold += gold;
            player.Level = 1;
            player.EXP = 0;
            stage = 1;

            switch (player.Job)
            {
                case "전사":
                    player.Attack = 15;
                    player.Defense = 10;
                    player.Hp = 120;
                    player.MaxHp = 120;
                    break;
                case "마법사":
                    player.Attack = 20;
                    player.Defense = 5;
                    player.Hp = 80;
                    player.MaxHp = 80;
                    break;
                case "도적":
                    player.Attack = 12;
                    player.Defense = 7;
                    player.Hp = 100;
                    player.MaxHp = 100;
                    break;
            }

            lose = true;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Battle!! - Result\n");
                Console.WriteLine("You Lose");
                Console.WriteLine($"Lv.{player.Level} {player.Name}");
                Console.WriteLine($"HP {player.MaxHp} => {originalHp}");

                Console.Write("\n0.다음\n>>");

                if (int.TryParse(Console.ReadLine(), out int input))
                {
                    if (input == 0)
                    {
                        Console.Clear();
                        Console.WriteLine("패배했습니다.\n");
                        Console.WriteLine("[최종 능력치]");
                        Console.WriteLine($"Lv.{player.Level} {player.Name}");
                        Console.WriteLine($"공격력 : {player.TotalAttack}");
                        Console.WriteLine($"방어력 : {player.TotalDefense}");
                        Console.WriteLine($"HP {player.MaxHp}");
                        Console.WriteLine("레벨이 1로 돌아갑니다.\n");
                        Console.WriteLine($"Lv : {lastStats[0]} => {player.Level}");
                        Console.WriteLine($"공격력 : {lastStats[1]} => {player.TotalAttack}");
                        Console.WriteLine($"방어력 : {lastStats[2]} => {player.TotalDefense}");
                        Console.WriteLine($"HP : {lastStats[3]} => {player.MaxHp}");
                        Console.WriteLine($"레벨에 대한 보상으로 {gold} Gold를 획득했습니다.");
                        Console.WriteLine("\n아무 키나 누르면 계속합니다...");
                        Console.ReadKey();
                        return;
                    }
                    else
                    {
                        Console.Write("잘못된 입력입니다.");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
                else
                {
                    Console.Write("잘못된 입력입니다.");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
    }
}