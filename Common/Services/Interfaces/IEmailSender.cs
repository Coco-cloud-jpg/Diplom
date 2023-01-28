using Common.Emails;

namespace Common.Services.Interfaces
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
    }
}
