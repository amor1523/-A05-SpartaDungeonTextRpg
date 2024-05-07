[코드컨벤션]

	1. namespace, class, struct : PascalCase
	2. variable: camelCase
	3. Function : PascalCase / 함수 내부: camelCase
	4. Public : PascalCase / Non Public : _camelCase


[깃허브컨벤션]

	<Branch 생성, Main으로 Merge>
	Branch 생성 : (본인 이름의 이니셜)_(세부기능)으로 작성
	Init : 프로젝트 초기 생성
	Add : 생성 후 첫 커밋전까지의 과정
	
	
	<Branch 내부작업>
	Fix : 버그, 오타 수정
	Complex : 여러가지 일 수행
	Feature : 새로운 기능 추가
	Update : 기존 기능을 업데이트 했을 때

	Rename : 파일 혹은 폴더명 수정하거나 옮기는 경우
	Remove : 파일을 삭제하는 작업만 수행하는 경우
	Chore : 기타 변경사항

	Refactor : 더 나은 코드로 변경
	Move : 코드의 이동이 있는 경우

	예시) 1. [Feature] 몬스터 전투 기능
		  2. [Update] 몬스터 전투 추가 기능

	
# A05 오지고지리조 스파르타 던전

## 프로젝트 설명
- 스파르타 던전은 텍스트로 구성된 RPG 게임입니다.
#### 팀원소개
##### 각 팀원이 구현한 기능

<details><summary>팀장 : 이경현</summary>

- 구현기능
  - 레벨업 기능
  - 보상 추가
  - 아이템 적용
  - 상점 기능

</details>

<details><summary>팀원 : 정지효A</summary>

- 캐릭터 생성 기능
- 직업 선택 기능
- 게임 저장하기

</details>

<details><summary>팀원 : 이정빈</summary>

- 몬스터 종류 및 보스 추가
- 퀘스트 기능
- 퀘스트 선택과 완료 기능

</details>

<details><summary>팀원 : 김창민</summary>

- 스킬 기능
- 전투 기능
- 보스 스테이지

</details>

## 개발환경 
- Visual Studio 2022 , C#
## 개발 기간
- 2024.04.29 ~ 2024.05.07
## 게임 화면
### 로비
- 캐릭터의 닉네임을 입력하고 직업을 선택한후 시작화면으로 이동
<details><summary></summary>

![인트로 메인화면 (1)](https://github.com/amor1523/-A05-SpartaDungeonTextRpg/assets/167047045/e0b1e092-8a3a-4e26-bde2-f424da8060b8)

<details><summary>코드 보기</summary>

```csharp
public void MainMenu()
{
    Console.Clear();
    Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
    Console.WriteLine("이제 전투를 시작할 수 있습니다.\n");
    Console.WriteLine("1. 상태보기");
    if (player.Level >= 1 && player.Level <= 4)

        Console.WriteLine($"2. 전투시작 (현재 진행 : {player.Level} Stage)");
    else
    {
        Console.Write($"2. 전투시작 ");
        ConsoleUtility.PrintTextHighlights(ConsoleColor.Red, "", "(현재 진행 : 보스 Stage!!)");
    }
    Console.WriteLine("3. 인벤토리");
    Console.WriteLine("4. 상점");
    Console.WriteLine("5. 물약사용");
    Console.WriteLine("6. 퀘스트");
    Console.WriteLine("7. 게임종료");
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine("0. 저장하기");
    Console.WriteLine();

    int input = ConsoleUtility.PromptMenuChoice(0, 7);
    switch (input)
    {
        case 0:
            JsonSerialize.SaveData(player, item, potion, quest);
            break;
        case 1:
            StatusMenu();
            break;

        case 2:
            battle = new Battle(player, monsters, this, skill, potion);
            battle.BattleMenu();
            break;

        case 3:
            item.Inventory();
            break;

        case 4:
            item.Shop();
            break;
        case 5:
            potion.PotionInventory();
            break;
        case 6:
            quest.QuestList(player, Quest.questData);
            break;
        case 7:
            GamePlay = false;
            Console.WriteLine("\n게임을 종료합니다.");
            break;
    }
}
```

</details>
</details>

### 상태창
-현재 캐릭터의 정보 및 능력치를 확인할 수 있는 공간

- 아이템을 장착하는 경우 추가된 능력치 확인가능
<details><summary></summary>
	
![A05_TextRPG (5)](https://github.com/amor1523/-A05-SpartaDungeonTextRpg/assets/167047045/166b1b57-f304-47fc-a267-99c8a241009f)
<details><summary>코드 보기</summary>

```csharp
    public void StatusMenu()
    {
        player.Atk = player.NonEquipAtk;
        player.Def = player.NonEquipDef;

        bool flagEquipArmor = false;
        bool flagEquipWeapon = false;

        int equipArmorPower = 0;
        int equipWeaponPower = 0;

        int equipAtk = 0;
        int equipDef = 0;

        Console.Clear();
        ConsoleUtility.ShowTitle("[상태보기]");
        Console.WriteLine("캐릭터의 정보를 표시합니다.\n");
        Console.WriteLine($"Lv. {player.Level}");
        Console.WriteLine($"{player.Name} ({dict[player.Job]})");

        if (Item.InventoryIndex.Count != 0)
        {
            foreach (var equip in Item.InventoryIndex)
            {
                if (equip.FlagEquip)
                {
                    if (equip.AttackPower != 0)
                    {
                        flagEquipWeapon = true;
                        equipWeaponPower += equip.AttackPower;
                    }
                    if (equip.DefensivePower != 0)
                    {
                        flagEquipArmor = true;
                        equipArmorPower += equip.DefensivePower;
                    }
                }
            }
            equipAtk = player.Atk + equipWeaponPower;
            equipDef = player.Def + equipArmorPower;
        }

        if (!flagEquipWeapon)
        {
            Console.Write(ConsoleUtility.PadRightForMixedText("공격력", 10));
            Console.WriteLine($" : {player.Atk}");
        }
        else
        {
            Console.Write(ConsoleUtility.PadRightForMixedText("공격력", 10));
            Console.WriteLine($" : {player.Atk} (+{equipWeaponPower})");
        }
        if (!flagEquipArmor)
        {
            Console.Write(ConsoleUtility.PadRightForMixedText("방어력", 10));
            Console.WriteLine($" : {player.Def}");
        }
        else
        {
            Console.Write(ConsoleUtility.PadRightForMixedText("방어력", 10));
            Console.WriteLine($" : {player.Def} (+{equipArmorPower})");
        }

        Console.Write(ConsoleUtility.PadRightForMixedText("HP / MAXHP", 10));
        Console.WriteLine($" : {player.Hp} / {player.MaxHp}");

        Console.Write(ConsoleUtility.PadRightForMixedText("MP / MAXMP", 10));
        Console.WriteLine($" : {player.Mp} / {player.MaxMp}");

        Console.Write(ConsoleUtility.PadRightForMixedText("Gold", 10));
        Console.WriteLine($" : {player.Gold} G");

        Console.Write(ConsoleUtility.PadRightForMixedText("Exp", 10));
        Console.WriteLine($" : {player.Exp}");

        if (player.Level < 5)
            Console.WriteLine($"LevelUp까지 남은 Exp -> {player.LevelUpExp - player.Exp}\n");

        Console.WriteLine("0. 나가기\n");

        if (equipAtk > 0)
            player.Atk = equipAtk;
        else
            player.Atk = player.NonEquipAtk;
        if (equipDef > 0)
            player.Def = equipDef;
        else
            player.Def = player.NonEquipDef;

        int input = ConsoleUtility.PromptMenuChoice(0, 0);
        switch (input)
        {
            case 0:
                return;
        }
    }
}
```

</details>
</details>

### 인벤토리
- 상점에서 구입한 아이템이나 던전을 돌고 얻은 보상들을 확인하고 착용할수있는 공간
<details><summary></summary>

![A05_TextRPG (1) (1)](https://github.com/amor1523/-A05-SpartaDungeonTextRpg/assets/167047045/f719d48f-0e64-42d2-b664-950ee774637a)

![A05_TextRPG (4)](https://github.com/amor1523/-A05-SpartaDungeonTextRpg/assets/167047045/3b579c60-8dac-4bb9-86f4-82783d3d6e48)

</details>
<details><summary>코드 보기</summary>

```csharp
    public void GetItem()
{
    // 방어구(Id) : 1번 부터 시작
    // 공격 무기(Id) : 1001번 부터 시작
    // 퀘스트 Reward(Id) : 3001번부터 시작
    ItemIndex.Add(new Item("수련자 갑옷", 1, 1000, 5, 0, "수련에 도움을 주는 옷입니다."));
    ItemIndex.Add(new Item("무쇠 갑옷", 2, 1500, 9, 0, "무쇠로 만들어져 튼튼한 갑옷입니다."));
    ItemIndex.Add(new Item("스파르타의 갑옷", 3, 2000, 15, 0, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다."));
    ItemIndex.Add(new Item("낡은 검", 1001, 500, 0, 2, "쉽게 볼 수 있는 낡은 검 입니다."));
    ItemIndex.Add(new Item("청동 도끼", 1002, 1000, 0, 5, "어디선가 사용됐던거 같은 도끼입니다."));
    ItemIndex.Add(new Item("스파르타의 창", 1003, 2000, 0, 7, "스파르타의 전사들이 사용했다는 전설의 창입니다."));
    ItemIndex.Add(new Item("쓸만한 방패", 3001, 0, 10, 0, "마을 사람들이 만들어준 쓸만한 방패입니다."));
}

public void Inventory()
{
    Console.Clear();
    Console.WriteLine("인벤토리");
    Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
    Console.WriteLine("[아이템 목록]\n");
    Console.WriteLine("1. 장착 관리");
    Console.WriteLine("0. 나가기\n");

    int input = ConsoleUtility.PromptMenuChoice(0, 1);

    switch (input)
    {
        case 0:
            return;
        case 1:
            InventoryEquip();
            break;
    }
}

private void InventoryEquip()
{
    Console.Clear();
    Console.WriteLine("인벤토리 - 장착관리");
    Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
    Console.WriteLine("[아이템 목록]");

    if (InventoryIndex.Count != 0)
    {
        int index = 1;
        foreach (var item in InventoryIndex)
        {
            if (item.FlagEquip)
                Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"- {index} ");
            if (item.FlagEquip)
            {
                Console.Write("[E]");
                Console.Write(ConsoleUtility.PadRightForMixedText(item.Name, 15));
            }
            else
                Console.Write(ConsoleUtility.PadRightForMixedText(item.Name, 18));

            Console.Write(" | ");

            if (item.AttackPower != 0)
                Console.Write($"공격력 {(item.AttackPower >= 0 ? "+" : "")} {ConsoleUtility.PadRightForMixedText(item.AttackPower.ToString(), 5)}");
            if (item.DefensivePower != 0)
                Console.Write($"방어력 {(item.DefensivePower >= 0 ? "+" : "")} {ConsoleUtility.PadRightForMixedText(item.DefensivePower.ToString(), 5)}");

            Console.Write(" | ");

            Console.WriteLine(item.Explain);
            index++;
            Console.ResetColor();
        }
    }
    else
        Console.WriteLine("보유하고 있는 장비가 없습니다.");

    Console.WriteLine("\n0. 나가기\n");

    int input = ConsoleUtility.PromptMenuChoice(0, InventoryIndex.Count);

    if (input == 0)
        return;
    else
    {
        Item selectItem = InventoryIndex[input - 1];
        if (selectItem != null)
        {
            if (!selectItem.FlagEquip)
                selectItem.FlagEquip = true;
            else
                selectItem.FlagEquip = false;
            InventoryEquip();
            return;
        }
    }
}
```
</details>
</details>
### 상점
- 던전에서 얻은 GOLD로 아이템을 살수있는 공간
<details><summary></summary>

![A05_TextRPG (4) (1)](https://github.com/amor1523/-A05-SpartaDungeonTextRpg/assets/167047045/d247094e-3574-4a65-b3c9-d8e92c2a2ad3)
<details><summary>코드 보기</summary>

```csharp
 {
    Console.Clear();
    Console.WriteLine("상점");
    Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");

    Console.WriteLine("[보유 골드]");
    Console.WriteLine($"{player.Gold} G\n");
    Console.WriteLine("[아이템 목록]");

    if (ItemIndex.Count != 0)
    {
        int index = 1;
        foreach (var item in ItemIndex)
        {
            if (item.Gold != 0)
            {
                if (item.FlagBuy)
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("- ");
                if (FlagShopBuy)
                {
                    Console.Write($"{index} ");
                }
                Console.Write(ConsoleUtility.PadRightForMixedText(item.Name, 20));

                Console.Write(" | ");

                if (item.AttackPower != 0)
                {
                    if(!item.FlagBuy)
                        Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"공격력 {(item.AttackPower >= 0 ? "+" : "")} {ConsoleUtility.PadRightForMixedText(item.AttackPower.ToString(), 7)}");
                    if(!item.FlagBuy)
                        Console.ResetColor();
                }
                if (item.DefensivePower != 0)
                {
                    if(!item.FlagBuy)
                        Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($"방어력 {(item.DefensivePower >= 0 ? "+" : "")} {ConsoleUtility.PadRightForMixedText(item.DefensivePower.ToString(), 7)}");
                    if(!item.FlagBuy)
                        Console.ResetColor();
                }
                Console.Write(" | ");

                Console.Write(ConsoleUtility.PadRightForMixedText(item.Explain, 55));

                Console.Write(" | ");

                if (!item.FlagBuy)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(ConsoleUtility.PadRightForMixedText(item.Gold.ToString() + " G", 5));
                    Console.ResetColor();
                }
                else
                    Console.WriteLine(ConsoleUtility.PadRightForMixedText("구매완료", 5));

                index++;
                Console.ResetColor();
            }
        }
    }
    if (potion.PotionIndex.Count != 0)
    {
        int index = 7;
        foreach (var item in potion.PotionIndex)
        {
            if (item.FlagBuy)
                Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("- ");
            if (FlagShopBuy)
            {
                Console.Write($"{index} ");
            }
            Console.Write(ConsoleUtility.PadRightForMixedText(item.Name, 20));

            Console.Write(" | ");

            if (item.Id == 2001)
                Console.Write(ConsoleUtility.PadRightForMixedText("보유 개수: " + item.Count.ToString() + " 개", 16));
            else
                Console.Write(ConsoleUtility.PadRightForMixedText("", 16));

            Console.Write(" | ");

            Console.Write(ConsoleUtility.PadRightForMixedText(item.Explain, 55));

            Console.Write(" | ");

            if (!item.FlagBuy)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(ConsoleUtility.PadRightForMixedText(item.Gold.ToString() + " G", 5));
                Console.ResetColor();
            }
            else
                Console.WriteLine(ConsoleUtility.PadRightForMixedText("구매완료", 5));

            index++;
            Console.ResetColor();
        }
    }

    int input;
    Console.WriteLine();
    if (!FlagShopBuy)
        Console.WriteLine("1. 아이템 구매");
    Console.WriteLine("0. 나가기\n");
    if (!FlagShopBuy)
        input = ConsoleUtility.PromptMenuChoice(0, 1);
    else
        input = ConsoleUtility.PromptMenuChoice(0, ItemIndex.Count + potion.PotionIndex.Count);

    if (!FlagShopBuy)
    {
        switch (input)
        {
            case 0:
                return;
            case 1:
                FlagShopBuy = true;
                Shop();
                break;
        }
    }
    else
    {
        if (input == 0)
        {
            FlagShopBuy = false;
            return;
        }
        else
        {
            if (input <= 6)
            {
                Item selectItem = ItemIndex[input - 1];

                if (selectItem != null)
                {
                    if (!selectItem.FlagBuy)
                    {
                        selectItem.BuyItem(player, ItemIndex, Quest.quest);
                        if (selectItem.FlagBuy)
                            InventoryIndex.Add(selectItem);
                        Thread.Sleep(500);
                        Shop();
                        return;
                    }
                    else
                    {
                        Console.WriteLine("\n이미 구매한 아이템 입니다.");
                        Thread.Sleep(500);
                        Shop();
                        return;
                    }
                }
            }
            else
            {
                Potion selectPotion = potion.PotionIndex[input - 7];

                if (selectPotion != null)
                {
                    if (!selectPotion.FlagBuy)
                    {
                        selectPotion.BuyItem(player);
                        if (selectPotion.FlagBuy)
                        {
                            selectPotion.Count += 1;
                            if (selectPotion.Id == 2001)
                                selectPotion.FlagBuy = false;
                        }
                        Thread.Sleep(500);
                        Shop();
                        return;
                    }
                    else
                    {
                        Console.WriteLine("이미 구매한 아이템 입니다.");
                        Thread.Sleep(500);
                        Shop();
                        return;
                    }
                }
            }
        }
    }
}
```

</details>
</details>

### 퀘스트
- 퀘스트를 받거나 퀘스트를 완료해서 보상을 얻는 공간
<details><summary></summary>

![A05_TextRPG (2) (1)](https://github.com/amor1523/-A05-SpartaDungeonTextRpg/assets/167047045/baf088f1-a4cf-469b-88e8-d3d3f140e8ff)
<details><summary>코드 보기</summary>

```csharp
     public class Quest
    {
        private Player player;
        public static Quest quest = new Quest();
        public static Quest0 quest0 = new Quest0(); //퀘스트 선택에 따른 클래스 호출을 위한 선언
        public static Quest1 quest1 = new Quest1(); //퀘스트 선택에 따른 클래스 호출을 위한 선언
        public static Quest2 quest2 = new Quest2(); //퀘스트 선택에 따른 클래스 호출을 위한 선언
        public static List<Quest> questData = new List<Quest>(); //퀘스트 전체 목록
        public List<Quest> questList = new List<Quest>(); //활성화 퀘스트 목록, 클리어 한 퀘스트는 라인업에서 지우고 리스트 새로 작성 - bool ClearQuest
        int input;
        public void quests()
        {
            questData.Clear();
            questData.Add(new Quest(0, "아이템 구매", false, false, 0, 1, 50, 20));
            questData.Add(new Quest(1, "거미 처치", false, false, 0, 5, 100 , 80));
            questData.Add(new Quest(2, "스킬 사용", false, false, 0, 1, 80, 50));
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

        public void QuestList(Player player, List<Quest> questData) // 퀘스트 리스트 출력
        {
            int i;
            Console.Clear();
            setactiveQuests(questData); // 갱신 된 퀘스트 데이터를 새로 받아옴

            // 낡은검 구매 여부 확인
            if (Item.ItemIndex[3].FlagBuy == true && Quest.questData[0].Count < Quest0.questData[0].GoalCount)
            {
                Quest.questData[0].Count += 1;
            }

            Console.WriteLine("Quest!\n"); // 퀘스트 텍스트 색상 변경 필요
            for( i = 0;i < questList.Count;i++)
            {
                if (questList[i].Count == questList[i].GoalCount && questList[i].AcceptQuest == true)
                {
                    Console.Write(ConsoleUtility.PadRightForMixedText("[퀘스트 완료] ", 6));
                    Console.WriteLine($"{i + 1}. {questList[i].Title}"); // 퀘스트 완료조건 충족시 퀘스트 완료 표시
                }
                else if (questList[i].AcceptQuest == true)
                {
                    Console.Write(ConsoleUtility.PadRightForMixedText("[수락중] ", 6));
                    Console.WriteLine($"{i + 1}. {questList[i].Title}"); // 퀘스트 수락중이라면 앞에 수락중 표시
                }
                else
                {
                    Console.Write(ConsoleUtility.PadRightForMixedText("[대기중] ", 6));
                    Console.WriteLine($"{i + 1}. {questList[i].Title}");
                }
            }
            if(questList.Count == 0)
            {
                Console.WriteLine("현재 받을 수 있는퀘스트가 없습니다."); // 퀘스트를 전부 클리어하면 출력 될 메세지
            }
            Console.WriteLine($"\n0. 나가기\n");
            input = ConsoleUtility.PromptMenuChoice(0, i); 
            if(input < i+1 && input >0) // 받아 온 키 입력값 기준으로 퀘스트 텍스트로 이동
            {
                Quest quest = questData[input - 1];
                switch (questList[input - 1].Number)
                {
                    case 0:
                        quest0.SelectQuest(player, questData,0);
                        break;
                    case 1:
                        quest1.SelectQuest(player, questData,1);
                        break;
                    case 2:
                        quest2.SelectQuest(player, questData,2);
                        break;
                }
            }
            else if(input == 0)
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

        public Quest(Player player)
        {
            this.player = player;
        }

        public Quest(int number, string title, bool acceptQuest, bool clearQuest, int count, int goalCount,int rewardGold, int rewardExp)
        {
            Number = number;
            Title = title;
            AcceptQuest = acceptQuest;
            ClearQuest = clearQuest;
            Count = count;
            GoalCount = goalCount;
            RewardGold = rewardGold;
            RewardExp = rewardExp;
        }

        // 역직렬화 후 Quest에게 QuestSave를 넘겨주기 위한 메서드 (Load시 사용)
        public void SetQuest(QuestSave questSave)
        {
            questData = questSave.questData;
            questList = questSave.questList;
        }


        public void Boolcondition(Player player, List<Quest> questData, int i) //퀘스트 조건 충족 조건 발동
        {
            if (questData[i].Count == questData[i].GoalCount && questData[i].AcceptQuest == true) // 카운트가 목표 카운트와 같다면 퀘스트 클리어 텍스트 출력 / 
            {
                QuestClear(player, questData, i);
            }
            else
            {
                BoolQuestAccept(player, questData, i);
            }
        }

        public void QuestClear(Player player, List<Quest> questData, int i) // 퀘스트 클리어 했을 경우 나오는 텍스트
        {
            Console.WriteLine("1. 보상 받기");
            Console.WriteLine("2. 돌아가기\n");
            input = ConsoleUtility.PromptMenuChoice(1, 2);
            switch (input)
            {
                case 1:
                    questData[i].ClearQuest = true;
                    questData[i].AcceptQuest = false;
                    Reciveitem(i);
                    ReciveGold(player, i);
                    ReciveExp(player, i);
                    QuestList(player, questData);
                    break;
                case 2:
                    QuestList(player, questData);
                    break;
            }
        }

        public void BoolQuestAccept(Player player, List<Quest> questData, int i) //퀘스트를 클리어하지 않은 경우
        {
            switch (questData[i].AcceptQuest)
            {
                case false:
                    Console.WriteLine("1. 수락");
                    Console.WriteLine("2. 거절\n");
                    input = ConsoleUtility.PromptMenuChoice(1, 2);
                    break;
                case true:
                    Console.WriteLine("이미 수락한 퀘스트 입니다.\n");
                    Console.WriteLine("0. 돌아가기.\n");
                    input = ConsoleUtility.PromptMenuChoice(0, 0);
                    break;
            }

            if (questData[i].AcceptQuest == false && input == 1)
            {
                questData[i].AcceptQuest = true;
                QuestList(player, questData);
                return;
            }
            else if ((questData[i].AcceptQuest == false && input == 2) || (questData[i].AcceptQuest == true && input == 0))
            {
                QuestList(player, questData);
                return;
            }
        }

        public void Reciveitem(int i)
        {
            if (questData[1].ClearQuest == true)
            {
                Item.InventoryIndex.Add(Item.ItemIndex[6]);
            }
        }
        public void ReciveGold(Player player, int i)
        {
            player.Gold += questData[i].RewardGold;
        }
        public void ReciveExp(Player player, int i)
        {
            player.Exp += questData[i].RewardExp;
        }
    }


    public class Quest0 : Quest
    {
        public void SelectQuest(Player player, List<Quest> questData, int i)
        {
            Console.Clear();
            Console.WriteLine("Quest!\n"); // 퀘스트 텍스트 색상 변경 필요
            Console.WriteLine("아이템 구매\n");
            Console.WriteLine("신참 모험가로군, 던전에 들어가기 위해선 장비가 필요하다네.");
            Console.WriteLine("상점에서 '낡은 검'을 구매해 보게나.\n");
            Console.WriteLine($"- 낡은 검 구매 ({questData[i].Count}/{questData[i].GoalCount})\n"); // 예시
            Console.WriteLine("- 보상");
            Console.WriteLine($"  {questData[i].RewardGold}G");
            Console.WriteLine($"  {questData[i].RewardExp}Exp\n");
            Boolcondition(player, questData,i);
        }
    }

    public class Quest1 :Quest
    {
        public void SelectQuest(Player player, List<Quest> questData, int i)
        {
            //Count = 0;//거미를 잡으면 카운트 ++ 작업 (항상 퀘스트 카운트 증가는 퀘스트가 수락 되어 있다는 조건에서 발동해야함)
            GoalCount = 5;
            RewardItem = "쓸만한 방패";
            RewardGold = 100;
            RewardExp = 80;
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
            Boolcondition(player, questData, i);
        }
    }
    public class Quest2 : Quest
    {
        public void SelectQuest(Player player, List<Quest> questData, int i)
        {
            Count = 0;//아무 스킬이나 사용하면 ++ 작업 (항상 퀘스트 카운트 증가는 퀘스트가 수락 되어 있다는 조건에서 발동해야함)
            GoalCount = 1;
            RewardGold = 80;
            RewardExp = 50;
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
            Boolcondition(player, questData, i);
        }
    }

}


```

</details>
</details>

### 던전
- 전투를 시작해서 몬스터들과 싸울수 있는 공간
<details><summary></summary>

![A05_TextRPG (3) (1)](https://github.com/amor1523/-A05-SpartaDungeonTextRpg/assets/167047045/d03f8f76-7af4-4372-8f11-cb0a980175fb)

![A05_TextRPG (1) (1)](https://github.com/amor1523/-A05-SpartaDungeonTextRpg/assets/167047045/196a735d-9002-40f9-b571-f77472efd968)
<details><summary>코드 보기</summary>

```csharp
  public class Battle
{
    Dictionary<Job, string> dict = new Dictionary<Job, string>()
        {
            {Job.Knight, "전사"},
            {Job.Mage, "마법사"},
            {Job.Archer, "궁수"}
        };
    private Monster.BossMonster boss;
    private Player player;
    private Potion potion;
    private List<Monster> monsters;
    private Random random = new Random();
    private GameManager gameManager;
    private Skill skill;
    private int beforeHp;
    private int beforeExp;
    private int beforeMp;
    private bool BattleMonster = false;
    private bool bossClear = false;
    private bool bossPhaseTwo = false;
    private bool playerAttack = false;
    private int bossDotCount = 0;
    private int timerCount = 0;
    private string bossUltimatePlayerInput = "";
    private string bossUltimateComputerInput = "";
    private int[] bossUltimateArray = new int[9];
    static System.Timers.Timer timer = new System.Timers.Timer();

    public Battle(Player player, List<Monster> monsters, GameManager gameManager, Skill skill, Potion potion)
    {
        this.player = player;
        this.monsters = monsters;
        this.gameManager = gameManager;
        this.skill = skill;
        this.potion = potion;
        beforeHp = player.Hp;
        beforeExp = player.Exp;
        beforeMp = player.Mp;
        this.potion = potion;
        boss = new Monster.BossMonster(1, "레드 드래곤 (보스)", 10, 60, 400, 400, 10000, 500);
    }
    public void TextPlayerInfo()
    {
        Console.WriteLine();
        Console.WriteLine("[내정보]");

        Console.Write(ConsoleUtility.PadRightForMixedText($"Lv.{player.Level}", 5));
        Console.Write(" |  ");
        Console.WriteLine($"{player.Name} ({dict[player.Job]})");

        Console.Write(ConsoleUtility.PadRightForMixedText("Hp", 5));
        Console.Write(" |  ");
        Console.WriteLine($"{player.Hp}/{player.MaxHp} ");

        Console.Write(ConsoleUtility.PadRightForMixedText("Mp", 5));
        Console.Write(" |  ");
        Console.WriteLine($"{player.Mp}/{player.MaxMp}\n");
    }

    public void TextMonsters()
    {
        Console.WriteLine("전투 중인 몬스터들:");
        DeadMonster();
    }

    public void TextSkillMenu()
    {
        Console.WriteLine("[스킬 목록]");
        for (int j = 0; j < skill.SkillNum; j++)
        {
            Console.WriteLine($"{skill.SkillNum}.{skill.Name}은 적에게 {skill.SkillDamage}를 입힌다. (Mp 소요 : {skill.MpCost})\n");
        }
    }

    public void TextAttackResult(int damageDealt, Monster selectedMonster, bool isMiss, bool isCritical)
    {
        Console.Clear();
        ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "Battle!!\n");
        Console.WriteLine($"{player.Name} 의 공격!");
        Thread.Sleep(500);

        if (isMiss)
        {
            damageDealt = 0;
            Console.WriteLine($"Lv.{selectedMonster.Level} {selectedMonster.Name} 을(를) 공격했지만 아무일도  일어나지 않았습니다.\n");
            Thread.Sleep(500);
            Console.WriteLine("0.다음");
            int input1 = ConsoleUtility.PromptMenuChoice(0, 0);
            if (input1 == 0)
            {
                PromptForNextAction();
            }
        }
        else
        {
            Console.WriteLine($"Lv.{selectedMonster.Level} {selectedMonster.Name} 을(를) 맞췄습니다. [데미지 : {damageDealt}]");
            if (isCritical)
            {
                Console.WriteLine($"Lv.{selectedMonster.Level} {selectedMonster.Name} 을(를) 맞췄습니다. [데미지 : {damageDealt}] - 치명타 공격!!");
            }
        }
    }

    public void TextEnemyAttack(Monster targetMonster, int damageDealt)
    {
        ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "Battle!!\n");
        Console.WriteLine($"{targetMonster.Name} 의 공격!");
        Thread.Sleep(500);
        player.TakeDamage(damageDealt);
        Console.WriteLine($"Lv.{player.Level} {player.Name} 을/를 맞췄습니다. [데미지 : {damageDealt}]");
        Thread.Sleep(500);
        Console.WriteLine();
        Console.WriteLine($"Lv.{player.Level} {player.Name}");
        Thread.Sleep(500);
        Console.WriteLine($"HP {player.Hp}\n");
        Thread.Sleep(500);
    }

    public void TextBattle()
    {
        Console.Clear();
        ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "Battle!!\n");
        Thread.Sleep(500);
    }

    public void TextBattleBoss()
    {
        Console.Clear();
        ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "BossStage!!\n");
        Thread.Sleep(500);
    }

    public void PromptForNextAction()
    {
        Console.WriteLine("0.다음\n");
        int input = ConsoleUtility.PromptMenuChoice(0, 0);
        if (input == 0)
        {
            // 몬스터들이 플레이어를 한 번씩 공격
            foreach (Monster monster in this.monsters)
            {
                Console.Clear();
                EnemyAttack(monster);
            }

            // 모든 몬스터가 죽었을 때 승리 처리
            if (this.monsters.All(m => m.IsDead))
            {
                foreach (var monster in monsters)
                {
                    player.Exp += monster.RewardExp;
                }
                BattleResult(true);
                return;
            }
            else
            {
                // 모든 몬스터가 공격한 후에 플레이어가 살아있는지 확인
                if (!player.IsDead)
                {
                    BattleMenu();
                    return;
                }
                else
                {
                    BattleResult(false);
                    return;
                }
            }
        }
    }

    public void BattleMenu()
    {
        if (player.Level >= 1 && player.Level <= 4)
        {
            TextBattle();
            TextMonsters();
            TextPlayerInfo();

            Console.WriteLine("1. 일반 공격");
            Console.WriteLine("2. 스킬 사용");
            Console.WriteLine("3. 포션 사용");
            Console.WriteLine("4. 도망 치기\n");

            int input = ConsoleUtility.PromptMenuChoice(1, 4);
            switch (input)
            {
                case 1:
                    BattleMonster = true;
                    PlayerAttack();
                    break;
                case 2:
                    BattleMonster = true;
                    SkillAttack();
                    break;
                case 3:
                    UsePotion();
                    break;
                case 4:
                    RunAway();
                    break;
            }
        }
        else
            BossStage();
    }

    public void SkillAttack()
    {
        int deadMonsterIdx = 0;
        TextBattle();
        TextMonsters();
        TextPlayerInfo();
        TextSkillMenu();
        Console.WriteLine("0.취소\n");

        int input = ConsoleUtility.PromptMenuChoice(0, 1);
        if (input == 0)
        {
            BattleMonster = false;
            BattleMenu();
            return;
        }
        Console.WriteLine("\n[공격할  몬스터]");

        int input2 = ConsoleUtility.PromptMenuChoice(1, monsters.Count);
        // 선택한 몬스터 인덱스
        deadMonsterIdx = input2 - 1;
        // 공격할 몬스터
        Monster selectedMonster = monsters[deadMonsterIdx];

        // 선택한 몬스터가 이미 죽은 상태인지 확인
        if (selectedMonster.IsDead)
        {
            Console.WriteLine("\n잘못된 입력입니다.");
            Thread.Sleep(500);
            SkillAttack(); // 다시 공격 메뉴로 돌아감
            return;
        }

        int usedMana = skill.MpCost;
        if (player.Mp < usedMana)
        {
            Console.WriteLine("마나가 부족하여 스킬을 사용할수없습니다.");
            Thread.Sleep(500);
            SkillAttack();
            return;
        }
        skill.UseSkill();
        int damageDealt = skill.SkillDamage;
        selectedMonster.TakeDamage(damageDealt);
        player.UseMp(usedMana);
        Thread.Sleep(500);

        Console.Clear();
        ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", $"Battle!!!\n");
        Console.WriteLine($"{player.Name} 의 공격!");
        Thread.Sleep(500);
        ConsoleUtility.PrintTextHighlights(ConsoleColor.Yellow, "", $"{skill.Name}!!!\n");
        Console.WriteLine($"Lv.{selectedMonster.Level} {selectedMonster.Name} 을(를) 맞췄습니다. [데미지 : {damageDealt}]\n");
        Thread.Sleep(500);
        Console.WriteLine($"Lv.{selectedMonster.Level} {selectedMonster.Name}");
        Thread.Sleep(500);
        Console.WriteLine($"HP {selectedMonster.Hp}\n");
        Thread.Sleep(500);

        BattleMonster = false;
        PromptForNextAction();
    }
    public void PlayerAttack()
    {

        TextBattle();
        TextMonsters();
        TextPlayerInfo();
        Console.WriteLine("0.취소\n");

        int input = ConsoleUtility.PromptMenuChoice(0, monsters.Count);
        if (input == 0)
        {
            BattleMonster = false;
            return;
        }
        // 선택한 몬스터 인덱스
        int deadMonsterIdx = input - 1;
        // 공격할 몬스터
        Monster selectedMonster = monsters[deadMonsterIdx];

        // 선택한 몬스터가 이미 죽은 상태인지 확인
        if (selectedMonster.IsDead)
        {
            Console.WriteLine("\n잘못된 입력입니다.");
            Thread.Sleep(500);
            BattleMenu();
            return; // 다시 공격 메뉴로 돌아감
        }

        int baseDamage = player.Atk;
        int errorDamage = (int)Math.Ceiling(baseDamage * 0.1);
        int minDamage = baseDamage - errorDamage;
        int maxDamage = baseDamage + errorDamage;
        int damageDealt = random.Next(minDamage, maxDamage);


        // 회피 확률
        int missChance = 10;
        // 회피확률 생산
        bool isMiss = random.Next(100) < missChance;

        Console.Clear();
        ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "Battle!!\n");
        Console.WriteLine($"{player.Name} 의 공격!");
        Thread.Sleep(500);

        //회피 발생하면 데미지 0으로 적용
        if (isMiss)
        {
            damageDealt = 0;
            Console.WriteLine($"Lv.{selectedMonster.Level} {selectedMonster.Name} 을(를) 공격했지만 아무일도  일어나지 않았습니다.\n");
        }
        else
        {
            // 치명타 여부를 결정하기 위한 확률 30퍼
            int critChance = 30;
            // 랜덤한 확률을 생성하여 치명타 여부 결정
            bool isCritical = random.Next(100) < critChance;
            // 치명타 데미지 계산
            int critDamage = (int)(damageDealt * 1.6); // 치명타 데미지는 일반 데미지의 1.6배로 가정
                                                       // 치명타가 발생하면 추가 데미지 적용

            if (isCritical)
            {
                damageDealt += critDamage;
                Console.WriteLine($"Lv.{selectedMonster.Level} {selectedMonster.Name} 을(를) 맞췄습니다. [데미지 : {damageDealt}]- 치명타 공격!!\n");
            }
            else
            {
                Console.WriteLine($"Lv.{selectedMonster.Level} {selectedMonster.Name} 을(를) 맞췄습니다. [데미지 : {damageDealt}]\n");
            }
            selectedMonster.TakeDamage(damageDealt);
        }
        Thread.Sleep(500);
        Console.WriteLine($"Lv.{selectedMonster.Level} {selectedMonster.Name}");
        Thread.Sleep(500);
        Console.WriteLine($"HP {selectedMonster.Hp}\n");
        Thread.Sleep(500);
        BattleMonster = false;
        PromptForNextAction();
    }

    public void EnemyAttack(Monster targetMonster)
    {
        int playerHp = player.Hp;
        int damageDealt = 0;
        //공격할 몬스터가  살아있는지 확인
        if (!targetMonster.IsDead && playerHp > 0)
        {
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "Battle!!\n");
            Console.WriteLine($"{targetMonster.Name} 의 공격!");
            Thread.Sleep(500);
            if (targetMonster.Atk-player.Def > 0)
                damageDealt = targetMonster.Atk - player.Def;
            else
                damageDealt = 0;

            player.TakeDamage(damageDealt);

            if (damageDealt > 0)
                Console.WriteLine($"Lv.{player.Level} {player.Name} 을/를 맞췄습니다. [데미지 : {damageDealt}]\n");
            else
                ConsoleUtility.PrintTextHighlights(ConsoleColor.Blue, "", $"방어력이 너무 높아서 공격이 통하지 않았다..!! [데미지 : {damageDealt}]\n");
            Thread.Sleep(500);
            Console.WriteLine($"Lv.{player.Level} {player.Name}");
            Thread.Sleep(500);
            Console.WriteLine($"HP {player.Hp}\n");
            Thread.Sleep(500);
        }
        else if (targetMonster.IsDead)
        {
            if(targetMonster.Id == 3 && Quest.questData[1].Count < Quest1.quest1.GoalCount && targetMonster.QuestCount == false )
            {
                Quest.questData[1].Count++;
                targetMonster.QuestCount = true;
            }
        }
        else if (!targetMonster.IsDead && playerHp == 0)
        {
            BattleResult(false);
            return;
        }
    }

    public void BattleResult(bool victory)
    {
        // 레벨업 유무 확인
        int playerLevel = player.Level;
        bool flagLevelUp = LevelUp();

        Console.Clear();
        ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "전투 결과\n");
        if (victory)
        {
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Green, "", "전투 승리 !!!\n");

            if (!bossClear)
                Console.WriteLine("던전에서 몬스터를 잡았습니다.\n");
            else
                Console.WriteLine("던전 보스인 레드 드래곤을 잡았습니다!!\n");

            ConsoleUtility.PrintTextHighlights(ConsoleColor.Yellow, "", "[캐릭터 정보]");
            if (!flagLevelUp)
                Console.WriteLine($"Lv.{player.Level} {player.Name}");
            else
                Console.WriteLine($"Lv.{playerLevel} {player.Name} -> Lv.{player.Level} {player.Name}");

            Thread.Sleep(500);
            Console.WriteLine($"HP {beforeHp} -> {player.Hp} {((beforeHp - player.Hp) > 0 ? $"(-{beforeHp-player.Hp})" : "")}");
            Thread.Sleep(500);
            Console.WriteLine($"MP {beforeMp} -> {player.Mp} {((beforeMp - player.Mp) > 0 ? $"(-{beforeMp - player.Mp})" : "")}");
            if (player.Mp < player.MaxMp)
                player.Mp += 10;
            Console.WriteLine($"exp {beforeExp} -> {player.Exp} {((player.Exp - beforeExp) > 0 ? $"(+{player.Exp - beforeExp})" : "")}");
            Thread.Sleep(500);
            if (player.Level < 5)
            {
                Console.WriteLine($"LevelUp까지 남은 Exp -> {player.LevelUpExp - player.Exp}\n");
                Thread.Sleep(500);
            }
            else
                Console.WriteLine();

            if (flagLevelUp)
            {
                ConsoleUtility.PrintTextHighlights(ConsoleColor.Yellow, "", "[레벨업! 능력치 상승!!]");
                Console.WriteLine($"MaxHp {player.MaxHp-10} -> {player.MaxHp}");
                Console.WriteLine($"MaxMp {player.MaxMp - 10} -> {player.MaxMp}");
                Console.WriteLine($"Atk {player.NonEquipAtk - 1} -> {player.NonEquipAtk}");
                Console.WriteLine($"Def {player.NonEquipDef - 1} -> {player.NonEquipDef}\n");
                Console.WriteLine("레벨업으로 플레이어의 HP와 MP가 최대치까지 회복됩니다.\n");

                player.Hp = player.MaxHp;
                player.Mp = player.MaxMp;
            }

            RewardItem();

            // 보스 클리어시 게임 종료.
            if (bossClear)
            {
                gameManager.GamePlay = false;
                ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "\n보스를 클리어하여 게임이 종료됩니다.");
                ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", "플레이 해주셔서 감사합니다.");

                Console.WriteLine("\n0. 종료\n");
                int endInput = ConsoleUtility.PromptMenuChoice(0, 0);
                if (endInput == 0)
                    return;
            }
            // 몬스터 생성함수
            GetMonster();

            Thread.Sleep(500);
            Console.WriteLine("\n0. 다음");
            int input = ConsoleUtility.PromptMenuChoice(0, 0);
            switch (input)
            {
                case 0:
                    BossStage();
                    break;            
            }
        }
        else
        {
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Red, "", "전투 패배...\n");
            Thread.Sleep(500);
            Console.WriteLine($"Lv.{player.Level} {player.Name}");
            Console.WriteLine($"HP {beforeHp} -> {player.Hp}\n");

            Console.WriteLine("1. 다시 도전!\n");
            Console.WriteLine("0. 게임 종료..\n");
            int input = ConsoleUtility.PromptMenuChoice(0, 1);
            switch (input)
            {
                case 0:
                    gameManager.GamePlay = false;
                    Console.WriteLine("\n게임을 종료합니다.");
                    break;
                case 1:
                    player.Hp = player.MaxHp;
                    player.Mp = player.MaxMp;
                    break;
            }
        }
    }

    public bool LevelUp()
    {
        bool flagLevelUp;

        if (player.Exp >= player.LevelUpExp && (player.Level >= 1 && player.Level <= 4))
        {
            flagLevelUp = true;
            player.NonEquipAtk += 1;
            player.NonEquipDef += 1;
            player.MaxHp += 10;
            player.MaxMp += 10;

            if (player.Exp >= 200 && player.Exp < 350)
            {
                player.Level = 2;
                player.LevelUpExp = 350;
            }
            else if (player.Exp >= 350 && player.Exp < 500)
            {
                player.Level = 3;
                player.LevelUpExp = 500;
            }
            else if (player.Exp >= 500 && player.Exp < 650)
            {
                player.Level = 4;
                player.LevelUpExp = 650;
            }
            else if (player.Exp >= 650)
                player.Level = 5;
        }
        else
            flagLevelUp = false;

        return flagLevelUp;
    }

    public void RewardItem()
    {
        // 포션 보상 여부를 결정하기 위한 확률 50퍼
        int getPotionChance = 50;
        int addGold = random.Next(200, 501);
        // 랜덤한 확률을 생성하여 포션 여부 결정
        bool isGetPotion = random.Next(100) < getPotionChance;

        ConsoleUtility.PrintTextHighlights(ConsoleColor.Yellow, "", "[획득 아이템]");

        Console.WriteLine($"+ {addGold} Gold");
        Thread.Sleep(500);
        player.Gold += addGold;

        if (isGetPotion)
        {
            int addPotion = random.Next(1, 3);
            potion.PotionIndex[0].Count += addPotion;
            Console.WriteLine($"+ {potion.PotionIndex[0].Name} {addPotion}개");
        }
    }
    public void DeadMonster()
    {
        for(int i = 0; i < monsters.Count; i++)
        {
            if (!monsters[i].IsDead)
            {
                if (!BattleMonster)
                    Console.WriteLine($"- Lv.{monsters[i].Level} {monsters[i].Name} HP {(monsters[i].IsDead ? "Dead" : monsters[i].Hp.ToString())}");
                else
                    Console.WriteLine($"{i+1}. Lv.{monsters[i].Level} {monsters[i].Name} HP {(monsters[i].IsDead ? "Dead" : monsters[i].Hp.ToString())}");
            }
            else
            {
                if(!BattleMonster)
                    ConsoleUtility.PrintTextHighlights(ConsoleColor.DarkGray, "", $"- Lv.{monsters[i].Level} {monsters[i].Name} HP Dead");
                else
                    ConsoleUtility.PrintTextHighlights(ConsoleColor.DarkGray, "", $"- Lv.{monsters[i].Level} {monsters[i].Name} HP Dead");
            }

        }
    }
    public void  RunAway() 
    {
        // 몬스터 생성함수
        GetMonster();

        int beforeHp = player.Hp;
        int damage = 5;
        player.TakeDamage(damage);
        Console.Clear();
        Console.WriteLine("겁을 먹고 도망쳤다.\n");
        Thread.Sleep(500);
        Console.WriteLine($"도망치면서 등에 칼이 꽃혔다. HP [{beforeHp}] -> HP [{player.Hp}]\n");
        Thread.Sleep(500);
        Console.WriteLine("0.다음\n");
        Thread.Sleep(500);

        int input = ConsoleUtility.PromptMenuChoice(0, 0);
        switch (input)
        {
            case 0:
                break;
        }
        
    }
    private void GetMonster()
    {
        monsters.Clear();

        // 여기에 새로운 몬스터를 추가하는 코드를 작성합니다.
        // 예를 들어, 플레이어 레벨에 따라 다른 몬스터를 추가할 수 있습니다.
        Monster monster = new Monster();
        monster.Monsters(player.Level); // 플레이어 레벨에 맞게 몬스터 생성
        monster.GenerateMonster(); // 몬스터 생성
        monsters.AddRange(monster.CreatedMonster); // 생성된 몬스터를 리스트에 추가
    }

```

</details>
</details>

### 보스전
- 일정 레벨에 도달하면 도전할수있는 보스 전투 공간
<details><summary></summary>

![A05_TextRPG (2) (1)](https://github.com/amor1523/-A05-SpartaDungeonTextRpg/assets/167047045/c54b1cf8-9619-43a6-aed4-f469818ce5f5)

![A05_TextRPG (3) (1)](https://github.com/amor1523/-A05-SpartaDungeonTextRpg/assets/167047045/b3961af2-6308-47b9-bc76-056eed6572bf)
<details><summary>코드 보기</summary>

```csharp
    public void BossText() 
    {
        if (CheckBossStage() == false)
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Red, "", "보스 스테이지는 5레벨부터 입장 가능합니다! 더 성장해서 도전해보세요..!!\n");
        if (CheckBossStage() == true)
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Red, "", "보스 스테이지 입장이 가능합니다!!! 도전하시겠습니까?\n");
        Thread.Sleep(500);
        Console.WriteLine("0. 마을로 돌아가기\n");

        if(CheckBossStage() == true)
            Console.WriteLine("1. 보스전 입장!\n");
    }
    public void BossStage()
    {
        if (CheckBossStage() == false)
        {
            Console.Clear(); 
            BossText();
            int input = ConsoleUtility.PromptMenuChoice(0, 0);
            switch (input)
            {
                case 0:
                    break;
            }
        }
        else if(CheckBossStage() == true) 
        {
            Console.Clear();
            BossText();
            int input = ConsoleUtility.PromptMenuChoice(0, 1);
            switch (input)
            {
                case 0:
                    break;
                case 1:
                    BossBattle();
                    break;
            }
        }
    }
    public void BossBattle()
    {
        if (bossPhaseTwo && !bossClear && playerAttack)
        {
            int damageTime = 10; // 보스의 플레이어에게 입히는 지속 데미지
            int dot = 3;
            InflictDamageOverTime(damageTime, dot);
        }

        if (!bossPhaseTwo || (bossPhaseTwo && bossDotCount > 0))
        {
            TextBattleBoss();
            Console.Write($"Lv.{boss.Level} ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{boss.Name} ");
            Console.ResetColor();
            Console.WriteLine($"HP : {boss.Hp}");
        }

        // 보스 체력이 250 이하면 2페이즈 실행
        if (boss.Hp <= 250 && !bossPhaseTwo && bossDotCount == 0)
        {
            playerAttack = false;
            bossPhaseTwo = true;
            Console.WriteLine("\n보스가 분노합니다! 두 번째 페이즈로 진입합니다.\n");
            Thread.Sleep(500);

            // 두 번째 페이즈 시작을 알리는 텍스트 출력
            Console.WriteLine("2번째 페이즈로 진입합니다!\n");
            Thread.Sleep(500);

            // 보스가 플레이어에게 지속 데미지를 입히는 스킬을 사용
            Console.WriteLine("보스가 플레이어에게 지속 데미지를 입히는 스킬을 사용합니다!\n");
            int damageTime = 10; // 보스의 플레이어에게 입히는 지속 데미지
            int dot = 3; // 지속 시간 (예: 3턴 동안 지속됨)
            Thread.Sleep(500);

            Console.WriteLine("0. 다음\n");
            int input2 = ConsoleUtility.PromptMenuChoice(0, 0);

            InflictDamageOverTime(damageTime, dot);
            return;
        }

        if (!bossPhaseTwo || (bossPhaseTwo && bossDotCount > 0))
        {
            TextPlayerInfo();

            Console.WriteLine("1. 일반 공격");
            Console.WriteLine("2. 스킬 사용");
            Console.WriteLine("3. 포션 사용\n");
            int input = ConsoleUtility.PromptMenuChoice(1, 3);
            switch (input)
            {
                case 1:
                    playerAttack = true;
                    Attack();
                    break;
                case 2:
                    playerAttack = false;
                    PlayerSkillAttack();
                    break;
                case 3:
                    playerAttack = false;
                    UsePotion();
                    break;
            }
        }
    }
    public void InflictDamageOverTime(int damage, int dot)
    {
        if (bossDotCount < dot)
        {
            TextBattleBoss();

            Console.WriteLine($"플레이어는 {dot}턴 동안 {damage}의 지속 데미지를 입습니다!\n");
            Thread.Sleep(500);

            // 플레이어에게 지속 데미지 입힘
            player.TakeDamage(damage);
            // 플레이어의 현재 체력 출력
            Console.WriteLine($"플레이어의 현재 체력: {player.Hp}\n");
            bossDotCount += 1;
            Thread.Sleep(500);
            if(bossDotCount < dot)
                Console.WriteLine($"플레이어는 {dot - bossDotCount}턴 뒤 보스의 스킬이 해제됩니다.\n");
            else
            {
                Console.WriteLine("보스의 스킬이 해제됐습니다.\n");
                bossPhaseTwo = false;
            }
            Thread.Sleep(500);
        }

        if (player.IsDead)
        {
            Console.WriteLine($"플레이어가 쓰러졌습니다!");
            Thread.Sleep(500);
            BattleResult(false);
            return;
        }
        else
        {
            Console.WriteLine($"0.다음\n");
            int input = ConsoleUtility.PromptMenuChoice(0, 0);
            if (input == 0 && bossDotCount == 1)
            {
                BossBattle();
                return;
            }
        }
    }
    public bool CheckBossStage()
    {
        return player.Level >= 5; // 플레이어 레벨이 5 이상이면 보스 스테이지로 진입
    }
    public void Attack()
    {
        TextBattleBoss();
        Console.Write($"Lv.{boss.Level} ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"{boss.Name} ");
        Console.ResetColor();
        Console.WriteLine($"HP : {boss.Hp}\n");

        // 플레이어의 공격력 계산
        int baseDamage = player.Atk;
        int errorDamage = (int)Math.Ceiling(baseDamage * 0.1);
        int minDamage = baseDamage - errorDamage;
        int maxDamage = baseDamage + errorDamage;
        int damageDealt = random.Next(minDamage, maxDamage);

        // 회피 확률
        int missChance = 10;
        bool isMiss = random.Next(100) < missChance;

        if (isMiss)
        {
            Console.WriteLine($"{player.Name}의 공격!");
            Thread.Sleep(500);
            damageDealt = 0;
            Console.WriteLine($"보스를 공격했지만 아무일도 일어나지 않았습니다.\n");
        }
        else
        {
            // 치명타 확률
            int critChance = 30;
            bool isCritical = random.Next(100) < critChance;

            // 치명타 공격 시 데미지 계산
            int critDamage = (int)(damageDealt * 1.6);
            if (isCritical)
            {
                damageDealt += critDamage;
                Console.WriteLine($"{boss.Name} 을(를) 맞췄습니다. [데미지 : {damageDealt}] - 치명타 공격!!\n");
            }
            else
            {
                Console.WriteLine($"{boss.Name} 을(를) 맞췄습니다. [데미지 : {damageDealt}]\n");
            }
        }

        // 보스에게 데미지 적용
        boss.TakeDamage(damageDealt);

        Thread.Sleep(500);
        Console.WriteLine($"{boss.Name}");
        Thread.Sleep(500);
        Console.WriteLine($"HP {boss.Hp}\n");

        if (boss.IsDead)
        {
            Thread.Sleep(500);
            Console.WriteLine($"보스를 처치하였습니다!");
            Thread.Sleep(500);
            bossClear = true;
            BattleResult(true);
            return;
        }
        else
        {
            // 보스의 공격
            Thread.Sleep(500);

            Console.WriteLine("0. 다음\n");
            int input = ConsoleUtility.PromptMenuChoice(0, 0);
            BossAttack();
            return;
        }
    }

    private int ComputerInput(int baseNumber, int exponent)
    {
        int result = 1;

        for (int number = 1; number <= exponent; number++)
        {
            result *= baseNumber;
        }
        return result;
    }

    public bool BossUltimate()
    {
        ConsoleUtility.PrintTextHighlights(ConsoleColor.Red, "", $"{boss.Name}의 궁극기 시전중!! [데미지 : 50]\n");
        Thread.Sleep(500);
        ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", $"4초 안에 제시된 숫자를 모두 입력해주세요!");
        Thread.Sleep(500);
        ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", $"(콤마, 띄어쓰기 생략 후 입력)\n");
        Thread.Sleep(500);
        ConsoleUtility.PrintTextHighlights(ConsoleColor.Cyan, "", $"(입력은 한 번만 가능합니다. 신중하게 입력해주세요!)\n");
        Thread.Sleep(500);
        Console.Write("--- ");
        for (int number = 0; number < bossUltimateArray.Length; number++)
        {
            if (number == bossUltimateArray.Length - 1)
                Console.Write($"{bossUltimateArray[number]}");
            else
                Console.Write($"{bossUltimateArray[number]}, ");
        }
        Console.WriteLine(" ---\n");
        Console.Write(">> ");

        timer = new System.Timers.Timer();
        timer.Interval = 500;
        timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
        timerCount = 0;
        timer.Start();

        bossUltimatePlayerInput = Console.ReadLine().ToString();

        if (bossUltimatePlayerInput == bossUltimateComputerInput)
        {
            Thread.Sleep(1000);
            return true;
        }
        else
        {
            Thread.Sleep(100);
            return false;
        }
    }

    void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        timerCount++;
        if (bossUltimatePlayerInput == bossUltimateComputerInput)
        {
            timer.Stop();
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Green, "", "\n궁극기 파훼 성공!!!\n");
            Thread.Sleep(500);
        }
        if (timerCount == 7 && bossUltimatePlayerInput != bossUltimateComputerInput)
        {
            timer.Stop();
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Red, "", "\n\n궁극기 파훼 실패...");
            Thread.Sleep(500);
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Red, "", "\n(입력을 마치지 못하셨다면 엔터키를 눌러주세요.)");
        }
            
    }

    public void BossAttack()
    {
        int damageDealt = 0;
        TextBattleBoss();

        // 궁극기 확률
        int ultimateChance = 50;
        int ultimateDamage = 50;
        bool isUltimate = false;

        if (boss.Hp < 250)
        {
            isUltimate = random.Next(100) < ultimateChance;
            if (isUltimate)
            {
                int computerInput = 0;
                for (int number =0; number < bossUltimateArray.Length; number++)
                {
                    bossUltimateArray[number] = random.Next(1,9);
                }
                for (int number = 0; number < bossUltimateArray.Length; number++)
                {
                    computerInput += (bossUltimateArray[number] * ComputerInput(10, bossUltimateArray.Length-number-1));
                    bossUltimateComputerInput = computerInput.ToString();
                }
                // 궁극기 패턴
                if (BossUltimate())
                {
                    ConsoleUtility.PrintTextHighlights(ConsoleColor.Blue, "", "궁극기를 파훼하여 아무런 일도 생기지 않았습니다.\n");
                }
                else
                {
                    damageDealt = ultimateDamage;
                    player.TakeDamage(damageDealt);

                    ConsoleUtility.PrintTextHighlights(ConsoleColor.Red, "", $"{boss.Name}의 궁극기!!");
                    Console.WriteLine($"{player.Name}을/를 맞췄습니다. [데미지 : {damageDealt}]\n");
                }
            }
        }

        if (!isUltimate)
        {
            
            int minDamage = boss.Atk - (int)(boss.Atk * 0.1f);
            int maxDamage = boss.Atk + (int)(boss.Atk * 0.1f);
            damageDealt = random.Next(minDamage, maxDamage) - player.Def;

            if (damageDealt < 0)
                damageDealt = 0;

            player.TakeDamage(damageDealt);
            ConsoleUtility.PrintTextHighlights(ConsoleColor.Red, "", $"{boss.Name}의 공격!!");
            Thread.Sleep(500);
            if (damageDealt > 0)
            {
                Console.WriteLine($"{player.Name}을/를 맞췄습니다. [데미지 : {damageDealt}]\n");
            }
            else
                ConsoleUtility.PrintTextHighlights(ConsoleColor.Blue, "", $"방어력이 너무 높아서 공격이 통하지 않았다..!! [데미지 : {damageDealt}]\n");
        }

        Thread.Sleep(500);
        Console.WriteLine($"{player.Name}");
        Thread.Sleep(500);
        Console.WriteLine($"HP {player.Hp}\n");

        if (player.IsDead)
        {
            Console.WriteLine($"플레이어가 쓰러졌습니다!");
            Thread.Sleep(500);
            BattleResult(false);
            return;
        }
        else
        {
            Console.WriteLine($"0.다음\n");
            int input = ConsoleUtility.PromptMenuChoice(0, 0);
            if (input == 0)
            {
                Console.Clear();
                BossBattle();
                return;
            }
        }
    }
    public void PlayerSkillAttack()
    {
        TextBattleBoss();
        Console.Write($"Lv.{boss.Level} ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"{boss.Name} ");
        Console.ResetColor();
        Console.WriteLine($"HP : {boss.Hp}\n");
        TextPlayerInfo();
        TextSkillMenu();
        Console.WriteLine("0.취소\n");


        int input = ConsoleUtility.PromptMenuChoice(0, 1); // 보스 선택 옵션 제거

        if (input == 0)
        {
            Console.Clear();
            BossBattle();
            return;
        }
        // 스킬 사용 대상으로 선택한 보스가 아닌 보스 리스트 중 첫 번째 보스를 대상으로 스킬 사용
        Monster selectedBoss = boss;
        int usedMana = skill.MpCost;
        if (player.Mp < usedMana)
        {
            Console.WriteLine("마나가 부족하여 스킬을 사용할수없습니다.");
            Thread.Sleep(500);
            PlayerSkillAttack();
            return;
        }

        skill.UseSkill();
        int damageDealt = skill.SkillDamage;
        selectedBoss.TakeDamage(damageDealt);
        player.UseMp(usedMana);
        Thread.Sleep(500);


        TextBattleBoss();
        Console.WriteLine($"{player.Name} 의 공격!");
        Thread.Sleep(500);
        ConsoleUtility.PrintTextHighlights(ConsoleColor.Yellow, "", $"{skill.Name}!!!\n");
        Console.WriteLine($"{selectedBoss.Name} 을(를) 맞췄습니다. [데미지 : {damageDealt}]\n");
        Thread.Sleep(500);
        Console.WriteLine($"{selectedBoss.Name}");
        Thread.Sleep(500);
        Console.WriteLine($"HP {selectedBoss.Hp}\n");
        Thread.Sleep(500);

        Console.WriteLine("0. 다음\n");
        int input2 = ConsoleUtility.PromptMenuChoice(0, 0);

        BossAttack();
        return;
    }


    public void UsePotion()
    {
        Console.Clear();
        Console.WriteLine("[포션 목록]");
        if (potion.PotionIndex[0].Count == 0)
            Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("- ");

        Console.Write(ConsoleUtility.PadRightForMixedText(potion.PotionIndex[0].Name, 20));

        Console.Write(" | ");

        Console.Write(ConsoleUtility.PadRightForMixedText("보유 개수: " + potion.PotionIndex[0].Count.ToString() + " 개", 15));

        Console.Write(" | ");

        Console.WriteLine(ConsoleUtility.PadRightForMixedText(potion.PotionIndex[0].Explain, 55));

        Console.ResetColor();

        Console.WriteLine("\n1. 사용");
        Console.WriteLine("0. 나가기\n");

        int input = ConsoleUtility.PromptMenuChoice(0, 1);
        switch (input)
        {
            case 0:
                if (player.Level == 5)
                    BossBattle();
                else
                    BattleMenu();
                break;
            case 1:
                if (potion.PotionIndex[0].Count > 0)
                {
                    potion.UsePotion(player, 0);
                    if (potion.PotionIndex[0].FlagUse)
                    {
                        potion.PotionIndex[0].Count -= 1;
                        potion.PotionIndex[0].FlagUse = false;
                    }
                }
                else
                {
                    Console.WriteLine("\n선택하신 포션의 수량이 부족합니다.");
                }

                Thread.Sleep(500);
                UsePotion();
                break;
        }
    }
}

```

</details>
</details>
