using System;
using _A05_SpartaDungeonTextRpg;
using SpartaDungeonTextRpg;

public class Potion
{
    private bool FlagInventoryUse = false;
    public string Name { get; set; }
    public int Id { get; set; }
    public string Explain { get; set; }
    public int Gold { get; set; }
    public int Count { get; set; }
    public bool FlagUse { get; set; }
    public bool FlagBuy { get; set; }

    private Player player;
    public List<Potion> PotionIndex = new List<Potion>();

    public Potion()
    {

    }

    public Potion(string name, int id, string explain, int gold, int count)
    {
        Name = name;
        Id = id;
        Explain = explain;
        Count = count;
        Gold = gold;

        FlagUse = false;
        FlagBuy = false;
    }

    public Potion(Player player)
    {
        this.player = player;
    }

    // 역직렬화 후 PotionData를 넘겨주기 위한 메서드 (Load시 사용)
    public void SetPotion(PotionData potionData)
    {
        PotionIndex = potionData.PotionIndex;
    }

    public void BuyItem(Player player)
    {
        if (!FlagBuy)
        {
            if (player.Gold - Gold >= 0)
            {
                player.Gold -= Gold;
                FlagBuy = true;
                Console.WriteLine("\n구매를 완료했습니다.");
            }
            else
                Console.WriteLine("\nGold가 부족합니다.");
        }
    }

    public void GetPotion()
    {
        // 물약(Id) : 2001번 부터 시작
        PotionIndex.Add(new Potion("체력 회복 물약", 2001, "체력이 30 회복 됩니다.", 500, 0));
        PotionIndex.Add(new Potion("공격력 증가 물약", 2002, "공격력이 10 증가합니다.", 10000, 0));
        PotionIndex.Add(new Potion("방어력 증가 물약", 2003, "방어력이 10 증가합니다.", 10000, 0));
    }

    public void UsePotion(Player player, int index)
    {
        this.player = player;

        if (PotionIndex[index].Id == 2001)
        {
            if (player.MaxHp == player.Hp)
            {
                Console.WriteLine("\n회복할 체력이 없습니다.");
            }
            else
            {
                PotionIndex[index].FlagUse = true;
                player.Hp += 30;

                if (player.Hp > player.MaxHp)
                {
                    int overHp = player.Hp - player.MaxHp;
                    player.Hp = player.MaxHp;
                    Console.WriteLine($"\n캐릭터의 MaxHp를 {overHp} 만큼 초과하여 최대 체력까지만 회복.");
                }
                else
                    Console.WriteLine($"\n회복을 완료했습니다.\n현재 캐릭터의 Hp : {player.Hp} 입니다.");
            }
        }
        else if (PotionIndex[index].Id == 2002)
        {
            PotionIndex[index].FlagUse = true;
            player.NonEquipAtk += 10;
            Console.WriteLine($"\n공격력이 증가했습니다.\n현재 캐릭터의 Atk: {player.NonEquipAtk} 입니다.");
        }
        else if (PotionIndex[index].Id == 2003)
        {
            PotionIndex[index].FlagUse = true;
            player.NonEquipDef += 10;
            Console.WriteLine($"\n방어력 증가했습니다.\n현재 캐릭터의 Def: {player.NonEquipDef} 입니다.");
        }
    }

    public void PotionInventory()
    {
        player.Atk = player.NonEquipAtk;
        player.Def = player.NonEquipDef;

        Console.Clear();
        Console.WriteLine("인벤토리- 포션");
        Console.WriteLine("보유 중인 포션을 사용할 수 있습니다.\n");
        Console.WriteLine("[포션 목록]\n");
        int count = 0;

        if (PotionIndex.Count != 0)
        {
            int index = 1;
            foreach (var potion in PotionIndex)
            {
                if (potion.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    count += 1;
                }
                Console.Write("- ");
                if (FlagInventoryUse)
                {
                    Console.Write($"{index} ");
                }
                Console.Write(ConsoleUtility.PadRightForMixedText(potion.Name, 20));

                Console.Write(" | ");

                Console.Write(ConsoleUtility.PadRightForMixedText(potion.Explain, 40));

                Console.Write(" | (남은 포션 :");

                Console.WriteLine(ConsoleUtility.PadRightForMixedText(potion.Count.ToString() + " 개 )", 5));

                index++;
                Console.ResetColor();
            }
        }
        else
            Console.WriteLine("보유하고 있는 포션이 없습니다.");

        int input;
        if (!FlagInventoryUse)
            Console.WriteLine("1. 사용");
        Console.WriteLine("0. 나가기\n");

        if (!FlagInventoryUse)
            input = ConsoleUtility.PromptMenuChoice(0, 1);
        else
            input = ConsoleUtility.PromptMenuChoice(0, PotionIndex.Count);

        if (!FlagInventoryUse)
        {
            switch (input)
            {
                case 0:
                    return;
                case 1:
                    if (count == 3)
                    {
                        Console.WriteLine("\n사용 가능한 포션이 없습니다.");
                        Thread.Sleep(500);
                    }
                    else
                    {
                        FlagInventoryUse = true;
                    }
                    PotionInventory();
                    break;
            }
        }
        else
        {
            if (input == 0)
            {
                FlagInventoryUse = false;
                return;
            }
            else
            {
                Potion selectPotion = PotionIndex[input - 1];

                if (selectPotion != null)
                {
                    if (selectPotion.Count > 0)
                    {
                        UsePotion(player, input - 1);
                        if (selectPotion.FlagUse)
                        {
                            selectPotion.Count -= 1;
                            selectPotion.FlagUse = false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("\n선택하신 포션의 수량이 부족합니다.");

                    }
                    Thread.Sleep(500);
                    PotionInventory();
                }
            }
        }
    }
}
