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
        public int Count { get; set; }
        public int GoalCount { get; set; }
        public bool AcceptQuest { get; set; }
        public string? Rewarditem { get; }
        public int RewardGold { get; set; }
        public int RewardExp { get; set; }
        public bool ClearQuest { get; set; }
    }


    class itemQuest0 : Quest
    {
        int input;

        public void SelectQuest()
        {
            Count = 0; //아이템 구매 탭에서 조건을 만족하면 카운트 ++ 작업 (항상 퀘스트 카운트 증가는 퀘스트가 수락 되어 있다는 조건에서 발동해야함)
            GoalCount = 1;
            RewardGold = 5;
            RewardExp = 2;
            Console.Clear();
            Console.WriteLine("Quest!\n"); // 퀘스트 텍스트 색상 변경 필요
            Console.WriteLine("아이템 장착\n");
            Console.WriteLine("신참 모험가로군, 던전에 들어가기 위해선 장비가 필요하다네.");
            Console.WriteLine("상점에서 '낡은 검'을 구매해 보게나.\n");
            Console.WriteLine($"- 낡은 검{Count}/{GoalCount}\n"); // 예시
            Console.WriteLine("- 보상");
            Console.WriteLine($"  {RewardGold}G");
            Console.WriteLine($"  {RewardExp}exp\n");
            Boolcondition();

        }

        public void Boolcondition()
        {
            if (Count == GoalCount)
            {
                QuestClear();
            }
            else
            {
                BoolQuestAccept();
            }
        }

        public void QuestClear()
        {
            ClearQuest = true;
            AcceptQuest = false;
            Console.WriteLine("1. 보상 받기");
            Console.WriteLine("2. 돌아가기");
        }

        public void BoolQuestAccept()
        {
            switch (AcceptQuest)
            {
                case false:
                    Console.WriteLine("1. 수락");
                    Console.WriteLine("2. 거절");
                    input = ConsoleUtility.PromptMenuChoice(1, 2);

                    break;
                case true:
                    Console.WriteLine("이미 수락한 퀘스트 입니다.");
                    Console.WriteLine("0. 돌아가기.");
                    input = ConsoleUtility.PromptMenuChoice(0, 0);
                    break;
            }

            if (AcceptQuest == false || input == 1) //퀘스트를 수락한 경우
            {
                AcceptQuest = true;
                //퀘스트 목록 앞에 수락중 텍스트 추가 필요 - 퀘스트 목록 창에서 설정
                QuestList(); //퀘스트 리스트 작업필요
            }
            else if ((AcceptQuest == false || input == 2) && (AcceptQuest == true || input == 0)) // 퀘스트를 거절하거나 이미 수락된 퀘스트인 경우
            {
                QuestList();
            }
        }
    }
}
