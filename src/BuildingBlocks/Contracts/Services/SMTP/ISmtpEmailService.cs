using Shared.Services.Email;

namespace Contracts.Services.SMTP;

public interface ISmtpEmailService : IEmailService<MailRequest>
{

}
