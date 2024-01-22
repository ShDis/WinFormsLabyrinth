namespace WinFormsLabyrinth
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            StartLabyrinth();
        }

        public void StartLabyrinth()
        {
            // очищение старых пикселей
            if (display != null)
            {
                foreach (Control control in display)
                {
                    if (control != null)
                    {
                        control.Dispose();
                    }
                }
            }
            // очищение старого игрока
            if (player != null)
            {
                player.Dispose();
            }

            // на случай дальнейшей модификации со сменой количества строк / столбцов
            int labyrinthRows = 10;
            int labyrinthColumns = 10;

            // создание лабиринта
            Labyrinth labyrinth = new Labyrinth(labyrinthRows, labyrinthColumns);

            // визуализация
            display = labyrinth.GetLabyrinth(blockPixelSize);
            foreach (var i in display)
            {
                this.Controls.Add(i);
            }

            // помещение игрока
            player = new Label()
            {
                BackColor = Color.Brown,
                Size = new Size(blockPixelSize, blockPixelSize),
                Location = new Point((display.GetLength(1) - 2) * blockPixelSize, 1 * blockPixelSize)
            };
            this.Controls.Add(player);
            player.BringToFront();
        }

        public Label[,] display; // клетки проходов и стен
        public Label player; // игрок
        public int blockPixelSize = 30; // размер элемента в пикселях

        private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (player == null)
                return;
            // передвижение
            switch (e.KeyCode)
            {
                case Keys.Left:
                    if (display[player.Location.Y / blockPixelSize, player.Location.X / blockPixelSize - 1].BackColor != Color.Black)
                        player.Location = new Point(player.Location.X - blockPixelSize, player.Location.Y);
                    break;
                case Keys.Right:
                    if (display[player.Location.Y / blockPixelSize, player.Location.X / blockPixelSize + 1].BackColor != Color.Black)
                        player.Location = new Point(player.Location.X + blockPixelSize, player.Location.Y);
                    break;
                case Keys.Up:
                    if (display[player.Location.Y / blockPixelSize - 1, player.Location.X / blockPixelSize].BackColor != Color.Black)
                        player.Location = new Point(player.Location.X, player.Location.Y - blockPixelSize);
                    break;
                case Keys.Down:
                    if (display[player.Location.Y / blockPixelSize + 1, player.Location.X / blockPixelSize].BackColor != Color.Black)
                        player.Location = new Point(player.Location.X, player.Location.Y + blockPixelSize);
                    break;
            }
            // перегенерация при прохождении
            if (display[player.Location.Y / blockPixelSize, player.Location.X / blockPixelSize].BackColor == Color.Green)
            {
                StartLabyrinth();
            }
        }
    }
}
