using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
namespace Domain.Entities; 

[Table("Ethnicities")]
public class Ethnicities : BaseEntity {
    public string Name { get; set; }
}