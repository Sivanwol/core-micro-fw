namespace Domain.Entities.Matching;

public class UserPicks {
    public UserMatches Match { get; set; }
    public Users MatchUser { get; set; }
    public Media MatchMedia { get; set; }
}