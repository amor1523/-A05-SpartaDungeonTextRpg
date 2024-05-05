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
        public int NonEquipAtk { get; set; }
        public int Def { get; set; }
        public int NonEquipDef { get; set; }
        public int Hp { get; set; }
        public int MaxHp { get; set; }
        public int Mp { get; set; }
        public int MaxMp { get; set; }
        public float Gold { get; set; }
        public bool IsDead => Hp <= 0;
        // Exp, LevelUpExp 변수 추가
        public int Exp { get; set; }
        public int LevelUpExp { get; set; }

        public Player()
       {
            // 초기값
            Level = 1;
            Gold = 150000f;
            LevelUpExp = 200;
        }

        public void TakeDamage(int damage)
        {
            Hp -= damage;
            if (Hp < 0)
                Hp = 0;
        }
        public void UseMp(int mana)
        {
            if (Mp < mana)
            {
                Mp = Mp;
            }
            else{
                Mp -= mana;
            }

        }

        // 역직렬화 후 Player에게 PlayerData를 넘겨주기 위한 메서드 (LoadData시 사용)
        public void SetPlayer(PlayerData playerData)
        {
            Name = playerData.Name;
            Job = playerData.Job;
            Level = playerData.Level;
            Atk = playerData.Atk;
            NonEquipAtk = playerData.NonEquipAtk;
            Def = playerData.Def;
            NonEquipDef = playerData.NonEquipDef;
            Hp = playerData.Hp;
            MaxHp = playerData.MaxHp;
            Mp = playerData.Mp;
            MaxMp = playerData.MaxMp;
            Gold = playerData.Gold;
            Exp = playerData.Exp;
            LevelUpExp = playerData.LevelUpExp;
        }

    }
}
    




