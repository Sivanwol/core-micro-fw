using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Application.Queries;
using Domain.Entities.Common;
namespace Domain.Entities;

[Table("client-hardware")]
public class ClientHardware : BaseEntity, ISoftDeletable {
    [ForeignKey("Client")]
    public int ClientId { get; set; }

    public Clients Client { get; set; }

    [ForeignKey("Network")]
    public int NetworkId { get; set; }

    public ClientNetworks Network { get; set; }

    [ForeignKey("Asset")]
    public int AssetId { get; set; }

    public Assets Asset { get; set; }

    [StringLength(100)]
    public string Label { get; set; }

    [Column(TypeName = "text")]
    public string Notes { get; set; }

    public DateTime? DisabledAt { get; set; }
    public ICollection<ClientServerHasTags> Tags { get; set; }
    public ICollection<ClientServersHasAssets> Assets { get; set; }
    public DateTime? DeletedAt { get; set; }
}