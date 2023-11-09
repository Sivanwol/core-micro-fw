using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.Entities;

[Table("PartnerEthnicities")]
public class PartnerEthnicities {
    public int UserId { get; set; }
    public int EthnicityId { get; set; }
}