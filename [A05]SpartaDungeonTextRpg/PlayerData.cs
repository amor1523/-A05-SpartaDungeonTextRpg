using SpartaDungeonTextRpg;
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
        public int Def;
        public int Hp;
        public int Mp;
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
            Def = player.Def;
            Hp = player.Hp;
            Mp = player.Mp;
            Gold = player.Gold;
            Exp = player.Exp;
            LevelUpExp = player.LevelUpExp;
        } 

    }

    public class ItemData
    {
        public bool FlagShopBuy;
        public bool FlagBuy;
        public bool FlagEquip;

        public ItemData(Item item)
        {
            FlagShopBuy = item.FlagShopBuy;
            FlagBuy = item.FlagBuy;
            FlagEquip = item.FlagEquip;
        }

    }
}
