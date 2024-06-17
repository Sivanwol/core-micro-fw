using System.ComponentModel;
namespace Infrastructure.GQL;

[Description("Client Object")]
public class ClientContact {
    [Description("Client Contact Id")]
    public int Id { get; set; }

    [Description("Client")]
    public Client Client { get; set; }

    [Description("Contact First Name")]
    public string FirstName { get; set; }

    [Description("Contact Last Name")]
    public string LastName { get; set; }

    [Description("Contact Email")]
    public string Email { get; set; }

    [Description("Contact Phone 1")]
    public string Phone1 { get; set; }

    [Description("Contact Phone 2")]
    public string? Phone2 { get; set; }

    [Description("Contact Fax")]
    public string? Fax { get; set; }

    [Description("Contact Whatsapp")]
    public string? Whatsapp { get; set; }

    [Description("Contact Country")]
    public Country Country { get; set; }

    [Description("Contact Postal Code")]
    public string? PostalCode { get; set; }

    [Description("Contact Address")]
    public string? Address { get; set; }

    [Description("Contact City")]
    public string? City { get; set; }

    [Description("Contact State")]
    public string? State { get; set; }

    [Description("Contact Job Title")]
    public string JobTitle { get; set; }

    [Description("Contact Department")]
    public string? Department { get; set; }

    [Description("Contact Company")]
    public string Company { get; set; }

    [Description("Contact Notes")]
    public string? Notes { get; set; }

    [Description("Client Created At")]
    public DateTime CreatedAt { get; set; }
}