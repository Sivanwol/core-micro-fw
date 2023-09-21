namespace Application.Utils;

public static class SystemClock {
    private static IClock _clock = new DefaultClock();

    public static void SetClock(IClock clock) {
        _clock = clock;
    }

    public static DateTimeOffset Now() {
        return _clock.Now();
    }

    private class DefaultClock : IClock {
        public DateTimeOffset Now() => new DateTimeOffset(DateTime.Now.ToUniversalTime());
    }
}