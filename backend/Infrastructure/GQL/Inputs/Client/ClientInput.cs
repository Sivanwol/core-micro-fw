using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GraphQL.AspNet.Attributes;
using Infrastructure.Requests.Processor.Services.Client;
namespace Infrastructure.GQL.Inputs.Client;

[Description("Client Create Object input")]
public class ClientInput {
    [Description("if this client is a child of another client")]
    public int? ParentId { get; set; }

    [Description("client name")]
    [GraphField(TypeExpression = "String!")]
    [Required]
    public string Name { get; set; }

    [Description("client description")]
    public string Description { get; set; }

    [Description("client website")]
    [GraphField(TypeExpression = "String!")]
    [Required]
    public string Website { get; set; }

    [Description("client Country Id")]
    [GraphField(TypeExpression = "Int!")]
    [Required]
    public int CountryId { get; set; }

    [Description("client address")]
    [GraphField(TypeExpression = "String!")]
    [Required]
    public string? Address { get; set; }

    [Description("client city")]
    [GraphField(TypeExpression = "String!")]
    [Required]
    public string? City { get; set; }


    public CreateClientRequest ToProcessEntity(Guid loggedInUserId, string ipAddress, string userAgent) {
        return new CreateClientRequest {
            LoggedInUserId = loggedInUserId,
            ParentId = ParentId,
            Name = Name,
            Description = Description,
            Website = Website,
            CountryId = CountryId,
            Address = Address,
            City = City,
            UserAgent = userAgent,
            IpAddress = ipAddress
        };
    }

    public UpdateClientRequest ToProcessEntity(Guid loggedInUserId, int clientId, string ipAddress, string userAgent) {
        return new UpdateClientRequest {
            LoggedInUserId = loggedInUserId,
            ClientId = clientId,
            ParentId = ParentId,
            Name = Name,
            Description = Description,
            Website = Website,
            CountryId = CountryId,
            Address = Address,
            City = City,
            UserAgent = userAgent,
            IpAddress = ipAddress
        };
    }
}