namespace UniXiangqi.Domain.Entities.Game
{
    public class ChessPosition
    {
        public int Row { get; set; }
        public int Col { get; set; }

        public ChessPosition(int row, int col)
        {
            Row = row;
            Col = col;
        }
    }

}
