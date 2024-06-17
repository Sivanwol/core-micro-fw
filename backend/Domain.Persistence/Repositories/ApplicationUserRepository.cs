using Application.Configs;
using Application.Exceptions;
using Application.Utils;
using Domain.Entities;
using Domain.Filters;
using Domain.Persistence.Context;
using Domain.Persistence.Repositories.Common;
using Domain.Persistence.Repositories.Interfaces;
using Infrastructure.Enums;
using Infrastructure.GQL.Inputs.User;
using Microsoft.EntityFrameworkCore;
using Serilog;
namespace Domain.Persistence.Repositories;

public class ApplicationUserRepository : BaseRepository, IApplicationUserRepository {
    private readonly BackendApplicationConfig _config;
    private readonly ICountriesRepository _countriesRepository;
    private readonly ILanguagesRepository _languagesRepository;
    public ApplicationUserRepository(
        IDomainContext context,
        BackendApplicationConfig config,
        ICountriesRepository countriesRepository,
        ILanguagesRepository languagesRepository
    ) : base(context) {
        _config = config;
        _countriesRepository = countriesRepository;
        _languagesRepository = languagesRepository;
    }


    public async Task SaveDisplayLanguage(Guid userId, int languageId) {
        Log.Logger.Information($"Saving display language for user {userId}-{languageId}");
        var languageData = await _languagesRepository.GetById(languageId);
        if (languageData == null) {
            throw new EntityNotFoundException("Languages", languageId.ToString());
        }
        var user = await GetById(userId);
        if (user == null) {
            throw new EntityNotFoundException("User", userId.ToString());
        }
        user.DisplayLanguage = languageData;
        await Context.Instance.SaveChangesAsync();
    }

    public async Task DeleteUser(Guid userId) {
        Log.Logger.Information($"Delete new user with id {userId}");
        var user = await GetById(userId);
        if (user == null) {
            throw new EntityNotFoundException("User", userId.ToString());
        }

    }

    public async Task<bool> UpdateUserPreference(Guid userId, IList<UserPreferenceInput> preferences) {
        Log.Logger.Information("Update user preference for user {UserId}", userId);
        return await Context.ExecuteTransactionAsync(async () => {
            var user = await GetById(userId);
            if (user == null) {
                throw new EntityNotFoundException("User", userId.ToString());
            }
            foreach (var preference in preferences) {
                var userPreference = user.Preferences.FirstOrDefault(w => w.PreferenceKey == preference.Key);
                if (userPreference == null) {
                    userPreference = new ApplicationUserPreferences {
                        UserId = userId,
                        PreferenceKey = preference.Key,
                        PreferenceValue = preference.Value
                    };
                    user.Preferences.Add(userPreference);
                    continue;
                }
                userPreference.PreferenceValue = preference.Value;
            }
            await Context.Instance.SaveChangesAsync();
            return true;
        }, (ex) => {
            Log.Logger.Error(ex, "Failed to update user preference for user {UserId}", userId);
            return Task.FromResult(false);
        });
    }

    public async Task ResetUserPreference(Guid userId) {
        Log.Logger.Information("Reset user preference for user {UserId}", userId);
        var user = await GetById(userId);
        if (user == null) {
            throw new EntityNotFoundException("User", userId.ToString());
        }
        Context.UserPreferences.RemoveRange(user.Preferences);
        await Context.Instance.SaveChangesAsync();
    }
    public async Task ExportUser(Guid userId) {
        Log.Logger.Information($"Export new user with id {userId}");
        var user = await GetById(userId);
        if (user == null) {
            throw new EntityNotFoundException("User", userId.ToString());
        }
    }
    public async Task<ApplicationUser?> GetById(Guid id) {
        Log.Logger.Information($"Fetching user with id {id}");
        var user = Context.Users
            .Include(w => w.Country)
            .Include(w => w.DisplayLanguage)
            .FirstOrDefault(w => w.DeletedAt == null && w.Id == id.ToString());
        if (user == null) {
            throw new EntityNotFoundException("User", id.ToString());
        }
        user.Preferences = await GetPreferencesByUserId(id);
        return user;
    }
    public async Task<List<ApplicationUserPreferences>> GetPreferencesByUserId(Guid id) {
        Log.Logger.Information($"Fetching preferences for user with id {id}");
        return await Context.UserPreferences
            .Where(r => r.UserId == id)
            .ToListAsync();
    }

    public async Task<ApplicationUser?> GetUserByToken(string userToken) {
        Log.Logger.Information($"Fetching user with token {userToken}");
        return Context.Users.FirstOrDefault(w => w.DeletedAt == null && w.Token == userToken);
    }

    public async Task<ApplicationUser?> LocateUserByPhone(int countryId, string phoneNumber) {
        Log.Logger.Information($"Locating user with phone number {countryId}-{phoneNumber}");
        var user = Context.Users.First(w => w.DeletedAt == null && w.Country.Id == countryId && w.PhoneNumber == phoneNumber);
        if (user == null) {
            throw new EntityNotFoundException("User", $"{countryId}-{phoneNumber}");
        }
        return user;
    }

    public async Task<ApplicationUser?> LocateUserByEmail(string email) {
        Log.Logger.Information($"Locating user with email {email}");
        var user = Context.Users.First(w => w.Email == email);
        if (user == null) {
            throw new EntityNotFoundException("User", email);
        }
        return user;
    }
    public async Task<ApplicationUserOtpCodes?> GenerateOtpCode(ApplicationUser user, int expiredCodeInMinutes, bool forceSent, MFAProvider provider) {
        Log.Logger.Information($"Generate otp code for user {user.Id}-{provider}");
        var totalCodeSent = Context.UserOtpCodes.Count(w =>
            w.UserId.Equals(user.Id) && w.ExpirationDate <= SystemClock.Now().AddMinutes(expiredCodeInMinutes) && w.ExpirationDate >= SystemClock.Now() &&
            w.ComplateAt == null);
        if (totalCodeSent != 0 && !forceSent)
            return null;
        var entity = new ApplicationUserOtpCodes {
            UserId = Guid.Parse(user.Id),
            Token = StringExtensions.GenerateToken(),
            Code = provider == MFAProvider.Email ? ApplicationUserOtpCodes.GenerateCodeAsToken() : ApplicationUserOtpCodes.GenerateCode(),
            ProviderType = provider,
            ExpirationDate = SystemClock.Now().AddMinutes(expiredCodeInMinutes),
            CreatedAt = SystemClock.Now(),
            UpdatedAt = SystemClock.Now()
        };
        var expiredDate = SystemClock.Now().AddMinutes(forceSent ? 0 : expiredCodeInMinutes);
        var entities = Context.UserOtpCodes.Where(w =>
            w.UserId.Equals(Guid.Parse(user.Id)) && w.ExpirationDate <= expiredDate && w.ExpirationDate >= SystemClock.Now() && w.ComplateAt == null);
        if (entities.Any()) {
            Log.Logger.Information($"Clearing existing otp code for user {user.Id}-{provider}");
            foreach (var item in entities) {
                item.ComplateAt = SystemClock.Now();
                item.UpdatedAt = SystemClock.Now();
            }
        }
        Context.UserOtpCodes.Add(entity);
        await Context.Instance.SaveChangesAsync();
        return entity;
    }
    public async Task<ApplicationUserOtpCodes> GenerateRegistrationOtpCode(ApplicationUser user) {
        var entity = new ApplicationUserOtpCodes {
            UserId = Guid.Parse(user.Id),
            Token = StringExtensions.GenerateToken(),
            Code = ApplicationUserOtpCodes.GenerateCodeAsToken(),
            ExpirationDate = SystemClock.Now().AddDays(_config.RegisterEmailConfirmationCodeExpiredInMinutes),
            ProviderType = MFAProvider.Email,
            CreatedAt = SystemClock.Now(),
            UpdatedAt = SystemClock.Now()
        };
        Context.UserOtpCodes.Add(entity);
        await Context.Instance.SaveChangesAsync();
        return entity;
    }

    public async Task<ApplicationUserOtpCodes?> IsExistingOtpEntity(string userToken, string token, int expiredCodeInMinutes) {
        Log.Logger.Information($"Checking if otp code exist for user [{userToken} , {token}]");
        var user = await GetUserByToken(userToken);
        if (user == null) {
            return null;
        }
        var entity = Context.UserOtpCodes.FirstOrDefault(w =>
            w.UserId.Equals(Guid.Parse(user.Id)) && w.Token == token && w.ExpirationDate >= SystemClock.Now() &&
            w.ExpirationDate <= w.CreatedAt.AddMinutes(expiredCodeInMinutes) &&
            !w.ComplateAt.HasValue);
        return entity;
    }

    public async Task<int> GetTotalOtpCodesWithinExpirationDate(Guid userId, int expiredCodeInMinutes) {
        Log.Logger.Information($"Getting total otp code for user {userId}");
        var query = from w in Context.UserOtpCodes
            where w.UserId.Equals(userId) &&
                  w.ExpirationDate >= SystemClock.Now() &&
                  !w.ComplateAt.HasValue
            select w;
        query.Where(w => w.ExpirationDate <= w.CreatedAt.AddMinutes(expiredCodeInMinutes));
        return query.Count();
    }

    public async Task<bool> IsAllowToResendOtpCode(ApplicationUser user, int expiredCodeInMinutes, int maxResendCount) {
        Log.Logger.Information($"Checking if user {user.Id} is allow to resend otp code");
        var totalCodesSent = await GetTotalOtpCodesWithinExpirationDate(Guid.Parse(user.Id), expiredCodeInMinutes);
        return totalCodesSent <= maxResendCount;
    }

    public async Task ClearOtpCodes(Guid userId) {
        Log.Logger.Information($"Clearing otp code for user {userId}");
        var entities = Context.UserOtpCodes.Where(w => w.UserId.Equals(userId) && w.ExpirationDate >= SystemClock.Now() && !w.ComplateAt.HasValue);
        if (entities.Any()) {
            Log.Logger.Information($"Clearing existing otp code for user {userId} total of {entities.Count()} entities found");
            foreach (var item in entities) {
                if (item == null)
                    continue;
                item.ComplateAt = SystemClock.Now();
                item.UpdatedAt = SystemClock.Now();
            }
            await Context.Instance.SaveChangesAsync();
        }
    }

    public async Task<ApplicationUserOtpCodes?> FetchOTP(ApplicationUser user) {
        Log.Logger.Information($"Fetching otp code for user {user.Id}");
        return (from w in Context.UserOtpCodes
            where w.UserId.Equals(Guid.Parse(user.Id)) && w.ExpirationDate >= SystemClock.Now() && !w.ComplateAt.HasValue
            orderby w.ExpirationDate descending
            select w).FirstOrDefault();
    }

    public async Task<IEnumerable<ApplicationUser>> GetUsers(RecordFilterPagination<ApplicationUserFilters> filter) {
        Log.Logger.Information($"Fetching users with filter {filter}");
        var query = (Context.Users.AsQueryable());
        query = query.Where(u => u.RegisterCompletedAt != null);
        query = filter.ApplyQuery(query);
        return query.ToList();
    }

    public async Task<int> GetUsersTotalRecords(RecordFilterPagination<ApplicationUserFilters> filter) {
        Log.Logger.Information($"Fetching users total records with filter {filter}");
        var query = Context.Users.AsQueryable();
        query = query.Where(u => u.RegisterCompletedAt != null);
        query = filter.ApplyQuery(query);
        return (int)Math.Ceiling(query.Count() / (double)filter.PageSize);
    }

    public async Task<int> GetUsersTotalPages(RecordFilterPagination<ApplicationUserFilters> filter) {
        Log.Logger.Information($"Fetching users total pages with filter {filter}");
        var query = Context.Users.AsQueryable();
        query = query.Where(u => u.RegisterCompletedAt != null);
        return filter.ApplyQuery(query).Count();
    }
    public async Task<ApplicationUser> UpdateMyProfile(Guid userId, string phoneNumber, string firstName, string lastName, Countries country, string address) {
        var user = await GetById(userId);
        if (user == null) {
            throw new EntityNotFoundException("User", userId.ToString());
        }
        user.Address = address;
        user.Country = country;
        user.FirstName = firstName;
        user.LastName = lastName;

        await Context.Instance.SaveChangesAsync();
        return user;
    }
}