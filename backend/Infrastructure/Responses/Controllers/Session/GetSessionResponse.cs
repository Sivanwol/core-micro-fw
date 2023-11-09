using Infrastructure.Responses.Common;
namespace Infrastructure.Responses.Controllers.Session;

public class GetSessionResponse {
    public SessionInfo Info { get; set; }
    public IEnumerable<MatchingUser> Users { get; set; }
}