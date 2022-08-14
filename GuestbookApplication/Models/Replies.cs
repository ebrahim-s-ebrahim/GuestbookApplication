namespace GuestbookApplication.Models
{
    public class Replies
    {
        public int ParentMsgId { get; set; }
        public int ChildMsgId { get; set; }
        public virtual ICollection<Message> ParentMessages { get; set; }
        public virtual ICollection<Message> ChildMessages { get; set; }
    }
}
