using System;
using _A05_SpartaDungeonTextRpg;
using SpartaDungeonTextRpg;
using System;
using System.Reflection.Emit;
public class Item
{
    private bool FlagShopBuy = false;
    private Player player;
    private Potion potion;
    public List<Item> ItemIndex = new List<Item>();
    public List<Item> InventoryIndex = new List<Item>();
    private bool FlagShopBuy = false;
    
    public string Name { get; }
    public int Id { get; }
    public int Gold { get; }
    public string Explain { get; }
    public int DefensivePower { get; }
    public int AttackPower { get; }
    public bool FlagBuy { get; set; }
    public bool FlagEquip { get; set; }

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

    public Item(Player player, Potion potion)
    {
        this.player = player;
        this.potion = potion;
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
        // 방어구 : 1번 부터 시작, 공격 무기 : 1001번 부터 시작, 물약 : 2001번 부터 시작
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
                    Console.Write($"공격력 {(item.AttackPower >= 0 ? "+" : "")}{item.AttackPower} ");
                if (item.DefensivePower != 0)
                    Console.Write($"방어력 {(item.DefensivePower >= 0 ? "+" : "")}{item.DefensivePower} ");

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
                if(item.FlagBuy)
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("- ");
                if (FlagShopBuy)
                {
                    Console.Write($"{index} ");
                }
                Console.Write(ConsoleUtility.PadRightForMixedText(item.Name, 20));

                Console.Write(" | ");

                if (item.AttackPower != 0)
                    Console.Write($"공격력 {(item.AttackPower >= 0 ? "+" : "")} {ConsoleUtility.PadRightForMixedText(item.AttackPower.ToString(), 3)} ");
                if (item.DefensivePower != 0)
                    Console.Write($"방어력 {(item.DefensivePower >= 0 ? "+" : "")} {ConsoleUtility.PadRightForMixedText(item.DefensivePower.ToString(), 3)} ");

                Console.Write(" | ");

                Console.Write(ConsoleUtility.PadRightForMixedText(item.Explain, 55));

                Console.Write(" | ");

                if (!item.FlagBuy)
                    Console.WriteLine(ConsoleUtility.PadRightForMixedText(item.Gold.ToString() + " G", 5));
                else
                    Console.WriteLine(ConsoleUtility.PadRightForMixedText("구매완료", 5));

                index++;
                Console.ResetColor();
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
                Console.Write(ConsoleUtility.PadRightForMixedText(item.Name, 36));

                Console.Write(" | ");

                Console.Write(ConsoleUtility.PadRightForMixedText(item.Explain, 55));

                Console.Write(" | ");

                if (!item.FlagBuy)
                    Console.WriteLine(ConsoleUtility.PadRightForMixedText(item.Gold.ToString() + " G", 5));
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
                            selectItem.BuyItem(player);
                            if (selectItem.FlagBuy)
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
                                selectPotion.Count += 1 ;
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
                }
            }
        }
    }
}