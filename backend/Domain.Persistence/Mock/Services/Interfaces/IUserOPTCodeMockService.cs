using Domain.Entities;
namespace Domain.Persistence.Mock.Services.Interfaces;

public interface IUserOPTCodeMockService {
    public ApplicationUserOtpCodes GetOne(int userId);
    public ApplicationUserOtpCodes GetOne(int userId, bool active);
    public ApplicationUserOtpCodes GetOne(int userId, bool active, string code);

    public IEnumerable<ApplicationUserOtpCodes> GetMany(int userId); // will return a list of codes for a user that mix active and not 
    public IEnumerable<ApplicationUserOtpCodes> GetMany(int userId, bool active); // will return a list of codes for a user that are either active or not
}