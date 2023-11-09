using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.User;
using Infrastructure.Enums;
using Microsoft.AspNetCore.Identity;
namespace Domain.Entities;

[Table("User")]
public class Users : IdentityUser {
    [Column("Id")]
    public int UserId { get; set; }

    public string Token { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public int CountryId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool EmailVerified { get; set; }
    public bool PhoneVerify { get; set; }
    public int DefaultImageId { get; set; }
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
    public DateTime UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public UserProfileShortInfo ToUserProfileShortInfo(Media media) {
        return new UserProfileShortInfo {
            UserId = UserId,
            Email = Email,
            FirstName = FirstName,
            LastName = LastName,
            PhoneNumber = PhoneNumber,
            CountryId = CountryId,
            Latitude = Latitude,
            Longitude = Longitude,
            BirthDate = BirthDate,
            Gender = Gender,
            DefaultImageId = DefaultImageId,
            DefualtMediaUrl = media.FileUrl,
            Height = Height,
            MeasureUnits = MeasureUnits,
            LanguageId = LanguageId,
            EthnicityId = EthnicityId,
            ReligionId = ReligionId,
            DefualtMainMediaWidth = media.Width,
            DefualtMainMediaHeight = media.Height
        };
    }
}