namespace UniXiangqi.Domain.Entities.Game.Pieces
{
    public class Chariot : BasePiece
    {
        public override List<ChessPosition> CanPieceMoves(int row, int col, Board chessPieces)
        {
            List<ChessPosition> listOfPossibleMoves = new List<ChessPosition>();
            ChessPiece selectPiece = chessPieces.GetInfoAtPosition(row, col);

            // Kiểm tra các hướng di chuyển của con xe (theo chiều dọc và chiều ngang)
            int[][] possibleMoves =
            {
                new int[] { -1, 0 }, // Lên
                new int[] { 1, 0 },  // Xuống
                new int[] { 0, -1 }, // Trái
                new int[] { 0, 1 }   // Phải
            };

            foreach (int[] move in possibleMoves)
            {
                int newRow = row + move[0];
                int newCol = col + move[1];
                while (newRow >= 0 && newRow < 10 && newCol >= 0 && newCol < 9)
                {
                    ChessPiece targetPiece = chessPieces.GetInfoAtPosition(newRow, newCol);

                    if (targetPiece.Name == "0")
                    {
                        listOfPossibleMoves.Add(new ChessPosition(newRow, newCol));
                        newRow += move[0];
                        newCol += move[1];
                    }
                    else if (targetPiece.Name != "0" && targetPiece.Side != selectPiece.Side)
                    {
                        listOfPossibleMoves.Add(new ChessPosition(newRow, newCol));
                        newRow += move[0];
                        newCol += move[1];
                        break;
                    }
                    else
                    {
                        // Ô đích có quân cờ, dừng lại
                        break;
                    }
                }
            }

            return listOfPossibleMoves;
        }
    }
}
