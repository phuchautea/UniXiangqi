using System.Collections.Generic;

namespace UniXiangqi.Domain.Entities.Game
{
    public abstract class BasePiece
    {
        public virtual List<ChessPosition> CanPieceMoves(int row, int col, Board board)
        {
            return new List<ChessPosition>();
        }
        public static bool IsRedSide(string side)
        {
            return side == "R" ? true : false;
        }
        public static bool IsRiver(int row, bool isRedSide)
        {
            // Hàng sông đối với bên đỏ (isRedSide = true) là 5-6
            // Hàng sông đối với bên đen (isRedSide = false) là 4-5
            return (
                (isRedSide && row >= 5 && row <= 6) ||
                (!isRedSide && row >= 4 && row <= 5)
            );
        }
    }
}
