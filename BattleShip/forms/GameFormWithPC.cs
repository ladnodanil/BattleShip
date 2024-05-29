using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleShip
{
    public partial class GameFormWithPC : Form
    {
        private const int cellSize = 30; // Размер одной ячейки
        private const int gridSize = 10; // Размер сетки 10x10


        private Button[,] buttonsPlayer;
        private Button[,] buttonsEnemy;

        

        private Player player;
        private Player enemy;

        

        
        private bool isPlayerTurn = true;

        public GameFormWithPC(Player player, Button[,] buttonsPlayer)
        {
            this.buttonsPlayer = buttonsPlayer;
            
            this.player = player;

            enemy = new Player("БОТ");

            InitializeComponent();
            InitializeBoard();
            PlaceEnemyShips();
        }

        private void InitializeBoard()
        {

            buttonsEnemy = new Button[gridSize, gridSize];

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    buttonsPlayer[x, y] = new Button
                    {
                        Width = cellSize,
                        Height = cellSize,
                        Location = new Point(x * cellSize, y * cellSize),
                        Tag = new Point(x, y)

                    };
                    this.Controls.Add(buttonsPlayer[x, y]);
                }
            }


            // Создание доски для игрока 2
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    buttonsEnemy[x, y] = new Button
                    {
                        Width = cellSize,
                        Height = cellSize,
                        Location = new Point(x * cellSize + gridSize * cellSize + 20, y * cellSize),
                        Tag = new Point(x, y),
                        BackColor = Color.LightGray
                    };
                    buttonsEnemy[x, y].Click += new EventHandler(PlayerShootingCell_Click);
                    this.Controls.Add(buttonsEnemy[x, y]);
                }
            }
            UpdatePlacementBoard();
        }
        private void PlayerShootingCell_Click(object sender, EventArgs e)
        {
            if (!isPlayerTurn) return;

            Button clickedButton = sender as Button;
            Point location = (Point)clickedButton.Tag;
            int x = location.X;
            int y = location.Y;

            // Логика выстрела игрока
            if (!enemy.Board.Cells[x, y].IsHit)
            {
                enemy.Board.Cells[x, y].IsHit = true;

                if (enemy.Board.Cells[x, y].IsOccupied)
                {
                    //clickedButton.BackColor = Color.Red; // Попадание
                    clickedButton.Text = "X";
                }
                else
                {
                    //clickedButton.BackColor = Color.Blue; // Промах
                    clickedButton.Text = "*";
                }

                if (CheckGameOver(enemy.Board))
                {
                    MessageBox.Show($"{player.Name} wins!");
                }
                else
                {
                    isPlayerTurn = false;
                    EnemyTurn();
                }
            }
        }
        private void ShootingCell_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            Point location = (Point)clickedButton.Tag;
            int x = location.X;
            int y = location.Y;

            // Логика выстрелов
            if (!player.Board.Cells[x, y].IsHit)
            {
                player.Board.Cells[x, y].IsHit = true;

                if (player.Board.Cells[x, y].IsOccupied)
                {
                    clickedButton.BackColor = Color.Red; // Попадание
                    //buttonsPlayer[x, y].Text = "X";
                }
                else
                {
                    clickedButton.BackColor = Color.Blue; // Промах
                    //buttonsPlayer[x, y].Text = "*";
                }

                // Проверить состояние игры
                CheckGameState();
            }
        }
        private void CheckGameState()
        {
            // Проверить, выиграл ли текущий игрок
            if (IsGameOver())
            {
                MessageBox.Show($"{player.Name} wins!");
                // Сбросить или завершить игру
            }
        }

        private bool IsGameOver()
        {
            // Проверить, потоплены ли все корабли
            return !player.Board.Cells.Cast<Cell>().Any(cell => cell.IsOccupied && !cell.IsHit);
        }
        
        private void UpdatePlacementBoard()
        {
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    UpdatePlacementButton(buttonsPlayer[x, y], x, y);
                }
            }
        }

        private void UpdatePlacementButton(Button button, int x, int y)
        {
            Cell cell = player.Board.Cells[x, y];
            if (cell.IsOccupied)
            {
                button.BackColor = Color.Gray; // Цвет корабля
            }
            else
            {
                button.BackColor = Color.LightGray; // Цвет пустой ячейки
            }
        }
        private void PlaceEnemyShips()
        {
            int[] shipLengths = { 4, 3, 2, 1 };

            Random rand = new Random();

            foreach (int length in shipLengths)
            {
                bool placed = false;
                while (!placed)
                {
                    int x = rand.Next(gridSize);
                    int y = rand.Next(gridSize);
                    bool isHorizontal = rand.Next(2) == 0;

                    Ship ship = new Ship(new Position(x, y), length, isHorizontal);

                    if (enemy.Board.PlaceShip(ship))
                    {
                        placed = true;
                    }
                }
            }
        }
        private void EnemyTurn()
        {
            Random rand = new Random();
            int x, y;

            do
            {
                x = rand.Next(gridSize);
                y = rand.Next(gridSize);
            }
            while (player.Board.Cells[x, y].IsHit);

            player.Board.Cells[x, y].IsHit = true;

            if (player.Board.Cells[x, y].IsOccupied)
            {
                buttonsPlayer[x, y].BackColor = Color.Red; // Попадание
                //buttonsPlayer[x, y].Text = "X";
            }
            else
            {
                buttonsPlayer[x, y].BackColor = Color.Blue; // Промах
                //buttonsPlayer[x, y].Text = "*";
            }

            if (CheckGameOver(player.Board))
            {
                MessageBox.Show($"{enemy.Name} победил!");
            }
            else
            {
                isPlayerTurn = true;
            }
        }
        private bool CheckGameOver(Board board)
        {
            return !board.Cells.Cast<Cell>().Any(cell => cell.IsOccupied && !cell.IsHit);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
