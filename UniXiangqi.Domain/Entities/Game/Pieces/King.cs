namespace UniXiangqi.Domain.Entities.Game.Pieces
{
    public class King : BasePiece
    {
        public override List<ChessPosition> CanPieceMoves(int row, int col, Board chessPieces)
        {
            List<ChessPosition> listOfPossibleMoves = new List<ChessPosition>();
            ChessPiece selectPiece = chessPieces.GetInfoAtPosition(row, col);
            int[][] possibleMoves =
            {
                new int[] { -1, 0 }, // Lên
                new int[] { 1, 0 }, // Xuống
                new int[] { 0, -1 }, // Trái
                new int[] { 0, 1 }, // Phải
            };

            foreach (int[] move in possibleMoves)
            {
                int newRow = row + move[0];
                int newCol = col + move[1];

                if (
                    (selectPiece.Side == "R" && newRow >= 0 && newRow <= 2 && newCol >= 3 && newCol <= 5) || // Cung đỏ
                    (selectPiece.Side == "B" && newRow >= 7 && newRow <= 9 && newCol >= 3 && newCol <= 5) // Cung đen
                )
                {
                    ChessPiece targetPiece = chessPieces.GetInfoAtPosition(newRow, newCol);
                    // TODO: Lấy vị trí tướng địch và kiểm tra không để đối mặt nhau
                    if (
                        chessPieces.GetInfoAtPosition(newRow, newCol).Name == "0" ||
                        (targetPiece.Side != selectPiece.Side &&
                        targetPiece.Type != "V" &&
                        newCol != col)
                    )
                    {
                        listOfPossibleMoves.Add(new ChessPosition(newRow, newCol));
                    }
                }
            }

            return listOfPossibleMoves;
        }
    }
}
