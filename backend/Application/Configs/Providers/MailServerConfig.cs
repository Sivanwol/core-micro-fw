namespace Application.Configs.Providers;

public class MailServerConfig {
    public string FromAddress { get; set; }
    public string DisplayName { get; set; }
    public string ServerAddress { get; set; }
    public int Port { get; set; }
    public bool IsUseSsl { get; set; }
    public bool IsStartTls { get; set; }
    public bool IsUseCredentials { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}