using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TextRPG_Week3;

namespace TextRPG_Week3
{
    public class BattleSystem
    {
        // 적 캐릭터 목록 (기본 설정)
        List<Enemy> enemyList = new List<Enemy>
        {
            new Enemy(2, "미니언", 15, 5),
            new Enemy(3, "공허충", 10, 9),
            new Enemy(5, "대포미니언", 25, 8)
        };

        // 현재 전투 중 등장한 적 목록
        List<Enemy> appearEnemies = new List<Enemy>();

        // 전투 시작 전 플레이어 HP 저장
        int originalHp;

        // 전투 상태 열거형
        public enum BattleMode
        {
            Encounter,      // 적과 조우했을 때
            PlayerAttack    // 플레이어가 공격 선택한 상태
        }

        // 적과 조우하는 과정
        public void Encounting(GameSystem gameSystem, Character player, BattleMode mode)
        {
            originalHp = player.Hp;
            appearEnemies.Clear();

            Random random = new Random();
            int enemyCount = random.Next(1, 5); //Next 괄호 안에 들어가는 부분이 몇 마리인지 정하는 부분

            for (int i = 0; i < enemyCount; i++)
            {
                int randomIndex = random.Next(enemyList.Count);
                Enemy selectedEnemy = enemyList[randomIndex];
                appearEnemies.Add(new Enemy(selectedEnemy.Level, selectedEnemy.Name, selectedEnemy.Hp, selectedEnemy.Attack));
            }

            Battle(gameSystem, player, BattleMode.Encounter);
        }

        // 전투 루프 (상태에 따라 전투 UI 및 로직 분기)
        public void Battle(GameSystem gameSystem, Character player, BattleMode mode)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Battle!!\n");

                if (mode == BattleMode.Encounter)
                {
                    // 적 상태 출력
                    for (int i = 0; i < appearEnemies.Count; i++)
                    {
                        if (appearEnemies[i].IsDead) Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine($"Lv.{appearEnemies[i].Level} {appearEnemies[i].Name} HP {(!appearEnemies[i].IsDead ? appearEnemies[i].Hp : "Dead")}");
                        Console.ResetColor();
                    }

                    // 플레이어 정보 출력
                    Console.WriteLine("\n[내정보]");
                    Console.WriteLine($"Lv.{player.Level}  {player.Name} ({player.Job})");
                    Console.WriteLine($"HP {player.Hp}/{player.MaxHp}\n");

                    int input = gameSystem.Select(new string[] { "1.공격" }, false);
                    if (input == 1) Battle(gameSystem, player, BattleMode.PlayerAttack);
                }
                else if (mode == BattleMode.PlayerAttack)
                {
                    // 공격 대상 선택 화면
                    Console.Clear();
                    Console.WriteLine("Battle!!\n");

                    for (int i = 0; i < appearEnemies.Count; i++)
                    {
                        if (appearEnemies[i].IsDead) Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine($"{i + 1} Lv.{appearEnemies[i].Level} {appearEnemies[i].Name} HP {(!appearEnemies[i].IsDead ? appearEnemies[i].Hp : "Dead")}");
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
                            EnemyAttack(appearEnemies, player);
                            return;
                        }
                        else if (input >= 1 && input <= appearEnemies.Count)
                        {
                            if (appearEnemies[input - 1].IsDead)
                            {
                                Console.WriteLine($"{appearEnemies[input - 1].Name}은 이미 죽었다.");
                                Thread.Sleep(1000);
                                Console.Clear();
                                continue;
                            }
                            PlayerAttack(appearEnemies, input, player);
                            return;
                        }
                    }
                    else
                    {
                        gameSystem.Message("잘못된 입력입니다.");
                        continue;
                    }
                }
            }
        }

        // 플레이어의 공격 처리
        void PlayerAttack(List<Enemy> enemies, int select, Character player)
        {
            Enemy selectedEnemy = enemies[select - 1];
            int Damage = (int)player.TotalAttack;
            selectedEnemy.Hp -= Damage;
            if (selectedEnemy.Hp <= 0) selectedEnemy.IsDead = true;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Battle!!\n");
                Console.WriteLine($"{player.Name} 의 공격!");
                Console.WriteLine($"Lv.{selectedEnemy.Level} {selectedEnemy.Name}을(를) 맞췄습니다. [데미지 : {player.TotalAttack}]\n");
                Console.WriteLine($"Lv.{selectedEnemy.Level} {selectedEnemy.Name}");
                Console.WriteLine($"HP {selectedEnemy.Hp + Damage} -> {(selectedEnemy.IsDead ? "Dead" : selectedEnemy.Hp)}");

                Console.Write("\n0.다음\n>>");
                if (int.TryParse(Console.ReadLine(), out int input))
                {
                    if (input == 0)
                    {
                        if (appearEnemies.All(enemy => enemy.IsDead))
                        {
                            BattleResult(Result.Win, player);
                            return;
                        }
                        EnemyAttack(enemies, player);
                        return;
                    }
                    else
                    {
                        Console.Write("잘못된 입력입니다.");
                        Thread.Sleep(1000);
                        Console.Clear();
                    }
                }
                else
                {
                    Console.Write("잘못된 입력입니다.");
                    Thread.Sleep(1000);
                    Console.Clear();
                }
            }
        }

        // 적의 공격 처리
        void EnemyAttack(List<Enemy> enemies, Character player)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].IsDead) continue;

                player.Hp -= enemies[i].Attack;

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Battle!!\n");
                    Console.WriteLine($"{enemies[i].Name} 의 공격!");
                    Console.WriteLine($"Lv.{player.Level} {player.Name}을(를) 맞췄습니다. [데미지 : {enemies[i].Attack}]\n");
                    Console.WriteLine($"Lv.{player.Level} {player.Name}");
                    Console.WriteLine($"HP {player.Hp + enemies[i].Attack} -> {player.Hp}");
                    Console.Write("\n0.다음\n>>");

                    if (int.TryParse(Console.ReadLine(), out int input))
                    {
                        if (input == 0)
                        {
                            if (player.Hp <= 0)
                            {
                                BattleResult(Result.Lose, player);
                                return;
                            }
                            break;
                        }
                        else
                        {
                            Console.Write("잘못된 입력입니다.");
                            Thread.Sleep(1000);
                            Console.Clear();
                        }
                    }
                    else
                    {
                        Console.Write("잘못된 입력입니다.");
                        Thread.Sleep(1000);
                        Console.Clear();
                    }
                }
            }
        }

        // 전투 결과 열거형
        enum Result
        {
            Win,
            Lose
        }

        // 전투 종료 처리
        void BattleResult(Result mode, Character player)
        {
            Console.Clear();
            Console.WriteLine("Battle!! - Result\n");
            Console.WriteLine(mode == Result.Win ? "Victory" : "You Lose");
            Console.WriteLine(mode == Result.Win ? $"\n던전에서 몬스터 {appearEnemies.Count}마리를 잡았습니다." : "");
            Console.WriteLine($"Lv.{player.Level} {player.Name}");
            Console.WriteLine($"HP {player.MaxHp} -> {player.Hp}");
            Console.Write("\n0.다음\n>>");

            if (int.TryParse(Console.ReadLine(), out int input))
            {
                if (input == 0)
                {
                    if (mode == Result.Lose) Environment.Exit(0);
                }
                else
                {
                    Console.Write("잘못된 입력입니다.");
                    Thread.Sleep(1000);
                    Console.Clear();
                }
            }
            else
            {
                Console.Write("잘못된 입력입니다.");
                Thread.Sleep(1000);
                Console.Clear();
            }
        }
    }
}
