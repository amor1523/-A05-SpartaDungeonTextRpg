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

	
# A05 오리고지리조 스파르타 던전

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
-Visual Studio 2022 , C#

## 게임 화면
### 로비
- 캐릭터의 닉네임을 입력하고 직업을 선택한후 시작화면으로 이동
<details><summary></summary>

![인트로 메인화면2](https://github.com/amor1523/-A05-SpartaDungeonTextRpg/assets/167047045/beaebcff-fa1b-4483-bff6-4e294a4a8b70)

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
- 현재 캐릭터의 정보 및 능력치를 확인할수있는 공간
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

### 상점
- 던전에서 얻은 GOLD로 아이템을 살수있는 공간
<details><summary></summary>

![A05_TextRPG (4) (1)](https://github.com/amor1523/-A05-SpartaDungeonTextRpg/assets/167047045/d247094e-3574-4a65-b3c9-d8e92c2a2ad3)

</details>

### 퀘스트
- 퀘스트를 받거나 퀘스트를 완료해서 보상을 얻는 공간
<details><summary></summary>

![A05_TextRPG (2) (1)](https://github.com/amor1523/-A05-SpartaDungeonTextRpg/assets/167047045/baf088f1-a4cf-469b-88e8-d3d3f140e8ff)

</details>

### 던전
- 전투를 시작해서 몬스터들과 싸울수 있는 공간
<details><summary></summary>

![A05_TextRPG (3) (1)](https://github.com/amor1523/-A05-SpartaDungeonTextRpg/assets/167047045/d03f8f76-7af4-4372-8f11-cb0a980175fb)

![A05_TextRPG (1) (1)](https://github.com/amor1523/-A05-SpartaDungeonTextRpg/assets/167047045/196a735d-9002-40f9-b571-f77472efd968)

</details>

### 보스전
- 일정 레벨에 도달하면 도전할수있는 보스 전투 공간
<details><summary></summary>

![A05_TextRPG (2) (1)](https://github.com/amor1523/-A05-SpartaDungeonTextRpg/assets/167047045/c54b1cf8-9619-43a6-aed4-f469818ce5f5)

![A05_TextRPG (3) (1)](https://github.com/amor1523/-A05-SpartaDungeonTextRpg/assets/167047045/b3961af2-6308-47b9-bc76-056eed6572bf)

</details>
