using Infrastructure.Responses.Common;
namespace Infrastructure.Responses.Controllers.User;

public class GetUserPickResponse {
    public IEnumerable<MatchingUser> NewMatchs { get; set; }
    public IEnumerable<MatchingUser> OpenMatchs { get; set; }
}