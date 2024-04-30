using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace _A05_SpartaDungeonTextRpg
{
    internal class Monster
    {
        public int Id { get; }
        public string Name { get; }
        public int Level { get; }
        public int Atk { get; }
        public int Hp { get; set; }
        public int RewardGold { get; }
        public int RewardExp { get; }
        public bool IsDead { get; set; } // 죽었을 경우 비활성화 하기 위한 bool

        public Monster(int id, string name, int level, int atk, int hp, int gold, int exp, bool isDead = false)
        {
            Id = id;
            Name = name;
            Level = level;
            Atk = atk;
            Hp = hp;
            RewardGold = gold;
            RewardExp = exp;
            IsDead = isDead;
        }

    }
}
