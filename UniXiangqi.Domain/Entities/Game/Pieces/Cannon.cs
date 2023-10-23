namespace UniXiangqi.Domain.Entities.Game.Pieces
{
    public class Cannon : BasePiece
    {
        public override List<ChessPosition> CanPieceMoves(int row, int col, Board chessPieces)
        {
            List<ChessPosition> listOfPossibleMoves = new List<ChessPosition>();
            ChessPiece selectPiece = chessPieces.GetInfoAtPosition(row, col);

            // Kiểm tra các 4 hướng (lên, xuống, trái, phải) từ vị trí (row, col)
            int[][] possibleMoves =
            {
                new int[] { -1, 0 }, // Lên
                new int[] { 1, 0 },  // Xuống
                new int[] { 0, -1 }, // Trái
                new int[] { 0, 1 }   // Phải
            };

            foreach (int[] move in possibleMoves)
            {
                bool hasCannonInBetween = false; // Đánh dấu có quả pháo ở giữa

                for (int step = 1; step < 10; step++)
                {
                    int newRow = row + move[0] * step;
                    int newCol = col + move[1] * step;

                    // Kiểm tra xem nếu ô (newRow, newCol) nằm trong biên của bàn cờ
                    if (newRow >= 0 && newRow < 10 && newCol >= 0 && newCol < 9)
                    {
                        ChessPiece targetPiece = chessPieces.GetInfoAtPosition(newRow, newCol);
                        if (targetPiece.Name == "0")
                        {
                            // Nếu đang trong quá trình di chuyển qua một ô trống
                            if (!hasCannonInBetween)
                            {
                                listOfPossibleMoves.Add(new ChessPosition(newRow, newCol));
                            }
                        }
                        else
                        {
                            // Nếu gặp quân địch, kiểm tra nếu có quả pháo ở giữa
                            if (hasCannonInBetween)
                            {
                                if (chessPieces.GetInfoAtPosition(newRow, newCol).Name != "0" && targetPiece.Side != selectPiece.Side)
                                {
                                    listOfPossibleMoves.Add(new ChessPosition(newRow, newCol));
                                    break; // Dừng kiểm tra khi tìm thấy quân địch
                                }
                            }
                            else
                            {
                                hasCannonInBetween = true; // Đánh dấu có quả pháo ở giữa
                            }
                        }
                    }
                    else
                    {
                        // Nếu vượt ra khỏi biên của bàn cờ, dừng kiểm tra
                        break;
                    }
                }
            }
            return listOfPossibleMoves;
        }
    }
}
