namespace Infrastructure.Services.Auth.Sender;

public interface ISmsSender {
    Task SendSmsAsync(string number, string message);
}