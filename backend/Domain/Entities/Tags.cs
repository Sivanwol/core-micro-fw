using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;
namespace Domain.Entities;

[Table("tags")]
public class Tags : BaseEntity {
    [StringLength(50)]
    public string Name { get; set; }

    public ICollection<ClientEmployeeHasTags> ClientEmployees { get; set; }
    public ICollection<ClientNetworkHasTags> ClientNetworks { get; set; }
    public ICollection<ClientServerHasTags> ClientServers { get; set; }
}