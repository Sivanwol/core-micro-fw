namespace Application.Configs;

public abstract class BaseConfig {
    public bool IsTesting { get; set; }
    public bool EnableSwagger { get; set; }
    public string ConnectionString { get; set; }
    public int APIMajorVersion { get; set; }
    public int APIMinorVersion { get; set; }
}