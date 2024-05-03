using _A05_SpartaDungeonTextRpg;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using static Skill;


namespace SpartaDungeonTextRpg
{
    public class GameManager
    {
        public bool GamePlay = true;

        Dictionary<Job, string> dict = new Dictionary<Job, string>() // 직업출력 딕셔너리
        {
            {Job.Knight, "전사"},
            {Job.Mage, "마법사"},
            {Job.Archer, "궁수"}
        };
        public static GameManager gamemanager;
        private Player player;
        private List<Monster> monsters;
        private Random random = new Random();
        private Battle battle;
        private Item item;
        private Potion potion;
        private Skill skill;
        private Quest quest = new Quest();

        public GameManager()
        {
            InitializeGame();
            JsonSerialize.LoadData(this, player, item, potion, quest);
            
            MainMenu();
            battle = new Battle(player, monsters, this, skill, potion);
        }

        private void InitializeGame()
        {
            player = new Player();
            monsters = new List<Monster>(); // 몬스터 리스트 초기화

            potion = new Potion(player);
            item = new Item(player, potion);
            potion.GetPotion();
            item.GetItem();

            switch (player.Job)
            {
                case Job.Knight:
                    skill = new KnightSkill("크게휘두르기", 20, 30, 1);
                    break;
                case Job.Mage:
                    skill = new MageSkill("파이어 볼", 35, 40, 1);
                    break;
                case Job.Archer:
                    skill = new ArcherSkill("트리플 샷", 25, 30, 1);
                    break;
            }

            // 몬스터 생성 및 추가
            Monster monster = new Monster();
            monster.Monsters(player.Level); // 플레이어 레벨에 맞게 몬스터 생성
            monster.GenerateMonster(); // 몬스터 생성
            monsters.AddRange(monster.CreatedMonster); // 생성된 몬스터를 리스트에 추가
            
            quest.quests();
        }

        public void PlayerName()
        {
            Console.Clear();
            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
            Console.Write("원하시는 이름을 설정해주세요\n>> ");
            player.Name = Console.ReadLine();
            PlayerJob();
        }

        public void PlayerJob()
        {
            Console.Clear();
            Console.WriteLine("던전에 들어가기 전 당신의 직업을 선택해주세요.");
            Console.WriteLine("직업마다 기본 스탯과 스킬이 다를 수 있습니다.");
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Blue, "", "[1] ", "전사   | Atk: 10, Def: 5, Hp: 100");
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Blue, "", "[2] ", "마법사 | Atk: 8, Def: 3, Hp: 80");
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Blue, "", "[3] ", "궁수 | Atk: 13, Def: 4, Hp: 90");

            int input = ConsoleUtility.PromptMenuChoice(1, 3);
            switch (input)
            {
                case 1:
                    player.Job = Job.Knight;
                    player.Atk = 100;
                    player.Def = 5;
                    player.Hp = 100;
                    player.MaxHp = player.Hp;
                    player.NonEquipAtk = player.Atk;
                    player.NonEquipDef = player.Def;
                    player.Mp = 50;
                    player.MaxMp = player.Mp;
                    break;

                case 2:
                    player.Job = Job.Mage;
                    player.Atk = 8;
                    player.Def = 3;
                    player.Hp = 80;
                    player.MaxHp = player.Hp;
                    player.NonEquipAtk = player.Atk;
                    player.NonEquipDef = player.Def;
                    player.Mp = 100;
                    player.MaxMp = player.Mp;
                    break;
                case 3:
                    player.Job = Job.Archer;
                    player.Atk = 13;
                    player.Def = 4;
                    player.Hp = 90;
                    player.MaxHp = player.Hp;
                    player.NonEquipAtk = player.Atk;
                    player.NonEquipDef = player.Def;
                    player.Mp = 40;
                    player.MaxMp = player.Mp;
                    break;
            }
        }

        public void MainMenu()
        {
            Console.Clear();
            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
            Console.WriteLine("이제 전투를 시작할 수 있습니다.\n");
            Console.WriteLine("1. 상태보기");
            Console.WriteLine("2. 전투시작");
            Console.WriteLine("3. 인벤토리");
            Console.WriteLine("4. 상점");
            Console.WriteLine("5. 물약사용");
            Console.WriteLine("6. 퀘스트");
            Console.WriteLine("7. 게임종료");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("0. 저장하기");
            Console.WriteLine();

            int input = ConsoleUtility.PromptMenuChoice(0, 7);
            switch (input)
            {
                case 0:
                    JsonSerialize.SaveData(player, item, potion, quest);
                    break;
                case 1:
                    StatusMenu();
                    break;

                case 2:
                    battle = new Battle(player, monsters, this, skill, potion);
                    battle.BossStage();
                    break;

                case 3:
                    item.Inventory();
                    break;

                case 4:
                    item.Shop();
                    break;
                case 5:
                    potion.PotionInventory();
                    break;
                case 6:
                    quest.QuestList(player, Quest.questData);
                    break;
                case 7:
                    GamePlay = false;
                    Console.WriteLine("\n게임을 종료합니다.");
                    break;
            }
        }

        public void StatusMenu()
        {
            player.Atk = player.NonEquipAtk;
            player.Def = player.NonEquipDef;

            bool flagEquipArmor = false;
            bool flagEquipWeapon = false;

            int equipArmorPower = 0;
            int equipWeaponPower = 0;

            int equipAtk = 0;
            int equipDef = 0;

            Console.Clear();
            ConsoleUtility.ShowTitle("[상태보기]");
            Console.WriteLine("캐릭터의 정보를 표시합니다.\n");
            Console.WriteLine($"Lv. {player.Level}");
            Console.WriteLine($"{player.Name} ({dict[player.Job]})");

            if (Item.InventoryIndex.Count != 0)
            {
                foreach (var equip in Item.InventoryIndex)
                {
                    if (equip.FlagEquip)
                    {
                        if (equip.AttackPower != 0)
                        {
                            flagEquipWeapon = true;
                            equipWeaponPower += equip.AttackPower;
                        }
                        if (equip.DefensivePower != 0)
                        {
                            flagEquipArmor = true;
                            equipArmorPower += equip.DefensivePower;
                        }
                    }
                }
                equipAtk = player.Atk + equipWeaponPower;
                equipDef = player.Def + equipArmorPower;
            }

            if (!flagEquipWeapon)
                Console.WriteLine($"공격력 : {player.Atk}");
            else
                Console.WriteLine($"공격력 : {player.Atk} (+{equipWeaponPower})");
            if (!flagEquipArmor)
                Console.WriteLine($"방어력 : {player.Def}");
            else
                Console.WriteLine($"방어력 : {player.Def} (+{equipArmorPower})");
            Console.WriteLine($"체력 : {player.Hp}");
            Console.WriteLine($"Gold : {player.Gold} G");
            Console.WriteLine($"Exp : {player.Exp}");
            Console.WriteLine($"LevelUp까지 남은 Exp -> {player.LevelUpExp - player.Exp}\n");

            Console.WriteLine("0. 나가기\n");

            player.Atk = equipAtk;
            player.Def = equipDef;

            int input = ConsoleUtility.PromptMenuChoice(0, 0);
            switch (input)
            {
                case 0:
                    return;
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            GameManager gameManager = new GameManager();

            while (gameManager.GamePlay)
                while (gameManager.GamePlay)
                {
                    gameManager.MainMenu();
                }
        }
    }
}
