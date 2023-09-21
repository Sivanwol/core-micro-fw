namespace Application.Security;

public enum ScopeMode {
    Read,
    Write
}

public static class Scopes {
    private static readonly List<string> scopes = new() {
        "games:read",
        "games:write",
        "clients:read",
        "clients:write"
    };

    public static List<string> GetScopes() {
        return scopes;
    }

    public static string GetClientScope(ScopeMode mode) {
        return mode switch {
            ScopeMode.Read => "clients:read",
            ScopeMode.Write => "clients:write",
            _ => throw new NotImplementedException()
        };
    }

    public static string GetGameScope(ScopeMode mode) {
        return mode switch {
            ScopeMode.Read => "games:read",
            ScopeMode.Write => "games:write",
            _ => throw new NotImplementedException()
        };
    }
}