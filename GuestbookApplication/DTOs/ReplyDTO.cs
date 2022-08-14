namespace GuestbookApplication.DTOs
{
    public class ReplyDTO
    {
        public int ParentMessageId { get; set; }
        public string MessageContent { get; set; }
        public int UserId { get; set; }
    }
}
