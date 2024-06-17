using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GraphQL.AspNet.Attributes;
using Infrastructure.Requests.Processor.Services.Client;
namespace Infrastructure.GQL.Inputs.Client;

[Description("Client Contact Update Object input")]
public class ClientContactUpdateInput {
    [Description("client Id")]
    [GraphField(TypeExpression = "Int!")]
    [Required]
    public int ClientId { get; set; }

    [Description("client contact First Name")]
    public string? FirstName { get; set; }

    [Description("client contact Last Name")]
    public string? LastName { get; set; }

    [Description("client contact Email")]
    public string? Email { get; set; }

    [Description("client contact Phone 1")]
    public string? Phone1 { get; set; }

    [Description("client contact Phone 2")]
    public string? Phone2 { get; set; }

    [Description("client contact Fax")]
    public string? Fax { get; set; }

    [Description("client contact Whatsapp")]
    public string? Whatsapp { get; set; }

    [Description("client Country Id")]
    public int? CountryId { get; set; }

    [Description("client contact Postal Code")]
    public string? PostalCode { get; set; }

    [Description("client contact Address")]
    public string? Address { get; set; }

    [Description("client contact City")]
    public string? City { get; set; }

    [Description("client contact State")]
    public string? State { get; set; }

    [Description("client contact Job Title")]
    public string? JobTitle { get; set; }

    [Description("client contact Department")]
    public string? Department { get; set; }

    [Description("client contact Company")]
    public string? Company { get; set; }

    [Description("client contact Notes")]
    public string? Notes { get; set; }

    public UpdateClientContractRequest ToProcessEntity(Guid loggedInUserId, int clientContactId, string ipAddress, string userAgent) {
        return new UpdateClientContractRequest() {
            LoggedInUserId = loggedInUserId,
            ClientContactId = clientContactId,
            ClientId = ClientId,
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            Phone1 = Phone1,
            Phone2 = Phone2,
            Fax = Fax,
            Whatsapp = Whatsapp,
            CountryId = CountryId,
            PostalCode = PostalCode,
            Address = Address,
            City = City,
            State = State,
            JobTitle = JobTitle,
            Department = Department,
            Company = Company,
            Notes = Notes,
            UserAgent = userAgent,
            IpAddress = ipAddress
        };
    }
}