using Microsoft.Extensions.Options;
namespace Infrastructure.Services.Auth.Sender;

public class AuthMessageSender(IOptions<SmsSettings> optionsSmsAccessor, IOptions<EmailSettings> optionsEmailAccessor)
    : IEmailSender, ISmsSender {
    public SmsSettings SmsSettings { get; } = optionsSmsAccessor.Value; // set only via Secret Manager
    public EmailSettings EmailSettings { get; } = optionsEmailAccessor.Value; // set only via Secret Manager

    public Task SendEmailAsync(string email, string subject, string message) {
        // Plug in your email service here to send an email.
        return Task.FromResult(0);
    }

    public Task SendSmsAsync(string number, string message) {
        return Task.FromResult(0);
        // // Plug in your SMS service here to send a text message.
        // // Your Controllers SID from twilio.com/console
        // var accountSid = Options.SMSAccountIdentification;
        // // Your Auth Token from twilio.com/console
        // var authToken = Options.SMSAccountPassword;
        //
        // TwilioClient.Init(accountSid, authToken);
        //
        // return MessageResource.CreateAsync(
        //     messagingServiceSid: Options.SMSAccountMessagingServiceSid,
        //     to: new PhoneNumber(number),
        //     from: new PhoneNumber(Options.SMSAccountFrom),
        //     body: message);
    }
}