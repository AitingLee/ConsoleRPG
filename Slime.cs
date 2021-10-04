using System;
using System.Collections.Generic;
using System.Text;

namespace HW6
{
    public class Slime : Monster
    {
        private readonly Random rand = new Random();

        public override void Fight(ref Hero hero,ref bool endFight)
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
                }
            }
            bool skip = false;
            if (!endTurn && states["暈眩"] == "on")
            {
                Console.WriteLine($"{name}被暈眩，跳過一回合");
                states["暈眩"] = "off";
                skip = true;
            }
            if (!endTurn && !skip)
            {
                Console.WriteLine($"{name}使出普通攻擊");
                int damage = atk - hero.def;
                if (damage < 0)
                {
                    damage = 0;
                }
                hero.hp -= damage;
                if (hero.hp < 0) 
                { 
                    hero.hp = 0; 
                }
                Console.WriteLine($"造成傷害 : {damage} \n{hero.name}血量: {hero.hp} / {hero.maxHp}");
                if (hero.hp <= 0)    //英雄死亡
                {
                    hero.HeroDead();
                    endFight = true;
                }
            }
        }

        public override string[] Drop()
        {
            //史萊姆最多掉落3個物品（骰3次骰子到3格陣列內)，機率如下：50%沒東西；20%為金幣；20%為果凍；10%為炸彈
            string[] drop = new string[3];
            for (int i = 0; i < 3; i++)
            {
                int dropNo = rand.Next(0, 10);
                int dropCase;
                if (dropNo < 5)
                {
                    dropCase = 0;
                }
                else if (dropNo < 6)
                {
                    dropCase = 1;
                }
                else if (dropNo < 8)
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
                        drop[i] = "果凍";
                        break;
                    case 3:
                        drop[i] = "炸彈";
                        break;
                }
            }
            return drop;
        }

        public List<Slime> CreateSlimes(ref Hero hero, int no)
        {
            List<Slime> slimes = new List<Slime>();
            for (int i = 0; i < no; i++)    //設定各史萊姆的屬性
            {
                slimes.Add(new Slime()
                {
                    name = $"史萊姆 {i + 1} 號",
                    lv = rand.Next(1, 4),
                    condition = 0,
                    drop = Drop(),
                    states = new Dictionary<string, string>(),
                });
                slimes[i].atk = rand.Next(10, 15) + (slimes[i].lv * 5);
                slimes[i].agi = rand.Next(1, 5) + (slimes[i].lv);
                slimes[i].def = rand.Next(5, 10) + (slimes[i].lv * 2);
                slimes[i].exp = 10 + (slimes[i].lv * 5);
                slimes[i].maxHp = 50 + (slimes[i].lv * 20);
                slimes[i].hp = slimes[i].maxHp;

                slimes[i].states.Add("燃燒", "off");
                slimes[i].states.Add("暈眩", "off");
                bool set = false;
                while (!set)
                {
                    int x = rand.Next(0, 11);
                    int y = rand.Next(0, 11);
                    while ((3 < x && x < 7) && (3 < y && y < 7))      // 地圖(4,4)~(6,6)為村莊，史萊姆不能出生在這裡，迴圈到直到在外面
                    {
                        x = rand.Next(0, 11);
                        y = rand.Next(0, 11);
                    }
                    if (String.IsNullOrEmpty(hero.map[x, y]))
                    {
                        slimes[i].location = (x, y);
                        hero.map[x, y] = slimes[i].name;
                        set = true;
                    }
                }
            }
            return slimes;
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
                        int gold = rand.Next(5, 10);
                        hero.gold += gold;
                        Console.WriteLine($"獲得金幣{gold}元");
                        put++;
                        break;
                    case "果凍":
                        if (hero.PutBag(GameManager.Gm.jelly))
                        {
                            Console.WriteLine($"獲得:果凍");
                        }
                        else
                        {
                            Console.WriteLine($"背包已滿，放不下囉！");
                        }
                        put++;
                        break;
                    case "炸彈":
                        if (hero.PutBag(GameManager.Gm.jelly))
                        {
                            Console.WriteLine($"獲得:果凍");
                        }
                        else
                        {
                            Console.WriteLine($"背包已滿，放不下囉！");
                        };
                        put++;
                        break;
                }
            }
            if (put == 0)
            {
                Console.WriteLine("沒有獲得物品");
            }
            hero.exp += exp;      //增加經驗值
            if (hero.exp >= hero.maxExp)    //升等
            {
                hero.LevelUp();
            }
        }
    }
}
