using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    public class Ship
    {
        public int Length { get; set; }
        public Position Position { get; set; }

        public bool IsHorizontal { get; set; }
        public Ship(Position position, int length, bool isHorizontal)
        {
            Position = position;
            Length = length;
            IsHorizontal = isHorizontal;
        }
    }
}
