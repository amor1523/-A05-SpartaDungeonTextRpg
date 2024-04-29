namespace _A05_SpartaDungeonTextRpg
{
    public class GameManager
    {
        private Player player;

        public GameManager()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            player = new Player("Chad", "전사", 1, 10, 5, 100, 1500f);
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
                    // 2. 전투시작
                    break;

            }
        }

        // 플레이어의 현재 상태를 보여줍니다.
        public void StatusMenu()
        {
            Console.Clear();
            ConsoleUtility.PrintTextHighlights("", "[상태보기]");
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
