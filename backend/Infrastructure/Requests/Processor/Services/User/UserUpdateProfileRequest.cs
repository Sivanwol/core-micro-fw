using Application.Responses.Base;
using Infrastructure.Enums;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.User;

public class UpdateUserProfileInfo {
    public Gender Gender { get; set; }
    public float Height { get; set; }
    public MeasureUnit HeightMeasureUnit { get; set; }
    public int ReligionId { get; set; }
    public int EthnicityId { get; set; }
}

public class UpdateUserProfilePreference {
    public Gender Gender { get; set; }
    public float HeightFrom { get; set; }
    public float HeightTo { get; set; }
    public int AgeFrom { get; set; }
    public int AgeTo { get; set; }
    public IEnumerable<int> ReligionIds { get; set; }
    public IEnumerable<int> EthnicityIds { get; set; }
}

public class UserUpdateProfileRequest : IRequest<EmptyResponse> {
    public int UserId { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public int CountryId { get; set; }
    public int LanguageId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime BirthDate { get; set; }
    public UpdateUserProfileInfo PresonalInfo { get; set; }
    public UpdateUserProfilePreference Preference { get; set; }
}