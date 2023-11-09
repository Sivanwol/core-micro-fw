using System.ComponentModel.DataAnnotations;
using Infrastructure.Enums;
using Infrastructure.Requests.Processor.Services.User;
using Swashbuckle.AspNetCore.Annotations;
namespace Infrastructure.Requests.Controllers.User;

[SwaggerSchema(Required = new[] {
    "Register new user update user info stage 2: update user info request"
})]
[SwaggerTag("Controllers")]
public class UpdateRegisterNewUserPreferenceRequest {
    [Required]
    [SwaggerSchema("Gender")]
    public string Gender { get; set; }

    [Required]
    [SwaggerSchema("Height From")]
    public float HeightFrom { get; set; }

    [Required]
    [SwaggerSchema("Height To")]
    public float HeightTo { get; set; }


    [Required]
    [SwaggerSchema("Age From")]
    public int AgeFrom { get; set; }

    [Required]
    [SwaggerSchema("Age To")]
    public int AgeTo { get; set; }

    [Required]
    [SwaggerSchema("Religion Ids")]
    public IEnumerable<int> ReligionIds { get; set; }

    [Required]
    [SwaggerSchema("Ethnicity Ids")]
    public IEnumerable<int> EthnicityIds { get; set; }

    public NewUserRegisterPreferenceRequest ToProcessorEntity() {
        return new NewUserRegisterPreferenceRequest {
            Gender = Enum.Parse<Gender>(Gender),
            HeightFrom = HeightFrom,
            HeightTo = HeightTo,
            AgeFrom = AgeFrom,
            AgeTo = AgeTo,
            ReligionIds = ReligionIds,
            EthnicityIds = EthnicityIds,
            UserId = 0
        };
    }
    public UpdateUserProfilePreference ToProcessorUpdateEntity() {
        return new UpdateUserProfilePreference {
            Gender = Enum.Parse<Gender>(Gender),
            HeightFrom = HeightFrom,
            HeightTo = HeightTo,
            AgeFrom = AgeFrom,
            AgeTo = AgeTo,
            ReligionIds = ReligionIds,
            EthnicityIds = EthnicityIds,
        };
    }
}