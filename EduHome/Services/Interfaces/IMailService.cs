using EduHome.DataTransferObjects;

namespace EduHome.Services.Interfaces;

public interface IMailService
{
	Task SendEmailAsync(MailRequest mailRequest);
}