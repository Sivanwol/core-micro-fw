using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;
namespace Domain.Entities;

[Table("providers_has_provider_category")]
public class ProviderHasProviderCategory : BaseEntityWithoutId {
    public int ProviderId { get; set; }
    public Providers Provider { get; set; }

    public int CategoryId { get; set; }
    public ProviderCategory Category { get; set; }
}