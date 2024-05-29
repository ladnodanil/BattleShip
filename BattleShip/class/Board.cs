using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    public class Board
    {
        public Cell[,] Cells { get; private set; }

        public int MapSize = 10;

        public List<Ship> ships { get; private set; }


        public Board()
        {
            Cells = new Cell[MapSize, MapSize];

            ships = new List<Ship>();

            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    Cells[i, j] = new Cell(new Position(i,j));
                }
            }

        }

        public bool PlaceShip(Ship ship)
        {
            // Проверка, что корабль помещается на доску
            if (ship.IsHorizontal)
            {
                if (ship.Position.X + ship.Length > MapSize)
                    return false;

                // Проверяем, что клетки, на которые мы хотим разместить корабль, свободны и корабли не стоят вплотную
                for (int i = 0; i < ship.Length; i++)
                {
                    int x = ship.Position.X + i;
                    int y = ship.Position.Y;

                    if (x >= MapSize || Cells[x, y].IsOccupied || !AreAdjacentCellsFree(x, y))
                        return false;
                }

                // Размещаем корабль на доске и отмечаем занятость клеток
                for (int i = 0; i < ship.Length; i++)
                {
                    int x = ship.Position.X + i;
                    int y = ship.Position.Y;
                    Cells[x, y].IsOccupied = true;
                }
            }
            else
            {
                if (ship.Position.Y + ship.Length > MapSize)
                    return false;

                // Проверяем, что клетки, на которые мы хотим разместить корабль, свободны и корабли не стоят вплотную
                for (int i = 0; i < ship.Length; i++)
                {
                    int x = ship.Position.X;
                    int y = ship.Position.Y + i;

                    if (y >= MapSize || Cells[x, y].IsOccupied || !AreAdjacentCellsFree(x, y))
                        return false;
                }

                // Размещаем корабль на доске и отмечаем занятость клеток
                for (int i = 0; i < ship.Length; i++)
                {
                    int x = ship.Position.X;
                    int y = ship.Position.Y + i;
                    Cells[x, y].IsOccupied = true;
                }
            }

            // Добавляем корабль в список кораблей
            ships.Add(ship);
            return true;
        }

        private bool AreAdjacentCellsFree(int x, int y)
        {
            // Проверка всех соседних клеток на наличие кораблей
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    int nx = x + dx;
                    int ny = y + dy;

                    if (nx >= 0 && nx < MapSize && ny >= 0 && ny < MapSize)
                    {
                        if (Cells[nx, ny].IsOccupied)
                            return false;
                    }
                }
            }
            return true;
        }

        public void RemoveShip(Ship ship)
        {
            if (ships.Contains(ship))
            {
                // Освобождаем клетки, занятые кораблем
                if (ship.IsHorizontal)
                {
                    for (int i = 0; i < ship.Length; i++)
                    {
                        Cells[ship.Position.X + i, ship.Position.Y].IsOccupied = false;
                    }
                }
                else
                {
                    for (int i = 0; i < ship.Length; i++)
                    {
                        Cells[ship.Position.X, ship.Position.Y + i].IsOccupied = false;
                    }
                }
                 ships.Remove(ship);
            }
        }
        public void Clear()
        {
            ships.Clear();
            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    Cells[i, j] = new Cell(new Position(i, j));
                }
            }
        }


    }
}
