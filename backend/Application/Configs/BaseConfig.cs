namespace Application.Configs;

public abstract class BaseConfig {
    public bool DeveloperMode { get; set; } = true;
    public bool DeveloperModeEnabledOTP { get; set; } = false;
    public bool EnableSwagger { get; set; }
    public string ConnectionString { get; set; }
    public int APIMajorVersion { get; set; }
    public int APIMinorVersion { get; set; }
}