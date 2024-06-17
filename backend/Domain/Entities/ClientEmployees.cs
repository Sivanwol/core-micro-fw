using System.ComponentModel.DataAnnotations.Schema;
using Application.Queries;
using Domain.Entities.Common;
namespace Domain.Entities;

[Table("client-employees")]
public class ClientEmployees : BaseEntity, ISoftDeletable {
    [ForeignKey("Client")]
    public int ClientId { get; set; }

    public Clients Client { get; set; }

    [ForeignKey("Network")]
    public int NetworkId { get; set; }

    public ClientNetworks Network { get; set; }

    [ForeignKey("Server")]
    public int ServerId { get; set; }

    public ClientServers Server { get; set; }

    [ForeignKey("Asset")]
    public int AssetId { get; set; }

    public Assets Asset { get; set; }

    [ForeignKey("ContactEmployee")]
    public int ContactEmployeeId { get; set; }

    public Contacts ContactEmployee { get; set; }

    public DateTime? DisabledAt { get; set; }
    public ICollection<ClientEmployeeHasTags> Tags { get; set; }
    public ICollection<ClientEmployeesHasAssets> Assets { get; set; }
    public DateTime? DeletedAt { get; set; }
}