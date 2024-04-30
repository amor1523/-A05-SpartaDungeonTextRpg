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
        // Level Atk Def 레벨업에 따른 수치 변경으로 인해 set 추가
        public int Level { get; set; }
        public int Atk { get; set; }
        public int Def { get; set; }
        public int Hp { get; set; }
        public float Gold { get; set; }
        public bool IsDead => Hp <= 0;
        // Exp, LevelUpExp 변수 추가
        public int Before_Exp { get; set; }
        public int After_Exp { get; set; }
        public int LevelUpExp { get; set; }


        public Player()
        {
            // 초기값
            Level = 1;
            Gold = 1500f;
            Before_Exp = 0;
            After_Exp = 0;
            LevelUpExp = 10;
        }

        public void TakeDamage(int damage)
        {
            Hp -= damage;
            if (Hp < 0)
                Hp = 0;
        }
      

    }
}
    




