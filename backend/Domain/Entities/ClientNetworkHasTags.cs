using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;
namespace Domain.Entities;

[Table("client-network-has-tags")]
public class ClientNetworkHasTags : BaseEntityWithoutId {
    public int NetworkId { get; set; }
    public ClientNetworks Network { get; set; }
    public int TagId { get; set; }
    public Tags Tag { get; set; }
}