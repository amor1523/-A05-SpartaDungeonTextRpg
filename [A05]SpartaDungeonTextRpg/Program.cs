using _A05_SpartaDungeonTextRpg;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SpartaDungeonTextRpg
{
    public class GameManager
    {
        private Player player;
        private List<Monster> monsters;
        private Random random = new Random();
        private Battle battle;

        public GameManager()
        {
            InitializeGame();
            battle = new Battle(player, monsters);
        }

        // 이하 생략



        private void InitializeGame()
        {
            player = new Player("Chad", "전사", 1, 10, 5, 100, 1500f);
            monsters = new List<Monster>(); // 몬스터 리스트 초기화

            // 몬스터 생성 및 추가
            Monster monster = new Monster(0, "고블린", 1, 20, 35, 20, 50);
            monster.Monsters(player.Level); // 플레이어 레벨에 맞게 몬스터 생성
            monster.GenerateMonster(); // 몬스터 생성
            monsters.AddRange(monster.CreatedMonster); // 생성된 몬스터를 리스트에 추가
        }

        public void MainMenu()
        {
            Console.Clear();
            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
            Console.WriteLine("이제 전투를 시작할 수 있습니다.\n");
            Console.WriteLine("1. 상태보기");
            Console.WriteLine("2. 전투시작");
            Console.WriteLine();

            int input = ConsoleUtility.PromptMenuChoice(1, 2);
            switch (input)
            {
                case 1:
                    StatusMenu();
                    break;

                case 2:
                    battle.BattleMenu();
                    break;
            }
        }

        public void StatusMenu()
        {
            Console.Clear();
            ConsoleUtility.ShowTitle("[상태보기]");
            Console.WriteLine("캐릭터의 정보를 표시합니다.\n");
            Console.WriteLine($"Lv. {player.Level}");
            Console.WriteLine($"{player.Name} ({player.Job})");
            Console.WriteLine($"공격력 :{player.Atk}");
            Console.WriteLine($"방어력 :{player.Def}");
            Console.WriteLine($"체력 :{player.HP}");
            Console.WriteLine($"Gold : {player.Gold} G\n");
            Console.WriteLine("0. 나가기\n");

            int input = ConsoleUtility.PromptMenuChoice(0, 0);
            switch (input)
            {
                case 0:
                    MainMenu();
                    break;
            }
        }

        
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                GameManager gameManager = new GameManager();
                gameManager.MainMenu();
            }
        }
    }
}
