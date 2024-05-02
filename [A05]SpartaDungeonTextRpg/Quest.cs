using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using _A05_SpartaDungeonTextRpg;
using SpartaDungeonTextRpg;


namespace _A05_SpartaDungeonTextRpg
{
    public class Quest
    {
        public static Quest quest = new Quest();
        public static List<Quest> questData = new List<Quest>(); //퀘스트 전체 목록
        public List<Quest> questList = new List<Quest>(); //활성화 퀘스트 목록, 클리어 한 퀘스트는 라인업에서 지우고 리스트 새로 작성 - bool ClearQuest
        int input;
        public void quests()
        {
            questData.Clear();
            questData.Add(new Quest(0, "아이템 구매", false, false, 0));
            questData.Add(new Quest(1, "거미 처치", false, false, 0));
            questData.Add(new Quest(2, "스킬 사용", false, false, 0));
        }

        public void setactiveQuests(List<Quest> questData) // 클리어하지 않은 퀘스트만 리스트에 저장
        {
            questList.Clear();
            int max = questData.Count;
            for (int i = 0; i < max; i++)
            {
                Quest questinfo = questData[i];
                if (questinfo.ClearQuest == false) //클리어한 퀘스트는 목록에서 삭제
                {
                    questList.Add(questinfo);
                }
            }
        }

        public void QuestList(List<Quest> questData) // 퀘스트 리스트 출력
        {
            Quest0 quest0 = new Quest0(); //퀘스트 선택에 따른 클래스 호출을 위한 선언
            Quest1 quest1 = new Quest1();
            Quest2 quest2 = new Quest2();
            int i;
            Console.Clear();
            setactiveQuests(questData); // 갱신 된 퀘스트 데이터를 새로 받아옴
            Console.WriteLine("Quest!\n"); // 퀘스트 텍스트 색상 변경 필요
            for( i = 0;i < questList.Count;i++)
            {
                if (questList[i].AcceptQuest == true)
                {
                    Console.WriteLine($"-수락중-{i + 1}. {questList[i].Title}"); // 퀘스트 수락중이라면 앞에 수락중 표시
                }
                else
                {
                    Console.WriteLine($"{i + 1}. {questList[i].Title}");
                }
            }
            if(questList.Count == 0)
            {
                Console.WriteLine("현재 받을 수 있는퀘스트가 없습니다."); // 퀘스트를 전부 클리어하면 출력 될 메세지
            }
            Console.WriteLine($"{i+1}. 나가기");
            input = ConsoleUtility.PromptMenuChoice(1, i+1); 
            if(input < i+1) // 받아 온 키 입력값 기준으로 퀘스트 텍스트로 이동
            {
                Quest quest = questData[input - 1];
                switch (questList[input - 1].Number)
                {
                    case 0:
                        quest0.SelectQuest(questData,0);
                        break;
                    case 1:
                        quest1.SelectQuest(questData,1);
                        break;
                    case 2:
                        quest2.SelectQuest(questData,2);
                        break;
                }
            }
            else if(input == i+1)
            {
                return;
            }
        }


        public int Number { get; set; }
        public string? Title { get; set; }
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

        public Quest(int number, string title, bool acceptQuest, bool clearQuest, int count)
        {
            Number = number;
            Title = title;
            AcceptQuest = acceptQuest;
            ClearQuest = clearQuest;
            Count = count;
        }



        public void Boolcondition(List<Quest> questData, int i) //퀘스트 조건 충족 조건 발동
        {
            if (questData[i].Count == GoalCount && questData[i].AcceptQuest == true) // 카운트가 목표 카운트와 같다면 퀘스트 클리어 텍스트 출력 / 
            {
                QuestClear(questData, i);
            }
            else
            {
                BoolQuestAccept(questData, i);
            }
        }

        public void QuestClear(List<Quest> questData, int i) // 퀘스트 클리어 했을 경우 나오는 텍스트
        {
            Console.WriteLine("1. 보상 받기");
            Console.WriteLine("2. 돌아가기");
            input = ConsoleUtility.PromptMenuChoice(1, 2);
            switch (input)
            {
                case 1:
                    questData[i].ClearQuest = true;
                    questData[i].AcceptQuest = false;
                    if(RewardItem != null)
                    {

                    }
                    QuestList(questData);
                    break;
                case 2:
                    QuestList(questData);
                    break;
            }
        }

        public void BoolQuestAccept(List<Quest> questData, int i) //퀘스트를 클리어하지 않은 경우
        {
            switch (questData[i].AcceptQuest)
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

            if (questData[i].AcceptQuest == false && input == 1)
            {
                questData[i].AcceptQuest = true;
                QuestList(questData);
            }
            else if ((questData[i].AcceptQuest == false && input == 2) || (questData[i].AcceptQuest == true && input == 0))
            {
                QuestList(questData);
            }
        }
    }


    public class Quest0 : Quest
    {
        public void SelectQuest(List<Quest> questData, int i)
        {
            Count = 0;
            GoalCount = 1;
            RewardGold = 5;
            RewardExp = 2;
            Console.Clear();
            Console.WriteLine("Quest!\n"); // 퀘스트 텍스트 색상 변경 필요
            Console.WriteLine("아이템 구매\n");
            Console.WriteLine("신참 모험가로군, 던전에 들어가기 위해선 장비가 필요하다네.");
            Console.WriteLine("상점에서 '낡은 검'을 구매해 보게나.\n");
            Console.WriteLine($"- 낡은 검 구매 ({questData[i].Count}/{GoalCount})\n"); // 예시
            Console.WriteLine("- 보상");
            Console.WriteLine($"  {RewardGold}G");
            Console.WriteLine($"  {RewardExp}Exp\n");
            Boolcondition(questData,i);
        }
    }

    public class Quest1 :Quest
    {
        public void SelectQuest(List<Quest> questData, int i)
        {
            //Count = 0;//거미를 잡으면 카운트 ++ 작업 (항상 퀘스트 카운트 증가는 퀘스트가 수락 되어 있다는 조건에서 발동해야함)
            GoalCount = 5;
            RewardItem = "쓸만한 방패";
            RewardGold = 10;
            RewardExp = 8;
            AcceptQuest = false;
            ClearQuest = false;
            Console.Clear();
            Console.WriteLine("Quest!\n"); // 퀘스트 텍스트 색상 변경 필요
            Console.WriteLine("거미 처치\n");
            Console.WriteLine("요즘 마을 밖에 사람 몸통만한 거미들이 나왔다고 하네");
            Console.WriteLine("마을에 왕래하는 행상인들이 마주치면 혼비백산하여 도망치는 상황이 발생해 고민이 이만저만이 아니야.");
            Console.WriteLine("개체 수를 줄여주면 보답하겠네.\n");
            Console.WriteLine($"- 거미 5마리 처치 ({questData[i].Count}/{GoalCount})\n");
            Console.WriteLine("- 보상");
            Console.WriteLine($"  {RewardItem} X 1");
            Console.WriteLine($"  {RewardGold}G");
            Console.WriteLine($"  {RewardExp}Exp\n");
            Boolcondition(questData, i);
        }
    }
    public class Quest2 : Quest
    {
        public void SelectQuest(List<Quest> questData, int i)
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
            Console.WriteLine($"- 스킬 사용 ({questData[i].Count}/{GoalCount})\n");
            Console.WriteLine("- 보상");
            Console.WriteLine($"  {RewardGold}G");
            Console.WriteLine($"  {RewardExp}Exp\n");
            Boolcondition(questData, i);
        }
    }

}

