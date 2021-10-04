using System;
using System.Collections.Generic;
using System.Text;

namespace HW6
{
    public class Consumable : Item
    {
        public int heal;
        public int addAtk;     //目前消耗品有兩種用途:補血/加攻擊力

        public override bool OnUse(ref Hero hero)
        {
            bool use = false;
            string description = $"物品名稱:{name}\n";
            if (heal != 0)
            {
                description += $"恢復血量:{heal}\n";
            }
            if (addAtk != 0)
            {
                description += $"增加攻擊力:{addAtk}\n";
            }
            Console.WriteLine($"物品資訊:\n{description}");
            Console.WriteLine("按Y使用，或按其他鍵返回");
            switch (Console.ReadLine().ToUpper())
            {
                case "Y":
                    
                    hero.atk += addAtk;
                    use = true;
                    Console.WriteLine($"成功使用道具{name}");
                    if (heal != 0)
                    {
                        if ((hero.maxHp - hero.hp) < heal)
                        {
                            heal = hero.maxHp - hero.hp;
                        }
                        hero.hp += heal;
                        Console.WriteLine($"恢復血量:{heal}\t目前血量{hero.hp}/{hero.maxHp}");
                    }
                    if (addAtk != 0)
                    {
                        Console.WriteLine( $"增加攻擊力:{addAtk}\t目前攻擊力{hero.atk}");
                    }
                    break;
                default:
                    Console.WriteLine("取消使用");
                    break;
            }
            return use;
        }

        
    }
}
