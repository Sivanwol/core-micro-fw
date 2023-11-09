namespace Domain.Entities.User;

public class UserSessionMessages {
    public Sessions Session { get; set; }
    public IEnumerable<UserProfileShortInfo> MessagingUserProfiles { get; set; }
    public IEnumerable<UserMessages> Messages { get; set; }
}