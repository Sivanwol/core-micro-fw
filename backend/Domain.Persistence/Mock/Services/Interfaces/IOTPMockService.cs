using Domain.Entities;
namespace Domain.Persistence.Mock.Services.Interfaces;

public interface IOTPMockService {
    public ApplicationUserOtpCodes GetOne(Guid userId);
    public ApplicationUserOtpCodes GetOne(Guid userId, bool active);
    public ApplicationUserOtpCodes GetOne(Guid userId, bool active, string code);

    public IEnumerable<ApplicationUserOtpCodes> GetMany(Guid userId); // will return a list of codes for a user that mix active and not 
    public IEnumerable<ApplicationUserOtpCodes> GetMany(Guid userId, bool active); // will return a list of codes for a user that are either active or not
}