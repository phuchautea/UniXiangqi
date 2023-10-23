namespace UniXiangqi.Application.DTOs.Room
{
    public class GetAllRoomsResponse
    {
        public string Code { get; set; }
        public string HostUserName { get; set; }
        public string OpponentUserName { get; set; }
        public bool IsRated { get; set; }
        public int GameTimer { get; set; }
        public int MoveTimer { get; set; }
    }
}
