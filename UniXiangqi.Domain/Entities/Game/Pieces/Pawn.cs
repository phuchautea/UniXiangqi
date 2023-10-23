namespace UniXiangqi.Domain.Entities.Game.Pieces
{
    public class Pawn : BasePiece
    {
        public override List<ChessPosition> CanPieceMoves(int row, int col, Board chessPieces)
        {
            List<ChessPosition> listOfPossibleMoves = new List<ChessPosition>();
            ChessPiece selectPiece = chessPieces.GetInfoAtPosition(row, col);

            List<int[]> possibleMoves = new List<int[]>();

            if (selectPiece.Side == "B") // Quân cờ đen
            {
                possibleMoves.Add(new int[] { -1, 0 }); // Di chuyển lên

                if (row <= 4)
                {
                    // Nếu quân đen đang ở phía trên sông, có thêm nước đi sang trái và sang phải
                    possibleMoves.Add(new int[] { 0, 1 });
                    possibleMoves.Add(new int[] { 0, -1 });
                }
            }
            else if (selectPiece.Side == "R") // Quân cờ đỏ
            {
                possibleMoves.Add(new int[] { 1, 0 }); // Di chuyển xuống

                if (row >= 5)
                {
                    // Nếu quân đỏ đang ở phía dưới sông, có thêm nước đi sang trái và sang phải
                    possibleMoves.Add(new int[] { 0, 1 });
                    possibleMoves.Add(new int[] { 0, -1 });
                }
            }

            foreach (int[] move in possibleMoves)
            {
                int newRow = row + move[0];
                int newCol = col + move[1];

                if (newRow >= 0 && newRow < 10 && newCol >= 0 && newCol < 9)
                {
                    ChessPiece targetPiece = chessPieces.GetInfoAtPosition(newRow, newCol);

                    if (chessPieces.GetInfoAtPosition(newRow, newCol).Name == "0" || (chessPieces.GetInfoAtPosition(newRow, newCol).Name != "0" && targetPiece.Side != selectPiece.Side))
                    {
                        listOfPossibleMoves.Add(new ChessPosition(newRow, newCol));
                        Console.WriteLine($"Quân 'tốt' có thể đi đến [{newRow}, {newCol}]");
                    }
                }
            }
            return listOfPossibleMoves;
        }
    }
}
