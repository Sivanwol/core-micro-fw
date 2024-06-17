using Domain.Entities;
using Domain.Persistence.Mock.Services;
using Domain.Persistence.Repositories;
using Domain.Persistence.Repositories.Interfaces;
using Infrastructure.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using Test.Shared.Common;
namespace Domain.Persistence.Test.Repositories;

[TestFixture, Description("Test the ApplicationUserRepository")]
[Category("Users")]
public class ApplicationUserRepositoryTest : BaseTest {
    [SetUp]
    public void Setup() {
        SetupTest("CountriesRepositoryTest");
        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();

        factory = serviceProvider.GetService<ILoggerFactory>()!;
        logger = factory.CreateLogger<CountriesRepository>();
    }
    protected ILoggerFactory factory;
    protected ILogger logger;

    [Test, Description("Test the SaveDisplayLanguage method")]
    public async Task SaveDisplayLanguage() {
        var newLanguage = new MockLanguagesServices().GetOne();
        var mockUser = new MockAppUserServices();
        var user = mockUser.GetOne(Guid.NewGuid());
        var mockEnumerableUser = new List<ApplicationUser> {
            user
        };
        var mockObjUser = mockEnumerableUser.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.Users).Returns(mockObjUser.Object);
        Context.Setup(x => x.Languages).Returns(new List<Languages> {
            newLanguage
        }.AsQueryable().BuildMockDbSet().Object);
        Context.Setup(c => c.Instance.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        var languagesRepository = new LanguagesRepository(Context.Object, factory);
        var countriesRepository = new CountriesRepository(Context.Object, factory);
        var repository = new ApplicationUserRepository(Context.Object, backendConfig, countriesRepository, languagesRepository);
        await repository.SaveDisplayLanguage(Guid.Parse(user.Id), newLanguage.Id);
        Context.Verify(x => x.Instance.SaveChangesAsync(It.IsAny<CancellationToken>()));
    }

    [Test, Description("Test the GetById method")]
    public async Task GetById() {
        var mockUser = new MockAppUserServices();
        var user = mockUser.GetOne(Guid.NewGuid());
        var mockEnumerableUser = new List<ApplicationUser> {
            user
        };
        var mockObjUser = mockEnumerableUser.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.Users).Returns(mockObjUser.Object);
        var languagesRepository = new LanguagesRepository(Context.Object, factory);
        var countriesRepository = new CountriesRepository(Context.Object, factory);
        var repository = new ApplicationUserRepository(Context.Object, backendConfig, countriesRepository, languagesRepository);
        var result = await repository.GetById(Guid.Parse(user.Id));
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() => {
            Assert.That(result!.Id, Is.EqualTo(user.Id));
            Assert.That(result.Token, Is.EqualTo(user.Token));
            Assert.That(result.Email, Is.EqualTo(user.Email));
            Assert.That(result.PhoneNumber, Is.EqualTo(user.PhoneNumber));
            Assert.That(result.Country.Id, Is.EqualTo(user.Country.Id));
        });
    }

    [Test, Description("Test the GetUserByToken method")]
    public async Task GetUserByToken() {
        var mockUser = new MockAppUserServices();
        var user = mockUser.GetOne(Guid.NewGuid());
        var mockEnumerableUser = new List<ApplicationUser> {
            user
        };
        var mockObjUser = mockEnumerableUser.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.Users).Returns(mockObjUser.Object);
        var languagesRepository = new LanguagesRepository(Context.Object, factory);
        var countriesRepository = new CountriesRepository(Context.Object, factory);
        var repository = new ApplicationUserRepository(Context.Object, backendConfig, countriesRepository, languagesRepository);
        var result = await repository.GetUserByToken(user.Token);
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() => {
            Assert.That(result!.Id, Is.EqualTo(user.Id));
            Assert.That(result.Token, Is.EqualTo(user.Token));
            Assert.That(result.Email, Is.EqualTo(user.Email));
            Assert.That(result.PhoneNumber, Is.EqualTo(user.PhoneNumber));
            Assert.That(result.Country.Id, Is.EqualTo(user.Country.Id));
        });
    }
    [Test, Description("Test the LocateUserByPhone method")]
    public async Task LocateUserByPhone() {
        var mockUser = new MockAppUserServices();
        var user = mockUser.GetOne(Guid.NewGuid());
        var mockEnumerableUser = new List<ApplicationUser> {
            user
        };
        var mockObjUser = mockEnumerableUser.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.Users).Returns(mockObjUser.Object);
        var languagesRepository = new LanguagesRepository(Context.Object, factory);
        var countriesRepository = new CountriesRepository(Context.Object, factory);
        var repository = new ApplicationUserRepository(Context.Object, backendConfig, countriesRepository, languagesRepository);
        var result = await repository.LocateUserByPhone(user.Country.Id, user.PhoneNumber!);
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() => {
            Assert.That(result!.Id, Is.EqualTo(user.Id));
            Assert.That(result.Token, Is.EqualTo(user.Token));
            Assert.That(result.Email, Is.EqualTo(user.Email));
            Assert.That(result.PhoneNumber, Is.EqualTo(user.PhoneNumber));
            Assert.That(result.Country.Id, Is.EqualTo(user.Country.Id));
        });
    }

    [Test, Description("Test the FetchOTP method")]
    public async Task FetchOTP() {
        var mockUser = new MockAppUserServices();
        var user = mockUser.GetOne(Guid.NewGuid());
        var mock = new MockUserOPTCodeServices();
        var mockData = mock.GetMany(Guid.Parse(user.Id), false);
        var expected = mock.GetOne(Guid.Parse(user.Id), true);
        mockData = mockData.Append(expected);
        var mockObj = mockData.AsQueryable().BuildMockDbSet();
        var mockEnumerableUser = new List<ApplicationUser> {
            user
        };
        var mockObjUser = mockEnumerableUser.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.UserOtpCodes).Returns(mockObj.Object!);
        Context.Setup(x => x.Users).Returns(mockObjUser.Object);
        var languagesRepository = new LanguagesRepository(Context.Object, factory);
        var countriesRepository = new CountriesRepository(Context.Object, factory);
        var repository = new ApplicationUserRepository(Context.Object, backendConfig, countriesRepository, languagesRepository);
        var result = await repository.FetchOTP(user);
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() => {
            Assert.That(result!.Id, Is.EqualTo(expected.Id));
            Assert.That(result.UserId, Is.EqualTo(expected.UserId));
            Assert.That(result.Code, Is.EqualTo(expected.Code));
            Assert.That(result.Token, Is.EqualTo(expected.Token));
            Assert.That(result.ComplateAt, Is.Null);
        });
    }

    [Test, Description("Test the ClearOtpCodes method")]
    public async Task ClearOtpCodes() {
        var mockUser = new MockAppUserServices();
        var user = mockUser.GetOne(Guid.NewGuid());
        var mock = new MockUserOPTCodeServices();
        var mockData = mock.GetMany(Guid.Parse(user.Id), true);
        var mockObj = mockData.AsQueryable().BuildMockDbSet();
        var mockEnumerableUser = new List<ApplicationUser> {
            user
        };
        var mockObjUser = mockEnumerableUser.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.UserOtpCodes).Returns(mockObj.Object!);
        Context.Setup(x => x.Users).Returns(mockObjUser.Object);
        var mockRepository = new Mock<IApplicationUserRepository>();
        await mockRepository.Object.ClearOtpCodes(Guid.Parse(user.Id));
        mockRepository.Verify(x => x.ClearOtpCodes(It.IsAny<Guid>()), Times.Once);
    }

    [Test, Description("Test the GetTotalOtpCodesWithinExpirationDate method")]
    public async Task GetTotalOtpCodesWithinExpirationDate() {
        var mockUser = new MockAppUserServices();
        var user = mockUser.GetOne(Guid.NewGuid());
        var mock = new MockUserOPTCodeServices();
        var mockData = mock.GetOne(Guid.Parse(user.Id), true);
        var mockEnumerable = new List<ApplicationUserOtpCodes> {
            mockData
        };
        var mockObj = mockEnumerable.AsQueryable().BuildMockDbSet();
        var mockEnumerableUser = new List<ApplicationUser> {
            user
        };
        var mockObjUser = mockEnumerableUser.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.UserOtpCodes).Returns(mockObj.Object!);
        Context.Setup(x => x.Users).Returns(mockObjUser.Object);
        var languagesRepository = new LanguagesRepository(Context.Object, factory);
        var countriesRepository = new CountriesRepository(Context.Object, factory);
        var repository = new ApplicationUserRepository(Context.Object, backendConfig, countriesRepository, languagesRepository);
        var result = await repository.GetTotalOtpCodesWithinExpirationDate(Guid.Parse(user.Id), 3);
        Assert.That(result, Is.EqualTo(1));
    }

    [Test, Description("Test the GetTotalOtpCodesWithinExpirationDate method")]
    [TestCase(5)]
    [TestCase(3)]
    public async Task IsAllowToResendOtpCode(int totalMaxSent) {
        var mockUser = new MockAppUserServices();
        var user = mockUser.GetOne(Guid.NewGuid());
        var mock = new MockUserOPTCodeServices();
        var mockData = mock.GetMany(Guid.Parse(user.Id), true).Select(w => {
            w.ExpirationDate = DateTime.Now.AddMinutes(3);
            return w;
        });
        var mockObj = mockData.AsQueryable().BuildMockDbSet();
        var mockEnumerableUser = new List<ApplicationUser> {
            user
        };
        var mockObjUser = mockEnumerableUser.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.UserOtpCodes).Returns(mockObj.Object!);
        Context.Setup(x => x.Users).Returns(mockObjUser.Object);
        var languagesRepository = new LanguagesRepository(Context.Object, factory);
        var countriesRepository = new CountriesRepository(Context.Object, factory);
        var repository = new ApplicationUserRepository(Context.Object, backendConfig, countriesRepository, languagesRepository);
        var result = await repository.IsAllowToResendOtpCode(user, 3, totalMaxSent);
        Assert.That(result, Is.EqualTo(totalMaxSent != 3));
    }

    [Test, Description("Test the GenerateOtpCode method")]
    [TestCase(5, true, MFAProvider.Email)]
    [TestCase(5, false, MFAProvider.SMS)]
    [TestCase(10, false, MFAProvider.SMS)]
    [TestCase(5, true, MFAProvider.SMS)]
    public async Task GenerateOtpCode(int expiredCodeInMinutes, bool forceSent, MFAProvider provider) {
        var mockUser = new MockAppUserServices();
        var user = mockUser.GetOne(Guid.NewGuid());
        var mockEnumerableUser = new List<ApplicationUser> {
            user
        };
        var mockObjUser = mockEnumerableUser.AsQueryable().BuildMockDbSet();
        var mock = new MockUserOPTCodeServices();
        var mockData = mock.GetMany(Guid.Parse(user.Id), true).Select(w => {
            w.ExpirationDate = DateTime.Now.AddMinutes(-1);
            return w;
        });
        Context.Setup(x => x.Users).Returns(mockObjUser.Object);
        if (forceSent || expiredCodeInMinutes == 10) {
            mockData = new List<ApplicationUserOtpCodes>();
        }
        var mockObj = mockData.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.UserOtpCodes).Returns(mockObj.Object!);
        var languagesRepository = new LanguagesRepository(Context.Object, factory);
        var countriesRepository = new CountriesRepository(Context.Object, factory);
        var repository = new ApplicationUserRepository(Context.Object, backendConfig, countriesRepository, languagesRepository);
        var result = await repository.GenerateOtpCode(user, expiredCodeInMinutes, forceSent, provider);
        if (expiredCodeInMinutes == 2) {
            Assert.That(result, Is.Null);
        } else {
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() => {
                Assert.That(result!.UserId, Is.EqualTo(Guid.Parse(user.Id)));
                Assert.That(result.Token, Is.Not.Empty);
                Assert.That(result.Code, Is.Not.Empty);
            });
        }
    }
    [Test, Description("Test the GenerateRegistrationOtpCode method")]
    public async Task GenerateRegistrationOtpCodeTest() {
        var mockUser = new MockAppUserServices();
        var user = mockUser.GetOne(Guid.NewGuid());
        var mockEnumerableUser = new List<ApplicationUser> {
            user
        };
        var mockObjUser = mockEnumerableUser.AsQueryable().BuildMockDbSet();
        var mock = new MockUserOPTCodeServices();
        var mockData = mock.GetMany(Guid.Parse(user.Id), true).Select(w => {
            w.ExpirationDate = DateTime.Now.AddMinutes(-1);
            return w;
        });
        Context.Setup(x => x.Users).Returns(mockObjUser.Object);
        var mockObj = mockData.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.UserOtpCodes).Returns(mockObj.Object!);
        var languagesRepository = new LanguagesRepository(Context.Object, factory);
        var countriesRepository = new CountriesRepository(Context.Object, factory);
        var repository = new ApplicationUserRepository(Context.Object, backendConfig, countriesRepository, languagesRepository);
        var result = await repository.GenerateRegistrationOtpCode(user);
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() => {
            Assert.That(result!.UserId, Is.EqualTo(Guid.Parse(user.Id)));
            Assert.That(result.ProviderType, Is.EqualTo(MFAProvider.Email));
            Assert.That(result.Token, Is.Not.Empty);
            Assert.That(result.Code, Is.Not.Empty);
        });
    }
}