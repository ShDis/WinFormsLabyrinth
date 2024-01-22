using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsLabyrinth
{
    public class Labyrinth
    {
        public Labyrinth(int rows, int columns) 
        {
            this.rows = rows > 3 ? rows : 10;
            this.cols = cols > 3 ? columns : 10;
            // инициализация клеток
            grid = new Cell[this.rows, this.cols];
            for (int i = 0; i < this.rows; i++)
            {
                for (int j = 0; j < this.cols; j++)
                {
                    grid[i, j] = new Cell(i, j);
                }
            }
            // генерация
            this.Gen();
        }
        private int rows;
        public int Rows { get { return rows; } }
        private int cols;
        public int Cols { get { return cols; } }
        private Cell[,] grid;

        /// <summary>
        /// Получение визуализации лейблами через конвертацию поля клеток в поле клеток со стенами
        /// </summary>
        /// <param name="blockPixelSize">Размер элемента в пикселях</param>
        /// <returns>Таблица элементов с цветами</returns>
        public Label[,] GetLabyrinth(int blockPixelSize)
        {
            int blocksRow = rows * 2 + 1; // строки х2 + столько же +1 стен
            int blocksCol = cols * 2 + 1; // аналогично для колонок

            Label[,] outlables = new Label[blocksRow, blocksCol]; // табличка клеток проходов и стен

            // покраска клеток проходов и стен
            for (int i = 0; i < blocksRow; i++)
            {
                for (int j = 0; j < blocksCol; j++)
                {
                    // если условия ниже не пройдут - значит белый проход
                    Color currcolor = Color.White;
                    // боковые стенки
                    if (i == 0 || j == 0 || i == blocksRow - 1 || j == blocksCol - 1)
                        currcolor = Color.Black;
                    // угловые стенки
                    if (i % 2 == 0 && j % 2 == 0)
                        currcolor = Color.Black;
                    // северные стенки от клеток
                    if (i % 2 == 0 && j % 2 == 1 && i < blocksRow - 1 && i != 0)
                        if (!grid[(i + 1) / 2, j / 2].NorthConnected)
                            currcolor = Color.Black;
                    // восточные стенки от клеток
                    if (i % 2 == 1 && j % 2 == 0 && j != blocksCol - 1 && j > 1)
                        if (!grid[(i + 1) / 2 - 1, j / 2 - 1].EastConnected)
                            currcolor = Color.Black;
                    // финиш
                    if (i == blocksRow - 2 && j == 1)
                        currcolor = Color.Green;
                    // формирование квадратика    
                    Label label = new Label()
                    {
                        BackColor = currcolor,
                        Size = new Size(blockPixelSize, blockPixelSize),
                        Location = new Point(j * blockPixelSize, i * blockPixelSize)
                    };
                    outlables[i, j] = label;
                }
            }
            return outlables;
        }

        /// <summary>
        /// Генератор лабиринта
        /// </summary>
        public void Gen()
        {
            var rand = new Random((int)DateTime.Now.Ticks);

            for (int i = 0; i < rows; i++)
            {
                var run = new List<Cell>();

                for (int j = 0; j < cols; j++)
                {
                    run.Add(grid[i, j]);

                    var atEasternBoundary = j == cols - 1; // справа граница лабиринта
                    var atNorthernBoundary = i == 0; // сверху граница лабиринта
                    var shouldCloseOut = atEasternBoundary || (!atNorthernBoundary && rand.Next(2) == 0); // нужно ли соединение на север

                    if (shouldCloseOut)
                    {
                        var member = run[rand.Next(run.Count)];
                        if (member.Row - 1 != -1)
                            member.NorthConnected = true;
                        run.Clear();
                    }
                    else
                    {
                        grid[i, j].EastConnected = true;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Клетка
    /// </summary>
    public class Cell
    {
        public Cell(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
        private int row;
        public int Row { get => row; }
        private int col;
        public int Col { get => col; }
        /// <summary>
        /// Есть ли соединение на сервер
        /// </summary>
        public bool NorthConnected { get; set; } = false;
        /// <summary>
        /// Есть ли соединение на восток
        /// </summary>
        public bool EastConnected { get; set; } = false;
    }
}