using Application.Enums;

namespace Application.Exceptions;

public class MFAProviderNotImplementedException : BaseException {

    public MFAProviderNotImplementedException(int countryId, string providerName) : base("Counties", StatusCodeErrors.NotFound,
        $"MFA provider on country [{countryId}, {providerName}] Not Found") { }
    public MFAProviderNotImplementedException(int countryId) : base("Counties", StatusCodeErrors.NotFound,
        $"MFA provider on country [{countryId}, N/A] Not Found") { }
}