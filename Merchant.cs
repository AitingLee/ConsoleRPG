using System;
using System.Collections.Generic;
using System.Text;

namespace HW6
{
    public class Merchant : Npc
    {
        string[] namePool = new string[] { "佛格斯", "愛麗沙", "蕾拉", "安黛莉", "西蒙", "瓦爾特", "科拿克", "皮林", "布萊恩", "巴塔那", "黛西", "諾瑪", "普娜西雅","洛伊德", "可可"};

        public override void Talk(ref Hero hero)
        {
            Console.WriteLine("我這裡有一些好貨，走過路過不要錯過~");
            bool finish = false;
            Dictionary <string, int> price = new Dictionary <string, int>
            {{"果凍",10 }, { "菠菜", 20 }, { "炸彈", 50 },{ "閃光彈", 60 },{ "木劍", 30 },{ "火焰弓", 150 } };

            while (!finish)
            {
                Console.WriteLine($"請選擇動作\n1:讓我看看你都賣些什麼吧\n2:我這裡有一些東西要賣\n按其他任意鍵:結束對話");
                switch (Console.ReadLine())
                {
                    case "1":
                        bool endSell = false;
                        hero.ShowBag();
                        Console.WriteLine($"請選擇要購買的物品\n");
                        while (!endSell)
                        {
                            int noSell = 1;
                            foreach (var item in price)
                            {
                                Console.Write($"{noSell} : {item.Key} - {item.Value} 元  ");
                                noSell += 1;
                            }
                            Console.Write("X : 結束購買");
                            switch (Console.ReadLine().ToUpper())
                            {
                                case "1":           //買果凍
                                    hero.Purchase(GameManager.Gm.jelly);
                                    break;
                                case "2":           //買菠菜
                                    hero.Purchase(GameManager.Gm.spinach);
                                    break;
                                case "3":           //買炸彈
                                    hero.Purchase(GameManager.Gm.bomb);
                                    break;
                                case "4":           //買閃光彈
                                    hero.Purchase(GameManager.Gm.flashBomb);
                                    break;
                                case "5":           //買木劍
                                    hero.Purchase(GameManager.Gm.woodSword);
                                    break;
                                case "6":           //買火焰弓
                                    hero.Purchase(GameManager.Gm.fireBow);
                                    break;
                                case "X":
                                    endSell = true;
                                    break;
                                default:
                                    Console.WriteLine("輸入錯誤，請重新輸入");
                                    break;
                            }
                        }
                        if (endSell)
                        {
                            Console.WriteLine("已結束購買");
                            hero.ShowBag();
                        }
                        break;

                    case "2":
                        bool endBuy = false;
                        hero.ShowBag();
                        Console.WriteLine($"請選擇要販售的物品\n");
                        while (!endBuy)
                        {
                            int noBuy = 1;
                            foreach (var item in price)
                            {
                                Console.Write($"{noBuy} : {item.Key} - {item.Value / 2} 元  ");
                                noBuy += 1;
                            }
                            Console.Write("X : 結束販售");
                            switch (Console.ReadLine().ToUpper())
                            {
                                case "1":           //賣果凍
                                    hero.Sell(GameManager.Gm.jelly);
                                    break;
                                case "2":           //賣菠菜
                                    hero.Sell(GameManager.Gm.spinach);
                                    break;
                                case "3":           //賣炸彈
                                    hero.Sell(GameManager.Gm.bomb);
                                    break;
                                case "4":           //賣閃光彈
                                    hero.Sell(GameManager.Gm.flashBomb);
                                    break;
                                case "5":           //賣木劍
                                    hero.Sell(GameManager.Gm.woodSword);
                                    break;
                                case "6":           //賣火焰弓
                                    hero.Sell(GameManager.Gm.fireBow);
                                    break;
                                case "X":
                                    endBuy = true;
                                    break;
                                default:
                                    Console.WriteLine("輸入錯誤，請重新輸入");
                                    break;
                            }
                            break;
                        }
                        break;
                    default:
                        finish = true;
                        break;
                }
            }
            if (finish)
            {
                Console.WriteLine("謝謝惠顧～今天打烊了，先回家啦！");
                hero.map[location.x, location.y] = "";      //NPC輸了就消失在地圖上
            }
        }
        public List<Merchant> CreateMerchants (ref Hero hero, int no)
        {
            List<Merchant> merchants = new List<Merchant>();
            Random rand = new Random();
            for (int i = 0; i < no; i++)    //設定商人名稱及位置
            {
                merchants.Add(new Merchant()  { name = $"商人 - {namePool[i]}" });
                bool set = false;
                while (!set)
                {
                    if (i == 1)         //第1隻商人一定會出生在地圖(4,4)~(6,6)的村莊地區
                    {
                        int x = rand.Next(4, 7);
                        int y = rand.Next(4, 7);
                        if (String.IsNullOrEmpty(hero.map[x, y]))
                        {
                            merchants[i].location = (x, y);
                            hero.map[x, y] = merchants[i].name;
                            set = true;
                        }
                    }
                    else                //第2~6隻商人隨便出生在全地圖
                    {
                        int x = rand.Next(0, 11);
                        int y = rand.Next(0, 11);
                        if (String.IsNullOrEmpty(hero.map[x, y]))
                        {
                            merchants[i].location = (x, y);
                            hero.map[x, y] = merchants[i].name;
                            set = true;
                        }
                    }
                }
            }
            return merchants;
        }
    }
}
