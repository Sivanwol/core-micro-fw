namespace Application.Configs.Providers;

public class InfoUSmsConfig : BaseSMSConfig {
    public bool IsEnabled { get; set; } = false;
    public string Sender { get; set; }
    public string Token { get; set; }
    public string UserName { get; set; }
}