using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.Entities;

[Table("PartnerReligions")]
public class PartnerReligions {
    public int UserId { get; set; }
    public int ReligionId { get; set; }
}