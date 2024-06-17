using System.Security;
using Microsoft.Extensions.Logging;
namespace Infrastructure.Utils.SMS.Providers;

public class BaseSMSProvider {
    protected string _code;
    protected string _countryNumber;
    protected ILogger _logger;
    protected string _messageTemplate;
    protected string _phoneNumber;
    protected string _timeToSend;
    public BaseSMSProvider(ILoggerFactory logger) {
        _logger = logger.CreateLogger<BaseSMSProvider>();
    }
    public virtual void Init(string countryNumber, string phoneNumber, string messageTemplate) {
        _logger.LogInformation("BaseSMSProvider init");
        _messageTemplate = SecurityElement.Escape(messageTemplate);
        _countryNumber = countryNumber;
        _phoneNumber = phoneNumber;
    }
    public virtual void SendSMS(string code) {
        _logger.LogInformation("BaseSMSProvider SendSMS");
        _timeToSend = DateTime.Now.ToString();
    }
}