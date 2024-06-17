using System.ComponentModel;
namespace Infrastructure.GQL;

[Description("Client Object")]
public class Client {
    [Description("Client Id")]
    public int Id { get; set; }

    [Description("Client Owner User")]
    public User OwnerUser { get; set; }

    [Description("Client Parent Id")]
    public Client? Parent { get; set; }

    [Description("Client Name")]
    public string Name { get; set; }

    [Description("Client Description")]
    public string Description { get; set; }

    [Description("Client Website")]
    public string Website { get; set; }

    [Description("Client Country")]
    public Country Country { get; set; }

    [Description("Client Address")]
    public string? Address { get; set; }

    [Description("Client City")]
    public string? City { get; set; }

    [Description("Client Created At")]
    public DateTime CreatedAt { get; set; }
}