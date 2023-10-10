using Infrastructure.Enums;

namespace Infrastructure.Models.Account;

public class SendCodeToProviderRequest {
    public AuthProvidersMFA Provider { get; set; }
}