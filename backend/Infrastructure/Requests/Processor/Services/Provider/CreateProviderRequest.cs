using Infrastructure.Enums;
using Infrastructure.Requests.Processor.Common;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Requests.Processor.Services.Provider;

public class CreateProviderRequest : BaseRequest<Infrastructure.GQL.Vendor>
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int CountryId { get; set; }
    public IFormFile Logo { get; set; }
    public string SiteUrl { get; set; }
    public string SupportUrl { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
    public string? SupportPhone { get; set; }
    public string? SupportEmail { get; set; }
    public ProviderType ProviderType { get; set; }
    public ICollection<int> Categories { get; set; }

}
