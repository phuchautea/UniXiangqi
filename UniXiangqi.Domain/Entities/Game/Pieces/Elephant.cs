namespace UniXiangqi.Domain.Entities.Game.Pieces
{
    public class Elephant : BasePiece
    {
        public override List<ChessPosition> CanPieceMoves(int row, int col, Board chessPieces)
        {
            List<ChessPosition> listOfPossibleMoves = new List<ChessPosition>();

            // Kiểm tra các 4 hướng chéo có thể đi từ vị trí (row, col)
            int[][] possibleMoves =
            {
                new int[] { -2, -2 },
                new int[] { -2, 2 },
                new int[] { 2, -2 },
                new int[] { 2, 2 },
            };

            ChessPiece selectPiece = chessPieces.GetInfoAtPosition(row, col);

            foreach (int[] move in possibleMoves)
            {
                int newRow = row + move[0];
                int newCol = col + move[1];

                // Kiểm tra xem nếu ô (newRow, newCol) nằm trong biên của bàn cờ
                if (newRow >= 0 && newRow < 10 && newCol >= 0 && newCol < 9)
                {
                    ChessPiece targetPiece = chessPieces.GetInfoAtPosition(newRow, newCol);

                    // Kiểm tra xem con tượng có thể đi đến ô (newRow, newCol) hay không
                    // Con tượng di chuyển theo đường chéo, nên ta kiểm tra xem có quân cờ nào nằm trên đường đi không
                    bool isPathClear = true;
                    int rowDirection = newRow > row ? 1 : -1;
                    int colDirection = newCol > col ? 1 : -1;
                    int currentRow = row + rowDirection;
                    int currentCol = col + colDirection;

                    while (currentRow != newRow && currentCol != newCol)
                    {
                        // Kiểm tra xem con tượng có đi qua sông không
                        if (IsRiver(currentRow, IsRedSide(selectPiece.Side)))
                        {
                            isPathClear = false;
                            break;
                        }

                        if (chessPieces.GetInfoAtPosition(currentRow, currentCol).Name != "0")
                        {
                            isPathClear = false;
                            break;
                        }

                        currentRow += rowDirection;
                        currentCol += colDirection;
                    }

                    if (isPathClear)
                    {
                        if (chessPieces.GetInfoAtPosition(newRow, newCol).Name == "0" || (chessPieces.GetInfoAtPosition(newRow, newCol).Name != "0" && targetPiece.Side != selectPiece.Side))
                        {
                            // Quân tượng có thể đi đến ô (newRow, newCol)
                            listOfPossibleMoves.Add(new ChessPosition(newRow, newCol));
                        }
                    }
                }
            }

            return listOfPossibleMoves;
        }
    }
}
