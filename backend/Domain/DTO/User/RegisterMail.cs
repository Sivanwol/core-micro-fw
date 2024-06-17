namespace Domain.DTO.User;

public class RegisterMail {
    public string Title { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailConfirmationLink { get; set; }
    public string HomePageUrl { get; set; }
    public string HostImageUrl { get; set; }
}