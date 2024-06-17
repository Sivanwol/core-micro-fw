using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;
namespace Domain.Entities;

[Table("providers_has_contacts")]
public class ProviderHasContact : BaseEntityWithoutId {
    public int ProviderId { get; set; }
    public Providers Provider { get; set; }
    public int ContactId { get; set; }
    public Contacts Contact { get; set; }
}