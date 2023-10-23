namespace UniXiangqi.Domain.Entities.Game
{
    public class ChessPiece
    {
        public string Type { get; set; }
        public string Side { get; set; }
        public string Name { get; set; }

        public ChessPiece(string type, string side, string name)
        {
            Type = type;
            Side = side;
            Name = name;
        }
    }

}
