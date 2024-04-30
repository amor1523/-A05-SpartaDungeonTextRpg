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
        public List<Monster> MonsterData = new List<Monster>(); // 몬스터 정보
        public List<Monster> CreatedMonster {  get; set; } // 몬스터 랜덤 생성 리스트
        public Random random = new Random();
        
        public void Monsters() // 몬스터 데이터 입력
        {
            MonsterData.Add(new Monster(0, "고블린", random.Next(1, 4), 20 + Level * 2 + random.Next(-1, 2), 35 + Level * 5 + random.Next(-1, 2), 20 + Level * 2 + random.Next(-1, 2), 50 + Level * 2, false));
            MonsterData.Add(new Monster(1, "늑대", random.Next(1, 4), 15 + Level * 2 + random.Next(-1, 2), 20 + Level * 5 + random.Next(-1, 2), 18 + Level * 2 + random.Next(-1, 2), 40 + Level * 2, false));
            MonsterData.Add(new Monster(2, "박쥐", random.Next(1, 4), 20 + Level * 2 + random.Next(-1, 2), 15 + Level * 3 + random.Next(-1, 2), 18 + Level * 2 + random.Next(-1, 2), 35 + Level * 2, false));
            MonsterData.Add(new Monster(3, "거미", random.Next(1, 4), 10 + Level * 2 + random.Next(-1, 2), 10 + Level * 3 + random.Next(-1, 2), 10 + Level * 2 + random.Next(-1, 2), 15 + Level * 2, false));
        }

        int monsterNumber = 3;
        Monster m;
        public void GenerateMonster() //몬스터 생성 메서드
        {
            for (int i = 0; i < monsterNumber; i++)
            {
                Monster monsterinfo = MonsterData[random.Next(0, 4)];
                m = new Monster(monsterinfo.Id,monsterinfo.Name,monsterinfo.Level,monsterinfo.Atk,monsterinfo.Hp,monsterinfo.RewardGold,monsterinfo.RewardExp);
            }
            CreatedMonster.Add(m); // 리스트에 입력
        }



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
