using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace BattleShip
{
    public partial class PlaceForm : Form
    {
        private const int cellSize = 30; // Размер одной ячейки
        private const int gridSize = 10; // Размер сетки 10x10

        private Player player;

        private Button[,] buttonsPlayer;
        private Label draggedShip;
        private Point initialShipLocation;
        private bool isDragging = false;

        private Socket clientSocket;
        private bool isHorizontal = true; // Переменная для хранения ориентации корабля

        private Dictionary<int, int> shipCounts = new Dictionary<int, int>
        {
            { 1, 4 },
            { 2, 3 },
            { 3, 2 },
            { 4, 1 }
        };

        public PlaceForm(Player player)
        {
            this.player = player;
            InitializeComponent();

            InitializeBoard();
            InitializeShips();
        }

        public PlaceForm(Player player, Socket socket)
        {
            this.clientSocket = socket;
            this.player = player;
            InitializeComponent();

            InitializeBoard();
            InitializeShips();
        }

        private void InitializeBoard()
        {
            buttonsPlayer = new Button[gridSize, gridSize];

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    buttonsPlayer[x, y] = new Button
                    {
                        Width = cellSize,
                        Height = cellSize,
                        Location = new Point(x * cellSize + 20, y * cellSize + 40),
                        Tag = new Point(x, y),
                        BackColor = Color.LightGray
                    };
                    this.Controls.Add(buttonsPlayer[x, y]);
                }
            }
        }

        private void InitializeShips()
        {
            Label[] ships = { label6, label7, label8, label9 };
            int[] shipLengths = { 4, 3, 2, 1 };
            for (int i = 0; i < ships.Length; i++)
            {
                ships[i].Tag = new Ship(new Position(-1, -1), shipLengths[i], true);
                ships[i].MouseDown += new MouseEventHandler(Ship_MouseDown);
                ships[i].MouseMove += new MouseEventHandler(Ship_MouseMove);
                ships[i].MouseUp += new MouseEventHandler(Ship_MouseUp);
            }
        }

        private void Ship_MouseDown(object sender, MouseEventArgs e)
        {
            draggedShip = sender as Label;
            initialShipLocation = draggedShip.Location;
            isDragging = true;
        }

        private void Ship_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                draggedShip.Left = e.X + draggedShip.Left - draggedShip.Width / 2;
                draggedShip.Top = e.Y + draggedShip.Top - draggedShip.Height / 2;
            }
        }

        private void Ship_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
            Point newLocation = draggedShip.Location;
            Point? cell = GetCellFromLocation(newLocation);

            if (cell.HasValue)
            {
                Point cellPoint = cell.Value;
                Ship ship = draggedShip.Tag as Ship;
                ship.Position = new Position(cellPoint.X, cellPoint.Y);
                ship.IsHorizontal = isHorizontal;

                if (player.Board.PlaceShip(ship))
                {
                    if (shipCounts[ship.Length] > 0)
                    {
                        shipCounts[ship.Length]--;
                        UpdatePlacementBoard();
                        if (AllShipsPlaced())
                        {
                            MessageBox.Show("Все корабли размещены. Готовы к игре!");
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Вы уже разместили все корабли длиной {ship.Length}.");
                        player.Board.RemoveShip(ship);
                    }
                }
                else
                {
                    MessageBox.Show("Неверное размещение корабля. Попробуйте снова.");
                    draggedShip.Location = initialShipLocation; // Вернуть на исходную позицию
                }
            }
            else
            {
                draggedShip.Location = initialShipLocation; // Вернуть на исходную позицию
            }
            draggedShip.Location = initialShipLocation;
        }

        private bool AllShipsPlaced()
        {
            foreach (var count in shipCounts.Values)
            {
                if (count > 0) return false;
            }
            return true;
        }

        private Point? GetCellFromLocation(Point location)
        {
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    Rectangle cellRect = new Rectangle(buttonsPlayer[x, y].Location, buttonsPlayer[x, y].Size);
                    if (cellRect.Contains(location))
                    {
                        return new Point(x, y);
                    }
                }
            }
            return null;
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (AllShipsPlaced())
            {
                if (clientSocket == null)
                {
                    GameFormWithPC gameForm = new GameFormWithPC(player, buttonsPlayer);
                    gameForm.Show();
                    this.Hide();
                }
                else
                {
                    GameFromOnlineClient gameForm = new GameFromOnlineClient(player, buttonsPlayer, clientSocket);
                    gameForm.Show();
                    this.Hide();
                    SendBoard();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, разместите все корабли перед началом игры.");
            }
        }

        public void SendBoard()
        {
            StringBuilder dataBuilder = new StringBuilder();
            dataBuilder.Append("PLACE:");

            foreach (Cell cell in player.Board.Cells)
            {
                if (cell.IsOccupied)
                {
                    dataBuilder.Append($"{cell.Position.X},{cell.Position.Y}|");
                }
            }

            SendDataToServer(dataBuilder.ToString());
        }

        private void SendDataToServer(string data)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            clientSocket.Send(buffer);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            player.Board.Clear();
            
            UpdatePlacementBoard();
        }
    }
}
