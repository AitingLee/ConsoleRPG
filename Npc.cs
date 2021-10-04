using System;
using System.Collections.Generic;
using System.Text;

namespace HW6
{
    public abstract class Npc
    {
        public abstract void Talk(ref Hero hero);
        public string name;
        public (int x, int y) location;
    }
}
