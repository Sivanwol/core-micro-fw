using Application.Utils;
using Domain.Entities;
using Domain.Persistence.Mock.Configs;
using Domain.Persistence.Mock.Services.Interfaces;
namespace Domain.Persistence.Mock.Services;

public class MockUserOPTCodeServices : IOTPMockService {
    public MockUserOPTCodeServices() {
        OTPCodeFaker = new AppUserOTPCodeMockConfig();
    }
    public AppUserOTPCodeMockConfig OTPCodeFaker { get; set; }

    public ApplicationUserOtpCodes GetOne(Guid userId) {
        var dt = SystemClock.Now();
        var otp = OTPCodeFaker.Generate(1).First();
        otp.UserId = userId;
        otp.ExpirationDate = dt.AddMinutes(15);
        return otp;
    }

    public ApplicationUserOtpCodes GetOne(Guid userId, bool active) {
        var dt = SystemClock.Now();
        return OTPCodeFaker.Generate(1).First(x => {
            x.UserId = userId;
            x.ExpirationDate = dt.AddMinutes(15);
            x.ComplateAt = active ? null : dt;
            return true;
        });
    }

    public ApplicationUserOtpCodes GetOne(Guid userId, bool active, string code) {
        var dt = SystemClock.Now();
        return OTPCodeFaker.Generate(1).First(x => {
            x.UserId = userId;
            x.ComplateAt = active ? null : dt;
            x.ExpirationDate = dt.AddMinutes(15);
            x.Code = code;
            return true;
        });
    }

    public IEnumerable<ApplicationUserOtpCodes> GetMany(Guid userId) {
        var dt = SystemClock.Now();
        return OTPCodeFaker.Generate(5).Select(x => {
            x.UserId = userId;
            x.ExpirationDate = dt.AddMinutes(15);
            x.ComplateAt = null;
            return x;
        });
    }

    public IEnumerable<ApplicationUserOtpCodes> GetMany(Guid userId, bool active) {
        var dt = SystemClock.Now();
        return OTPCodeFaker.Generate(5).Select(x => {
            x.UserId = userId;
            x.ExpirationDate = dt.AddMinutes(15);
            x.ComplateAt = active ? null : dt;

            return x;
        });
    }
}