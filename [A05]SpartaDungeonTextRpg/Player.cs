using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace _A05_SpartaDungeonTextRpg
{
    internal class Player
    {
        public string Name { get; }
        public string Job { get; }

        // Level Atk Def 레벨업에 따른 수치 변경으로 인해 set 추가
        public int Level { get; set; }
        public int Atk { get; set; }
        public int Def { get; set; }
        public int Hp { get; }
        public float Gold { get; set; }

        // Exp, LevelUpExp 변수 추가
        public int Before_Exp { get; set; }
        public int After_Exp { get; set; }
        public int LevelUpExp { get; set; }

        public Player(string name, string job, int level, int atk, int def, int hp, float gold)
        {
            Name = name;
            Job = job;
            Level = level;
            Atk = atk;
            Def = def;
            Hp = hp;
            Gold = gold;

            // 초기값
            Before_Exp = 0;
            After_Exp = 0;
            LevelUpExp = 10;
        }
    }
}
