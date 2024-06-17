using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GraphQL.AspNet.Attributes;
using Infrastructure.Requests.Processor.Services.Client;
namespace Infrastructure.GQL.Inputs.Client;

[Description("Client Contact Create Object input")]
public class ClientContactInput {
    [Description("client Id")]
    [GraphField(TypeExpression = "Int!")]
    [Required]
    public int ClientId { get; set; }

    [Description("client contact First Name")]
    [GraphField(TypeExpression = "String!")]
    [Required]
    public string FirstName { get; set; }

    [Description("client contact Last Name")]
    [GraphField(TypeExpression = "String!")]
    [Required]
    public string LastName { get; set; }

    [Description("client contact Email")]
    [GraphField(TypeExpression = "String!")]
    [Required]
    public string Email { get; set; }

    [Description("client contact Phone 1")]
    [GraphField(TypeExpression = "String!")]
    [Required]
    public string Phone1 { get; set; }

    [Description("client contact Phone 2")]
    public string? Phone2 { get; set; }

    [Description("client contact Fax")]
    public string? Fax { get; set; }

    [Description("client contact Whatsapp")]
    public string? Whatsapp { get; set; }

    [Description("client Country Id")]
    [GraphField(TypeExpression = "Int!")]
    [Required]
    public int CountryId { get; set; }

    [Description("client contact Postal Code")]
    public string? PostalCode { get; set; }

    [Description("client contact Address")]
    public string? Address { get; set; }

    [Description("client contact City")]
    public string? City { get; set; }

    [Description("client contact State")]
    public string? State { get; set; }

    [Description("client contact Job Title")]
    [GraphField(TypeExpression = "String!")]
    [Required]
    public string JobTitle { get; set; }

    [Description("client contact Department")]
    public string? Department { get; set; }

    [Description("client contact Company")]
    [GraphField(TypeExpression = "String!")]
    [Required]
    public string Company { get; set; }

    [Description("client contact Notes")]
    public string? Notes { get; set; }

    public CreateClientContractRequest ToProcessEntity(Guid loggedInUserId, string ipAddress, string userAgent) {
        return new CreateClientContractRequest() {
            LoggedInUserId = loggedInUserId,
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