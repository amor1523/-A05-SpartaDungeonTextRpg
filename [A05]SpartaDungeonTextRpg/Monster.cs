using System;
using System.Collections.Generic;

namespace _A05_SpartaDungeonTextRpg
{
    public class Monster
    {
        public List<Monster> MonsterData = new List<Monster>(); // 몬스터 정보
        public List<Monster> CreatedMonster = new List<Monster>(); // 몬스터 랜덤 생성 리스트
        private Random random = new Random();

        public void Monsters(int level) // 몬스터 데이터 입력
        {
            MonsterData.Add(new Monster(0, "고블린", level, 10 + level + random.Next(-1, 2), 30 + level * 2 + random.Next(-1, 2), 100 + level * 2 + random.Next(-1, 2), 40 + level * 2, false, false));
            MonsterData.Add(new Monster(1, "늑대", level, 12 + level + random.Next(-1, 2), 20 + level * 2 + random.Next(-1, 2), 80 + level * 2 + random.Next(-1, 2), 35 + level * 2, false, false));
            MonsterData.Add(new Monster(2, "박쥐", level, 15 + level + random.Next(-1, 2), 15 + level * 1 + random.Next(-1, 2), 70 + level * 2 + random.Next(-1, 2), 20 + level * 2, false, false));
            MonsterData.Add(new Monster(3, "거미", level, 8 + level + random.Next(-1, 2), 10 + level + random.Next(-1, 2), 50 + level * 2 + random.Next(-1, 2), 10 + level * 2, false, false));
            MonsterData.Add(new Monster(4, "오크", level, 18 + level + random.Next(-1, 3), 35 + level * 2 + random.Next(-1, 2), 150 + level * 2 + random.Next(-1, 2), 50 + level * 2, false, false));
            MonsterData.Add(new Monster(5, "위습", level, 15 + level + random.Next(-1, 3), 18 + level * 2 + random.Next(-1, 2), 60 + level * 2 + random.Next(-1, 2), 40 + level * 2, false, false));
        }

        Monster m;
        public void GenerateMonster() //몬스터 생성 메서드
        {
            int monsterNumber = random.Next(1, 4); // 무작위로 1에서 3 사이의 몬스터 수 생성

            for (int i = 0; i < monsterNumber; i++)
            {
                Monster monsterinfo = MonsterData[random.Next(0, MonsterData.Count)];
                m = new Monster(monsterinfo.Id, monsterinfo.Name, monsterinfo.Level, monsterinfo.Atk, monsterinfo.Hp, monsterinfo.RewardGold, monsterinfo.RewardExp);
                CreatedMonster.Add(m); // 리스트에 입력
            }
        }


        public int Id { get; }
        public string Name { get; }
        public int Level { get; }
        public int Atk { get; set; }
        public int Hp { get; set; }
        public int MaxHp { get; set; }
        public int RewardGold { get; }
        public int RewardExp { get; }
        public bool IsDead => Hp <= 0; // 죽었을 경우 비활성화 하기 위한 bool
        public bool QuestCount { get; set; }

        public Monster(int id, string name, int level, int atk, int hp, int gold, int exp, bool isDead = false, bool questCount = false)
        {
            Id = id;
            Name = name;
            Level = level;
            Atk = atk;
            Hp = hp;
            RewardGold = gold;
            RewardExp = exp;
            QuestCount = questCount;
        }
        public class BossMonster : Monster
        {
          
            public BossMonster(int id, string name, int level, int atk, int hp, int maxHp, int gold, int exp, bool isDead = false, bool questCount = false)
                : base(id, name, level, atk, hp, gold, exp, isDead, questCount)
            {
                
            }
        }
        public Monster()
        {

        }

        public void TakeDamage(int damage)
        {
            Hp -= damage;
            if (Hp < 0)
                Hp = 0;
        }


    }
}