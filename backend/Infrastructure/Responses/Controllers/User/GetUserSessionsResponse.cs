using Infrastructure.Responses.Common;
namespace Infrastructure.Responses.Controllers.User;

public class GetUserSessionsResponse {
    public IEnumerable<MatchingUser> Sessions { get; set; }
}