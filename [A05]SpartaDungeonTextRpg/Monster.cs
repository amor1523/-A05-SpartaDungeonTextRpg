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
        public List<Monster> monsterData = new List<Monster>(); // 몬스터 정보
        public Random random = new Random();

        public void Monsters()
        {
            monsterData.Add(new Monster(0, "고블린", random.Next(1, 3), 20 + Level * 2 + random.Next(-1, 2), 35 + Level * 5 + random.Next(-1, 2), 20 + Level * 2 + random.Next(-1, 2), 50 + Level * 2));
            monsterData.Add(new Monster(1, "늑대", random.Next(1, 3), 15 + Level * 2 + random.Next(-1, 2), 20 + Level * 5 + random.Next(-1, 2), 18 + Level * 2 + random.Next(-1, 2), 40 + Level * 2));
            monsterData.Add(new Monster(2, "박쥐", random.Next(1, 3), 20 + Level * 2 + random.Next(-1, 2), 15 + Level * 3 + random.Next(-1, 2), 18 + Level * 2 + random.Next(-1, 2), 35 + Level * 2));
            monsterData.Add(new Monster(3, "거미", random.Next(1, 3), 10 + Level * 2 + random.Next(-1, 2), 10 + Level * 3 + random.Next(-1, 2), 10 + Level * 2 + random.Next(-1, 2), 15 + Level * 2));
        }


        public int Id { get; }
        public string Name { get; }
        public int Level { get; }
        public int Atk { get; }
        public int Hp { get; }
        public int RewardGold { get; }
        public int RewardExp { get; set; }
        public bool IsDead {  get; set; } // 죽었을 경우 비활성화 하기 위한 bool

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
