using Application.Utils;

namespace Processor.test.Common;

public class MockClock : IClock {
    private readonly DateTimeOffset _utcNow;

    public MockClock(DateTimeOffset utcNow) {
        _utcNow = utcNow;
    }

    public DateTimeOffset Now() => _utcNow;
}