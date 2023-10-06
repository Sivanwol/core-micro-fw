namespace Infrastructure.Services.Auth.Sender;

public class SmsSettings {
    public string AccountIdentification { get; set; }
    public string AccountMessagingServiceSid { get; set; }
    public string AccountAuthToken { get; set; }
    public string AccountFrom { get; set; }
}