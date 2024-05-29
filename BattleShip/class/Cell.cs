using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    public class Cell
    {
        public bool IsOccupied {  get; set; } // поле показывает занята ли ячейка

        public bool IsHit { get; set; } // показывает был ли удар

        public Position Position { get; set; }


        public Cell(Position position) 
        {
            Position = position;

            IsOccupied = false;

            IsHit = false;
        }
    }
}
