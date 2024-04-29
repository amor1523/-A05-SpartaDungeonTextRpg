namespace _A05_SpartaDungeonTextRpg
{
    // 몬스터 실행용 클래스 입니다 나중에 삭제!!
    internal class Monster
    {
        public string Name { get; }
        public int Level { get; }
        public int Atk { get; }
        public int Hp { get; }
        

        public Monster(string name, int level, int atk, int hp)
        {
            Name = name;
            Level = level;
            Atk = atk;
            Hp = hp;
        }
    }

    public class GameManager
    {
        private Player player;
        private Monster monster;

        public GameManager()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            player = new Player("Chad", "전사", 1, 10, 5, 100, 1500f);
            monster = new Monster("미니언", 2, 5, 12);
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
                    BattleMenu();
                    break;

            }
        }

        // 플레이어의 현재 상태를 보여줍니다.
        public void StatusMenu()
        {
            Console.Clear();
            ConsoleUtility.ShowTitle("[상태보기]");
            Console.WriteLine("캐릭터의 정보를 표시합니다.\n");
            Console.WriteLine($"Lv. {player.Level}");
            Console.WriteLine($"{player.Name} ({player.Job})");
            Console.WriteLine($"공격력 :{player.Atk}");
            Console.WriteLine($"방어력 :{player.Def}");
            Console.WriteLine($"체력 :{player.Hp}");
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

        public void BattleMenu()
        {
            Console.Clear();

            // Stage 생성 / Monster.cs 몬스터 스폰 로직
            // 이미 생성되어있다면 새로 생성하지 않음

            ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "Battle!!\n");
            
            // 몬스터 상태에 따른 for문으로 몬스터 Status 표시

            // 예시몬스터
            Console.WriteLine($"Lv.{monster.Level} {monster.Name} HP {monster.Hp} ");
            
            Console.WriteLine();
            Console.WriteLine("[내정보]");
            Console.WriteLine($"Lv.{player.Level}  {player.Name} ({player.Job})");
            Console.WriteLine($"HP {player.Hp}\n");
            Console.WriteLine("1. 공격\n");

            int input = ConsoleUtility.PromptMenuChoice(1, 1);
            switch (input)
            {
                case 1:
                    BattleAttackSellect();
                    break;
            }
        }

        public void BattleAttackSellect()
        {
            Console.Clear();
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "Battle!!\n");

            // 몬스터 상태에 따른 for문으로 몬스터 Status 표시

            // 예시몬스터
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Magenta, "", "[1] ", $"Lv.{monster.Level} {monster.Name} HP {monster.Hp} ");

            Console.WriteLine();
            Console.WriteLine("[내정보]");
            Console.WriteLine($"Lv.{player.Level}  {player.Name} ({player.Job})");
            Console.WriteLine($"HP {player.Hp}\n");
            Console.WriteLine("0. 취소\n");

            int input = ConsoleUtility.PromptMenuChoice(0, 1); // MonsterCount 추가 후 수정, 몬스터 죽었을 경우 추가
            switch (input)
            {
                case 0:
                    BattleMenu();
                    break;
                case 1:
                    BattlePlayerAttack(); // 몬스터 선택
                    break;
            }

        }

        public void BattlePlayerAttack()
        {
            Console.Clear();
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "Battle!!\n");
            Console.WriteLine($"{player.Name} 의 공격!");
            Thread.Sleep(500);

            // if문으로
            Console.WriteLine($"Lv.{monster.Level} {monster.Name} 을/를 맞췄습니다. [데미지 : {player.Atk}]");
            Thread.Sleep(1000);

            Console.WriteLine();
            Console.WriteLine($"Lv.{monster.Level} {monster.Name}");
            Thread.Sleep(500);
            Console.WriteLine($"HP {monster.Hp} -> {monster.Hp-player.Atk}");
            Thread.Sleep(1000);

            Console.WriteLine();
            Console.WriteLine("0. 다음\n");

            // if문으로 몬스터가 죽었을때 작성


            // 이하 else문
            int input = ConsoleUtility.PromptMenuChoice(0, 0);
            switch (input)
            {
                case 0:
                    BattleEnemyAttack();
                    break;
            }
        }

        public void BattleEnemyAttack()
        {
            Console.Clear();
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "Battle!!\n");

            Console.WriteLine($"{monster.Name} 의 공격!");
            Thread.Sleep(500);

            // if문으로
            Console.WriteLine($"Lv.{player.Level} {player.Name} 을/를 맞췄습니다. [데미지 : {monster.Atk}]");
            Thread.Sleep(1000);

            Console.WriteLine();
            Console.WriteLine($"Lv.{player.Level} {player.Name}");
            Thread.Sleep(500);
            Console.WriteLine($"HP {player.Hp} -> {player.Hp - monster.Atk}");
            Thread.Sleep(1000);

            Console.WriteLine();
            //Console.WriteLine("0. 다음");

            //아래는 테스트용으로 만들었습니다.
            Console.WriteLine("1. 전투승리");
            Console.WriteLine("2. 전투패배\n");

            // if문으로 몬스터가 죽었을때 - BattleWin() , 플레이어가 죽었을때 작성 - BattleLose()
            // 이하 else문
            //int input = ConsoleUtility.PromptMenuChoice(0, 0);
            //switch (input)
            //{
            //    case 0:
            //        BattleMenu();
            //        break;
            //}

            // 아래는 테스트용으로 만들었습니다.
            int input = ConsoleUtility.PromptMenuChoice(1, 2);
            switch (input)
            {
                case 1:
                    BattleWin();
                    break;
                case 2:
                    BattleLose();
                    break;
            }
        }

        public void BattleWin()
        {
            Console.Clear();
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "Battle!! - Result\n");
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Green, "", "Victory\n");
            Thread.Sleep(500);
            Console.WriteLine("던전에서 몬스터 {}마리를 잡았습니다.\n"); // 몬스터 카운트
            Thread.Sleep(1000);
            Console.WriteLine($"Lv.{player.Level} {player.Name}");
            Thread.Sleep(1000);
            Console.WriteLine($"HP (전투 전 HP) -> {player.Hp}\n");
            Thread.Sleep(1000);
            Console.WriteLine("0. 다음\n");

            int input = ConsoleUtility.PromptMenuChoice(0, 0);
            switch (input)
            {
                case 0:
                    MainMenu();
                    break;
            }
        }

        public void BattleLose()
        {
            Console.Clear();
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "Battle!! - Result\n");
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Red, "", "Lose\n");
            Thread.Sleep(500);
            Console.WriteLine($"Lv.{player.Level} {player.Name}");
            Thread.Sleep(1000);
            Console.WriteLine($"HP (전투 전 HP) -> {player.Hp}\n");
            Thread.Sleep(1000);
            Console.WriteLine("0. 다음\n");

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
            while(true)
            {
            GameManager gameManager = new GameManager();
            gameManager.MainMenu();
            }
        }
    }
}
