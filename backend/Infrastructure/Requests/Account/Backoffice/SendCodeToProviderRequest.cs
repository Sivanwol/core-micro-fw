using Infrastructure.Enums;
namespace Infrastructure.Requests.Account.Backoffice;

public class SendCodeToProviderRequest {
    public AuthProvidersMFA Provider { get; set; }
}