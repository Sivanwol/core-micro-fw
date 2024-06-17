using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;
namespace Domain.Entities;

[Table("client-server-has-tags")]
public class ClientServerHasTags : BaseEntityWithoutId {
    public int ServerId { get; set; }
    public ClientServers Server { get; set; }
    public int TagId { get; set; }
    public Tags Tag { get; set; }
}