using System;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using _A05_SpartaDungeonTextRpg;
using SpartaDungeonTextRpg;
public class Battle
{
    Dictionary<Job, string> dict = new Dictionary<Job, string>()
        {
            {Job.Knight, "전사"},
            {Job.Mage, "마법사"},
            {Job.Archer, "궁수"}
        };

    private Player player;
    private Potion potion;
    private List<Monster> monsters;
    private Random random = new Random();
    private GameManager gameManager;
    private Skill skill;
    private int beforeHp;
    private int beforeExp;
    private int beforeMp;

    public Battle(Player player, List<Monster> monsters, GameManager gameManager, Skill skill, Potion potion)
    {
        this.player = player;
        this.monsters = monsters;
        this.gameManager = gameManager;
        this.skill = skill;
        this.potion = potion;
        beforeHp = player.Hp;
        beforeExp = player.Exp;
        beforeMp = player.Mp;
        this.potion = potion;
        // 프로그램에 있는 몬스터를 제거하고 여기에 새로운 몬스터를 추가할 수 있습니다.
    }
    public void BattleMenu()
    {
        Console.Clear();
        ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "Battle!!\n");

        DeadMonster();

        Console.WriteLine();
        Console.WriteLine("[내정보]");
        Console.WriteLine($"Lv.{player.Level}  {player.Name} ({dict[player.Job]})");
        Console.WriteLine($"HP {player.Hp}\n");
        Console.WriteLine("1. 일반 공격");
        Console.WriteLine("2. 스킬 사용");
        Console.WriteLine("3. 도망 치기");

        int input = ConsoleUtility.PromptMenuChoice(1, 3);
        switch (input)
        {
            case 1:
                PlayerAttack();
                break;
            case 2:
                SkillAttack();
                break;
            case 3:
                RunAway();
                break;
        }
    }
    public void SkillAttack()
    {
        Console.Clear();
        ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "Battle!!\n");
        Thread.Sleep(500);

        Console.WriteLine("전투 중인 몬스터들:");

       DeadMonster();

        Console.WriteLine();
        Console.WriteLine("[내정보]");
        Console.WriteLine($"Lv.{player.Level}  {player.Name} ({dict[player.Job]})");
        Console.WriteLine($"HP {player.Hp}/{beforeHp} ");
        Console.WriteLine($"MP {player.Mp}/{beforeMp}\n");

        Console.WriteLine("사용가능 스킬\n");
        Console.WriteLine("0.취소\n");
        for (int j = 0; j < skill.SkillNum; j++)
        {
            Console.WriteLine($"{skill.SkillNum}.{skill.Name}은 적에게 {skill.SkillDamage}를 입힌다.\n");
        }

        int input = ConsoleUtility.PromptMenuChoice(0, skill.SkillNum);
        if (input == 0)
        {
            Console.Clear();
            BattleMenu();
            return;
        }
        Console.WriteLine("공격할  몬스터\n");


        int input2 = ConsoleUtility.PromptMenuChoice(0, monsters.Count);
        // 선택한 몬스터 인덱스
        int deadMonsterIdx = input2 - 1;
        // 공격할 몬스터
        Monster selectedMonster = monsters[deadMonsterIdx];

        // 선택한 몬스터가 이미 죽은 상태인지 확인
        if (selectedMonster.IsDead)
        {
            Console.WriteLine("잘못된 입력입니다.\n");
            Thread.Sleep(1000);
            SkillAttack(); // 다시 공격 메뉴로 돌아감
            return;
        }



        int usedMana = skill.MpCost;
        if (player.Mp < usedMana)
        {
            Console.WriteLine("마나가 부족하여 스킬을 사용할수없습니다.");
            Thread.Sleep(1000);
            SkillAttack();
        }
        skill.UseSkill();
        int damageDealt = skill.SkillDamage;
        selectedMonster.TakeDamage(damageDealt);
        player.UseMp(usedMana);
        Thread.Sleep(1000);

        Console.Clear();
        Console.WriteLine();
        Console.WriteLine($"{skill.Name}!!!");
        Console.WriteLine($"Lv.{selectedMonster.Level} {selectedMonster.Name}");
        Thread.Sleep(500);
        Console.WriteLine($"HP {selectedMonster.Hp}\n");
        Thread.Sleep(1000);
        Console.WriteLine("0.다음");
        int inputs = ConsoleUtility.PromptMenuChoice(0, 0);
        if (inputs == 0)
        {
            // 몬스터들이 플레이어를 한 번씩 공격
            foreach (Monster monster in this.monsters)
            {
                Console.Clear();
                EnemyAttack(monster);
            }

            // 모든 몬스터가 죽었을 때 승리 처리
            if (this.monsters.All(m => m.IsDead))
            {
                foreach (var monster in monsters)
                    player.Exp += monster.RewardExp;
                BattleResult(true);
            }
            else
            {
                // 모든 몬스터가 공격한 후에 플레이어가 살아있는지 확인
                if (!player.IsDead)
                {
                    BattleMenu();
                }
                else
                {
                    BattleResult(false);
                }
            }
        }
    }
    public void PlayerAttack()
    {

        Console.Clear();
        ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "Battle!!\n");
        Thread.Sleep(500);

        DeadMonster();

        Console.WriteLine();
        Console.WriteLine("[내정보]");
        Console.WriteLine($"Lv.{player.Level}  {player.Name} ({dict[player.Job]})");
        Console.WriteLine($"HP {player.Hp}\n");
        Console.WriteLine("0.취소");

        int input = ConsoleUtility.PromptMenuChoice(0, monsters.Count);
        if (input == 0)
        {
            Console.Clear();
            BattleMenu();
            return;
        }
        // 선택한 몬스터 인덱스
        int deadMonsterIdx = input - 1;
        // 공격할 몬스터
        Monster selectedMonster = monsters[deadMonsterIdx];

        // 선택한 몬스터가 이미 죽은 상태인지 확인
        if (selectedMonster.IsDead)
        {
            Console.WriteLine("잘못된 입력입니다.\n");
            Thread.Sleep(1000);
            PlayerAttack(); // 다시 공격 메뉴로 돌아감
            return;
        }

        int baseDamage = player.Atk;
        int errorDamage = (int)Math.Ceiling(baseDamage * 0.1);
        int minDamage = baseDamage - errorDamage;
        int maxDamage = baseDamage + errorDamage;
        int damageDealt = random.Next(minDamage, maxDamage);


        // 회피 확률
        int missChance = 10;
        // 회피확률 생산
        bool isMiss = random.Next(100) < missChance;
        //회피 발생하면 데미지 0으로 적용
        if (isMiss)
        {
            Console.Clear();
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "Battle!!\n");
            Console.WriteLine($"{player.Name} 의 공격!");
            Thread.Sleep(500);
            damageDealt = 0;
            Console.WriteLine($"Lv.{selectedMonster.Level} {selectedMonster.Name} 을(를) 공격했지만 아무일도  일어나지 않았습니다.\n");
            Thread.Sleep(1000);
            Console.WriteLine("0.다음");
            int input1 = ConsoleUtility.PromptMenuChoice(0, 0);
            if (input1 == 0)
            {
                // 몬스터들이 플레이어를 한 번씩 공격
                foreach (Monster monster in this.monsters)
                {
                    Console.Clear();
                    EnemyAttack(monster);
                }

                // 모든 몬스터가 죽었을 때 승리 처리
                if (this.monsters.All(m => m.IsDead))
                {
                    BattleResult(true);
                }
                else
                {
                    // 모든 몬스터가 공격한 후에 플레이어가 살아있는지 확인
                    if (!player.IsDead)
                    {
                        BattleMenu();
                    }
                    else
                    {
                        BattleResult(false);
                    }
                }
            }
        }
        else
        {
            // 치명타 여부를 결정하기 위한 확률 15퍼
            int critChance = 65;
            // 랜덤한 확률을 생성하여 치명타 여부 결정
            bool isCritical = random.Next(100) < critChance;
            // 치명타 데미지 계산
            int critDamage = (int)(damageDealt * 1.6); // 치명타 데미지는 일반 데미지의 1.6배로 가정
                                                       // 치명타가 발생하면 추가 데미지 적용
            if (isCritical)
            {
                Console.Clear();
                Console.WriteLine($"{player.Name} 의 공격!");
                Thread.Sleep(500);
                damageDealt += critDamage;
                Console.WriteLine($"Lv.{selectedMonster.Level} {selectedMonster.Name} 을(를) 맞췄습니다. [데미지 : {damageDealt}]- 치명타 공격!!");
            }
            else
            {
                Console.WriteLine($"Lv.{selectedMonster.Level} {selectedMonster.Name} 을(를) 맞췄습니다. [데미지 : {damageDealt}]");
            }
            selectedMonster.TakeDamage(damageDealt);
        }
        Thread.Sleep(1000);
        Console.WriteLine();
        Console.WriteLine($"Lv.{selectedMonster.Level} {selectedMonster.Name}");
        Thread.Sleep(500);
        Console.WriteLine($"HP {selectedMonster.Hp}\n");
        Thread.Sleep(1000);
        Console.WriteLine("0.다음");
        int inputs = ConsoleUtility.PromptMenuChoice(0, 0);
        if (inputs == 0)
        {
            // 몬스터들이 플레이어를 한 번씩 공격
            foreach (Monster monster in this.monsters)
            {
                Console.Clear();
                EnemyAttack(monster);
            }

            // 모든 몬스터가 죽었을 때 승리 처리
            if (this.monsters.All(m => m.IsDead))
            {
                beforeExp = player.Exp;
                foreach (var monster in monsters)
                    player.Exp += monster.RewardExp;
                BattleResult(true);
            }
            else
            {
                // 모든 몬스터가 공격한 후에 플레이어가 살아있는지 확인
                if (!player.IsDead)
                {
                    BattleMenu();
                }
                else
                {
                    BattleResult(false);
                }
            }
        }
    }

    public void EnemyAttack(Monster targetMonster)
    {   //공격할 몬스터가  살아있는지 확인
        if (!targetMonster.IsDead)
        {
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "공격!!\n");
            Console.WriteLine($"{targetMonster.Name} 의 공격!");
            Thread.Sleep(500);
            int damageDealt = targetMonster.Atk;
            player.TakeDamage(damageDealt);
            Console.WriteLine($"Lv.{player.Level} {player.Name} 을/를 맞췄습니다. [데미지 : {damageDealt}]");
            Thread.Sleep(1000);
            Console.WriteLine();
            Console.WriteLine($"Lv.{player.Level} {player.Name}");
            Thread.Sleep(500);
            Console.WriteLine($"HP {player.Hp}\n");
            Thread.Sleep(1000);
        }
        else if (targetMonster.IsDead)
        {
            if(targetMonster.Id == 3 && Quest.questData[1].Count < Quest1.quest1.GoalCount && targetMonster.QuestCount == false )
            {
                Quest.questData[1].Count++;
                targetMonster.QuestCount = true;
            }
        }
    }

    public void BattleResult(bool victory)
    {
        monsters.Clear();

        // 여기에 새로운 몬스터를 추가하는 코드를 작성합니다.
        // 예를 들어, 플레이어 레벨에 따라 다른 몬스터를 추가할 수 있습니다.
        Monster monster = new Monster();
        monster.Monsters(player.Level); // 플레이어 레벨에 맞게 몬스터 생성
        monster.GenerateMonster(); // 몬스터 생성
        monsters.AddRange(monster.CreatedMonster); // 생성된 몬스터를 리스트에 추가
        // 레벨업 유무 확인
        int playerLevel = player.Level;
        bool flagLevelUp = LevelUp();

        Console.Clear();
        ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "전투 결과\n");
        if (victory)
        {
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Green, "", "전투 승리\n");
            Console.WriteLine("던전에서 몬스터를 잡았습니다.\n");
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Yellow, "", "[캐릭터 정보]");
            if (!flagLevelUp)
                Console.WriteLine($"Lv.{player.Level} {player.Name}");
            else
                Console.WriteLine($"Lv.{playerLevel} {player.Name} -> Lv. {player.Level} {player.Name}");

            Thread.Sleep(1000);
            Console.WriteLine($"HP {beforeHp} -> {player.Hp}");
            Thread.Sleep(1000);
            Console.WriteLine($"MP {player.Mp} -> {player.Mp + 10}");
            Console.WriteLine($"exp {beforeExp} -> {player.Exp}");
            Thread.Sleep(1000);
            if (player.Level < 5)
            {
                Console.WriteLine($"LevelUp까지 남은 Exp -> {player.LevelUpExp - player.Exp}\n");
                Thread.Sleep(1000);
            }

            RewardItem();

            Thread.Sleep(1000);
            Console.WriteLine("\n0. 다음");
        }
        else
        {
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Red, "", "전투 패배\n");
            Thread.Sleep(1000);
            Console.WriteLine($"Lv.{player.Level} {player.Name}");
            Console.WriteLine($"HP {beforeHp} -> {player.Hp}\n");
            Console.WriteLine("0. 다음\n");
        }

        int input = ConsoleUtility.PromptMenuChoice(0, 0);
        switch (input)
        {
            case 0:
                gameManager.MainMenu();
                break;
        }
    }

    public bool LevelUp()
    {
        bool flagLevelUp;

        if (player.Exp > player.LevelUpExp && (player.Level >= 1 && player.Level <= 4))
        {
            flagLevelUp = true;
            player.NonEquipAtk += 1;
            player.NonEquipDef += 1;

            if (player.Exp >= 200 && player.Exp < 350)
            {
                player.Level = 2;
                player.LevelUpExp = 350;
            }
            else if (player.Exp >= 350 && player.Exp < 500)
            {
                player.Level = 3;
                player.LevelUpExp = 500;
            }
            else if (player.Exp >= 500 && player.Exp < 650)
            {
                player.Level = 4;
                player.LevelUpExp = 650;
            }
            else if (player.Exp >= 650)
                player.Level = 5;
        }
        else
            flagLevelUp = false;

        return flagLevelUp;
    }

    public void RewardItem()
    {
        // 포션 보상 여부를 결정하기 위한 확률 50퍼
        int getPotionChance = 50;
        int addGold = random.Next(200, 501);
        // 랜덤한 확률을 생성하여 포션 여부 결정
        bool isGetPotion = random.Next(100) < getPotionChance;

        ConsoleUtility.PrintTextHighlights(ConsoleColor.Yellow, "", "[획득 아이템]");

        Console.WriteLine($"{addGold} Gold");
        Thread.Sleep(1000);
        player.Gold += addGold;

        if (isGetPotion)
        {
            int addPotion = random.Next(1, 3);
            potion.PotionIndex[0].Count += addPotion;
            Console.WriteLine($"{potion.PotionIndex[0].Name} - {addPotion}");
            Thread.Sleep(1000);
        }
    }
    public void DeadMonster()
    {
        for(int i = 0; i < monsters.Count; i++)
        {
            if (!monsters[i].IsDead)
            {
                Console.WriteLine($"Lv.{monsters[i].Level} {monsters[i].Name} HP {(monsters[i].IsDead ? "Dead" : monsters[i].Hp.ToString())}");

            }
            else
            {
                ConsoleUtility.PrintTextHighlights(ConsoleColor.DarkGray, "", $"Lv.{monsters[i].Level} {monsters[i].Name} HP Dead");
            }

        }
    }
    public void  RunAway() 
    {
        Console.Clear();
        Console.WriteLine("겁을 먹고 도망쳤다.");
        Thread.Sleep(1000);
        Console.WriteLine($"도망치면서 등에 칼이 꽃혔다. HP [{player.Hp}] -> HP [{player.Hp - 5}]");
        Thread.Sleep(1000);
        Console.WriteLine("");
        Console.WriteLine("0.다음.");

        Thread.Sleep(1000);
        int input = ConsoleUtility.PromptMenuChoice(0, 0);
        switch (input)
        {
            case 0:
                gameManager.MainMenu();
                break;
        }
    }
}