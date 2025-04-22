using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_Week3
{
    public class Character
    {
        public string Name { get; set; } = "이름";
        public string Job { get; set; } = "직업";
        public int Level { get; set; } = 1;
        public float Attack { get; set; } = 10;
        public int Defense { get; set; } = 5;
        public int Health { get; set; } = 100;
        public int Gold { get; set; } = 1500;

        public int EquipAttack { get; set; } = 0;
        public int EquipDefense { get; set; } = 0;
        public int EquipHealth { get; set; } = 0;

        public float TotalAttack => Attack;

        public int TotalDefence => Defense;

        public int TotalHealth => Health;

        public int FullHealth = 100;

        public void DisplayStatus(Character player)
        {
            Console.WriteLine($"Lv. 0{player.Level}");
            Console.WriteLine($"{player.Name} ( {player.Job} )");
            Console.WriteLine($"공격력 : {player.TotalAttack}" + (player.EquipAttack > 0 ? $" (+{player.EquipAttack})" : ""));
            Console.WriteLine($"방어력 : {player.TotalDefence}" + (player.EquipDefense > 0 ? $" (+{player.EquipDefense})" : ""));
            Console.WriteLine($"공격력 : {player.TotalHealth}" + (player.EquipHealth > 0 ? $" (+{player.EquipHealth})" : ""));
            Console.WriteLine($"Gold   : {player.Gold} G");
        }
    }
}
