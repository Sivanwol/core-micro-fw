using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
namespace Domain.Entities;

[Table("Languages")]
public class Languages : BaseEntity {
    public string Name { get; set; }
    public string Code { get; set; }
}