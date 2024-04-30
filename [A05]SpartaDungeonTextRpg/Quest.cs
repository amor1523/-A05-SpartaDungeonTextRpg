using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace _A05_SpartaDungeonTextRpg
{
    internal class Quest
    {
        public List<Quest> questdata = new List<Quest>();


        public void quests()
        {
            questdata.Add(new Quest(0, "아이템 구매",)); // 작업중
        }

        public int Number { get; }
        public string? title { get; }
        public int GoalCount { get; set; }
        public bool QuestAccept { get; set; }
        public string? Rewarditem { get; }
        public int RewardGold { get; }
        public int RewardExp { get; }
        public bool ClearQuest { get; set; }
    }


    class itemQuest0 : Quest
    {
        int goalCount = 0;
        int rewardgold = 5;
        int rewardexp = 2;
        int input;

        public void SelectQuest() 
        {
            Console.Clear();
            Console.WriteLine("Quest!\n"); // 퀘스트 텍스트 색상 변경 필요
            Console.WriteLine("아이템 장착\n");
            Console.WriteLine("신참 모험가로군, 던전에 들어가기 위해선 장비가 필요하다네.");
            Console.WriteLine("상점에서 '낡은 검'을 구매해 보게나.\n");
            Console.WriteLine($"- 낡은 검{goalCount}/1\n"); // 예시
            Console.WriteLine("- 보상");
            Console.WriteLine($"  {rewardgold}G");
            Console.WriteLine($"  {rewardexp}exp");
            switch (QuestAccept)
            {
                case false:
                    Console.WriteLine("1. 수락");
                    Console.WriteLine("2. 거절");
                    input = ConsoleUtility.PromptMenuChoice(1, 2);

                    break;
                case true:
                    Console.WriteLine("이미 수락한 퀘스트 입니다.");
                    Console.WriteLine("0. 돌아가기.");
                    break;
            }

            if (QuestAccept == false || input == 1)
            {
                QuestAccept = true;
                //퀘스트 목록 앞에 수락중 텍스트 추가 필요 - 퀘스트 목록 창에서 설정
                QuestList(); //퀘스트 리스트 작업필요
            }
    }
}
