﻿using System;
using _A05_SpartaDungeonTextRpg;
using SpartaDungeonTextRpg;

public class Skill
{
    public string Name { get; set; }
    public int MpCost { get; set; }
    public int SkillDamage { get; set; }
    public int SkillNum { get;  }
    
    public Skill(string name, int mpCost, int skillDamage,  int skillNum)
    {

        Name = name;
        MpCost = mpCost;
        SkillDamage = skillDamage;
        SkillNum = skillNum;

    }
    public class KnightSkill : Skill
    {
        public KnightSkill(string name, int mpCost, int skillDamage , int skillNum) : base(name, mpCost, skillDamage , skillNum)
        {
            // 전사 전용 스킬의 추가 초기화 로직
            name = "크게휘두르기";
            mpCost = 20;
            skillDamage = 30;
            skillNum = 1;
        }
    }

    public class MageSkill : Skill
    {
        public MageSkill(string name, int mpCost, int skillDamage, int skillNum) : base(name, mpCost, skillDamage, skillNum)
        {
            // 마법사 전용 스킬의 추가 초기화 로직
            name = "파이어 볼";
            mpCost = 35;
            skillDamage = 40;
        }
    }

    public class ArcherSkill : Skill
    {
        public ArcherSkill(string name, int mpCost, int skillDamage, int skillNum) : base(name, mpCost, skillDamage, skillNum)
        {
            // 궁수 전용 스킬의 추가 초기화 로직
            name = "트리플 샷";
            mpCost = 25;
            skillDamage = 30;
        }
    }
    public void UseSkill()
    {
        Console.WriteLine($"\n플레이어가 {Name}을(를) 사용했습니다.");
        Thread.Sleep(500);
        Console.WriteLine($"MP 소모: {MpCost}");
        Thread.Sleep(500);
        Console.WriteLine($"스킬 데미지: {SkillDamage}");
        Thread.Sleep(500);

        if (Quest.questData[2].Count < Quest2.quest2.GoalCount)
        {
            Quest.questData[2].Count++;
        }
    }

}
