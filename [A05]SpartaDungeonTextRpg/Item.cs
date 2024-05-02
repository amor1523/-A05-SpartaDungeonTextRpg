using _A05_SpartaDungeonTextRpg;
using SpartaDungeonTextRpg;
using System;
using System.Reflection.Emit;
public class Item
{
    private Player player = new Player();
    public List<Item> ItemIndex = new List<Item>();
    public List<Item> InventoryIndex = new List<Item>();
    public bool FlagShopBuy = false;
    
    public string Name { get; }
    public int Id { get; }
    public int Gold { get; }
    public string Explain { get; }
    public int DefensivePower { get; }
    public int AttackPower { get; }
    public bool FlagBuy { get; set; }
    public bool FlagEquip { get; set; }

    // 방어구 : 1번 부터 시작, 공격 무기 : 1001번 부터 시작
    public Item(string name, int id, int gold, int defensivepower, int attackpower, string explain)
    {
        Name = name;
        Id = id;
        Gold = gold;
        DefensivePower = defensivepower;
        AttackPower = attackpower;
        FlagBuy = false;
        FlagEquip = false;
        Explain = explain;
    }

    public Item()
    {

    }

    // 역직렬화 후 Item에게 ItemData를 넘겨주기 위한 메서드
    //public Item(ItemData itemData)
    //{
    //    FlagShopBuy = itemData.FlagShopBuy;
    //    FlagBuy = itemData.FlagBuy;
    //    FlagEquip = itemData.FlagEquip;
    //}



    private void BuyItem(Player player)
    {
        if (!FlagBuy)
        {
            if (player.Gold - Gold >= 0)
            {
                player.Gold -= Gold;
                FlagBuy = true;
                Console.WriteLine("구매를 완료했습니다.");
            }
            else
                Console.WriteLine("Gold가 부족합니다.");
        }
    }

    public void GetItem()
    {
        ItemIndex.Add(new Item("수련자 갑옷", 1, 1000, 5, 0, "수련에 도움을 주는 옷입니다."));
        ItemIndex.Add(new Item("무쇠 갑옷", 2, 1500, 9, 0, "무쇠로 만들어져 튼튼한 갑옷입니다."));
        ItemIndex.Add(new Item("스파르타의 갑옷", 3, 2000, 15, 0, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다."));
        ItemIndex.Add(new Item("낡은 검", 1001, 500, 0, 2, "쉽게 볼 수 있는 낡은 검 입니다."));
        ItemIndex.Add(new Item("청동 도끼", 1002, 1000, 0, 5, "어디선가 사용됐던거 같은 도끼입니다."));
        ItemIndex.Add(new Item("스파르타의 창", 1003, 2000, 0, 7, "스파르타의 전사들이 사용했다는 전설의 창입니다."));
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
                if (!item.FlagEquip)
                {
                    if (item.Id < 1000 && item.Id >= 1)
                        Console.WriteLine($"- {index} {item.Name}        || 방어력 + {item.DefensivePower}        || 장비설명 : {item.Explain}");
                    else if (item.Id > 1000 && item.Id < 2000)
                        Console.WriteLine($"- {index} {item.Name}        || 공격력 + {item.AttackPower}        || 장비설명 : {item.Explain}");
                    else
                        Console.WriteLine(" 아이템 ID 오류");
                }
                else
                {
                    if (item.Id < 1000 && item.Id >= 1)
                        ConsoleUtility.PrintTextHighlights(ConsoleColor.Blue, "", $"- {index} [E] {item.Name}        || 방어력 + {item.DefensivePower}        || 장비설명 : {item.Explain}");
                    else if (item.Id > 1000 && item.Id < 2000)
                        ConsoleUtility.PrintTextHighlights(ConsoleColor.Blue, "", $"- {index} [E] {item.Name}        || 공격력 + {item.AttackPower}        || 장비설명 : {item.Explain}");
                    else
                        Console.WriteLine(" 아이템 ID 오류");
                }
                index++;
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
            }
            else
            {
                Console.WriteLine("나올 수 있나?1");
            }
        }
    }

    public void Shop()
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
                if (!FlagShopBuy)
                {
                    if (!item.FlagBuy)
                    {
                        if (item.Id < 1000 && item.Id >= 1)
                            Console.WriteLine($"- {item.Name}        || 방어력 + {item.DefensivePower}        || 장비설명 : {item.Explain}        || {item.Gold} G");
                        else if (item.Id > 1000 && item.Id < 2000)
                            Console.WriteLine($"- {item.Name}        || 공격력 + {item.AttackPower}        || 장비설명 : {item.Explain}        || {item.Gold} G");
                        else
                            Console.WriteLine(" 아이템 ID 오류 ");
                    }
                    else
                    {
                        if (item.Id < 1000 && item.Id >= 1)
                            ConsoleUtility.PrintTextHighlights(ConsoleColor.DarkGray, "", $"- {item.Name}        || 방어력 + {item.DefensivePower}        || 장비설명 : {item.Explain}        || 구매완료");
                        else if (item.Id > 1000 && item.Id < 2000)
                            ConsoleUtility.PrintTextHighlights(ConsoleColor.DarkGray, "", $"- {item.Name}        || 공격력 + {item.AttackPower}        || 장비설명 : {item.Explain}        || 구매완료");
                    }
                }
                else
                {
                    if (!item.FlagBuy)
                    {
                        if (item.Id < 1000 && item.Id >= 1)
                            Console.WriteLine($"- {index} {item.Name}        || 방어력 + {item.DefensivePower}        || 장비설명 : {item.Explain}        || {item.Gold} G");
                        else if (item.Id > 1000 && item.Id < 2000)
                            Console.WriteLine($"- {index} {item.Name}        || 공격력 + {item.AttackPower}        || 장비설명 : {item.Explain}        || {item.Gold} G");
                    }
                    else
                    {
                        if (item.Id < 1000 && item.Id >= 1)
                            ConsoleUtility.PrintTextHighlights(ConsoleColor.DarkGray, "", $"- {index} {item.Name}        || 방어력 + {item.DefensivePower}        || 장비설명 : {item.Explain}        || 구매완료");
                        else if (item.Id > 1000 && item.Id < 2000)
                            ConsoleUtility.PrintTextHighlights(ConsoleColor.DarkGray, "", $"- {index} {item.Name}        || 공격력 + {item.AttackPower}        || 장비설명 : {item.Explain}        || 구매완료");
                    }
                    index++;
                }
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
            input = ConsoleUtility.PromptMenuChoice(0, ItemIndex.Count);

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
                Item selectItem = ItemIndex[input-1];

                if (selectItem != null)
                {
                    if (!selectItem.FlagBuy)
                    {
                        selectItem.BuyItem(player);
                        InventoryIndex.Add(selectItem);
                        Thread.Sleep(1000);
                        Shop();
                    }
                    else
                    {
                        Console.WriteLine("이미 구매한 아이템 입니다.");
                        Thread.Sleep(1000);
                        Shop();
                    }
                }
                else
                    Console.WriteLine("나올 수 있나?2");
            }
        }
    }
}