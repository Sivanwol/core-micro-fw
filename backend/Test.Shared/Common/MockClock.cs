using Application.Utils;
namespace Test.Shared.Common;

public class MockClock : IClock {
    private readonly DateTimeOffset _utcNow;

    public MockClock(DateTimeOffset utcNow) {
        _utcNow = utcNow;
    }

    public DateTimeOffset Now() => _utcNow;
}