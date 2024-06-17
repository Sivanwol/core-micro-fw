using System.ComponentModel.DataAnnotations.Schema;
using Application.Queries;
using Domain.Entities.Common;
namespace Domain.Entities;


[Table("client-server-has-assets")]
public class ClientServersHasAssets : BaseEntityWithoutId, ISoftDeletable {
    [ForeignKey("ClientServer")]
    public int ClientServerId { get; set; }

    public ClientServers Server { get; set; }

    [ForeignKey("Asset")]
    public int AssetId { get; set; }

    public Assets Asset { get; set; }

    public DateTime? DisabledAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}