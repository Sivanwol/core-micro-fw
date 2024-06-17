using Application.Configs.General;
using Domain.Entities;
using Domain.Persistence.Mock.Services;
using Domain.Persistence.Repositories;
using Infrastructure.Services.Email;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using Test.Shared.Common;
namespace Domain.Persistence.Test.Repositories;

[TestFixture] [Description("Test the OTPRepositoryTest")]
[Category("Users")]
public class OTPRepositoryTest : BaseTest {
    [SetUp]
    public void Setup() {
        SetupTest("CountriesRepositoryTest");
        _razorViewEngine = new Mock<IRazorViewEngine>();
        _serviceProvider = new Mock<IServiceProvider>();
        _tempDataProvider = new Mock<ITempDataProvider>();
        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();

        factory = serviceProvider.GetService<ILoggerFactory>()!;
        logger = factory.CreateLogger<CountriesRepository>();
    }
    private Mock<IRazorViewEngine> _razorViewEngine;
    private Mock<IServiceProvider> _serviceProvider;
    private Mock<ITempDataProvider> _tempDataProvider;
    protected ILoggerFactory factory;
    protected ILogger logger;


    [Test] [Description("Test the VerifyOTP method")]
    [TestCase(false, "aa654321bb")]
    [TestCase(false, "123456")]
    [TestCase(true, "654321")]
    public async Task VerifyOTP(bool overrideCode, string code) {
        const string viewName = "MyView";
        const string expectedString = "Mock view result";
        // Mock the view
        var view = new Mock<IView>();
        view.Setup(m => m.RenderAsync(It.IsAny<ViewContext>()))
            .Callback((ViewContext context) => context.Writer.Write(expectedString))
            .Returns(Task.FromResult(0));

        // Set up the view engine to return the mock view
        _razorViewEngine.Setup(e => e.FindView(It.IsAny<ActionContext>(), It.IsAny<string>(), It.IsAny<bool>()))
            .Returns(ViewEngineResult.Found(viewName, view.Object));

        var templateService = new TemplateService(
            _razorViewEngine.Object,
            _serviceProvider.Object,
            _tempDataProvider.Object);
        var mockUser = new MockAppUserServices();
        var user = mockUser.GetOne(Guid.NewGuid());
        var mock = new MockUserOPTCodeServices();
        var mockData = new List<ApplicationUserOtpCodes> {
            overrideCode ? mock.GetOne(Guid.Parse(user.Id), true, code) : mock.GetOne(Guid.Parse(user.Id), true)
        };
        var expected = mockData.First();
        var mockObj = mockData.AsQueryable().BuildMockDbSet();
        var mockEnumerableUser = new List<ApplicationUser> {
            user
        };
        var mockObjUser = mockEnumerableUser.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.UserOtpCodes).Returns(mockObj.Object!);
        Context.Setup(x => x.Users).Returns(mockObjUser.Object);
        var frontConfigMock = new Mock<IOptions<FrontEndPaths>>();
        var repository = new OTPRepository(Context.Object, LoggerFactory.Create(x => x.AddConsole()), templateService, frontConfigMock.Object, new Mock<IMailService>().Object,
            backendConfig);
        var result = await repository.VerifyOTP(user, expected, code);
        Assert.Multiple(() => {
            if (overrideCode) {
                Assert.That(result.IsValid, Is.True);
                Assert.That(result.User, Is.EqualTo(user));
            } else {
                Assert.That(result.IsValid, Is.False);
                Assert.That(result.User, Is.Null);
            }
        });
    }
}