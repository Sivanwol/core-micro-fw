using Infrastructure.Enums;
namespace Infrastructure.Requests.Account;

public class SendCodeToProviderRequest {
    public AuthProvidersMFA Provider { get; set; }
}