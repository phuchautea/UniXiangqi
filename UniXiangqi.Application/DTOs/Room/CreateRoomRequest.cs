namespace UniXiangqi.Application.DTOs.Room
{
    public class CreateRoomRequest
    {
        public int GameTimer { get; set; }
        public int MoveTimer { get; set; }
        public string HostUserId { get; set; } = string.Empty;
        public string HostSide { get; set; } = string.Empty;
        public bool IsRated { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}
