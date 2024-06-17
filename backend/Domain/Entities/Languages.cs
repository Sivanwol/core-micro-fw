using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;
using Infrastructure.GQL;
namespace Domain.Entities;

[Table("languages")]
public class Languages : BaseEntity {
    [Column(TypeName = "varchar(20)")]
    public string Name { get; set; }

    [Column(TypeName = "varchar(5)")]
    public string Code { get; set; }

    public string? Flag { get; set; }

    public DateTime? SupportedAt { get; set; }

    public Language ToGql() {
        return new() {
            Id = Id,
            Name = Name,
            Code = Code,
            Flag = Flag
        };
    }
}