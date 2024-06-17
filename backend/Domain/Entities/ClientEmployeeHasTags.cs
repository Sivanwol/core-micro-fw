using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;
namespace Domain.Entities;

[Table("client-employees-has-tags")]
public class ClientEmployeeHasTags : BaseEntityWithoutId {
    [Key]
    public int EmployeeId { get; set; }

    public ClientEmployees Employee { get; set; }

    [Key]
    public int TagId { get; set; }

    public Tags Tag { get; set; }
}