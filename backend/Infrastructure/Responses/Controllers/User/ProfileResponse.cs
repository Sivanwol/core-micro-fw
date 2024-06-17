namespace Infrastructure.Responses.Controllers.User;

public class ProfileResponse {
    public string ID { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public int CountryId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime? BirthDate { get; set; }
    public bool TermsApproved { get; set; }
    public string Gender { get; set; }
    public decimal Height { get; set; }
    public int DisplayLanguageId { get; set; }
}