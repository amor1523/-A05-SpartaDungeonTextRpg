using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace _A05_SpartaDungeonTextRpg
{
    public class Quest
    {
        public List<Quest> questData = new List<Quest>(); //퀘스트 전체 목록
        public List<Quest> questList = new List<Quest>(); //활성화 퀘스트 목록, 클리어 한 퀘스트는 라인업에서 지우고 리스트 새로 작성 - bool ClearQuest
        int input;

        public void quests()
        {
            questData.Add(new Quest(0, "아이템 구매", false, false));
            questData.Add(new Quest(1, "거미 처치", false, false));
            questData.Add(new Quest(2, "스킬 사용", false, false));
        }

        public void activeQuests() // 클리어하지 않은 퀘스트만 리스트에 저장
        {
            questList.Clear();
            int max = questData.Count;
            for (int i = 0; i < max; i++)
            {
                Quest questinfo = questData[i];
                if (questinfo.ClearQuest == false)
                {
                    questList.Add(questinfo);
                }
            }
        }

        public void QuestList() // 퀘스트 리스트 출력
        {
            Console.Clear();
            Console.WriteLine("Quest!\n"); // 퀘스트 텍스트 색상 변경 필요
            for(int i = 0;i < questList.Count;i++)
            {
                Console.WriteLine($"{questList[i].Number}. {questList[i].Title}");
            }
            Console.WriteLine("\n\n원하시는 퀘스트를 선택해주세요");
            Console.WriteLine(">>");
        }


        public int Number { get; }
        public string? Title { get; }
        public int Count { get; set; }
        public int GoalCount { get; set; }
        public bool AcceptQuest { get; set; }
        public string? RewardItem { get; set; }
        public int RewardGold { get; set; }
        public int RewardExp { get; set; }
        public bool ClearQuest { get; set; }


        public Quest()
        {

        }

        public Quest(int number, string title, bool acceptQuest = false, bool clearQuest = false)
        {
            Number = number;
            Title = title;
            AcceptQuest = acceptQuest;
            ClearQuest = clearQuest;
        }



        public void Boolcondition() //퀘스트 조건 충족 조건 발동
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

        public void QuestClear() // 퀘스트 클리어 했을 경우 발동하는 
        {
            Console.WriteLine("1. 보상 받기");
            Console.WriteLine("2. 돌아가기");
            input = ConsoleUtility.PromptMenuChoice(1, 2);
            Console.WriteLine("\n\n원하시는 행동을 선택해주세요");
            Console.WriteLine(">>");
            switch (input)
            {
                case 1:
                    ClearQuest = true;
                    AcceptQuest = false;
                    //아이템,골드,경험치 추가 필요
                    //QuestList(); //퀘스트 리스트 작업필요
                    break;
                case 2:
                    //QuestList(); //퀘스트 리스트 작업필요
                    break;
            }
        }

        public void BoolQuestAccept()
        {
            switch (AcceptQuest)
            {
                case false:
                    Console.WriteLine("1. 수락");
                    Console.WriteLine("2. 거절");
                    Console.WriteLine("\n\n원하시는 행동을 선택해주세요");
                    Console.WriteLine(">>");
                    input = ConsoleUtility.PromptMenuChoice(1, 2);
                    break;
                case true:
                    Console.WriteLine("이미 수락한 퀘스트 입니다.");
                    Console.WriteLine("0. 돌아가기.");
                    Console.WriteLine("\n\n원하시는 행동을 선택해주세요");
                    Console.WriteLine(">>");
                    input = ConsoleUtility.PromptMenuChoice(0, 0);
                    break;
            }

            if (AcceptQuest == false || input == 1) //퀘스트를 수락한 경우
            {
                AcceptQuest = true;
                //퀘스트 목록 앞에 수락중 텍스트 추가 필요 - 퀘스트 목록 창에서 설정
                //QuestList(); //퀘스트 리스트 작업필요
            }
            else if ((AcceptQuest == false || input == 2) && (AcceptQuest == true || input == 0)) // 퀘스트를 거절하거나 이미 수락된 퀘스트인 경우
            {
                //QuestList(); //퀘스트 리스트 작업필요
            }
        }
    }


    public class Quest0 : Quest
    {
        public void SelectQuest()
        {
            Count = 0; //아이템 구매 탭에서 조건을 만족하면 카운트 ++ 작업 (항상 퀘스트 카운트 증가는 퀘스트가 수락 되어 있다는 조건에서 발동해야함)
            GoalCount = 1;
            RewardGold = 5;
            RewardExp = 2;
            Console.Clear();
            Console.WriteLine("Quest!\n"); // 퀘스트 텍스트 색상 변경 필요
            Console.WriteLine("아이템 구매\n");
            Console.WriteLine("신참 모험가로군, 던전에 들어가기 위해선 장비가 필요하다네.");
            Console.WriteLine("상점에서 '낡은 검'을 구매해 보게나.\n");
            Console.WriteLine($"- 낡은 검 구매 ({Count}/{GoalCount})\n"); // 예시
            Console.WriteLine("- 보상");
            Console.WriteLine($"  {RewardGold}G");
            Console.WriteLine($"  {RewardExp}Exp\n");
            Boolcondition();
        }
    }

    public class Quest1 :Quest
    {
        public void SelectQuest()
        {
            Count = 0;//거미를 잡으면 카운트 ++ 작업 (항상 퀘스트 카운트 증가는 퀘스트가 수락 되어 있다는 조건에서 발동해야함)
            GoalCount = 5;
            RewardItem = "쓸만한 방패";
            RewardGold = 10;
            RewardExp = 8;
            Console.Clear();
            Console.WriteLine("Quest!\n"); // 퀘스트 텍스트 색상 변경 필요
            Console.WriteLine("거미 처치\n");
            Console.WriteLine("요즘 마을 밖에 사람 몸통만한 거미들이 나왔다고 하네");
            Console.WriteLine("마을에 왕래하는 행상인들이 마주치면 혼비백산하여 도망치는 상황이 발생해 고민이 이만저만이 아니야.");
            Console.WriteLine("개체 수를 줄여주면 보답하겠네.\n");
            Console.WriteLine($"- 거미 5마리 처치 ({Count}/{GoalCount})\n");
            Console.WriteLine("- 보상");
            Console.WriteLine($"  {RewardItem} X 1");
            Console.WriteLine($"  {RewardGold}G");
            Console.WriteLine($"  {RewardExp}Exp\n");
            Boolcondition();
        }
    }
    public class Quest2 : Quest
    {
        public void SelectQuest()
        {
            Count = 0;//아무 스킬이나 사용하면 ++ 작업 (항상 퀘스트 카운트 증가는 퀘스트가 수락 되어 있다는 조건에서 발동해야함)
            GoalCount = 1;
            RewardGold = 8;
            RewardExp = 5;
            Console.Clear();
            Console.WriteLine("Quest!\n"); // 퀘스트 텍스트 색상 변경 필요
            Console.WriteLine("스킬 사용\n");
            Console.WriteLine("자네 '스킬'을 알고있나?");
            Console.WriteLine("스킬을 활용하면 몬스터에게 높은 데미지를 줄 수 있지");
            Console.WriteLine("몬스터에게 스킬을 한 번 사용해 보게.\n");
            Console.WriteLine($"- 스킬 사용 ({Count}/{GoalCount})\n");
            Console.WriteLine("- 보상");
            Console.WriteLine($"  {RewardItem} X 1");
            Console.WriteLine($"  {RewardGold}G");
            Console.WriteLine($"  {RewardExp}Exp\n");
            Boolcondition();
        }
    }
}

