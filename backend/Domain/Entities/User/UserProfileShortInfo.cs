using Infrastructure.Enums;
using Infrastructure.Responses.Common;
namespace Domain.Entities.User;

public class UserProfileShortInfo {
    public int UserId { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public int CountryId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public decimal Height { get; set; }
    public MeasureUnit MeasureUnits { get; set; }
    public int LanguageId { get; set; }
    public int ReligionId { get; set; }
    public int EthnicityId { get; set; }
    public int DefaultImageId { get; set; }
    public string DefualtMediaUrl { get; set; }
    public int DefualtMainMediaWidth { get; set; }
    public int DefualtMainMediaHeight { get; set; }

    public MatchingUser ToResponse() {
        return new MatchingUser {
            UserId = UserId,
            FirstName = FirstName,
            LastName = LastName,
            MainMediaUrl = DefualtMediaUrl,
            MainMediaWidth = DefualtMainMediaWidth,
            MainMediaHeight = DefualtMainMediaHeight
        };
    }
}