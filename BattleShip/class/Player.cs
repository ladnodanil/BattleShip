﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    public class Player
    {
        public string Name { get; set; }

        public Board Board { get; set; }
        public Player(string name)
        {
            Name = name;

            Board = new Board();
        }
    }
}
