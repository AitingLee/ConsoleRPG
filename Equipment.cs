using System;
using System.Collections.Generic;
using System.Text;

namespace HW6
{
    public class Equipment : Item
    {
        public string type;
        public int addAtk, damage;
        public string effect;
        public string description;

        public override bool OnUse(ref Hero hero)
        {
            bool use = false;
            Console.WriteLine($"物品資訊:\n物品名稱:{name}\n");
            if (addAtk != 0)
            {
                Console.WriteLine($"增加攻擊力:{addAtk}\n");
            }
            Console.WriteLine($"裝備效果:{description}");
            Console.WriteLine("按Y裝備，或按其他鍵返回");
            switch(Console.ReadLine().ToUpper())
            {
                case "Y":
                    use = true;
                    break;
                default:
                    break;
            }
            return use;
        }

        public void BattalUse (ref Monster monster)
        {
            if (damage != 0)
            {
                monster.hp -= damage;
                if (monster.hp < 0) { monster.hp = 0; }
                Console.WriteLine($"造成 {damage} 傷害，{monster.name}血量: {monster.hp} / {monster.maxHp}");
            }
        }
        public void CheckEffect(ref Monster monster)
        {
            if (effect == "燃燒")
            {
                monster.states["燃燒"] = "on";
                Console.WriteLine("敵人已燃燒");
            }
            if (effect == "暈眩")
            {
                monster.states["暈眩"] = "on";
                Console.WriteLine("敵人已暈眩");
            }
        }
    }
}
