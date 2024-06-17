namespace Application.Utils;

public static class SystemClock {
    private static IClock _clock = new DefaultClock();

    public static void SetClock(IClock clock) {
        _clock = clock;
    }

    public static DateTime Now() {
        return _clock.Now();
    }

    private class DefaultClock : IClock {
        public DateTime Now() {
            return DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        }
    }
}