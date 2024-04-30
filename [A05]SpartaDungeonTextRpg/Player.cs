using System;

namespace _A05_SpartaDungeonTextRpg
{
    public class Player
    {   
        Random rand  = new Random();
        public string Name { get; }
        public string Job { get; }
        public int Level { get; }
        public int Atk { get; set; }
        public int Def { get; }
        public int HP { get; set; }
        public float Gold { get; set; }
        public bool IsDead => HP <= 0;

        public Player(string name, string job, int level, int attack, int defense, int health, float gold)
        {
            Name = name;
            Job = job;
            Level = level;
            Atk = attack;
            Def = defense;
            HP = health;
            Gold = gold;
        }
        public void TakeDamage(int damage)
        {
            HP -= damage;
            if (HP < 0)
                HP = 0;
        }
      

    }
}
    




