using DotEnv.Core;
namespace Application.Configs;

public class BackendSecretApplicationConfig {
    [EnvKey("JwtSecret")]
    public string JwtSecret { get; set; }

    [EnvKey("EMailPassword")]
    public string EMailPassword { get; set; }

    [EnvKey("EMailUser")]
    public string EMailUser { get; set; }

    [EnvKey("SMSProviderInfouToken")]
    public string SMSProviderInfouToken { get; set; }

    [EnvKey("DatabaseConnectionString")]
    public string DatabaseConnectionString { get; set; }

    [EnvKey("SeqApiKey")]
    public string SeqApiKey { get; set; }

    [EnvKey("SeqUrl")]
    public string SeqUrl { get; set; }

    [EnvKey("ServiceName")]
    public string ServiceName { get; set; }

    [EnvKey("EnableOpenTelemetry")]
    public bool EnableOpenTelemetry { get; set; }
}