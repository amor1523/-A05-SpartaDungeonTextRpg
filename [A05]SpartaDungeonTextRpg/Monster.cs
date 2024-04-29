using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace _A05_SpartaDungeonTextRpg
{
    internal class Monster
    {
        public string Name { get; }
        public int Level { get; }
        public int Atk { get; }
        public int Hp { get; }
        public int RewardGold { get; }
        public int RewardExp { get; set; }
        public bool IsDead {  get; set; } // 죽었을 경우 비활성화 하기 위한 bool

        public Random random = new Random();

        public Monster(string name, int level, int atk, int hp, int gold, int exp)
        {
            Name = name;
            Level = level;
            Atk = atk;
            Hp = hp;
            RewardGold = gold;
            RewardExp = exp;
        }
    }

    class Goblin : Monster
    {
        public Goblin(string name, int level, int atk, int hp, int gold, int exp) : base(name, level, atk, hp, gold, exp)
        {
            name = "고블린";
            level = random.Next(1,3);
            atk = 20 + level * 2 + random.Next(-1, 2);
            hp = 35 + level * 5 + random.Next(-1, 2);
            gold = 20 + level * 2 + random.Next(-1, 2);
            exp = 50 + level * 2;
        }
    }

    class Wolf : Monster
    {
        public Wolf(string name, int level, int atk, int hp, int gold, int exp) : base(name, level, atk, hp, gold, exp)
        {
            name = "늑대";
            level = random.Next(1, 3);
            atk = 15 + level * 2 + random.Next(-1, 2);
            hp = 20 + level * 5 + random.Next(-1, 2);
            gold = 18 + level * 2 + random.Next(-1, 2);
            exp = 40 + level * 2;
        }
    }

    class Bat : Monster
    {
        public Bat(string name, int level, int atk, int hp, int gold, int exp) : base(name, level, atk, hp, gold, exp)
        {
            name = "박쥐";
            level = random.Next(1, 3);
            atk = 20 + level * 2 + random.Next(-1, 2);
            hp = 15 + level * 3 + random.Next(-1, 2);
            gold = 18 + level * 2 + random.Next(-1, 2);
            exp = 35 + level * 2;
        }
    }

    class Spider : Monster
    {
        public Spider(string name, int level, int atk, int hp, int gold, int exp) : base(name, level, atk, hp, gold, exp)
        {
            name = "거미";
            level = random.Next(1, 3);
            atk = 10 + level * 2 + random.Next(-1, 2);
            hp = 10 + level * 3 + random.Next(-1, 2);
            gold = 10 + level * 2 + random.Next(-1, 2);
            exp = 15 + level * 2;
        }
    }
}
