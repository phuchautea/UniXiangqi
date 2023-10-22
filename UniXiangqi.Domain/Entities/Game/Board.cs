using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniXiangqi.Domain.Entities.Game
{
    public class Board
    {
        private string[,] chessPieces;

        public Board(string boardData)
        {
            int width = 9;
            string[] values = boardData.Split(',');

            // Create a 2D array and initialize it
            chessPieces = new string[values.Length / width, width];

            // Populate the 2D array
            for (int i = 0; i < values.Length; i++)
            {
                int x = i % width;
                int y = i / width;
                chessPieces[y, x] = values[i];
            }
        }
        public string ToBoardData()
        {
            int rows = chessPieces.GetLength(0);
            int columns = chessPieces.GetLength(1);
            string[] values = new string[rows * columns];

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    int index = y * columns + x;
                    values[index] = chessPieces[y, x];
                }
            }

            return string.Join(",", values);
        }

        public string GetPieceAtPosition(int x, int y)
        {
            if (x < 0 || x >= chessPieces.GetLength(1) || y < 0 || y >= chessPieces.GetLength(0))
            {
                return null;
            }
            return chessPieces[x, y];
        }
    }

}
