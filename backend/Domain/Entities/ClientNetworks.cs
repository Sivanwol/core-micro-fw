using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Application.Queries;
using Domain.Entities.Common;
namespace Domain.Entities;

[Table("client-networks")]
public class ClientNetworks : BaseEntity, ISoftDeletable {
    [ForeignKey("Client")]
    public int ClientId { get; set; }

    public Clients Client { get; set; }

    [ForeignKey("Asset")]
    public int AssetId { get; set; }

    public Assets Asset { get; set; }

    [StringLength(100)]
    public string Label { get; set; }

    [StringLength(20)]
    public string Color { get; set; } = "#000000";

    [Column(TypeName = "text")]
    public string Notes { get; set; }

    public DateTime? DisabledAt { get; set; }
    public ICollection<ClientNetworkHasTags> Tags { get; set; }
    public ICollection<ClientNetworksHasAssets> Assets { get; set; }
    public DateTime? DeletedAt { get; set; }
}