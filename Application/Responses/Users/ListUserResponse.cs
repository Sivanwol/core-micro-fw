namespace Application.Responses.Users; 

public class ListUserResponse {
    public long Id { get; set; }
    public string Auth0Id { get; set; }
    public bool Active { get; set; }
}