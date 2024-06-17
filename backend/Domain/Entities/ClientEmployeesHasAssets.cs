using System.ComponentModel.DataAnnotations.Schema;
using Application.Queries;
using Domain.Entities.Common;
namespace Domain.Entities;

[Table("client-employees-has-assets")]
public class ClientEmployeesHasAssets : BaseEntityWithoutId, ISoftDeletable {
    [ForeignKey("Client")]
    public int ClientEmployeeId { get; set; }

    public ClientEmployees Employee { get; set; }

    [ForeignKey("Asset")]
    public int AssetId { get; set; }

    public Assets Asset { get; set; }

    [ForeignKey("ContactEmployee")]
    public int ContactEmployeeId { get; set; }

    public ClientContacts ContactEmployee { get; set; }

    public DateTime? DisabledAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}