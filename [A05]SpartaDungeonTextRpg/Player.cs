using System;
using System.Numerics;

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
        public int Mp { get; set; }
        public float Gold { get; set; }
        public bool IsDead => Hp <= 0;
        // Exp, LevelUpExp 변수 추가
        public int Exp { get; set; }
        public int LevelUpExp { get; set; }

        public Player()
        {
            // 초기값
            Exp = 0;
            Level = 1;
            Gold = 1500f;
            LevelUpExp = 10;
        }

        public void TakeDamage(int damage)
        {
            Hp -= damage;
            if (Hp < 0)
                Hp = 0;
        }
      
        // 역직렬화 후 Player에게 넘겨주기 위한 메서드
        public Player(PlayerData playerData)
        {
            Name = playerData.Name;
            Job = playerData.Job;
            Level = playerData.Level;
            Atk = playerData.Atk;
            Def = playerData.Def;
            Hp = playerData.Hp;
            Mp = playerData.Mp;
            Gold = playerData.Gold;
            Exp = playerData.Exp;
            LevelUpExp = playerData.LevelUpExp;
        }

    }
}
    




