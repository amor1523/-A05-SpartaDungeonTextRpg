﻿using SpartaDungeonTextRpg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _A05_SpartaDungeonTextRpg
{

    // 필요한 정보를 저장
    public class PlayerData
    {
        public string Name;
        public Job Job;
        public int Level;
        public int Atk;
        public int NonEquipAtk;
        public int Def;
        public int NonEquipDef;
        public int Hp;
        public int MaxHp;
        public int Mp;
        public int MaxMp;
        public float Gold;
        public int Exp;
        public int LevelUpExp;

        public PlayerData()
        {
        }

        // SaveData 시 사용
        public PlayerData(Player player)
        {
            Name = player.Name;
            Job = player.Job;
            Level = player.Level;
            Atk = player.Atk;
            NonEquipAtk = player.NonEquipAtk;
            Def = player.Def;
            NonEquipDef = player.NonEquipDef;
            Hp = player.Hp;
            MaxHp = player.MaxHp;
            Mp = player.Mp;
            MaxMp = player.MaxMp;
            Gold = player.Gold;
            Exp = player.Exp;
            LevelUpExp = player.LevelUpExp;
        } 

    }

    public class ItemData
    {
        public List<Item> ItemIndex;
        public List<Item> InventoryIndex; 

        public ItemData()
        {
        }

        public ItemData(Item item)
        {
            ItemIndex = Item.ItemIndex;
            InventoryIndex = Item.InventoryIndex;
        }
    }

    public class PotionData
    {
        public List<Potion> PotionIndex;


        public PotionData()
        {

        }

        public PotionData(Potion potion)
        {
            PotionIndex = potion.PotionIndex;
        }
    }

    public class QuestSave // questData 변수가 존재해서 Save로 변환
    {
        public List<Quest> questData;
        public List<Quest> questList;


        public QuestSave()
        {

        }

        public QuestSave(Quest quest)
        {
            questData = Quest.questData;
            questList = quest.questList;
        }
    }
}
