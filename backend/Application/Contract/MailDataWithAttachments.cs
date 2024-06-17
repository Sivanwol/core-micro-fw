using Microsoft.AspNetCore.Http;
namespace Application.Contract;

public class MailDataWithAttachments : MailData {
    public MailDataWithAttachments(List<string> to, string subject, string? body = null, IFormFileCollection? attachments = null, string? from = null, string? displayName = null,
        string? replyTo = null, string? replyToName = null, List<string>? bcc = null, List<string>? cc = null) :
        base(to, subject, body, from, displayName, replyTo, replyToName, bcc, cc) {
        Attachments = attachments;
    }
    public IFormFileCollection? Attachments { get; set; }
}