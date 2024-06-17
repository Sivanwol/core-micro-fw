using Domain.Entities;
using Domain.Filters;
using Domain.Persistence.Repositories.Common.Interfaces;
using Infrastructure.Enums;
using Infrastructure.GQL.Inputs.User;
namespace Domain.Persistence.Repositories.Interfaces;

public interface IApplicationUserRepository : IGenericRepository<ApplicationUser, Guid> {
    Task SaveDisplayLanguage(Guid userId, int languageId);
    Task DeleteUser(Guid userId);
    Task ExportUser(Guid userId);
    Task<List<ApplicationUserPreferences>> GetPreferencesByUserId(Guid id);
    Task<bool> UpdateUserPreference(Guid userId, IList<UserPreferenceInput> preferences);
    Task ResetUserPreference(Guid userId);
    Task<ApplicationUser?> GetUserByToken(string userToken);
    Task<ApplicationUser?> LocateUserByPhone(int CountryId, string PhoneNumber);
    Task<ApplicationUser?> LocateUserByEmail(string email);
    Task<IEnumerable<ApplicationUser>> GetUsers(RecordFilterPagination<ApplicationUserFilters> filter);
    Task<int> GetUsersTotalPages(RecordFilterPagination<ApplicationUserFilters> filter);
    Task<int> GetUsersTotalRecords(RecordFilterPagination<ApplicationUserFilters> filter);
    Task<ApplicationUserOtpCodes?> GenerateOtpCode(ApplicationUser user, int expiredCodeInMinutes, bool forceSent, MFAProvider provider);
    Task<ApplicationUserOtpCodes> GenerateRegistrationOtpCode(ApplicationUser user);
    Task<int> GetTotalOtpCodesWithinExpirationDate(Guid userId, int expiredCodeInMinutes);
    Task<ApplicationUserOtpCodes?> IsExistingOtpEntity(string userToken, string token, int expiredCodeInMinutes);
    Task<bool> IsAllowToResendOtpCode(ApplicationUser user, int expiredCodeInMinutes, int maxResendCount);
    Task ClearOtpCodes(Guid userId);
    Task<ApplicationUserOtpCodes?> FetchOTP(ApplicationUser user);
    Task<ApplicationUser> UpdateMyProfile(Guid userId, string phoneNumber, string firstName, string lastName, Countries country, string address);
}