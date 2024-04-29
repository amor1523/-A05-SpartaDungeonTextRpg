using _A05_SpartaDungeonTextRpg;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

internal class Monster
{
    public string Name { get; set; }
    public int Level { get; set; }
    public int Atk { get; set; }
    public int Hp { get; set; }
    public bool Isdead => Hp <= 0;


    public Monster(string name, int level, int atk, int hp)
    {
        Name = name;
        Level = level;
        Atk = atk;
        Hp = hp;
        

    }
}
internal class Battle
{
    private Monster monster;
    private Player player;
    Random random = new Random();
    static public List<Monster> MonsterSpawner(Monster monster1, Monster monster2, Monster monster3)
    {
        
        List<Monster> monsters = new List<Monster>();
        Random rand = new Random();
        int MonsterCount = rand.Next(1, 5);
        for (int i = 0; i < MonsterCount; i++)
        {
            int RandomMonster = rand.Next(1, 4);
            if (RandomMonster == 1)
            {
                monsters.Add(monster1);
            }
            else if (RandomMonster == 2)
            {
                monsters.Add(monster2);
            }
            else if (RandomMonster == 3)
            {
                monsters.Add(monster3);
            }
        }
        return monsters;
    }
    public void Result(int Atk, Player player , Monster monster)
    {
        double attackWeak = Math.Ceiling(Atk * 0.1);
        int min = Atk - (int)attackWeak;
        int max = Atk - (int)attackWeak;
        int attack = random.Next(min, max);
        int input = ConsoleUtility.PromptMenuChoice(0, 0);
        Console.WriteLine("Battle!!\n");
        Console.WriteLine($"{player.Name}의 공격!\nLv.{monster.Level} {monster.Name} 을(를) 맞췄습니다. [데미지 : {attack}]\n");
        if (monster.Isdead == true)
        {
            Console.WriteLine($"Lv.{monster.Level} {monster.Name}\n HP {monster.Hp} -> Dead\n");

        }
        else if (monster.Isdead == false)
        {
            Console.WriteLine($"Lv.{monster.Level} {monster.Name}\n HP {monster.Hp} -> {(monster.Hp) - attack}\n");
        }
        Console.WriteLine("0.다음\n");
        Console.WriteLine(">>");

        switch (input)
        {
            case 0:
                EnemyTurn();
                break;
        }
    }
    // 공격에 대한 메소드
    
    public void EnemyTurn()
    {

    }
    public static void BattleMod()
	{
        
        int input = ConsoleUtility.PromptMenuChoice(0,1);

       switch (input)
		{
			case 0:
				
            case 1:
              
                break;




        }
		

	}
}
