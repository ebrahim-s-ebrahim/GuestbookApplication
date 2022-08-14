using GuestbookApplication.Models;

namespace GuestbookApplication.ViewModels
{
    public class ReplyViewModel
    {
        public string MessageContent { get; set; }
        public int UserId { get; set; }
        public List<MessageViewModel> Children { get; set; }
    }
}
