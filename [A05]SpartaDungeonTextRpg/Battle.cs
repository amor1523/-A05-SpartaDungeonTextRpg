using System;
using _A05_SpartaDungeonTextRpg;
using SpartaDungeonTextRpg;
public class Battle
{
    
        private Player player;
        private List<Monster> monsters;
        private Random random = new Random();

        public Battle(Player player, List<Monster> monsters)
        {
            this.player = player;
            this.monsters = monsters;
        }
        public void BattleMenu()
    {
        Console.Clear();
        ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "Battle!!\n");
        
        for (int i = 0; i < monsters.Count; i++)
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
      
        Console.WriteLine();
        Console.WriteLine("[내정보]");
        Console.WriteLine($"Lv.{player.Level}  {player.Name} ({player.Job})");
        Console.WriteLine($"HP {player.Hp}\n");
        Console.WriteLine("1. 공격\n");

        int input = ConsoleUtility.PromptMenuChoice(1, 1);
        switch (input)
        {
            case 1:
                PlayerAttack();
                break;
        }
    }
    public void PlayerAttack()
    {
        Console.Clear();
        ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "Battle!!\n");
        Thread.Sleep(500);
        //살아있는 몬스터를 리스트에 저장 
        List<Monster> aliveMonsters = monsters.Where(m => !m.IsDead).ToList();
        for (int i = 0; i < aliveMonsters.Count; i++)
        {
            Console.WriteLine($"{i + 1}. Lv.{aliveMonsters[i].Level} {aliveMonsters[i].Name} HP {aliveMonsters[i].Hp}");
        }

        Console.WriteLine();
        Console.WriteLine("[내정보]");
        Console.WriteLine($"Lv.{player.Level}  {player.Name} ({player.Job})");
        Console.WriteLine($"HP {player.Hp}\n");
        Console.WriteLine("0.취소");

        int input = ConsoleUtility.PromptMenuChoice(0, aliveMonsters.Count);
        if (input == 0)
        {
            Console.Clear();
            BattleMenu();
        }
        int baseDamage = player.Atk;
        int missDamage = (int)Math.Ceiling(baseDamage *0.1);
        int minDamage = baseDamage - missDamage;
        int maxDamage = baseDamage + missDamage;
        int damageDealt = random.Next(minDamage,maxDamage);
        Monster selectedMonster = aliveMonsters[input - 1];
        selectedMonster.TakeDamage(damageDealt);
        Console.WriteLine($"Lv.{selectedMonster.Level} {selectedMonster.Name} 을/를 맞췄습니다. [데미지 : {damageDealt}]");
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
            foreach (Monster monster in monsters)
            {
                Console.Clear();
                EnemyAttack(monster);
            }

            // 모든 몬스터가 죽었을 때 승리 처리
            if (monsters.All(m => m.IsDead))
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
       

    }

    public void BattleResult(bool victory)
    {
        // 레벨업 유무 확인
        bool flagLevelUp = LevelUp();

        Console.Clear();
        ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "전투 결과\n");
        if (victory)
        {
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Green, "", "전투 승리\n");
            Console.WriteLine("던전에서 몬스터를 잡았습니다.");
            if (!flagLevelUp)
                Console.WriteLine($"Lv.{player.Level} {player.Name}");
            else
                Console.WriteLine($"Lv.{player.Level - 1} {player.Name} -> {player.Level} {player.Name}");

            Thread.Sleep(1000);
            Console.WriteLine($"HP (전투 전 HP) -> {player.Hp}\n");
            Thread.Sleep(1000);
            Console.WriteLine($"exp {player.BeforeExp} -> {player.AfterExp}\n");
            Thread.Sleep(1000);
            Console.WriteLine($"LevelUp 까지 남은 exp -> {player.LevelUpExp - player.AfterExp}\n");
            Thread.Sleep(1000);
            Console.WriteLine("0. 다음\n");
        }
        else
        {
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Red, "", "전투 패배\n");
            Thread.Sleep(1000);
            Console.WriteLine($"Lv.{player.Level} {player.Name}");
            Console.WriteLine($"HP (전투 전 HP) -> {player.Hp}\n");
        }
        Console.WriteLine("0. 다음\n");
        int input = ConsoleUtility.PromptMenuChoice(0, 0);
        switch (input)
        {
            case 0:
                GameManager gameManager = new GameManager();
                gameManager.MainMenu();
                player.BeforeExp = player.AfterExp;
                break;
        }
    }

    public bool LevelUp()
    {
        bool flagLevelUp;

        if (player.AfterExp > player.LevelUpExp && (player.Level >= 1 && player.Level <= 5))
        {
            flagLevelUp = true;
            player.Level += 1;
            player.Atk += 1;
            player.Def += 1;

            if (player.Level == 2)
                player.LevelUpExp = 35;
            else if (player.Level == 3)
                player.LevelUpExp = 65;
            else if (player.Level == 4)
                player.LevelUpExp = 100;
        }
        else
            flagLevelUp = false;

        return flagLevelUp;
    }
}