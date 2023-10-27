using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
namespace Domain.Entities; 

[Table("Religions")]
public class Religions : BaseEntity {
    public string Name { get; set; }
}