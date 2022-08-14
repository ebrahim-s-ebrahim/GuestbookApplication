namespace GuestbookApplication.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public string MessageContent { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Replies> Replies { get; set; }
    }
}
