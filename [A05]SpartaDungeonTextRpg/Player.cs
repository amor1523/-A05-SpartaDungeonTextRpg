using System;

namespace _A05_SpartaDungeonTextRpg
{
    public enum Job
    {
        Knight,
        Mage,
        Archer
    }

    public class Player
    {   
        Random rand  = new Random();
        public string Name { get; set; }
        public Job Job { get; set; }
        public int Level { get; }
        public int Atk { get; set; }
        public int Def { get; set; }
        public int HP { get; set; }
        public float Gold { get; set; }
        public bool IsDead => HP <= 0;

        public Player()
        {
            Level = 1;
            Gold = 1500f;
            
      
        }

        public void TakeDamage(int damage)
        {
            HP -= damage;
            if (HP < 0)
                HP = 0;
        }
      

    }
}
    




