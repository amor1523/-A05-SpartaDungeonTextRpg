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
        Console.WriteLine($"HP {player.HP}\n");
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
        for (int i = 0; i < monsters.Count; i++)
        {
            Console.WriteLine($"{i + 1}. Lv.{monsters[i].Level} {monsters[i].Name} HP {monsters[i].Hp}");
        }
        Console.WriteLine();
        Console.WriteLine("[내정보]");
        Console.WriteLine($"Lv.{player.Level}  {player.Name} ({player.Job})");
        Console.WriteLine($"HP {player.HP}\n");
        Console.WriteLine("0.취소");

        int input = ConsoleUtility.PromptMenuChoice(0, monsters.Count);
        if (input == 0)
        {
            Console.Clear();
            BattleMenu();
        }

        Monster selectedMonster = monsters[input - 1];
        int damageDealt = player.Atk;
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
            Console.WriteLine($"HP {player.HP}\n");
            Thread.Sleep(1000);
        }
        else
        {
            ConsoleUtility.PrintTextHighlights(ConsoleColor.DarkGray, "", $"{targetMonster.Name} 은/는 이미 죽었습니다.\n");
        }

    }

    public void BattleResult(bool victory)
    {
        Console.Clear();
        ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "전투 결과\n");
        if (victory)
        {
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Green, "", "전투 승리\n");
            Console.WriteLine("던전에서 몬스터를 잡았습니다.");
            Console.WriteLine($"Lv.{player.Level} {player.Name}");
            Console.WriteLine($"HP (전투 전 HP) -> {player.HP}\n");
        }
        else
        {
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Red, "", "전투 패배\n");
            Console.WriteLine($"Lv.{player.Level} {player.Name}");
            Console.WriteLine($"HP (전투 전 HP) -> {player.HP}\n");
        }
        Console.WriteLine("0. 다음\n");
        int input = ConsoleUtility.PromptMenuChoice(0, 0);
        switch (input)
        {
            case 0:
                GameManager gameManager = new GameManager();
                gameManager.MainMenu();
                break;
        }
    }
}