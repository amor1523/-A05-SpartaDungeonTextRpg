using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using _A05_SpartaDungeonTextRpg;
using SpartaDungeonTextRpg;
using static System.Net.Mime.MediaTypeNames;
public class Battle
{
    Dictionary<Job, string> dict = new Dictionary<Job, string>()
        {
            {Job.Knight, "전사"},
            {Job.Mage, "마법사"},
            {Job.Archer, "궁수"}
        };
    private Monster.BossMonster boss;
    private Player player;
    private Potion potion;
    private List<Monster> monsters;
    private Random random = new Random();
    private GameManager gameManager;
    private Skill skill;
    private int beforeHp;
    private int beforeExp;
    private int beforeMp;
    private bool BattleMonster = false;
    private bool bossClear = false;
    private bool bossPhaseTwo = false;
    private int count = 0;

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
        boss = new Monster.BossMonster(1, "레드 드래곤 (보스)", 10, 40, 300, 300, 10000, 500);
    }
    public void TextPlayerInfo()
    {
        Console.WriteLine();
        Console.WriteLine("[내정보]");

        Console.Write(ConsoleUtility.PadRightForMixedText($"Lv.{player.Level}", 5));
        Console.Write(" |  ");
        Console.WriteLine($"{player.Name} ({dict[player.Job]})");

        Console.Write(ConsoleUtility.PadRightForMixedText("Hp", 5));
        Console.Write(" |  ");
        Console.WriteLine($"{player.Hp}/{player.MaxHp} ");

        Console.Write(ConsoleUtility.PadRightForMixedText("Mp", 5));
        Console.Write(" |  ");
        Console.WriteLine($"{player.Mp}/{player.MaxMp}\n");
    }

    public void TextMonsters()
    {
        Console.WriteLine("전투 중인 몬스터들:");
        DeadMonster();
    }

    public void TextSkillMenu()
    {
        Console.WriteLine("[스킬 목록]");
        for (int j = 0; j < skill.SkillNum; j++)
        {
            Console.WriteLine($"{skill.SkillNum}.{skill.Name}은 적에게 {skill.SkillDamage}를 입힌다. (Mp 소요 : {skill.MpCost})\n");
        }
    }

    public void TextAttackResult(int damageDealt, Monster selectedMonster, bool isMiss, bool isCritical)
    {
        Console.Clear();
        ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "Battle!!\n");
        Console.WriteLine($"{player.Name} 의 공격!");
        Thread.Sleep(500);

        if (isMiss)
        {
            damageDealt = 0;
            Console.WriteLine($"Lv.{selectedMonster.Level} {selectedMonster.Name} 을(를) 공격했지만 아무일도  일어나지 않았습니다.\n");
            Thread.Sleep(500);
            Console.WriteLine("0.다음");
            int input1 = ConsoleUtility.PromptMenuChoice(0, 0);
            if (input1 == 0)
            {
                PromptForNextAction();
            }
        }
        else
        {
            Console.WriteLine($"Lv.{selectedMonster.Level} {selectedMonster.Name} 을(를) 맞췄습니다. [데미지 : {damageDealt}]");
            if (isCritical)
            {
                Console.WriteLine($"Lv.{selectedMonster.Level} {selectedMonster.Name} 을(를) 맞췄습니다. [데미지 : {damageDealt}] - 치명타 공격!!");
            }
        }
    }

    public void TextEnemyAttack(Monster targetMonster, int damageDealt)
    {
        ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "Battle!!\n");
        Console.WriteLine($"{targetMonster.Name} 의 공격!");
        Thread.Sleep(500);
        player.TakeDamage(damageDealt);
        Console.WriteLine($"Lv.{player.Level} {player.Name} 을/를 맞췄습니다. [데미지 : {damageDealt}]");
        Thread.Sleep(500);
        Console.WriteLine();
        Console.WriteLine($"Lv.{player.Level} {player.Name}");
        Thread.Sleep(500);
        Console.WriteLine($"HP {player.Hp}\n");
        Thread.Sleep(500);
    }

    public void TextBattle()
    {
        Console.Clear();
        ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "Battle!!\n");
        Thread.Sleep(500);
    }

    public void TextBattleBoss()
    {
        Console.Clear();
        ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "BossStage!!\n");
        Thread.Sleep(500);
    }

    public void PromptForNextAction()
    {
        Console.WriteLine("0.다음\n");
        int input = ConsoleUtility.PromptMenuChoice(0, 0);
        if (input == 0)
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
                {
                    player.Exp += monster.RewardExp;
                }
                BattleResult(true);
                return;
            }
            else
            {
                // 모든 몬스터가 공격한 후에 플레이어가 살아있는지 확인
                if (!player.IsDead)
                {
                    BattleMenu();
                    return;
                }
                else
                {
                    BattleResult(false);
                    return;
                }
            }
        }
    }

    public void BattleMenu()
    {
        if (player.Level >= 1 && player.Level <= 4)
        {
            TextBattle();
            TextMonsters();
            TextPlayerInfo();

            Console.WriteLine("1. 일반 공격");
            Console.WriteLine("2. 스킬 사용");
            Console.WriteLine("3. 포션 사용");
            Console.WriteLine("4. 도망 치기\n");

            int input = ConsoleUtility.PromptMenuChoice(1, 4);
            switch (input)
            {
                case 1:
                    BattleMonster = true;
                    PlayerAttack();
                    break;
                case 2:
                    BattleMonster = true;
                    SkillAttack();
                    break;
                case 3:
                    UsePotion();
                    break;
                case 4:
                    RunAway();
                    break;
            }
        }
        else
            BossStage();
    }

    public void SkillAttack()
    {
        int deadMonsterIdx = 0;
        TextBattle();
        TextMonsters();
        TextPlayerInfo();
        TextSkillMenu();
        Console.WriteLine("0.취소\n");

        int input = ConsoleUtility.PromptMenuChoice(0, 1);
        if (input == 0)
        {
            BattleMonster = false;
            BattleMenu();
            return;
        }
        Console.WriteLine("\n[공격할  몬스터]");

        int input2 = ConsoleUtility.PromptMenuChoice(1, monsters.Count);
        // 선택한 몬스터 인덱스
        deadMonsterIdx = input2 - 1;
        // 공격할 몬스터
        Monster selectedMonster = monsters[deadMonsterIdx];

        // 선택한 몬스터가 이미 죽은 상태인지 확인
        if (selectedMonster.IsDead)
        {
            Console.WriteLine("\n잘못된 입력입니다.");
            Thread.Sleep(500);
            SkillAttack(); // 다시 공격 메뉴로 돌아감
            return;
        }

        int usedMana = skill.MpCost;
        if (player.Mp < usedMana)
        {
            Console.WriteLine("마나가 부족하여 스킬을 사용할수없습니다.");
            Thread.Sleep(500);
            SkillAttack();
            return;
        }
        skill.UseSkill();
        int damageDealt = skill.SkillDamage;
        selectedMonster.TakeDamage(damageDealt);
        player.UseMp(usedMana);
        Thread.Sleep(500);

        Console.Clear();
        ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", $"Battle!!!\n");
        Console.WriteLine($"{player.Name} 의 공격!");
        Thread.Sleep(500);
        ConsoleUtility.PrintTextHighlights(ConsoleColor.Yellow, "", $"{skill.Name}!!!\n");
        Console.WriteLine($"Lv.{selectedMonster.Level} {selectedMonster.Name} 을(를) 맞췄습니다. [데미지 : {damageDealt}]\n");
        Thread.Sleep(500);
        Console.WriteLine($"Lv.{selectedMonster.Level} {selectedMonster.Name}");
        Thread.Sleep(500);
        Console.WriteLine($"HP {selectedMonster.Hp}\n");
        Thread.Sleep(500);

        BattleMonster = false;
        PromptForNextAction();
    }
    public void PlayerAttack()
    {

        TextBattle();
        TextMonsters();
        TextPlayerInfo();
        Console.WriteLine("0.취소\n");

        int input = ConsoleUtility.PromptMenuChoice(0, monsters.Count);
        if (input == 0)
        {
            BattleMonster = false;
            return;
        }
        // 선택한 몬스터 인덱스
        int deadMonsterIdx = input - 1;
        // 공격할 몬스터
        Monster selectedMonster = monsters[deadMonsterIdx];

        // 선택한 몬스터가 이미 죽은 상태인지 확인
        if (selectedMonster.IsDead)
        {
            Console.WriteLine("\n잘못된 입력입니다.");
            Thread.Sleep(500);
            BattleMenu();
            return; // 다시 공격 메뉴로 돌아감
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

        Console.Clear();
        ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "Battle!!\n");
        Console.WriteLine($"{player.Name} 의 공격!");
        Thread.Sleep(500);

        //회피 발생하면 데미지 0으로 적용
        if (isMiss)
        {
            damageDealt = 0;
            Console.WriteLine($"Lv.{selectedMonster.Level} {selectedMonster.Name} 을(를) 공격했지만 아무일도  일어나지 않았습니다.\n");
        }
        else
        {
            // 치명타 여부를 결정하기 위한 확률 30퍼
            int critChance = 30;
            // 랜덤한 확률을 생성하여 치명타 여부 결정
            bool isCritical = random.Next(100) < critChance;
            // 치명타 데미지 계산
            int critDamage = (int)(damageDealt * 1.6); // 치명타 데미지는 일반 데미지의 1.6배로 가정
                                                       // 치명타가 발생하면 추가 데미지 적용

            if (isCritical)
            {
                damageDealt += critDamage;
                Console.WriteLine($"Lv.{selectedMonster.Level} {selectedMonster.Name} 을(를) 맞췄습니다. [데미지 : {damageDealt}]- 치명타 공격!!\n");
            }
            else
            {
                Console.WriteLine($"Lv.{selectedMonster.Level} {selectedMonster.Name} 을(를) 맞췄습니다. [데미지 : {damageDealt}]\n");
            }
            selectedMonster.TakeDamage(damageDealt);
        }
        Thread.Sleep(500);
        Console.WriteLine($"Lv.{selectedMonster.Level} {selectedMonster.Name}");
        Thread.Sleep(500);
        Console.WriteLine($"HP {selectedMonster.Hp}\n");
        Thread.Sleep(500);
        BattleMonster = false;
        PromptForNextAction();
    }

    public void EnemyAttack(Monster targetMonster)
    {
        int playerHp = player.Hp;
        int damageDealt = 0;
        //공격할 몬스터가  살아있는지 확인
        if (!targetMonster.IsDead && playerHp > 0)
        {
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "Battle!!\n");
            Console.WriteLine($"{targetMonster.Name} 의 공격!");
            Thread.Sleep(500);
            if (targetMonster.Atk-player.Def > 0)
                damageDealt = targetMonster.Atk - player.Def;
            else
                damageDealt = 0;

            player.TakeDamage(damageDealt);

            if (targetMonster.Atk - player.Def > 0)
                Console.WriteLine($"Lv.{player.Level} {player.Name} 을/를 맞췄습니다. [데미지 : {damageDealt}]");
            else
                ConsoleUtility.PrintTextHighlights(ConsoleColor.Blue, "", $"방어력이 너무 높아서 공격이 통하지 않았다..!! [데미지 : {damageDealt}]\n");
            Thread.Sleep(500);
            Console.WriteLine($"Lv.{player.Level} {player.Name}");
            Thread.Sleep(500);
            Console.WriteLine($"HP {player.Hp}\n");
            Thread.Sleep(500);
        }
        else if (targetMonster.IsDead)
        {
            if(targetMonster.Id == 3 && Quest.questData[1].Count < Quest1.quest1.GoalCount && targetMonster.QuestCount == false )
            {
                Quest.questData[1].Count++;
                targetMonster.QuestCount = true;
            }
        }
        else if (!targetMonster.IsDead && playerHp == 0)
        {
            BattleResult(false);
            return;
        }
    }

    public void BattleResult(bool victory)
    {
        // 레벨업 유무 확인
        int playerLevel = player.Level;
        bool flagLevelUp = LevelUp();

        Console.Clear();
        ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "전투 결과\n");
        if (victory)
        {
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Green, "", "전투 승리 !!!\n");

            if (!bossClear)
                Console.WriteLine("던전에서 몬스터를 잡았습니다.\n");
            else
                Console.WriteLine("던전 보스인 레드 드래곤을 잡았습니다!!\n");

            ConsoleUtility.PrintTextHighlights(ConsoleColor.Yellow, "", "[캐릭터 정보]");
            if (!flagLevelUp)
                Console.WriteLine($"Lv.{player.Level} {player.Name}");
            else
                Console.WriteLine($"Lv.{playerLevel} {player.Name} -> Lv.{player.Level} {player.Name}");

            Thread.Sleep(500);
            Console.WriteLine($"HP {beforeHp} -> {player.Hp} {((beforeHp - player.Hp) > 0 ? $"(-{beforeHp-player.Hp})" : "")}");
            Thread.Sleep(500);
            Console.WriteLine($"MP {beforeMp} -> {player.Mp} {((beforeMp - player.Mp) > 0 ? $"(-{beforeMp - player.Mp})" : "")}");
            if (player.Mp < player.MaxMp)
                player.Mp += 10;
            Console.WriteLine($"exp {beforeExp} -> {player.Exp} (+{player.Exp - beforeExp})");
            Thread.Sleep(500);
            if (player.Level < 5)
            {
                Console.WriteLine($"LevelUp까지 남은 Exp -> {player.LevelUpExp - player.Exp}\n");
                Thread.Sleep(500);
            }
            else
                Console.WriteLine();

            if (flagLevelUp)
            {
                ConsoleUtility.PrintTextHighlights(ConsoleColor.Yellow, "", "[레벨업! 능력치 상승!!]");
                Console.WriteLine($"MaxHp {player.MaxHp-10} -> {player.MaxHp}");
                Console.WriteLine($"MaxMp {player.MaxMp - 10} -> {player.MaxMp}");
                Console.WriteLine($"Atk {player.NonEquipAtk - 1} -> {player.NonEquipAtk}");
                Console.WriteLine($"Def {player.NonEquipDef - 1} -> {player.NonEquipDef}\n");
                Console.WriteLine("레벨업으로 플레이어의 HP와 MP가 최대치까지 회복됩니다.\n");

                player.Hp = player.MaxHp;
                player.Mp = player.MaxMp;
            }

            RewardItem();

            // 보스 클리어시 게임 종료.
            if (bossClear)
            {
                gameManager.GamePlay = false;
                ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "\n보스를 클리어하여 게임이 종료됩니다.");
                ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "플레이 해주셔서 감사합니다.");

                Console.WriteLine("\n0. 다음");
                int endInput = ConsoleUtility.PromptMenuChoice(0, 0);
                if (endInput == 0)
                    return;
            }
            // 몬스터 생성함수
            GetMonster();

            Thread.Sleep(500);
            Console.WriteLine("\n0. 다음");
            int input = ConsoleUtility.PromptMenuChoice(0, 0);
            switch (input)
            {
                case 0:
                    BossStage();
                    break;            
            }
        }
        else
        {
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Red, "", "전투 패배...\n");
            Thread.Sleep(500);
            Console.WriteLine($"Lv.{player.Level} {player.Name}");
            Console.WriteLine($"HP {beforeHp} -> {player.Hp}\n");

            Console.WriteLine("1. 다시 도전!\n");
            Console.WriteLine("0. 게임 종료..\n");
            int input = ConsoleUtility.PromptMenuChoice(0, 1);
            switch (input)
            {
                case 0:
                    gameManager.GamePlay = false;
                    Console.WriteLine("\n게임을 종료합니다.");
                    break;
                case 1:
                    player.Hp = player.MaxHp;
                    player.Mp = player.MaxMp;
                    break;
            }
        }
    }

    public bool LevelUp()
    {
        bool flagLevelUp;

        if (player.Exp >= player.LevelUpExp && (player.Level >= 1 && player.Level <= 4))
        {
            flagLevelUp = true;
            player.NonEquipAtk += 1;
            player.NonEquipDef += 1;
            player.MaxHp += 10;
            player.MaxMp += 10;

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

        Console.WriteLine($"+ {addGold} Gold");
        Thread.Sleep(500);
        player.Gold += addGold;

        if (isGetPotion)
        {
            int addPotion = random.Next(1, 3);
            potion.PotionIndex[0].Count += addPotion;
            Console.WriteLine($"+ {potion.PotionIndex[0].Name} {addPotion}개");
        }
    }
    public void DeadMonster()
    {
        for(int i = 0; i < monsters.Count; i++)
        {
            if (!monsters[i].IsDead)
            {
                if (!BattleMonster)
                    Console.WriteLine($"- Lv.{monsters[i].Level} {monsters[i].Name} HP {(monsters[i].IsDead ? "Dead" : monsters[i].Hp.ToString())}");
                else
                    Console.WriteLine($"{i+1}. Lv.{monsters[i].Level} {monsters[i].Name} HP {(monsters[i].IsDead ? "Dead" : monsters[i].Hp.ToString())}");
            }
            else
            {
                if(!BattleMonster)
                    ConsoleUtility.PrintTextHighlights(ConsoleColor.DarkGray, "", $"- Lv.{monsters[i].Level} {monsters[i].Name} HP Dead");
                else
                    ConsoleUtility.PrintTextHighlights(ConsoleColor.DarkGray, "", $"- Lv.{monsters[i].Level} {monsters[i].Name} HP Dead");
            }

        }
    }
    public void  RunAway() 
    {
        // 몬스터 생성함수
        GetMonster();

        int beforeHp = player.Hp;
        int damage = 5;
        player.TakeDamage(damage);
        Console.Clear();
        Console.WriteLine("겁을 먹고 도망쳤다.\n");
        Thread.Sleep(500);
        Console.WriteLine($"도망치면서 등에 칼이 꽃혔다. HP [{beforeHp}] -> HP [{player.Hp}]\n");
        Thread.Sleep(500);
        Console.WriteLine("0.다음\n");
        Thread.Sleep(500);

        int input = ConsoleUtility.PromptMenuChoice(0, 0);
        switch (input)
        {
            case 0:
                break;
        }
        
    }
    private void GetMonster()
    {
        monsters.Clear();

        // 여기에 새로운 몬스터를 추가하는 코드를 작성합니다.
        // 예를 들어, 플레이어 레벨에 따라 다른 몬스터를 추가할 수 있습니다.
        Monster monster = new Monster();
        monster.Monsters(player.Level); // 플레이어 레벨에 맞게 몬스터 생성
        monster.GenerateMonster(); // 몬스터 생성
        monsters.AddRange(monster.CreatedMonster); // 생성된 몬스터를 리스트에 추가
    }
    
    public void BossText() 
    {
        if (CheckBossStage() == false)
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Red, "", "보스 스테이지는 5레벨부터 입장 가능합니다! 더 성장해서 도전해보세요..!!\n");
        if (CheckBossStage() == true)
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Red, "", "보스 스테이지 입장이 가능합니다!!! 도전하시겠습니까?\n");
        Thread.Sleep(500);
        Console.WriteLine("0. 마을로 돌아가기\n");

        if(CheckBossStage() == true)
            Console.WriteLine("1. 보스전 입장!\n");
    }
    public void BossStage()
    {
        if (CheckBossStage() == false)
        {
            Console.Clear(); 
            BossText();
            int input = ConsoleUtility.PromptMenuChoice(0, 0);
            switch (input)
            {
                case 0:
                    break;
            }
        }
        else if(CheckBossStage() == true) 
        {
            Console.Clear();
            BossText();
            int input = ConsoleUtility.PromptMenuChoice(0, 1);
            switch (input)
            {
                case 0:
                    break;
                case 1:
                    BossBattle();
                    break;
            }
        }
    }
    public void BossBattle()
    {
        TextBattleBoss();
        Console.Write($"Lv.{boss.Level} ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"{boss.Name} ");
        Console.ResetColor();
        Console.WriteLine($"HP : {boss.Hp}");
        TextPlayerInfo();

        Console.WriteLine("1. 일반 공격");
        Console.WriteLine("2. 스킬 사용");
        Console.WriteLine("3. 포션 사용\n");
        int input = ConsoleUtility.PromptMenuChoice(1, 3);
        switch (input)
        {
            case 1:
                Attack();
                break;
            case 2:
                PlayerSkillAttack();
                break;
            case 3:
                UsePotion();
                break;
        }

        //////////////////////////////////////////////////////////////////////////////// 보스 패턴부분 수정 필요.
        /////////////////////////////////////////////////////////////////////////////// 보스 패턴 시간 기능 추가 필요.
        // 보스의 체력이 50% 이하로 떨어지면 두 번째 페이즈로 진입
        if (boss.Hp <= 150 && !bossClear)
        {
            bossPhaseTwo = true;
            Console.WriteLine("보스가 분노합니다! 두 번째 페이즈로 진입합니다.");
            Thread.Sleep(500);

            // 두 번째 페이즈 시작을 알리는 텍스트 출력
            Console.WriteLine("2번째 페이즈로 진입합니다!");
            Thread.Sleep(500);

            // 보스가 플레이어에게 지속 데미지를 입히는 스킬을 사용
            Console.WriteLine("보스가 플레이어에게 지속 데미지를 입히는 스킬을 사용합니다!");
            int damageTime = 10; // 보스의 플레이어에게 입히는 지속 데미지
            int dot = 3; // 지속 시간 (예: 3턴 동안 지속됨)
            Thread.Sleep(500);
            // 보스가 지속 데미지를 입히는 메서드 호출
            InflictDamageOverTime(damageTime, dot);

            Console.WriteLine("0. 다음\n");
            int input2 = ConsoleUtility.PromptMenuChoice(0, 0);
        }
        else if (bossPhaseTwo && !bossClear)  // 보스 페이즈 전역 변수로 수정 필요
        {
            int damageTime = 10; // 보스의 플레이어에게 입히는 지속 데미지
            int dot = 3;
            if (count < dot)
                InflictDamageOverTime(damageTime, dot);
            else
            {
                bossPhaseTwo = false;
                return;
            }
            Console.WriteLine("0. 다음\n");
            int input2 = ConsoleUtility.PromptMenuChoice(0, 0);
        }
    }
    public void InflictDamageOverTime(int damage, int dot)
    {
        TextBattleBoss();

        Console.WriteLine($"플레이어는 {dot}턴 동안 {damage}의 지속 데미지를 입습니다!\n");
        Console.WriteLine($"플레이어는 {dot-count}턴 뒤 보스의 스킬이 해제됩니다.\n");
        Thread.Sleep(500);

        if (count < dot)
        {
            // 플레이어에게 지속 데미지 입힘
            player.TakeDamage(damage);
            // 플레이어의 현재 체력 출력
            Console.WriteLine($"플레이어의 현재 체력: {player.Hp}");
            count += 1;
            Thread.Sleep(500);
        }
    }
    public bool CheckBossStage()
    {
        return player.Level >= 5; // 플레이어 레벨이 5 이상이면 보스 스테이지로 진입
    }
    public void Attack()
    {
        TextBattleBoss();
        Console.Write($"Lv.{boss.Level} ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"{boss.Name} ");
        Console.ResetColor();
        Console.WriteLine($"HP : {boss.Hp}\n");

        // 플레이어의 공격력 계산
        int baseDamage = player.Atk;
        int errorDamage = (int)Math.Ceiling(baseDamage * 0.1);
        int minDamage = baseDamage - errorDamage;
        int maxDamage = baseDamage + errorDamage;
        int damageDealt = random.Next(minDamage, maxDamage);

        // 회피 확률
        int missChance = 10;
        bool isMiss = random.Next(100) < missChance;

        if (isMiss)
        {
            Console.WriteLine($"{player.Name}의 공격!");
            Thread.Sleep(500);
            damageDealt = 0;
            Console.WriteLine($"보스를 공격했지만 아무일도 일어나지 않았습니다.\n");
        }
        else
        {
            // 치명타 확률
            int critChance = 30;
            bool isCritical = random.Next(100) < critChance;

            // 치명타 공격 시 데미지 계산
            int critDamage = (int)(damageDealt * 1.6);
            if (isCritical)
            {
                damageDealt += critDamage;
                Console.WriteLine($"Lv.{boss.Level} {boss.Name} 을(를) 맞췄습니다. [데미지 : {damageDealt}] - 치명타 공격!!\n");
            }
            else
            {
                Console.WriteLine($"Lv.{boss.Level} {boss.Name} 을(를) 맞췄습니다. [데미지 : {damageDealt}]\n");
            }
        }

        // 보스에게 데미지 적용
        boss.TakeDamage(damageDealt);

        Thread.Sleep(500);
        Console.WriteLine($"Lv.{boss.Level} {boss.Name}");
        Thread.Sleep(500);
        Console.WriteLine($"HP {boss.Hp}\n");

        if (boss.IsDead)
        {
            Thread.Sleep(500);
            Console.WriteLine($"보스를 처치하였습니다!");
            Thread.Sleep(500);
            bossClear = true;
            BattleResult(true);
            return;
        }
        else
        {
            // 보스의 공격
            Thread.Sleep(500);

            Console.WriteLine("0. 다음\n");
            int input = ConsoleUtility.PromptMenuChoice(0, 0);
            BossAttack();
            return;
        }
    }

    public void BossAttack()
    {
        TextBattleBoss();
        int damageDealt = boss.Atk - player.Def;

        if (damageDealt > 0)
            damageDealt = boss.Atk - player.Def;
        else
            damageDealt = 0;

        player.TakeDamage(damageDealt);
        ConsoleUtility.PrintTextHighlights(ConsoleColor.Red, "", $"{boss.Name}의 공격!!");
        Thread.Sleep(500);
        if(damageDealt > 0)
            Console.WriteLine($"{player.Name}을/를 맞췄습니다. [데미지 : {damageDealt}]\n");
        else
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Blue, "", $"방어력이 너무 높아서 공격이 통하지 않았다..!! [데미지 : {damageDealt}]\n");
        Thread.Sleep(500);

        if (player.IsDead)
        {
            Console.WriteLine($"플레이어가 쓰러졌습니다!");
            Thread.Sleep(500);
            BattleResult(false);
            return;
        }
        else
        {
            Console.WriteLine($"0.다음\n");
            int input = ConsoleUtility.PromptMenuChoice(0, 0);
            if (input == 0)
            {
                Console.Clear();
                BossBattle();
                return;
            }
        }
    }
    public void PlayerSkillAttack()
    {
        TextBattleBoss();
        Console.Write($"Lv.{boss.Level} ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"{boss.Name} ");
        Console.ResetColor();
        Console.WriteLine($"HP : {boss.Hp}\n");
        TextPlayerInfo();
        TextSkillMenu();
        Console.WriteLine("0.취소");


        int input = ConsoleUtility.PromptMenuChoice(0, 1); // 보스 선택 옵션 제거

        if (input == 0)
        {
            Console.Clear();
            BossBattle();
            return;
        }
        // 스킬 사용 대상으로 선택한 보스가 아닌 보스 리스트 중 첫 번째 보스를 대상으로 스킬 사용
        Monster selectedBoss = boss;
        int usedMana = skill.MpCost;
        if (player.Mp < usedMana)
        {
            Console.WriteLine("마나가 부족하여 스킬을 사용할수없습니다.");
            Thread.Sleep(500);
            PlayerSkillAttack();
            return;
        }

        skill.UseSkill();
        int damageDealt = skill.SkillDamage;
        selectedBoss.TakeDamage(damageDealt);
        player.UseMp(usedMana);
        Thread.Sleep(500);


        TextBattleBoss();
        Console.WriteLine($"{player.Name} 의 공격!");
        Thread.Sleep(500);
        ConsoleUtility.PrintTextHighlights(ConsoleColor.Yellow, "", $"{skill.Name}!!!\n");
        Console.WriteLine($"Lv.{selectedBoss.Level} {selectedBoss.Name} 을(를) 맞췄습니다. [데미지 : {damageDealt}]\n");
        Thread.Sleep(500);
        Console.WriteLine($"Lv.{selectedBoss.Level} {selectedBoss.Name}");
        Thread.Sleep(500);
        Console.WriteLine($"HP {selectedBoss.Hp}\n");
        Thread.Sleep(500);

        Console.WriteLine("0. 다음\n");
        int input2 = ConsoleUtility.PromptMenuChoice(0, 0);

        BossAttack();
        return;
    }


    public void UsePotion()
    {
        Console.Clear();
        Console.WriteLine("[포션 목록]");
        if (potion.PotionIndex[0].Count == 0)
            Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("- ");

        Console.Write(ConsoleUtility.PadRightForMixedText(potion.PotionIndex[0].Name, 20));

        Console.Write(" | ");

        Console.Write(ConsoleUtility.PadRightForMixedText("보유 개수: " + potion.PotionIndex[0].Count.ToString() + " 개", 15));

        Console.Write(" | ");

        Console.WriteLine(ConsoleUtility.PadRightForMixedText(potion.PotionIndex[0].Explain, 55));

        Console.ResetColor();

        Console.WriteLine("\n1. 사용");
        Console.WriteLine("0. 나가기");

        int input = ConsoleUtility.PromptMenuChoice(0, 1);
        switch (input)
        {
            case 0:
                BattleMenu();
                break;
            case 1:
                if (potion.PotionIndex[0].Count > 0)
                {
                    potion.UsePotion(player, 0);
                    if (potion.PotionIndex[0].FlagUse)
                    {
                        potion.PotionIndex[0].Count -= 1;
                        potion.PotionIndex[0].FlagUse = false;
                    }
                }
                else
                {
                    Console.WriteLine("\n선택하신 포션의 수량이 부족합니다.");
                }

                Thread.Sleep(500);
                UsePotion();
                break;
        }
    }
}


  