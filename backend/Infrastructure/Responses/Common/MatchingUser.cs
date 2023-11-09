namespace Infrastructure.Responses.Common;

public class MatchingUser {
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MainMediaUrl { get; set; }
    public int MainMediaWidth { get; set; }
    public int MainMediaHeight { get; set; }
}