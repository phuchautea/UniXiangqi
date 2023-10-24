using UniXiangqi.Domain.Entities.Game.Pieces;

namespace UniXiangqi.Domain.Entities.Game
{
    public class Board
    {
        public string[,] chessPieces;
        public List<ChessPiece> infoPieces = new List<ChessPiece>();

        public Dictionary<string, BasePiece> pieces = new Dictionary<string, BasePiece>();


        public Board(string boardData)
        {
            pieces.Clear();
            LoadPieces();
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
            LoadInfoPieces();
        }
        public string[,] ToChessPieces(string boardData)
        {
            string[,] chessPieces;
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
            return chessPieces;
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

        public string GetNamePieceAtPosition(int x, int y)
        {
            if (x >= 0 && x < chessPieces.GetLength(0) && y >= 0 && y < chessPieces.GetLength(1))
                return chessPieces[x, y];
            return null;
        }
        public ChessPiece GetInfoAtPosition(int x, int y)
        {
            if (x >= 0 && x < chessPieces.GetLength(0) && y >= 0 && y < chessPieces.GetLength(1))
            {
                string pieceValue = chessPieces[x, y];
                if (pieceValue != "0")
                {
                    string side = pieceValue[0].ToString();
                    string type = pieceValue[1].ToString();
                    return infoPieces.FirstOrDefault(p => p.Side == side && p.Type == type);
                }
                return new ChessPiece(null, null, "0");
            }

            return null;
        }
        public void LoadPieces()
        {
            pieces.Add("X", new Chariot());
            pieces.Add("M", new Horse());
            pieces.Add("T", new Elephant());
            pieces.Add("S", new Advisor());
            pieces.Add("V", new King());
            pieces.Add("P", new Cannon());
            pieces.Add("C", new Pawn());
        }
        public void LoadInfoPieces()
        {
            for (int row = 0; row < chessPieces.GetLength(0); row++)
            {
                for (int col = 0; col < chessPieces.GetLength(1); col++)
                {
                    string pieceValue = GetNamePieceAtPosition(row, col);
                    if (pieceValue != null && pieceValue != "0")
                    {
                        string side = pieceValue[0].ToString();
                        string type = pieceValue[1].ToString();

                        bool existingPiece = infoPieces.Any(p => p.Side == side && p.Type == type);
                        if (!existingPiece)
                        {
                            ChessPiece chessPiece = new ChessPiece(type, side, pieceValue);
                            infoPieces.Add(chessPiece);
                        }
                    }
                }
            }
        }
    }

}
