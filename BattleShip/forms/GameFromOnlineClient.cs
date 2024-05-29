using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleShip
{
    public partial class GameFromOnlineClient : Form
    {
        private const int cellSize = 30; // Размер одной ячейки
        private const int gridSize = 10; // Размер сетки 10x10

        private Socket clientSocket;

        private Button[,] buttonsPlayer;
        private Button[,] buttonsEnemy;

        private Player player;
        private Player enemy;
        private bool isPlayerTurn = true; // Переменная для отслеживания текущего хода

        public GameFromOnlineClient(Player player, Button[,] buttonsPlayer, Socket socket)
        {
            try
            {
                this.clientSocket = socket;
                this.player = player;
                this.buttonsPlayer = buttonsPlayer;
                enemy = new Player("Игрок с другого устройства");
                InitializeComponent();
                InitializeBoard();
                InitializeNetwork();
                this.Load += GameFromOnline_Load;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                    buttonsEnemy[x, y].Click += new EventHandler(ShootingCell_Click);
                    this.Controls.Add(buttonsEnemy[x, y]);
                }
            }
            UpdatePlacementBoard();
        }

        private void InitializeNetwork()
        {
            // Начало получения данных от сервера в отдельном потоке
            Task.Run(() => ReceiveData());
        }

        private void ShootingCell_Click(object sender, EventArgs e)
        {
            if (!isPlayerTurn) return; // Игрок не может стрелять, если не его ход

            Button clickedButton = sender as Button;
            Point location = (Point)clickedButton.Tag;
            int x = location.X;
            int y = location.Y;

            // Логика выстрелов
            if (!enemy.Board.Cells[x, y].IsHit)
            {
                enemy.Board.Cells[x, y].IsHit = true;

                if (enemy.Board.Cells[x, y].IsOccupied)
                {
                    clickedButton.Text = "X"; // Попадание
                }
                else
                {
                    clickedButton.Text = "*"; // Промах
                }

                if (CheckGameOver(enemy.Board))
                {
                    MessageBox.Show($"{player.Name} Победил");
                }

                // Отправка данных о выстреле на сервер
                SendDataToServer($"SHOOT:{x},{y}");
                isPlayerTurn = false;
                UpdateTurnDisplay();
            }
        }

        private bool CheckGameOver(Board board)
        {
            return !board.Cells.Cast<Cell>().Any(cell => cell.IsOccupied && !cell.IsHit);
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

        private void SendDataToServer(string data)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            clientSocket.Send(buffer);
        }

        private void ReceiveData()
        {
            while (true)
            {
                byte[] buffer = new byte[4096];
                int bytesRead = clientSocket.Receive(buffer);
                if (bytesRead > 0)
                {
                    string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    ProcessServerData(data);
                }
            }
        }

        private void ProcessServerData(string data)
        {
            string[] parts = data.Split(':');
            string command = parts[0];
            string[] parameters;

            switch (command)
            {
                case "PLACE":
                    parameters = parts[1].Split('|');
                    for (int i = 0; i < parameters.Length - 1; i++)
                    {
                        string[] pos = parameters[i].Split(',');
                        int xi = int.Parse(pos[0]);
                        int yi = int.Parse(pos[1]);
                        // Обработка размещения корабля
                        enemy.Board.Cells[xi, yi].IsOccupied = true;
                    }
                    break;

                case "SHOOT":
                    parameters = parts[1].Split(',');
                    int x = int.Parse(parameters[0]);
                    int y = int.Parse(parameters[1]);
                    // Обработка выстрела
                    if (!player.Board.Cells[x, y].IsHit)
                    {
                        player.Board.Cells[x, y].IsHit = true;

                        if (player.Board.Cells[x, y].IsOccupied)
                        {
                            buttonsPlayer[x, y].BackColor = Color.Red; // Попадание
                        }
                        else
                        {
                            buttonsPlayer[x, y].BackColor = Color.Blue; // Промах
                        }
                    }
                    // Смена хода
                    isPlayerTurn = true;
                    UpdateTurnDisplay();
                    break;

                    
            }
        }
        private void GameFromOnline_Load(object sender, EventArgs e)
        {
            UpdateTurnDisplay();
        }
        private void UpdateTurnDisplay()
        {
            // Обновление отображения текущего хода
            this.Invoke((Action)(() =>
            {
                if (isPlayerTurn)
                {
                    this.Text = $"{player.Name}, Ваш ход!";
                    EnableEnemyBoard(true);
                }
                else
                {
                    this.Text = $"{enemy.Name}, Ожидание хода противника...";
                    EnableEnemyBoard(false);
                }
            }));
        }

        private void EnableEnemyBoard(bool enable)
        {
            // Включение или отключение доски противника
            foreach (var button in buttonsEnemy)
            {
                button.Enabled = enable;
            }
        }
    }
}


