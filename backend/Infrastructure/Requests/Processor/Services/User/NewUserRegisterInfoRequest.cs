using Application.Responses.Base;
using Infrastructure.Enums;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.User;

public class NewUserRegisterInfoRequest : IRequest<EmptyResponse> {
    public int UserId { get; set; }
    public Gender Gender { get; set; }
    public float Height { get; set; }
    public MeasureUnit HeightMeasureUnit { get; set; }
    public int ReligionId { get; set; }
    public int EthnicityId { get; set; }
}