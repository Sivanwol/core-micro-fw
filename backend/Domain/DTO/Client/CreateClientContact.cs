namespace Domain.DTO.Client;

public class CreateClientContact {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone1 { get; set; }
    public string? Phone2 { get; set; }
    public string? Fax { get; set; }
    public string? Whatsapp { get; set; }
    public int CountryId { get; set; }
    public string? PostalCode { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string JobTitle { get; set; }
    public string? Department { get; set; }
    public string Company { get; set; }
    public string Notes { get; set; }
}