using Application.Configs.Providers;
using Infrastructure.Enums;
using Infrastructure.Utils.SMS.Providers;
using Microsoft.Extensions.Logging;
namespace Infrastructure.Utils.SMS;

public class SMSSender {
    private static BaseSMSProvider _provider;
    private static SMSSender _instance;
    private static ILoggerFactory _loggerFaactory;
    private static InfoUSmsConfig _config;
    private static ILogger _logger;

    public static SMSSender Instance
    {
        get { return _instance ??= new SMSSender(); }
    }

    public void Load(ILoggerFactory logger, InfoUSmsConfig config) {
        _logger = logger.CreateLogger<SMSSender>();
        _loggerFaactory = logger;
        _config = config;
    }
    public int GetSMSExpireInMinutes() {
        return _config.ExpiredInMinutes;
    }

    public void Init(SMSProviders provider, string countryNumber, string phoneNumber, string messageTemplate) {
        if (!_config.IsEnabled) throw new Exception("SMS is not enabled");
        switch (provider) {
            case SMSProviders.INFOU:
                _provider = new InfoUProvider(_loggerFaactory, _config);
                break;
        }
        _provider.Init(countryNumber, phoneNumber, messageTemplate);
        _logger.LogInformation($"Init: {provider}, {countryNumber}, {phoneNumber}, {messageTemplate}");
    }
    public void SendSMS(string code) {
        if (!_config.IsEnabled) throw new Exception("SMS is not enabled");
        _provider.SendSMS(code);
        _logger.LogInformation($"SendSMS: {code}");
    }
}