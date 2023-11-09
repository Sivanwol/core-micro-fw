using Application.Responses.Base;
using Infrastructure.Enums;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.User;

public class NewUserRegisterPreferenceRequest : IRequest<EmptyResponse> {
    public int UserId { get; set; }
    public Gender Gender { get; set; }
    public float HeightFrom { get; set; }
    public float HeightTo { get; set; }
    public int AgeFrom { get; set; }
    public int AgeTo { get; set; }
    public IEnumerable<int> ReligionIds { get; set; }
    public IEnumerable<int> EthnicityIds { get; set; }
}