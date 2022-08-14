namespace GuestbookApplication.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public int? ParentMessageId { get; set; }
        public string MessageContent { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual Message ParentMessage { get; set; }
        public virtual ICollection<Message> Children { get; set; }

    }
}
