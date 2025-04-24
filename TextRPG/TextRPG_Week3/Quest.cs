using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_Week3
{
    public class Quest
    {
        public string QuestName { get; set; }
        public string QuestDescription { get; set; }
        public int Reword { get; set; }
        public bool IsClear { get; set; }
        public int ClearCount { get; set; }

        public Quest() { }

        public Quest(string questName, string questDescription, int reword, bool isClear = false, int clearCount = 0)
        {
            QuestName = questName;
            QuestDescription = questDescription;
            Reword = reword;
            IsClear = isClear;
            ClearCount = clearCount;
        }
    }

    public class DungeonQuest : Quest
    {
        public int RequiredStage { get; set; }
        public int ReachedStage { get; set; }

        public DungeonQuest(string questName, string questDescription, int reword, int requiredStage, int reachedStage = 0)
            : base(questName, questDescription, reword)
        {
            RequiredStage = requiredStage;
            ReachedStage = reachedStage;
        }
    }

    public class DefeatQuest : Quest
    {
        public int RequiredDefeatCount { get; set; }
        public int DefeatCount { get; set; }

        public Func<Enemy, bool> Target { get; set; }
        public DefeatQuest(string questName, string questDescription,int reword, int requiredDefeatCount, Func<Enemy, bool> target, int defeatCount = 0)
            : base(questName, questDescription, reword)
        {
            RequiredDefeatCount = requiredDefeatCount;
            Target = target;
            DefeatCount = defeatCount;
        }
    }

    public static class QuestManager
    {
        public static List<Quest> Quests { get; set; } = new List<Quest>
        {
        new DungeonQuest("던전 도달!", "던전에 들어가 10층에 도달하자!", 5000, 10),
        new DungeonQuest("던전 타파!", "던전에 들어가 20층에 도달하자!", 10000, 20),
        new DefeatQuest("미니언 해치우기!", "미니언을 10마리 쓰러뜨리자!", 1000, 10, target: enemy => enemy.Name == "미니언"),
        new DefeatQuest("보스 격파자!", "아무 보스나 3번 처치하자!", 50000,3, target: enemy => BattleSystem.bossList.Contains(enemy))
        };
    }
}
