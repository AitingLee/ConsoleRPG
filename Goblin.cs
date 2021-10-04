using System;
using System.Collections.Generic;
using System.Text;

namespace HW6
{
    public class Goblin : Monster
    {
        private readonly Random rand = new Random();
        public override void Fight(ref Hero hero, ref bool endFight)
        {
            bool endTurn = false;
            // 執行現在狀態
            if (!endTurn && states["燃燒"] == "on")
            {
                hp -= 15;
                Console.WriteLine("受到燃燒傷害-15HP");
                Console.WriteLine($"{name}血量: {hp} / {maxHp}");
                if (hp < 0)
                {
                    Dead(ref hero);
                    endTurn = true;
                    endFight = true;
                }
            }
            bool skip = false;
            if (!endTurn && states["暈眩"] == "on")
            {
                Console.WriteLine($"{ name}被暈眩，跳過一回合");
                states["暈眩"] = "off";
                skip = true;
            }
            if (!endTurn && !skip)
            {
                int attackType = rand.Next(0, 10);       //哥布林有70%使出普通攻擊、30%使出暈眩攻擊
                if (attackType < 3)
                {
                    Console.WriteLine($"{name}使出大槌砸");
                    hero.states["暈眩"] = "on";
                    Console.WriteLine($"{hero.name}被暈眩");
                }
                else
                {
                    Console.WriteLine($"{name}使出普通攻擊");

                }
                int damage = atk  - hero.def;
                hero.hp -= damage;
                if (hero.hp < 0) { hero.hp = 0; }
                Console.WriteLine($"造成傷害: {damage} \n{hero.name}血量: {hero.hp} / {hero.maxHp}");
                if (hero.hp <= 0)    //英雄死亡
                {
                    hero.HeroDead();
                    endFight = true;
                }
            }
        }

        public override string[] Drop()
        {
            //哥布林最多掉落3個物品（骰3次骰子到3格陣列內)，機率如下：30%沒東西；30%為金幣；30%為菠菜；10%為木劍
            string[] drop = new string[3];
            for (int i = 0; i < 3; i++)
            {
                int dropNo = rand.Next(0, 10);
                int dropCase;
                if (dropNo < 3)
                {
                    dropCase = 0;
                }
                else if (dropNo < 6)
                {
                    dropCase = 1;
                }
                else if (dropNo < 9)
                {
                    dropCase = 2;
                }
                else
                {
                    dropCase = 3;
                }

                switch (dropCase)
                {
                    case 0:
                        drop[i] = "";
                        break;
                    case 1:
                        drop[i] = "金幣";
                        break;
                    case 2:
                        drop[i] = "菠菜";
                        break;
                    case 3:
                        drop[i] = "木劍";
                        break;
                }
            }
            return drop;
        }
        public List<Goblin> CreateGoblins(ref Hero hero, int no)
        {
            List<Goblin> goblins = new List<Goblin>();
            for (int i = 0; i < no; i++)    //設定各哥布林的屬性
            {
                goblins.Add (new Goblin()
                {
                    name = $"哥布林 {i + 1} 號",
                    lv = rand.Next(2, 5),
                    condition = 0,
                    drop = Drop(),
                    states = new Dictionary<string, string>(),
                });
                goblins[i].atk = rand.Next(10, 15) + (goblins[i].lv * 5);
                goblins[i].agi = rand.Next(5, 10) + (goblins[i].lv);
                goblins[i].def = rand.Next(5, 10) + (goblins[i].lv * 2);
                goblins[i].exp = 10 + (goblins[i].lv * 5);
                goblins[i].maxHp = 50 + (goblins[i].lv * 20);
                goblins[i].hp = goblins[i].maxHp;

                goblins[i].states.Add("燃燒", "off");
                goblins[i].states.Add("暈眩", "off");
                bool set = false;
                while (!set)
                {
                    int x = rand.Next(0, 11);
                    int y = rand.Next(0, 11);
                    while ((2 < x && x < 8) && (2 < y && y < 8))      // 地圖(3,3)~(7,7)為新手區，哥布林不能出生在這裡，迴圈到直到在外面
                    {
                        x = rand.Next(0, 11);
                        y = rand.Next(0, 11);
                    }
                    if (String.IsNullOrEmpty(hero.map[x, y]))
                    {
                        goblins[i].location = (x, y);
                        hero.map[x, y] = goblins[i].name;
                        set = true;
                    }
                }
            }
            return goblins;
        }
        public override void Dead(ref Hero hero)
        {
            Random rand = new Random();
            Console.WriteLine($"{name}已死亡，戰鬥結束，獲得經驗值{exp}");
            condition = 2;
            hero.map[location.x, location.y] = ""; //死亡時消失於地圖上
            int put = 0;
            foreach (string item in drop)
            {
                switch (item)
                {
                    case "金幣":
                        int gold = rand.Next(20, 30);
                        hero.gold += gold;
                        Console.WriteLine($"獲得金幣{gold}元");
                        put++;
                        break;
                    case "菠菜":
                        if (hero.PutBag(GameManager.Gm.spinach))
                        {
                            Console.WriteLine($"獲得:菠菜");
                        }
                        else
                        {
                            Console.WriteLine($"背包已滿，放不下囉！");
                        }
                        put++;
                        break;

                    case "木劍":
                        if (hero.PutBag(GameManager.Gm.woodSword))
                        {
                            Console.WriteLine($"獲得:木劍");
                        }
                        else
                        {
                            Console.WriteLine($"背包已滿，放不下囉！");
                        }
                        put++;
                        break;
                }
            }
            if (put == 0)
            {
                Console.WriteLine("沒有拾獲物品");
            }
            hero.exp += exp;      //增加經驗值
            if (hero.exp >= hero.maxExp)    //升等
            {
                hero.LevelUp();
            }
        }

    }
}
