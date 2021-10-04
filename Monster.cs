using System;
using System.Collections.Generic;
using System.Text;

namespace HW6
{
    public abstract class Monster //Monster 種類有 Slime、Goblin、Wolf
    {
        public abstract void Fight(ref Hero hero, ref bool endFight);       //攻擊方式
        public abstract string[] Drop();     //死亡掉落物容器
        public abstract void Dead(ref Hero hero);        //死亡時
        public string name;
        public int lv, atk, hp, maxHp, agi, def, exp, condition;
        public string[] drop;
        public (int x, int y) location;
        public Dictionary<string, string> states;
    }
}
    