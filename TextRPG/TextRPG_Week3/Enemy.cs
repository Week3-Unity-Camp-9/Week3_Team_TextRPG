using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_Week3
{
    public class Enemy
    {
        public int Level;   
        public string Name;
        public int Hp;
        public int Attack;
        public bool IsDead;

        public Enemy(int level, string name, int hp, int attack, bool isDead = false)
        {
            Level = level;
            Name = name;
            Hp = hp;
            Attack = attack;
            IsDead = isDead;
        }
    }
}
