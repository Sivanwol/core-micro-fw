namespace Application.Configs;

public class BackendApplicationConfig : BaseConfig {
    public bool DisableHealthCheck { get; set; }
    public string AzureServiceBus { get; set; }
    public string AzureServiceBusUri { get; set; }
    public string JwtIssuer { get; set; }
    public string JwtAudience { get; set; }
    public string JwtSecret { get; set; }
    public int JwtAccessTokenExpired { get; set; }
    public int JwtRefreshTokenExpired { get; set; }
}