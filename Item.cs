using System;
using System.Collections.Generic;
using System.Text;

namespace HW6
{
    public abstract class Item
    {
        public string name;
        public (int x, int y) space;
        public int price;


        public abstract bool OnUse(ref Hero hero);
    }
}
