namespace Application.Configs;

public class BackendApplicationConfig : BaseConfig {
    public bool DisableHealthCheck { get; set; }
    public string AzureServiceBus { get; set; }
    public string AzureServiceBusUri { get; set; }
    public string ServiceName { get; set; }
    public bool MaintenanceMode { get; set; }
    public bool EnableCache { get; set; }
    public bool EnableGraphQLCache { get; set; }
    
    public string UploadFolder { get; set; }
    
    #region File Upload Configs
    
    public int MaxFileSize { get; set; }
    #endregion

    #region Redis Configs

    public bool RedisSsl { get; set; }
    public string RedisHost { get; set; }
    public string RedisAuth { get; set; }
    public int RedisPort { get; set; }
    public int RedisDb { get; set; }

    #endregion

    #region Email Configs

    public string EmailFrom { get; set; } = "system@wolberg.pro";
    public string EmailFromName { get; set; } = "Wolberg Pro";
    public string ShadowEmailBcc { get; set; } = "shadow@wolberg.pro";
    public string EMailPassword { get; set; }
    public string EMailUser { get; set; }

    #endregion

    #region RabbitMQ Configs

    public bool RabbitMqSslProtocol { get; set; }
    public string RabbitMqHost { get; set; }
    public string RabbitMqVirtualHost { get; set; }
    public string RabbitMqUsername { get; set; }
    public string RabbitMqPassword { get; set; }

    #endregion

    #region Auth Configs

    public int RegisterEmailConfirmationCodeExpiredInMinutes { get; set; } = 30;

    #region JWT Configs

    public string JwtIssuer { get; set; }
    public string JwtAudience { get; set; }
    public string JwtSecret { get; set; }
    public int JwtAccessTokenExpired { get; set; }
    public int JwtRefreshTokenExpired { get; set; }

    #endregion

    #region OTP General Configs

    public int OTPCodeExpiredInMinutes { get; set; }
    public int OTPCodeTotalRetriesWithinSession { get; set; }

    #endregion

    #endregion

    #region Infou Sms Provider Configs

    public string SMSProviderInfouToken { get; set; }
    public string SMSProviderInfouSender { get; set; }
    public string SMSProviderInfouUserName { get; set; }

    #endregion

}