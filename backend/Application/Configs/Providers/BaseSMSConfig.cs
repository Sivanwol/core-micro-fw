namespace Application.Configs.Providers;

public abstract class BaseSMSConfig {
    public int ExpiredInMinutes { get; set; }
    public int TotalRetriesWithinSession { get; set; }
}