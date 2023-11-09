using System.ComponentModel.DataAnnotations;
using Infrastructure.Enums;
using Infrastructure.Requests.Processor.Services.User;
using Swashbuckle.AspNetCore.Annotations;
namespace Infrastructure.Requests.Controllers.User;

[SwaggerSchema(Required = new[] {
    "Register new user update user info stage 2: update user info request"
})]
[SwaggerTag("Controllers")]
public class UpdateRegisterNewUserInfoRequest {
    [Required]
    [SwaggerSchema("Gender")]
    public string Gender { get; set; }

    [Required]
    [SwaggerSchema("Height")]
    public float Height { get; set; }

    [Required]
    [SwaggerSchema("Height Measure Unit")]
    public string HeightMeasureUnit { get; set; }

    [Required]
    [SwaggerSchema("Religion Id")]
    public int ReligionId { get; set; }

    [Required]
    [SwaggerSchema("Ethnicity Id")]
    public int EthnicityId { get; set; }

    public NewUserRegisterInfoRequest ToProcessorEntity() {
        return new NewUserRegisterInfoRequest {
            Gender = Enum.Parse<Gender>(Gender),
            Height = Height,
            HeightMeasureUnit = Enum.Parse<MeasureUnit>(HeightMeasureUnit),
            ReligionId = ReligionId,
            EthnicityId = EthnicityId,
            UserId = 0
        };
    }
    public UpdateUserProfileInfo ToProcessorUpdateEntity() {
        return new UpdateUserProfileInfo {
            Gender = Enum.Parse<Gender>(Gender),
            Height = Height,
            HeightMeasureUnit = Enum.Parse<MeasureUnit>(HeightMeasureUnit),
            ReligionId = ReligionId,
            EthnicityId = EthnicityId,
        };
    }
}