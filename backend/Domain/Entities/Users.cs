using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Enums;
namespace Domain.Entities;

[Table("Users")]
public class Users : BaseEntity {
    public string Token { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public int CountryId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool EmailVerified { get; set; }
    public string DefaultImageId { get; set; }
    public DateTime BirthDate { get; set; }
    public bool TermsApproved { get; set; }
    public Gender Gender { get; set; }
    public decimal Height { get; set; }
    public MeasureUnit MeasureUnits { get; set; }
    public int LanguageId { get; set; }
    public int ReligionId { get; set; }
    public int EthnicityId { get; set; }
    public int PartnerAgeFrom { get; set; }
    public int PartnerAgeTo { get; set; }
    public int PartnerHeightFrom { get; set; }
    public int PartnerHeightTo { get; set; }
}