using Application.Utils;
namespace Test.Shared.Common;

public class MockClock : IClock {
    private readonly DateTime _utcNow;

    public MockClock(DateTime utcNow) {
        _utcNow = utcNow;
    }

    public DateTime Now() {
        return _utcNow;
    }
}