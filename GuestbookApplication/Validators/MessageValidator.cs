using FluentValidation;
using GuestbookApplication.Models;

namespace GuestbookApplication.Validators
{
    public class MessageValidator : AbstractValidator<Message>
    {
        public MessageValidator()
        {
            RuleFor(message => message.MessageId).NotNull();
            RuleFor(message => message.MessageContent).NotEmpty();
        }
    }
}
