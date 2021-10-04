using System;
using System.Collections.Generic;
using System.Text;

namespace HW6
{
    public sealed class GameManager
    {
        private static GameManager gm;
        private GameManager()
        {
        }
        public static GameManager Gm
        {
            get
            {
                if (gm == null)
                {
                    gm = new GameManager();
                }
                return gm;
            }
        }

        public Dictionary<String, User> Users = new Dictionary<String, User>();
        public class User
        {
            public string password;
            public Hero[] hero;
        }
        public Consumable jelly = new Consumable()
        {
            name = "果凍",
            space = (1, 1),
            addAtk = 0,
            heal = 50,
            price = 10,
        };
        public Consumable spinach = new Consumable()
        {
            name = "菠菜",
            space = (2, 1),
            addAtk = 2,
            heal = 30,
            price = 20,
        };
        public Equipment emptyEquipment = new Equipment()
        {
            name = "無",
        };
        public Equipment woodSword = new Equipment()
        {
            name = "木劍",
            type = "武器",
            space = (3, 1),
            addAtk = 10,
            description = "此裝備沒有任何特殊效果",
            price = 30,
        };
        public Equipment fireBow = new Equipment()
        {
            name = "火焰弓",
            type = "武器",
            space = (1, 3),
            addAtk = 20,
            effect = "燃燒",
            description = "攻擊時有50%機率使敵人燃燒（每回合開始時-15HP）",
            price = 150,
        };
        public Equipment bomb = new Equipment()
        {
            name = "炸彈",
            type = "戰鬥道具",
            space = (2, 2),
            damage = 50,
            description = "消耗道具，使敵人-100HP",
            price = 50,
        };
        public Equipment flashBomb = new Equipment()
        {
            name = "閃光彈",
            type = "戰鬥道具",
            space = (2, 2),
            effect = "暈眩",
            damage = 100,
            description = "消耗道具，造成敵人暈眩且-50HP",
            price = 60,
        };

        public void RunGame(ref Hero hero, ref bool select)
        {
            Console.WriteLine("請選擇其中一項動作");
            Console.WriteLine("0:返回選單 ; 1:狀態 ; 2:背包 ; 3:移動/戰鬥/互動 ; 4:進入新世界");
            switch (Console.ReadLine())
            {
                case "0":
                    Console.WriteLine("確定要回英雄選單嗎? \n Y:確定 ; 其他任意鍵:返回遊戲");
                    switch (Console.ReadLine().ToUpper())
                    {
                        case "Y":
                            Console.WriteLine("登出此英雄");
                            select = false;
                            break;
                        default:
                            Console.WriteLine("返回遊戲");
                            break;
                    }
                    break;
                case "1":
                    hero.ShowStatus();
                    break;
                case "2":
                    hero.ShowBag();
                    bool close = false;
                    while (!close)
                    {
                        Item used = hero.UseBag(ref close);
                        if (used != null)
                        {
                            if (used.OnUse(ref hero))
                            {
                                hero.OutBag(used);
                                if (used is Equipment equipment)
                                {
                                    hero.PutOn(equipment);
                                }
                            }
                        }
                        else if (used == null && !close)
                        {
                            Console.WriteLine("背包內沒有此道具");
                        }
                        else
                        {
                            break;
                        }
                    }
                    break;
                case "3":
                    string targetName = hero.Move();
                    Interaction(ref hero, targetName);
                    break;
                case "4":
                    Console.WriteLine("確定要進入新的世界嗎？ 按Y確認，其他鍵取消");
                    switch (Console.ReadLine().ToUpper())
                    {
                        case "Y":
                            Console.WriteLine("請輸入新世界的史萊姆數量(10~30隻)");
                            bool setSlime = false;
                            int slimeNo = 0;
                            while (!setSlime)
                            {
                                try
                                {
                                    slimeNo = Convert.ToInt32(Console.ReadLine());
                                    if (slimeNo > 30 | slimeNo < 10)
                                    {
                                        throw new Exception();
                                    }
                                    else
                                    {
                                        setSlime = true;
                                    }
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("輸入錯誤，請重新輸入史萊姆數量(10~30隻)");
                                }
                            }
                            Console.WriteLine("請輸入新世界的哥布林數量(10~20隻)");
                            bool setGoblin = false;
                            int goblinNo = 0;
                            while (!setGoblin)
                            {
                                try
                                {
                                    goblinNo = Convert.ToInt32(Console.ReadLine());
                                    if (goblinNo > 20 | goblinNo < 10)
                                    {
                                        throw new Exception();
                                    }
                                    else
                                    {
                                        setGoblin = true;
                                    }
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("輸入錯誤，請重新輸入哥布林數量(10~20隻)");
                                }
                            }

                            Console.WriteLine("請輸入新世界的野狼數量(10~20隻)");
                            bool setWolf = false;
                            int wolfNo = 0;
                            while (!setWolf)
                            {
                                try
                                {
                                    wolfNo = Convert.ToInt32(Console.ReadLine());
                                    if (wolfNo > 20 | wolfNo < 10)
                                    {
                                        throw new Exception();
                                    }
                                    else
                                    {
                                        setWolf = true;
                                    }
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("輸入錯誤，請重新輸入野狼數量(10~20隻)");
                                }
                            }

                            Console.WriteLine("請輸入新世界的商人數量(3~6隻)");
                            bool setMerchant = false;
                            int merchantNo = 0;
                            while (!setMerchant)
                            {
                                try
                                {
                                    merchantNo = Convert.ToInt32(Console.ReadLine());
                                    if (merchantNo > 6 | merchantNo < 3)
                                    {
                                        throw new Exception();
                                    }
                                    else
                                    {
                                        setMerchant = true;
                                    }
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("輸入錯誤，請重新輸入商人數量(3~6隻)");
                                }
                            }

                            Console.WriteLine("請輸入新世界的村民數量(5~10隻)");
                            bool setVillager = false;
                            int villagerNo = 0;
                            while (!setVillager)
                            {
                                try
                                {
                                    villagerNo = Convert.ToInt32(Console.ReadLine());
                                    if (villagerNo > 10 | villagerNo < 5)
                                    {
                                        throw new Exception();
                                    }
                                    else
                                    {
                                        setVillager = true;
                                    }
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("輸入錯誤，請重新輸入村民數量(5~10隻)");
                                }
                            }
                            
                            Console.WriteLine($"新世界建立中．．．地圖重置中．．．");
                            for (int i = 0; i < 11; i++)
                            {
                                for (int j = 0; j < 11; j++)
                                {
                                    hero.map[i, j] = "";
                                }
                            }

                            Console.WriteLine($"{hero.name}正在飛到新村莊．．．");
                            Random rand = new Random();
                            int x = rand.Next(4, 7);    //地圖用亂數預設在(4,4)到(6,6)的範圍內
                            int y = rand.Next(4, 7);
                            hero.map[x, y] = hero.name;
                            hero.location = (x, y);

                            Console.WriteLine($"放置史萊姆{slimeNo}隻．．．\n放置哥布林{goblinNo}隻．．．\n放置野狼{wolfNo}隻．．．");
                            hero.monsters = hero.CreateMonster(ref hero, slimeNo, goblinNo, wolfNo);

                            Console.WriteLine($"放置商人{merchantNo}隻．．．\n放置村民{villagerNo}隻");
                            hero.npcs = hero.CreateNpc(ref hero, merchantNo, villagerNo);

                            Console.WriteLine("歡迎來到一個嶄新的世界");
                            break;
                        default:
                            Console.WriteLine("還是先回原本的世界好了");
                            break;
                    }
                    break;
            }

        }

        public void Interaction(ref Hero hero, string targetName)
        {
            if (targetName != "")
            {
                foreach (Monster monster in hero.monsters)
                {
                    if (monster is Monster mon && mon.name == targetName)
                    {
                        Console.WriteLine($"怪物名稱:{mon.name}");
                        Console.WriteLine($"怪物等級:{mon.lv}");
                        switch (mon.condition)
                        {
                            case 0:
                                Console.WriteLine("怪物狀態:滿血");
                                break;
                            case 1:
                                Console.WriteLine($"怪物狀態:失血\t剩餘血量{mon.hp}/{mon.maxHp}");
                                break;
                            case 2:
                                Console.WriteLine($"怪物狀態:死亡");
                                break;
                        }
                        Console.WriteLine("確認要進入戰鬥請按Y，按其他鍵以返回");
                        switch (Console.ReadLine().ToUpper())
                        {
                            case "Y":
                                Battle(ref hero, ref mon);
                                break;
                            default:
                                Console.WriteLine("取消發起戰鬥");
                                break;
                        }
                    }
                }
                foreach (Npc npc in hero.npcs)
                {
                    if (npc.name == targetName)
                    {
                        npc.Talk(ref hero);
                    }
                }
            }
        }

        public void Battle(ref Hero hero, ref Monster monster)
        {
            Console.WriteLine($"進入與{monster.name}的戰鬥");
            Console.WriteLine($"敵人資訊\n血量:{monster.hp}/{monster.maxHp}\t等級:{monster.lv}\t攻擊力:{monster.atk}\t防禦力:{monster.def}\t閃避率:{monster.agi}");
            bool monsterDead = false;
            bool endFight = false;
            int round = 1;
            if (monster.agi < hero.agi)
            {
                while (!endFight)
                {
                    Console.WriteLine($"第{round}回合，{hero.name}先攻");
                    hero.FightRound(ref monster, ref monsterDead,ref endFight);
                    if (monsterDead)
                    {
                        monster.Dead(ref hero);
                        endFight = true;
                        break;
                    }
                    if (endFight)
                    {
                        break;
                    }
                    Console.WriteLine($"輪到{monster.name}了");
                    monster.Fight(ref hero, ref endFight);
                    round++;
                }
            }
            else
            {
                while (!endFight)
                {
                    Console.WriteLine($"第{round}回合，{monster.name}先攻");
                    monster.Fight(ref hero, ref endFight);
                    if (endFight)
                    {
                        break;
                    }
                    Console.WriteLine($"輪到{hero.name}了");
                    hero.FightRound(ref monster, ref monsterDead, ref endFight);
                    if (monsterDead)
                    {
                        monster.Dead(ref hero);
                        endFight = true;
                        break;
                    }
                    round++;
                }
            }
        }

    }

    
}
