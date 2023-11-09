namespace Domain.Entities.User;

public class UserProfile {
    public Users User { get; set; }
    public IEnumerable<PartnerEthnicities> PartnerEthnicities { get; set; }
    public IEnumerable<PartnerReligions> PartnerReligions { get; set; }
}