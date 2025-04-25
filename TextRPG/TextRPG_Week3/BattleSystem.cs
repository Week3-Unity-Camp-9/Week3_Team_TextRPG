using static TextRPG_Week3.Character;

namespace TextRPG_Week3
{
    public static class BattleSystem // 전투 시스템 클래스 정의
    {
        static public int stage = 1;

        static Random random = new Random();

        public static List<Enemy> enemyList = new List<Enemy>
        {
            new Enemy(1, "미니언", 10, 4),
            new Enemy(2, "공허충", 5, 8),
            new Enemy(4, "대포미니언", 20, 7),
        };
        public static List<Enemy> bossList = new List<Enemy>
        {
            new Boss(5, "내셔 남작", 85, 15, "불멸")
        };

        static List<Enemy> appearEnemies = new List<Enemy>();

        static int originalHp;
        public static bool lose;

        public static void Encounting(Character player)
        {
            originalHp = player.Hp; // 전투 시작 전 플레이어의 현재 HP를 저장
            lose = false;
            appearEnemies.Clear(); // 이전 전투에서 등장했던 적 목록을 초기화

            int min = Math.Min(1 + (stage / 3), 4);
            int enemyCount = random.Next(min, 5); // 1마리에서 4마리 사이의 적 개수를 랜덤으로 결정

            if (stage % 10 == 0) // 10의 배수 스테이지마다 보스 등장
            {
                appearEnemies.Clear();
                appearEnemies.Add(bossList[0]); // 보스 몬스터 추가
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
                if (appearEnemies.All(enemy => enemy.IsDead)) //적을 전부 쓰러트렸을때
                {
                    Result(player, ResultMode.Win);
                    return;
                }

                (float blockBonus, bool hidden) = ShowEnemies(player);
                EnemyAttack(appearEnemies, player, blockBonus, hidden);
                if (lose) return;
            }
        }

        // 적 표시
        static (float, bool) ShowEnemies(Character player)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Battle!!\n");
                foreach (var enemy in appearEnemies)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    if (enemy.IsDead) Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"Lv.{enemy.Level} {enemy.Name} HP {(enemy.IsDead ? "Dead" : enemy.Hp.ToString())}");
                    Console.ResetColor();
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n[내정보]");
                Console.WriteLine($"Lv.{player.Level}  {player.Name} ({player.Job})");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"HP {player.Hp}/{player.MaxHp}\n");

                int input = GameSystem.Select(new string[] { "1.공격", "2.스킬" }, false);

                if (input == 1)
                {
                    SelectTarget(player);
                    return (1, false);
                }
                else if (input == 2)
                {
                    (float blockBonus, bool hidden) = UseSkill(player);
                    return (blockBonus, hidden);
                }
                else continue;
            }
        }

        static (float, bool) UseSkill(Character player)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Battle!!\n");
                foreach (var enemy in appearEnemies)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    if (enemy.IsDead) Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"Lv.{enemy.Level} {enemy.Name} HP {(enemy.IsDead ? "Dead" : enemy.Hp.ToString())}");
                    Console.ResetColor();
                }
                (string skill1, string skill2) = player.GetSkills();
                string[] skills = new string[] { $"1.{skill1}", $"2.{skill2}" };
                int input = GameSystem.Select(skills, zeroSelection: "0.취소", question: "사용할 스킬을 골라주세요.\n>>");
                if (input == 0 || input == -1) continue;
                switch (player.Job)
                {
                    case PlayerClass.Warrior:
                        switch (input)
                        {
                            case 1:
                                SelectTarget(player, 2, canDodge: false);
                                break;
                            case 2:
                                SelectTarget(player, canDodge: false);
                                return (2, false);
                        }
                        break;
                    case PlayerClass.Wizard:
                        switch (input)
                        {
                            case 1:
                                SelectTarget(player, 1.5f, canDodge: false);
                                break;
                            case 2:
                                int count = random.Next(2, 5);
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine($"{count}번 공격!!");
                                Console.ReadKey();
                                for (int i = 0; i < count; i++)
                                {
                                    int randomTarget = random.Next(0, appearEnemies.Count);
                                    Enemy target = appearEnemies[randomTarget];
                                    int enemyHp = target.Hp;
                                    target.Hp -= (int)player.TotalAttack;
                                    Console.WriteLine($"Lv.{target.Level} {target.Name}을 맞췄다! [{(int)player.TotalAttack}의 데미지!]");
                                    Console.WriteLine($"HP {enemyHp} -> {target.Hp}\n");
                                    Thread.Sleep(500);
                                }
                                break;
                        }
                        break;
                    case PlayerClass.Thief:
                        switch (input)
                        {
                            case 1:
                                SelectTarget(player, 3);
                                break;
                            case 2:
                                SelectTarget(player);
                                return (1, true);
                        }
                        break;
                }
                break;
            }
            return (1, false);
        }

        static void SelectTarget(Character player, float attackBonus = 1f, bool canDodge = true)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Battle!!\n");

                for (int i = 0; i < appearEnemies.Count; i++)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    if (appearEnemies[i].IsDead) Console.ForegroundColor = ConsoleColor.DarkGray;

                    Console.WriteLine($"{i + 1}. Lv.{appearEnemies[i].Level} {appearEnemies[i].Name} HP {(appearEnemies[i].IsDead ? "Dead" : appearEnemies[i].Hp.ToString())}");
                    Console.ResetColor();
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n[내정보]");
                Console.WriteLine($"Lv.{player.Level}  {player.Name} ({player.Job})");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"HP {player.Hp}/{player.MaxHp}");
                Console.WriteLine("\n0.취소\n");
                Console.WriteLine("대상을 선택해 주세요.\n >>");
                if(int.TryParse(Console.ReadLine(), out int input))
                {
                    switch (input)
                    {
                        case -1:
                            continue;
                        case 0:
                            Console.WriteLine("행동을 취소하고 턴을 넘깁니다.");
                            Console.ReadKey();
                            return;
                        default:
                            if (input >= 1 && input <= appearEnemies.Count)
                            {
                                Enemy target = appearEnemies[input - 1];
                                if (target.IsDead)
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkRed;
                                    Console.WriteLine($"{target.Name}은 이미 죽었습니다.");
                                    Console.ReadKey();
                                    continue;
                                }
                                PlayerAttack(input, player, attackBonus, canDodge);
                                return;
                            }
                            break;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.ResetColor();
                    Console.ReadKey();
                }
            }
        }

        static void PlayerAttack(int select, Character player, float attackBonus, bool canDodge)
        {
            Enemy selectedEnemy = appearEnemies[select - 1]; // 선택된 적 객체 가져오기
            int Damage = (int)(player.TotalAttack * attackBonus); // 플레이어의 총 공격력을 기본 데미지로 설정

            bool critical = (random.Next(1, 101) < 15); // 15% 확률로 치명타 발생 여부 결정
            if (critical) Damage = (int)(player.TotalAttack * 1.6f); // 치명타 시 데미지 1.6배 증가

            bool hit = true;
            if (canDodge)
            {
                hit = (random.Next(1, 101) > 10); // 90% 확률로 공격 성공 여부 결정
                if (!hit) Damage = 0; // 공격 실패 시 데미지는 0
            }
            selectedEnemy.Hp -= Damage; // 선택된 적의 HP에서 데미지 감소
            if (selectedEnemy.Hp <= 0) selectedEnemy.IsDead = true; // 적의 HP가 0 이하가 되면 IsDead 상태를 true로 변경

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Battle!!\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{player.Name} 의 공격!");
            Console.Write($"Lv.{selectedEnemy.Level} {selectedEnemy.Name}을(를) ");
            if (hit)
            {
                Console.Write("맞췄습니다.");
                if (critical) Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"[데미지 : {Damage}]" + $"{(critical ? "- 치명타 공격!!" : "")}" + "\n");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Lv.{selectedEnemy.Level} {selectedEnemy.Name}");
                Console.WriteLine($"HP {selectedEnemy.Hp + Damage} => {(selectedEnemy.IsDead ? "Dead" : selectedEnemy.Hp)}");
            }
            else Console.WriteLine("공격했지만 아무일도 일어나지 않았습니다.\n");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("\n아무 키나 눌러서 계속\n>>");
            Console.ResetColor();
            Console.ReadKey();
        }

        static void EnemyAttack(List<Enemy> enemies, Character player, float blockBonus, bool hidden)
        {
            if (enemies.All(enemy => enemy.IsDead)) return;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Battle!!\n");
            int originalHp = player.Hp;
            for (int i = 0; i < enemies.Count; i++) // 등장한 모든 적에 대해 반복
            {
                if (enemies[i].IsDead) continue; // 이미 죽은 적은 공격하지 않음

                int Damage = (int)(enemies[i].Attack / blockBonus); // 현재 적의 공격력을 데미지로 설정

                bool critical = false;
                bool hit = false;
                bool block = false;

                if (!hidden)
                {
                    critical = (random.Next(1, 101) < 15); // 15% 확률로 치명타 발생 여부 결정
                    if (critical) Damage = (int)(enemies[i].Attack * 1.6f); // 치명타 시 데미지 1.6배 증가

                    hit = (random.Next(1, 101) > 10); // 90% 확률로 공격 성공 여부 결정
                    if (!hit) Damage = 0; // 공격 실패 시 데미지는 0

                    if (hit && player.TotalDefense - enemies[i].Attack > 0)
                    {
                        if (player.TotalDefense - enemies[i].Attack >= 100) block = true;
                        else
                        {
                            block = (random.Next(player.TotalDefense - enemies[i].Attack, 101) > 50);
                        }
                    }
                    if (block)
                    {
                        int blockDamage = (int)Damage / 2;
                        Damage = blockDamage;
                    }

                    player.Hp -= Damage; // 플레이어의 HP에서 데미지 감소
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{enemies[i].Name} 의 공격!");
                Console.Write($"Lv.{player.Level} {player.Name}을(를)");
                if (hit && !block)
                {
                    Console.Write("맞췄습니다.");
                    if (critical) Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"[데미지 : {Damage}]" + $"{(critical ? "- 치명타 공격!!" : "")}" + "\n");
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (!hit) Console.WriteLine("공격했지만 아무일도 일어나지 않았습니다.\n");
                else if (block) Console.WriteLine($"{(critical ? "강하게 " : "")}공격했지만 막아냈습니다! [데미지 : {Damage}]\n");
            }
            Console.WriteLine($"Lv.{player.Level} {player.Name}");
            if (player.Hp <= 0) Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"HP {originalHp} => {player.Hp}");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("\n아무 키나 눌러서 계속\n>>");
            Console.ResetColor();
            Console.ReadKey();
            if (player.Hp <= 0) // 플레이어의 HP가 0 이하이면
            {
                Result(player, ResultMode.Lose); // 전투 결과 처리 (패배)
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

        enum ResultMode
        {
            Win,
            Lose
        }
        static void Result(Character player, ResultMode mode)
        {
            foreach (Quest quest in QuestManager.Quests)
            {
                if (quest.IsClear || !quest.IsAccept) continue;

                switch (quest)
                {
                    case DefeatQuest defeatQuest:
                        foreach (Enemy enemy in appearEnemies) //나타난 몬스터들을 참조해서 그 수 만큼 반복
                        {
                            if (defeatQuest.Target(enemy)) //몬스터가 조건에 맞는 목표일때
                            {
                                defeatQuest.DefeatCount++;
                                if (defeatQuest.DefeatCount >= defeatQuest.RequiredDefeatCount)
                                {
                                    defeatQuest.IsClear = true;
                                    break;
                                }
                            }
                        }
                        break;

                    case DungeonQuest dungeonQuest:
                        if (stage > dungeonQuest.ReachedStage) dungeonQuest.ReachedStage = stage;
                        if (dungeonQuest.ReachedStage >= dungeonQuest.RequiredStage)
                        {
                            dungeonQuest.IsClear = true;
                        }
                        break;
                }
            }
            if (mode == ResultMode.Win) BattleWin(player);
            else BattleLose(player);
        }
        static void BattleWin(Character player)
        {
            int heal = (int)(player.MaxHp / 10);
            int originalEXP = player.EXP;
            int gold = (stage * 500) + (appearEnemies.Count * 100);

            player.Hp += heal;
            if (player.Hp > player.MaxHp) player.Hp = player.MaxHp;

            for (int i = 0; i < appearEnemies.Count; i++)
            {
                player.EXP += appearEnemies[i].Level;
            }

            player.Gold += gold;

            stage++;

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Battle!! - Result\n");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Victory");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n던전에서 몬스터 {appearEnemies.Count}마리를 잡았습니다.");
                Console.WriteLine($"체력을 {heal}만큼 회복합니다.");
                Console.WriteLine($"Lv.{player.Level} {player.Name}");
                Console.WriteLine($"HP {player.Hp - heal} => {player.Hp}");
                Console.WriteLine($"EXP {originalEXP} => {player.EXP}");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("[획득 보상]");
                Console.WriteLine($"{gold} Gold");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"{stage - 1} 스테이지 클리어!\n");

                foreach (Quest quest in QuestManager.Quests)
                {
                    if (quest.IsClear)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine($"퀘스트 : {quest.QuestName} 달성!");
                        Console.ResetColor();
                    }
                }

                int input = GameSystem.Select(zeroSelection: "0.다음", question: "\n>>");

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
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("레벨업!");
                            Console.WriteLine($"레벨이 {player.Level - 1}에서 {player.Level}이 되었습니다!");
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine($"공격력 : {player.TotalAttack - 0.5f}{(player.EquipAttack != 0 ? $"(+{player.EquipAttack})" : "")} => {player.TotalAttack}{(player.EquipAttack != 0 ? $"(+{player.EquipAttack})" : "")}");
                            Console.WriteLine($"방어력 : {player.TotalDefense - 1}{(player.EquipDefense != 0 ? $"(+{player.EquipDefense})" : "")} => {player.TotalDefense}{(player.EquipDefense != 0 ? $"(+{player.EquipDefense})" : "")}");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"체력을 {heal}만큼 회복합니다!");
                            Console.WriteLine($"{player.Hp - heal} => {player.Hp}");
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("\n아무 키나 누르면 계속합니다...");
                            Console.ReadKey();
                        }
                        else break;
                    }
                    break;
                }
            }
            return;
        }

        static void BattleLose(Character player)
        {
            lose = true;
            string[] lastStats = new string[] { player.Level.ToString(), player.TotalAttack.ToString(), player.TotalDefense.ToString(), player.MaxHp.ToString() };
            int originalHp = player.Hp;
            int gold = (int)((player.Level * 500));

            player.Level = 1;
            player.EXP = 0;
            player.Gold += gold;

            stage = 1;

            switch (player.Job)
            {
                case PlayerClass.Warrior:
                    player.Attack = 15;
                    player.Defense = 10;
                    player.Hp = 120;
                    player.MaxHp = 120;
                    break;
                case PlayerClass.Wizard:
                    player.Attack = 20;
                    player.Defense = 5;
                    player.Hp = 80;
                    player.MaxHp = 80;
                    break;
                case PlayerClass.Thief:
                    player.Attack = 12;
                    player.Defense = 7;
                    player.Hp = 100;
                    player.MaxHp = 100;
                    break;
            }

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Battle!! - Result\n");
                Console.WriteLine("You Lose");
                Console.WriteLine($"Lv.{player.Level} {player.Name}");
                Console.WriteLine($"HP {player.MaxHp} => {originalHp}");
                foreach (Quest quest in QuestManager.Quests)
                {
                    if (quest.IsClear)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine($"퀘스트 : {quest.QuestName} 달성!");
                        Console.ResetColor();
                    }
                }
                int input = GameSystem.Select(zeroSelection: "0.다음", question: "\n>>");

                if (input == 0)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("패배했습니다.\n");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("[최종 능력치]");
                    Console.WriteLine($"Lv.{player.Level} {player.Name}");
                    Console.WriteLine($"공격력 : {player.TotalAttack}");
                    Console.WriteLine($"방어력 : {player.TotalDefense}");
                    Console.WriteLine($"HP {player.MaxHp}");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\n레벨이 1로 돌아갑니다.\n");
                    Console.WriteLine($"Lv : {lastStats[0]} => {player.Level}");
                    Console.WriteLine($"공격력 : {lastStats[1]} => {player.TotalAttack}");
                    Console.WriteLine($"방어력 : {lastStats[2]} => {player.TotalDefense}");
                    Console.WriteLine($"HP : {lastStats[3]} => {player.MaxHp}");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"레벨에 대한 보상으로 {gold} Gold를 획득했습니다.");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("\n아무 키나 누르면 계속합니다...");
                    Console.ResetColor();
                    Console.ReadKey();
                    return;
                }
            }
        }
    }
}