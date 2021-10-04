using System;
using System.Collections.Generic;
using System.Text;

namespace HW6
{
    public class Hero
    {
        public string name;
        public int lv, atk, hp, maxHp, agi, def, exp, maxExp, gold;
        public double acc;
        public Dictionary<string,string> states;     //states是自己擁有的狀態
        public Dictionary<string, Equipment> equipped;
        public string[,] bag;  
        public string[,] map;  //每個hero有自己對應的地圖
        public (int x, int y) location;
        public List<Monster> monsters;
        public List<Npc> npcs;

        public Hero CreateHero()
        {
            Random rand = new Random();     //產生亂數初始值

            Console.WriteLine("請輸入英雄的名字:");
            Hero hero = new Hero()
            {
                name = Console.ReadLine(),     //玩家輸入英雄名稱
                lv = 1,
                atk = rand.Next(40, 50),
                agi = rand.Next(10, 15),
                def = rand.Next(10, 20),
                acc = Math.Round((rand.NextDouble()), 2),
                hp = 300,
                maxHp = 300,
                exp = 0,
                maxExp = 100,
                equipped = new Dictionary<string, Equipment>(2),   //預設裝備格只有武器跟戰鬥道具
                states = new Dictionary<string, string>(2),     //預設狀態只有燃燒跟暈眩
                bag = new string[9,7], //產生9x7的背包
                map = new string[11,11], //產生二維陣列創造預設為11x11方格的地圖，每格用字串儲存現在位置的人
                gold = 0,
            };
            hero.monsters = CreateMonster(ref hero, 30, 20, 20);  //預設產生30隻史萊姆，20隻哥布林，20隻狼
            hero.npcs = CreateNpc(ref hero, 6, 10); //預設產生6隻商人(村莊至少1隻)，10隻村民(村莊至少4隻)

            hero.equipped.Add("武器", GameManager.Gm.emptyEquipment);       //裝備預設全部為 null
            hero.equipped.Add("戰鬥道具", GameManager.Gm.emptyEquipment);

            hero.states.Add("燃燒", "off");
            hero.states.Add("暈眩", "off");

            for (int i =0; i <9; i++)        //包包預設全部為"空"
            {
                for (int j = 0; j < 7; j++)
                {
                    hero.bag[i,j] = "空,";
                }
            }
            
            int x = rand.Next(4, 7);    //地圖用亂數預設在(4,4)到(6,6)的範圍內
            int y = rand.Next(4, 7);
            hero.map[x, y] = hero.name;
            hero.location = (x, y);
            Console.WriteLine("創建完成");
            return hero;
        }
        public void ShowStatus()
        {
            Console.WriteLine($"英雄名稱:{name}");
            Console.WriteLine($"等級:{lv}");
            Console.WriteLine($"血量:{hp}/{maxHp}");
            Console.WriteLine($"經驗值:{exp}/{maxExp}");
            Console.WriteLine($"攻擊力:{atk}");
            Console.WriteLine($"防禦力:{def}");
            Console.WriteLine($"敏捷度:{agi}");
            Console.WriteLine($"命中加成:{acc}");
            Console.WriteLine($"已裝備武器:{equipped["武器"].name}");
            Console.WriteLine($"已裝備戰鬥道具:{equipped["戰鬥道具"].name}");
            bool found = false;
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == name)
                    {
                        found = true;
                        Console.WriteLine($"{name} 所在位置為: {i},{j}");
                    }
                }
            }
            if (found != true)
            {
                Console.WriteLine($"{name} 不在地圖內");
            }
        }
        public bool PutBag (Item item)
        {
            bool place = false;
            int placeX = -1;
            int placeY = -1;
            for (int i = 0; i < bag.GetLength(0); i++)
            {
                for (int j = 0; j < bag.GetLength(1); j++)
                {
                    if (bag[i, j] == "空,")
                    {
                        for (int m = 0; m < item.space.x; m++)
                        {
                            for (int n = 0; n < item.space.y; n++)
                            {
                                if ((i + m >= bag.GetLength(0)) | (j + n >= bag.GetLength(1)))
                                {
                                    place = false;
                                    break;
                                }
                                else if (bag[i + m, j + n] != "空,")
                                {
                                    place = false;
                                    break;
                                }
                                else if (bag[i + m, j + n] == "空,")
                                {
                                    place = true;
                                }
                            }
                            if (place == false) { break; }
                        }
                    }
                    if (place == true)
                    {
                        placeX = i;
                        placeY = j;
                        break;
                    }
                }
                if (place) {break;}
            }
            bool put = false;
            if (place)
            {
                for (int i = 0; i < item.space.x; i++)
                {
                    for (int j = 0; j < item.space.y; j++)
                    {
                        bag[placeX + i, placeY + j] = $"{item.name},";
                    }
                }
                put = true;
            }
            return put;
        }
        public int FindBag (Item item)       //在背包裡找物品,回傳有幾個該物品
        {
            int count = 0;
            for (int i = 0; i < bag.GetLength(0) ; i++)
            {
                for (int j = 0; j < bag.GetLength(1) ; j++)
                {
                    if (bag[i,j] == $"{item.name},")
                    {
                        count++;
                    }
                }
            }
            if (count % (item.space.x * item.space.y) != 0)
            {
                Console.WriteLine("查找背包出現錯誤");
            }
            return count / (item.space.x * item.space.y);
        }
        public void PutOn(Equipment equipment)
        {
            bool putOn = false;
            if (equipped[equipment.type].name == "無")     //若原本沒有裝備
            {
                putOn = true;
            }
            else       //若原本有裝備
            {
                Console.WriteLine($"英雄原有裝備{equipped[equipment.type].name}，須先卸下才可裝備。\n 確定請按Y，按其他任意按鍵以返回");
                switch (Console.ReadLine().ToUpper())
                {
                    case "Y":
                        if (PutBag(equipped[equipment.type]))
                        {
                            atk -= equipped[equipment.type].addAtk;
                            putOn = true;
                        }
                        else
                        {
                            Console.WriteLine("取消裝備");
                        }
                       
                        break;
                    default:
                        Console.WriteLine("取消裝備");
                        break;
                }
            }
            if (putOn)      //可裝備
            {
                equipped[equipment.type] = equipment;
                if (equipment.addAtk != 0)
                {
                    atk += equipment.addAtk;
                }
            }
        }
        public bool OutBag (Item item)
        {
            bool outBag = false;
            int outX = -1;
            int outY = -1;
            bool find = false;
            for (int i = 0; i < bag.GetLength(0); i++)
            {
                for (int j = 0; j < bag.GetLength(1); j++)
                {
                    if (bag[i, j] == $"{item.name},")
                    {
                        outX = i;
                        outY = j;
                        find = true;
                        break;
                    }
                }
                if(find)
                {
                    break;
                }
            }
            if (outX == -1 | outY == -1)
            {
                Console.WriteLine("移除背包出現錯誤");
                outBag = false;
            }
            else
            {
                for (int m = 0; m < item.space.x; m++)
                {
                    for (int n = 0; n < item.space.y; n++)
                    {
                        bag[outX + m, outY + n] = "空,";
                    }
                }
                outBag = true;
            }
            return outBag;
        }
        public void ShowBag()
        {
            Console.WriteLine($"{name}的背包:");
            for (int i = 0; i < bag.GetLength(0); i++)
            {
                for (int j =0; j <bag.GetLength(1); j++)
                {
                    Console.Write(bag[i, j]);
                }
                Console.Write("\n");
            }
            Console.WriteLine($"持有金錢：{gold}");
        }
        public Item UseBag(ref bool close)
        {
            Console.WriteLine("請選擇您要使用的道具");
            Console.WriteLine("1 : 果凍 ; 2 : 菠菜 ; 3 : 炸彈 ; 4 : 閃光彈 ; 5 : 木劍 ; 6 : 火焰弓 ; 其他任意鍵 : 不使用道具" );
            switch (Console.ReadLine().ToUpper())
            {
                case "1":           //用果凍
                    if (FindBag(GameManager.Gm.jelly) != 0)
                    {
                        return GameManager.Gm.jelly;
                    }
                    return null;
                case "2":           //用菠菜
                    if (FindBag(GameManager.Gm.spinach) != 0)
                    {
                        return GameManager.Gm.spinach;
                    }
                    return null;
                case "3":           //用炸彈
                    if (FindBag(GameManager.Gm.bomb) != 0)
                    {
                        return GameManager.Gm.bomb;
                    }
                    return null;
                case "4":           //買閃光彈
                    if (FindBag(GameManager.Gm.flashBomb) != 0)
                    {
                        return GameManager.Gm.flashBomb;
                    }
                    return null;
                case "5":           //買木劍
                    if (FindBag(GameManager.Gm.woodSword) != 0)
                    {
                        return GameManager.Gm.woodSword;
                    }
                    return null;
                case "6":           //買火焰弓
                    if (FindBag(GameManager.Gm.fireBow) != 0)
                    {
                        return GameManager.Gm.fireBow;
                    }
                    return null;
                default:
                    Console.WriteLine("返回");
                    close = true;
                    return null;
            }
        }
        public List<Monster> CreateMonster(ref Hero hero,int slimeNo, int goblinNo, int wolfNo)
        {
            List<Monster> monsters = new List<Monster>();

            Slime slime = new Slime();
            Goblin goblin = new Goblin();
            Wolf wolf = new Wolf();

            List<Slime> slimes = slime.CreateSlimes(ref hero, slimeNo);
            List<Goblin> goblins = goblin.CreateGoblins(ref hero, goblinNo);
            List<Wolf> wolfs = wolf.CreateWolfs(ref hero, wolfNo);

            monsters.AddRange(slimes);
            monsters.AddRange(goblins);
            monsters.AddRange(wolfs);

            return monsters;
        }
        public List<Npc> CreateNpc(ref Hero hero, int merchantNo, int villagerNo)
        {
            List<Npc> npcs = new List<Npc>();

            Merchant merchant = new Merchant();
            Villager villager = new Villager();

            List<Merchant> merchants = merchant.CreateMerchants(ref hero, merchantNo);
            List<Villager> villagers = villager.CreateVillagers(ref hero, villagerNo);

            npcs.AddRange(merchants);
            npcs.AddRange(villagers);

            return npcs;
        }
        public string Move()
        {
            try
            {
                if (!String.IsNullOrEmpty(map[location.x - 1, location.y]))     //找前方 ( x-1 , y )
                {
                    Console.WriteLine($"英雄前方位置( {location.x - 1} , {location.y} ) : {map[location.x - 1, location.y]}，按 W 進入互動");
                }
                else
                {
                    Console.WriteLine($"英雄前方位置( {location.x - 1} , {location.y} ) 沒有目標，按 W 向前移動");
                }
            }
            catch(Exception)
            {
                Console.WriteLine("你的前方是懸崖");
            }
            try
            {
                if (!String.IsNullOrEmpty(map[location.x, location.y - 1]))     //找左方 ( x , y-1 )
                {
                    Console.WriteLine($"英雄左方位置( {location.x} , {location.y - 1} ) : {map[location.x, location.y - 1]}，按 A 進入互動");
                }
                else
                {
                    Console.WriteLine($"英雄左方位置( {location.x} , {location.y - 1} ) 沒有目標，按 A 向左移動");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("你的左方是懸崖");
            }
            try
            {
                if (!String.IsNullOrEmpty(map[location.x + 1, location.y]))     //找後方 ( x+1 , y )
                {
                    Console.WriteLine($"英雄後方位置( {location.x + 1} , {location.y} ) : {map[location.x + 1, location.y]}，按 S 進入互動");
                }
                else
                {
                    Console.WriteLine($"英雄後方位置( {location.x + 1} , {location.y} ) 沒有目標，按 S 向後移動");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("你的後方是懸崖");
            }
            try
            {
                if (!String.IsNullOrEmpty(map[location.x, location.y + 1]))     //找右方 ( x , y+1 )
                {
                    Console.WriteLine($"英雄右方位置( {location.x} , {location.y + 1} ) : {map[location.x, location.y + 1]}，按 D 進入互動");
                }
                else
                {
                    Console.WriteLine($"英雄右方位置( {location.x} , {location.y + 1} ) 沒有目標，按 D 向右移動");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("你的右方是懸崖");
            }
            string targetName = "";
            switch (Console.ReadLine().ToUpper())
            {
                case "W":
                    try
                    {
                        if (!String.IsNullOrEmpty(map[location.x - 1, location.y]))        //如果前方位置有東西，把他設為目標(英雄不移動)
                        {
                            targetName = map[location.x - 1, location.y];
                        }
                        else       //目標位置沒東西，英雄才能站上去
                        {
                            map[location.x, location.y] = "";       //英雄原有位置設為空
                            location = (location.x - 1, location.y);        //移動英雄
                            map[location.x, location.y] = name;     //地圖新位置設為英雄名字
                            Console.WriteLine("移動了一步");
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("生命可貴，不要跳懸崖");
                    }
                    break;
                case "A":
                    try
                    {
                        if (!String.IsNullOrEmpty(map[location.x, location.y - 1]))
                        {
                            targetName = map[location.x, location.y - 1];
                        }
                        else
                        {
                            map[location.x, location.y] = "";       //英雄原有位置設為空
                            location = (location.x, location.y - 1);        //移動英雄
                            map[location.x, location.y] = name;     //地圖新位置設為英雄名字
                            Console.WriteLine("移動了一步");
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("生命可貴，不要跳懸崖");
                    }
                    break;
                case "S":
                    try
                    {
                        if (!String.IsNullOrEmpty(map[location.x + 1, location.y]))
                        {
                            targetName = map[location.x + 1, location.y];
                        }
                        else
                        {
                            map[location.x, location.y] = "";       //英雄原有位置設為空
                            location = (location.x + 1, location.y);        //移動英雄
                            map[location.x, location.y] = name;     //地圖新位置設為英雄名字
                            Console.WriteLine("移動了一步");
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("生命可貴，不要跳懸崖");
                    }
                    break;
                case "D":
                    try
                    {
                        if (!String.IsNullOrEmpty(map[location.x, location.y + 1]))
                        {
                            targetName = map[location.x, location.y + 1];
                        }
                        else
                        {
                            map[location.x, location.y] = "";       //英雄原有位置設為空
                            location = (location.x, location.y + 1);        //移動英雄
                            map[location.x, location.y] = name;     //地圖新位置設為英雄名字
                            Console.WriteLine("移動了一步");
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("生命可貴，不要跳懸崖");
                    }
                    break;
            }
            return targetName;
        }
        public void FightRound(ref Monster monster,ref bool monsterDead,ref bool endFight)
        {
            bool endTurn = false;
            // 執行英雄現在狀態
            if (!endTurn && states["燃燒"] == "on")
            {
                hp -= 15;
                Console.WriteLine("受到燃燒傷害-15HP");
                Console.WriteLine($"英雄血量: {hp} / {maxHp}");
                states["燃燒"] = "off";
                if (hp < 0)
                {
                    HeroDead();
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
                // 選擇本回合動作
                bool select = false;
                while (!select)
                {
                    Console.WriteLine($"輸入任意鍵攻擊 : 已裝備武器({equipped["武器"].name})\t或輸入 1 : 使用戰鬥道具({equipped["戰鬥道具"].name})\t2 : 逃跑(50%機率成功)");
                    switch (Console.ReadLine())
                    {
                        case "1":       //使用戰鬥道具
                            if (equipped["戰鬥道具"].name == "無")
                            {
                                Console.WriteLine("您沒有裝備戰鬥道具");
                            }
                            else
                            {
                                select = true;
                                equipped["戰鬥道具"].CheckEffect(ref monster);
                                equipped["戰鬥道具"].BattalUse(ref monster);
                                equipped["戰鬥道具"] = GameManager.Gm.emptyEquipment;   //使用後變為空裝備
                                if (monster.hp <= 0)
                                {
                                    monsterDead = true;
                                }
                            }
                            break;
                        case "2":       //逃跑
                            select = true;
                            Random rand = new Random();
                            int flee = rand.Next(0, 2);
                            if (flee == 0)
                            {
                                endFight = true;
                                Console.WriteLine("逃跑成功");
                            }
                            else
                            {
                                Console.WriteLine("逃跑失敗");
                            }
                            break;
                        default:        //普通攻擊
                            select = true;
                            bool hit = (agi / monster.agi + acc) > 1.5;
                            if (hit)
                            {
                                equipped["武器"].CheckEffect(ref monster);
                                int hitDamage = (int)(atk * 1.4 - monster.def * 2.5);
                                if (hitDamage <= 0) { hitDamage = 1; }
                                monster.hp -= hitDamage;
                                if (monster.hp <= 0)
                                {
                                    monster.hp = 0;     //若造成傷害後怪物血量<0則血量為0
                                    monsterDead = true;
                                }
                                Console.WriteLine($"命中!!\n造成傷害 : {hitDamage} \n目標血量 : {monster.hp} / {monster.maxHp}");
                                if ((monster.hp > 0) && (monster.hp < monster.maxHp))
                                {
                                    monster.condition = 1;
                                }
                            }
                            else    //若未命中 
                            {
                                Console.WriteLine("Miss");
                                Console.WriteLine("等級太低，請先挑戰其他怪物");
                            }
                            break;
                    }
                }
            }
        }
        public void HeroDead()
        {
            Console.WriteLine("英雄已死亡");
            map[location.x, location.y] = "";
            Random rand = new Random();
            int x = rand.Next(4, 7);
            int y = rand.Next(4, 7);
            map[x, y] = name;
            location = (x, y);
            hp = maxHp;
            exp -= 20;
            if (exp < 0)
            {
                exp = 0;
            }
            Console.WriteLine("英雄已於村莊內重生");
        }
        public void LevelUp()
        {
            lv += 1;
            exp -= maxExp;
            maxExp += 20;
            atk += 5;
            agi += 2;
            def += 2;
            acc = Math.Round((acc + 0.5), 2);
            maxHp += 20;
            hp = maxHp;
            Console.WriteLine("等級提升！英雄目前狀態:");
            ShowStatus();
        }
        public void Purchase(Item item)
        {
            Console.WriteLine($"購買{item.name} : 請輸入購買數量");
            bool input = false;
            bool check = false;
            bool cancel = false;
            int amount = 0;
            while (!check && !cancel)
            {
                while (!input)
                {
                    try
                    {
                        amount = Convert.ToInt32(Console.ReadLine());
                        input = true;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"輸入錯誤，請重新輸入");
                    }
                }
                Console.WriteLine($"購買{amount}個{item.name}，按 Y 確認，按 X 取消購買，按其他鍵重新輸入數量");
                switch (Console.ReadLine().ToUpper())
                {
                    case "Y":
                        int buyNo = 0;
                        int pay = 0;
                        if (gold < (item.price * amount))   //若買不起
                        {
                            Console.WriteLine("你太窮買不起啦，請重新輸入購買數量");
                            input = false;
                            break;
                        }
                        else
                        {
                            for (int i = 0; i < amount; i++)
                            {
                                bool put = PutBag(item);
                                if (put)
                                {
                                    buyNo += 1;
                                    pay += item.price;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        Console.WriteLine($"已成功購買{buyNo}個{item.name}，花費{pay}元");
                        gold -= pay;
                        Console.WriteLine($"持有金額: {gold}");
                        check = true;
                        break;
                    case "X":
                        cancel = true;
                        Console.WriteLine("已取消購買");
                        break;
                    default:
                        input = false;
                        break;
                }
            }
        }
        public void Sell(Item item)
        {
            int count = FindBag(item);
            if (count != 0)
            {
                Console.WriteLine($"您有{count}個{item.name}，請輸入要販售的數量");
                bool input = false;
                bool check = false;
                bool cancel = false;
                int amount = 0;
                while (!check && !cancel)
                {
                    while (!input)
                    {
                        try
                        {
                            amount = Convert.ToInt32(Console.ReadLine());
                            input = true;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine($"輸入錯誤，請重新輸入");
                        }
                        if (amount > count)
                        {
                            Console.WriteLine($"您只有{count}個{item.name}，請重新輸入");
                            input = false;
                        }
                    }
                    Console.WriteLine($"販售{amount}個{item.name}，按 Y 確認，按 X 取消販售，按其他鍵重新輸入數量");
                    switch (Console.ReadLine().ToUpper())
                    {
                        case "Y":
                            int sellNo = 0;
                            int reward = 0;
                            for (int i = 0; i < amount; i++)
                            {
                                if (OutBag(item))
                                {
                                    reward += (item.price / 2);
                                    sellNo += 1;
                                }
                                else
                                {
                                    Console.WriteLine("出現錯誤，取消販售");
                                    break;
                                }
                            }
                            Console.WriteLine($"已成功販售{sellNo}個{item.name}，獲得{reward}元");
                            gold += reward;
                            Console.WriteLine($"持有金額: {gold}");
                            check = true;
                            break;
                        case "X":
                            cancel = true;
                            Console.WriteLine("已取消購買");
                            break;
                        default:
                            input = false;
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine($"您沒有{item.name}");
            }
        }
    }

}
