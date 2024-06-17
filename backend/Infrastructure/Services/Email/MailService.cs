using Application.Configs;
using Application.Configs.Providers;
using Application.Contract;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Newtonsoft.Json;
using Serilog;
namespace Infrastructure.Services.Email;

public class MailService : IMailService {
    private readonly BackendApplicationConfig _backendApplicationConfig;

    private readonly MailServerConfig _settings;

    public MailService(IOptions<MailServerConfig> mailServerConfigOptions, BackendApplicationConfig backendApplicationConfig) {
        Log.Information($"MailService: {mailServerConfigOptions.Value}");
        _settings = mailServerConfigOptions.Value;
        _backendApplicationConfig = backendApplicationConfig;
    }

    public async Task<bool> SendWithAttachmentsAsync(MailDataWithAttachments mailData, CancellationToken ct = default) {
        try {
            var jsonString = JsonConvert.SerializeObject(mailData);
            Log.Information($"MailDataWithAttachments: {jsonString}");
            jsonString = JsonConvert.SerializeObject(_settings);
            Log.Information($"Mail Settings: {jsonString}");
            // Initialize a new instance of the MimeKit.MimeMessage class
            var mail = new MimeMessage();

            #region Sender / Receiver

            // Sender
            mail.From.Add(new MailboxAddress(_settings.DisplayName, mailData.From ?? _settings.FromAddress));
            mail.Sender = new MailboxAddress(mailData.DisplayName ?? _settings.DisplayName, mailData.From ?? _settings.FromAddress);

            // Receiver
            foreach (var mailAddress in mailData.To)
                mail.To.Add(MailboxAddress.Parse(mailAddress));

            // Set Reply to if specified in mail data
            if (!string.IsNullOrEmpty(mailData.ReplyTo))
                mail.ReplyTo.Add(new MailboxAddress(mailData.ReplyToName, mailData.ReplyTo));

            // BCC
            // Check if a BCC was supplied in the request
            if (mailData.Bcc != null) {
                // Get only addresses where value is not null or with whitespace. x = value of address
                foreach (var mailAddress in mailData.Bcc.Where(x => !string.IsNullOrWhiteSpace(x)))
                    mail.Bcc.Add(MailboxAddress.Parse(mailAddress.Trim()));
            }

            // CC
            // Check if a CC address was supplied in the request
            if (mailData.Cc != null) {
                foreach (var mailAddress in mailData.Cc.Where(x => !string.IsNullOrWhiteSpace(x)))
                    mail.Cc.Add(MailboxAddress.Parse(mailAddress.Trim()));
            }

            #endregion

            #region Content

            // Add Content to Mime Message
            var body = new BodyBuilder();
            mail.Subject = mailData.Subject;

            // Check if we got any attachments and add the to the builder for our message
            if (mailData.Attachments != null) {
                byte[] attachmentFileByteArray;
                foreach (var attachment in mailData.Attachments) {
                    if (attachment.Length > 0) {
                        using (var memoryStream = new MemoryStream()) {
                            attachment.CopyTo(memoryStream);
                            attachmentFileByteArray = memoryStream.ToArray();
                        }
                        body.Attachments.Add(attachment.FileName, attachmentFileByteArray, ContentType.Parse(attachment.ContentType));
                    }
                }
            }
            body.HtmlBody = mailData.Body;
            mail.Body = body.ToMessageBody();

            #endregion

            #region Send Mail

            using var smtp = new SmtpClient();


            if (_settings.IsUseSsl) {
                await smtp.ConnectAsync(_settings.ServerAddress, _settings.Port, SecureSocketOptions.SslOnConnect, ct);
            } else if (_settings.IsStartTls) {
                await smtp.ConnectAsync(_settings.ServerAddress, _settings.Port, SecureSocketOptions.StartTls, ct);
            }

            if (_settings.IsUseCredentials) {
                await smtp.AuthenticateAsync(_backendApplicationConfig.EMailUser, _backendApplicationConfig.EMailPassword, ct);
            }
            await smtp.SendAsync(mail, ct);
            await smtp.DisconnectAsync(true, ct);

            #endregion

        }
        catch (Exception e) {
            Log.Error(e, $"MailService: {e.Message}");
            return false;
        }
        return true;
    }

    public async Task<bool> SendAsync(MailData mailData, CancellationToken ct = default) {
        try {
            var jsonString = JsonConvert.SerializeObject(mailData);
            Log.Information($"MailData: {jsonString}");
            jsonString = JsonConvert.SerializeObject(_settings);
            Log.Information($"Mail Settings: {jsonString}");

            // Initialize a new instance of the MimeKit.MimeMessage class
            var mail = new MimeMessage();

            #region Sender / Receiver

            // Sender
            mail.From.Add(new MailboxAddress(_settings.DisplayName, mailData.From ?? _settings.FromAddress));
            mail.Sender = new MailboxAddress(mailData.DisplayName ?? _settings.DisplayName, mailData.From ?? _settings.FromAddress);

            // Receiver
            foreach (var mailAddress in mailData.To)
                mail.To.Add(MailboxAddress.Parse(mailAddress));

            // Set Reply to if specified in mail data
            if (!string.IsNullOrEmpty(mailData.ReplyTo))
                mail.ReplyTo.Add(new MailboxAddress(mailData.ReplyToName, mailData.ReplyTo));

            // BCC
            // Check if a BCC was supplied in the request
            if (mailData.Bcc != null) {
                // Get only addresses where value is not null or with whitespace. x = value of address
                foreach (var mailAddress in mailData.Bcc.Where(x => !string.IsNullOrWhiteSpace(x)))
                    mail.Bcc.Add(MailboxAddress.Parse(mailAddress.Trim()));
            }

            // CC
            // Check if a CC address was supplied in the request
            if (mailData.Cc != null) {
                foreach (var mailAddress in mailData.Cc.Where(x => !string.IsNullOrWhiteSpace(x)))
                    mail.Cc.Add(MailboxAddress.Parse(mailAddress.Trim()));
            }

            #endregion

            #region Content

            // Add Content to Mime Message
            var body = new BodyBuilder();
            mail.Subject = mailData.Subject;
            body.HtmlBody = mailData.Body;
            mail.Body = body.ToMessageBody();

            #endregion

            #region Send Mail

            using var smtp = new SmtpClient();

            if (_settings.IsUseSsl) {
                await smtp.ConnectAsync(_settings.ServerAddress, _settings.Port, SecureSocketOptions.SslOnConnect, ct);
            } else if (_settings.IsStartTls) {
                await smtp.ConnectAsync(_settings.ServerAddress, _settings.Port, SecureSocketOptions.StartTls, ct);
            } else {
                await smtp.ConnectAsync(_settings.ServerAddress, _settings.Port, SecureSocketOptions.None, ct);
            }

            if (_settings.IsUseCredentials) {
                await smtp.AuthenticateAsync(_settings.Username, _settings.Password, ct);
            }
            await smtp.SendAsync(mail, ct);
            await smtp.DisconnectAsync(true, ct);

            #endregion

        }
        catch (Exception e) {
            Log.Error(e, $"MailService: {e.Message}");
            return false;
        }
        return true;
    }
}