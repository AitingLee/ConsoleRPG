using System;
using System.Collections.Generic;
using System.Text;

namespace HW6
{
    class Villager : Npc
    {
        string[] namePool = new string[] { "佛格斯", "愛麗沙", "蕾拉", "安黛莉", "西蒙", "瓦爾特", "科拿克", "皮林", "布萊恩", "巴塔那", "黛西", "諾瑪", "普娜西雅", "洛伊德", "可可" };
        
        public override void Talk(ref Hero hero)
        {
            Random rand = new Random();
            int talkWay = rand.Next(0, 5);      //村民有五種對話可能
            switch (talkWay)
            {
                case 0:
                    Console.WriteLine($"{name} : {hero.name}，還不去打怪練功在這鬼混幹嘛");
                    break;
                case 1:
                    Console.WriteLine($"{name} : 我擋住你了？玩遊戲贏我，就讓你過");
                    break;
                case 2:     //玩撲克牌猜大小
                    Console.WriteLine($"{name} : 來玩一場撲克牌猜大小？\n同意按 Y ，按其他鍵結束對話");
                    if(Console.ReadLine().ToUpper() == "Y")
                    {
                        Card answer = new Card((Suits)rand.Next(0, 4), (Values)rand.Next(1, 14));
                        Console.WriteLine($"{name} : 只要你在 7 回合內猜出撲克牌，我就給你20金幣");
                        bool win = false;
                        for (int i = 1; i < 8; i++)
                        {
                            Console.WriteLine($"第 {i} 回合");
                            bool selectSuit = false;
                            int suitNo = 0;
                            while (!selectSuit)
                            {
                                Console.WriteLine("輸入要猜的花色：梅花 按 1, 方塊 按 2, 紅心 按 3, 黑桃 按 4");
                                try
                                {
                                    suitNo = Convert.ToInt32(Console.ReadLine());
                                    if (suitNo < 1 | suitNo >4)
                                    {
                                        Console.WriteLine("輸入錯誤，請重新輸入");
                                    }
                                    else
                                    {
                                        selectSuit = true;
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine("輸入錯誤，請重新輸入");
                                }
                            }
                            bool selectValue = false;
                            int valueNo = 0;
                            while (!selectValue)
                            {
                                Console.WriteLine("輸入要猜的號碼：輸入 1 ~ 13 ");
                                try
                                {
                                    valueNo = Convert.ToInt32(Console.ReadLine());
                                    if (valueNo < 1 | valueNo > 13)
                                    {
                                        Console.WriteLine("輸入錯誤，請重新輸入");
                                    }
                                    else
                                    {
                                        selectValue = true;
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine("輸入錯誤，請重新輸入");
                                }
                            }
                            Card guess = new Card((Suits)suitNo, (Values)valueNo);
                            if (guess.suit == answer.suit && guess.value == answer.value)
                            {
                                win = true;
                                Console.WriteLine($"{name} : 答案是{answer.Name}，你答對了！");
                                hero.gold += 20;
                                Console.WriteLine($"獲得20金幣，持有金幣{hero.gold}");
                                Console.WriteLine($"{name} : 可惡，居然輸了，我還是回家好了...");
                                hero.map[location.x, location.y] = "";      //NPC輸了就消失在地圖上
                                break;
                            }
                            else if (guess.suit < answer.suit)
                            {
                                Console.WriteLine($"{name} : 答案比{guess.Name}更大");
                            }
                            else if (guess.suit > answer.suit)
                            {
                                Console.WriteLine($"{name} : 答案比{guess.Name}更小");
                            }
                            else if (guess.suit == answer.suit)
                            {
                                if (guess.value < answer.value)
                                {
                                    Console.WriteLine($"{name} : 答案比{guess.Name}更大");
                                }
                                else if (guess.value > answer.value)
                                {
                                    Console.WriteLine($"{name} : 答案比{guess.Name}更小");
                                }
                            }
                        }
                        if (!win)
                        {
                            Console.WriteLine($"{name} : 哈哈！看來是我贏了！答案是{answer.Name}！");
                        }
                    }
                    break;
                case 3:     //玩猜數字
                    Console.WriteLine($"{name} : 來玩一場猜數字遊戲？\n同意按 Y ，按其他鍵結束對話");
                    if (Console.ReadLine().ToUpper() == "Y")
                    {
                        Console.WriteLine($"{name} : 只要你在 12 回合內猜出數字，我就給你50金幣");
                        bool win = false;
                        List<string> nums = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                        string[] answer = new string[4];
                        for (int i = 0; i < 4; i++)
                        {
                            int pick = rand.Next(0, nums.Count);
                            answer[i] = nums[pick];
                            nums.RemoveAt(pick);
                        }
                        for (int i = 1; i < 13; i++)
                        {
                            Console.WriteLine($"第 {i} 回合");
                            bool input = false;
                            string[] guess = new string[4];
                            while (!input)
                            {
                                Console.WriteLine("請輸入四個不重複的數字");
                                string readline = Console.ReadLine();
                                if (readline.Length != 4)
                                {
                                    Console.WriteLine("輸入錯誤，請重新輸入");
                                }
                                else
                                {
                                    for (int j = 0; j < 4; j++)
                                    {
                                          guess[j] = readline.Substring(j, 1);
                                    }
                                    input = true;
                                }
                                for (int m = 0; m < 4; m++)
                                {
                                    for (int n = 0; n < 4; n++)
                                    {
                                        if (m == n) continue;
                                        if (guess[m] == guess[n])
                                        {
                                            Console.WriteLine("出現重複數字，請重新輸入");
                                            input = false;
                                            break;
                                        }
                                    }
                                    if (!input)
                                    {
                                        break;
                                    }
                                }
                            }
                            int A = 0;
                            int B = 0;
                            for (int m = 0; m < 4; m++)
                            {
                                for (int n = 0; n < 4; n++)
                                {
                                    if (guess [m] == answer[n])
                                    {
                                        if (m == n)
                                        {
                                            A++;
                                        }
                                        else
                                        {
                                            B++;
                                        }
                                    }
                                }
                            }
                            Console.WriteLine($" {A} A {B} B");
                            if (A == 4)
                            {
                                win = true;
                                Console.Write($"{name} : 答案是");
                                for (int k = 0; k < 4; k++)
                                {
                                    Console.Write(answer[k]);
                                }
                                Console.Write("，你答對了！\n");
                                hero.gold += 50;
                                Console.WriteLine($"獲得50金幣，持有金幣{hero.gold}");
                                Console.WriteLine($"{name} : 可惡，居然輸了，我還是回家好了...");
                                hero.map[location.x, location.y] = "";      //NPC輸了就消失在地圖上
                                break;
                            }
                        }
                        if (!win)
                        {
                            Console.WriteLine($"{name} : 哈哈！看來是我贏了！答案是");
                            for (int i = 0; i <4; i++)
                            {
                                Console.Write(answer[i]);
                            }
                            Console.Write("\n");
                        }
                    }
                    break;
                default:
                    Console.WriteLine($"哈囉！{hero.name}，今天天氣真好");
                    break;
            }
            
            
        }
        public List<Villager> CreateVillagers(ref Hero hero, int no)
        {
            List<Villager> villagers = new List<Villager>();
            Random rand = new Random();
            for (int i = 0; i < no; i++)    //設定商人名稱及位置
            {
                villagers.Add( new Villager { name = $"村民 - {namePool[i]}" });
                bool set = false;
                while (!set)
                {
                    if (i < 4)         //前4隻商人一定會出生在地圖(4,4)~(6,6)的村莊地區
                    {
                        int x = rand.Next(4, 7);
                        int y = rand.Next(4, 7);
                        if (String.IsNullOrEmpty(hero.map[x, y]))
                        {
                            villagers[i].location = (x, y);
                            hero.map[x, y] = villagers[i].name;
                            set = true;
                        }
                    }
                    else                //第5隻以後的商人隨便出生在全地圖
                    {
                        int x = rand.Next(0, 11);
                        int y = rand.Next(0, 11);
                        if (String.IsNullOrEmpty(hero.map[x, y]))
                        {
                            villagers[i].location = (x, y);
                            hero.map[x, y] = villagers[i].name;
                            set = true;
                        }
                    }
                }
            }
            return villagers;
        }
        

    }
}
