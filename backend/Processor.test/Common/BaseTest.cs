using Application.Utils;
using Domain.Context;
using MediatR;
using Moq;
using Serilog;

namespace Processor.test.Common;

public class BaseTest {
    public DateTimeOffset MatchedDate { get; set; }
    public Mock<IDomainContext> Context { get; private set; }
    public Mock<IMediator> Mediator { get; private set; }

    public void SetupTest(string testServiceName) {
        // Create log file and redirect output to it
        Log.Logger = ServicesLogger.GetLoggerTesting($"Test::${testServiceName}");
        MatchedDate = DateTimeOffset.Now;
        SystemClock.SetClock(new MockClock(MatchedDate));
        Context = MockTestHelper.SetupContext();
        Mediator = new Mock<IMediator>();
    }

    public void ReloadData() {
        Context = MockTestHelper.SetupContext();
    }

    [TearDown]
    public void TearDown() {
        Log.CloseAndFlush();
    }
}