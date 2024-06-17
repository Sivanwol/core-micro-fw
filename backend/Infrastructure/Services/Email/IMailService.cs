using Application.Contract;
namespace Infrastructure.Services.Email;

public interface IMailService {
    Task<bool> SendAsync(MailData mailData, CancellationToken ct);
    Task<bool> SendWithAttachmentsAsync(MailDataWithAttachments mailData, CancellationToken ct);
}