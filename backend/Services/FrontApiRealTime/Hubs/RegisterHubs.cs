using FrontApiRealTime.Hubs.Common;

namespace FrontApiRealTime.Hubs;

public static class RegisterHubs {
    public static void Register(WebApplication app) {
        app.MapHub<GeneralHub>("/general");
    }
}