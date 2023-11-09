using Infrastructure.Enums;
namespace Infrastructure.Requests.Controllers.Backoffice;

public class SendCodeToProviderRequest {
    public MFAProvider Provider { get; set; }
}