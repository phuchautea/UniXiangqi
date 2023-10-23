namespace UniXiangqi.Domain.Entities.Game.Pieces
{
    public class Horse : BasePiece
    {
        public override List<ChessPosition> CanPieceMoves(int row, int col, Board chessPieces)
        {
            List<ChessPosition> listOfPossibleMoves = new List<ChessPosition>();

            // Kiểm tra các 8 hướng chữ L có thể đi từ vị trí (row, col)
            int[][] possibleMoves =
            {
                new int[] { -1, -2 },
                new int[] { -2, -1 },
                new int[] { -2, 1 },
                new int[] { -1, 2 },
                new int[] { 1, -2 },
                new int[] { 2, -1 },
                new int[] { 2, 1 },
                new int[] { 1, 2 }
            };

            ChessPiece selectPiece = chessPieces.GetInfoAtPosition(row, col);

            foreach (int[] move in possibleMoves)
            {
                int newRow = row + move[0];
                int newCol = col + move[1];

                // Kiểm tra xem nếu ô (newRow, newCol) nằm trong biên của bàn cờ
                if (newRow >= 0 && newRow < 10 && newCol >= 0 && newCol < 9)
                {
                    // Kiểm tra xem ô (newRow, newCol) có quân cờ hoặc là ô trống
                    ChessPiece targetPiece = chessPieces.GetInfoAtPosition(newRow, newCol);

                    // Kiểm tra các ô trung gian trên đường đi từ (row, col) đến (newRow, newCol)
                    bool isPathClear = true;
                    if (Math.Abs(newRow - row) == 2 && Math.Abs(newCol - col) == 1)
                    {
                        // Di chuyển theo đường chéo, kiểm tra theo chiều dọc
                        int midRow = (row + newRow) / 2;
                        if (chessPieces.GetInfoAtPosition(midRow, col).Name != "0")
                        {
                            isPathClear = false;
                        }
                    }
                    else if (Math.Abs(newRow - row) == 1 && Math.Abs(newCol - col) == 2)
                    {
                        // Di chuyển theo đường chéo, kiểm tra theo chiều ngang
                        int midCol = (col + newCol) / 2;
                        if (chessPieces.GetInfoAtPosition(row, midCol).Name != "0")
                        {
                            isPathClear = false;
                        }
                    }

                    if (isPathClear)
                    {
                        if (chessPieces.GetInfoAtPosition(newRow, newCol).Name == "0" || (chessPieces.GetInfoAtPosition(newRow, newCol).Name != "0" && targetPiece.Side != selectPiece.Side))
                        {
                            listOfPossibleMoves.Add(new ChessPosition(newRow, newCol));
                        }
                    }
                }
            }

            return listOfPossibleMoves;
        }
    }
}
