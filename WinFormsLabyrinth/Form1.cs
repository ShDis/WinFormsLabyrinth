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
            // �������� ������ ��������
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
            // �������� ������� ������
            if (player != null)
            {
                player.Dispose();
            }

            // �� ������ ���������� ����������� �� ������ ���������� ����� / ��������
            int labyrinthRows = 10;
            int labyrinthColumns = 10;

            // �������� ���������
            Labyrinth labyrinth = new Labyrinth(labyrinthRows, labyrinthColumns);

            // ������������
            display = labyrinth.GetLabyrinth(blockPixelSize);
            foreach (var i in display)
            {
                this.Controls.Add(i);
            }

            // ��������� ������
            player = new Label()
            {
                BackColor = Color.Brown,
                Size = new Size(blockPixelSize, blockPixelSize),
                Location = new Point((display.GetLength(1) - 2) * blockPixelSize, 1 * blockPixelSize)
            };
            this.Controls.Add(player);
            player.BringToFront();
        }

        public Label[,] display; // ������ �������� � ����
        public Label player; // �����
        public int blockPixelSize = 30; // ������ �������� � ��������

        private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (player == null)
                return;
            // ������������
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
            // ������������� ��� �����������
            if (display[player.Location.Y / blockPixelSize, player.Location.X / blockPixelSize].BackColor == Color.Green)
            {
                StartLabyrinth();
            }
        }
    }
}
