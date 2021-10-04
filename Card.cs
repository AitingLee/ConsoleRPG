using System;
using System.Collections.Generic;
using System.Text;

namespace HW6
{
    public enum Suits { 梅花 = 1, 方塊 = 2, 紅心 = 3, 黑桃 = 4 }
    public enum Values
    {
        一 = 1,
        二 = 2,
        三 = 3,
        四 = 4,
        五 = 5,
        六 = 6,
        七 = 7,
        八 = 8,
        九 = 9,
        十 = 10,
        J = 11,
        Q = 12,
        K = 13,
    }
    public class Card
    {
        public Suits suit;
        public Values value;
        public Card(Suits suit, Values value)
        {
            this.suit = suit;
            this.value = value;
        }
        public string Name
        {
            get
            {
                return suit.ToString() + value.ToString();
            }

        }
    }
}
