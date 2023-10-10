namespace Infrastructure.Services.Auth.Sender;

public interface IEmailSender {
    Task SendEmailAsync(string email, string subject, string message);
}