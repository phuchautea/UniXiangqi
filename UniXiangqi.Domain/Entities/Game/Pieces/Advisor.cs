namespace UniXiangqi.Domain.Entities.Game.Pieces
{
    public class Advisor : BasePiece
    {
        public override List<ChessPosition> CanPieceMoves(int row, int col, Board chessPieces)
        {
            List<ChessPosition> listOfPossibleMoves = new List<ChessPosition>();
            ChessPiece selectPiece = chessPieces.GetInfoAtPosition(row, col);

            // Kiểm tra các hướng di chuyển có thể của quân Sĩ
            int[][] possibleMoves =
            {
                new int[] { -1, -1 }, // Di chuyển lên và sang trái
                new int[] { -1, 1 },  // Di chuyển lên và sang phải
                new int[] { 1, -1 },  // Di chuyển xuống và sang trái
                new int[] { 1, 1 },   // Di chuyển xuống và sang phải
            };

            foreach (int[] move in possibleMoves)
            {
                int newRow = row + move[0];
                int newCol = col + move[1];

                // Kiểm tra xem nếu ô (newRow, newCol) nằm trong biên của bàn cờ
                if (newRow >= 0 && newRow < 10 && newCol >= 0 && newCol < 9)
                {
                    ChessPiece targetPiece = chessPieces.GetInfoAtPosition(newRow, newCol);

                    // Kiểm tra các điều kiện khác tùy thuộc vào quy tắc của quân Sĩ trong cờ tướng
                    bool isMoveValid = false;

                    // Kiểm tra điều kiện con Sĩ chỉ đi được trong phạm vi
                    // Con Sĩ chỉ được nằm tại 3 dòng đầu hoặc 3 dòng cuối bàn cờ
                    // Và cột nằm trong khoảng từ 3 đến 5
                    if ((row < 3 && newRow < 3 && col >= 3 && col <= 5 && newCol >= 3 && newCol <= 5) ||
                        (row > 6 && newRow > 6 && col >= 3 && col <= 5 && newCol >= 3 && newCol <= 5))
                    {
                        isMoveValid = true;
                    }

                    if (isMoveValid)
                    {
                        if (chessPieces.GetInfoAtPosition(newRow, newCol).Name == "0" || (chessPieces.GetInfoAtPosition(newRow, newCol).Name != "0" && targetPiece.Side != selectPiece.Side))
                        {
                            // Quân 'Sĩ' có thể đi đến ô (newRow, newCol)
                            listOfPossibleMoves.Add(new ChessPosition(newRow, newCol));
                            Console.WriteLine($"Quan 'Si' co the di dến [{newRow}, {newCol}]");
                        }
                    }
                }
            }
            return listOfPossibleMoves;
        }
    }
}
