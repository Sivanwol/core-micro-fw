using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
namespace Domain.Entities;

[Table("Countries")]
public class Countries : BaseEntity {
    public string CountryName { get; set; }
    public string CountryCode { get; set; }
    public string CountryCode3 { get; set; }
    public string CountryNumber { get; set; }
}