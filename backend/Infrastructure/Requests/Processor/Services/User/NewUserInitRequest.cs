using Application.Responses.Base;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.User;

public class NewUserInitRequest : IRequest<EmptyResponse> {
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public int CountryId { get; set; }
    public int LanguageId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime BirthDate { get; set; }
}