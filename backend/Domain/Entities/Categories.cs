using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
namespace Domain.Entities;

[Table("Categories")]
public class Categories : BaseEntity {
    public string Name { get; set; }
    public int Score { get; set; }
}