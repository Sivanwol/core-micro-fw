using System.Text.RegularExpressions;
using Serilog.Enrichers.Sensitive;
namespace Application.Utils.Logs.Masking;

public class EmailMaskOperator : EmailAddressMaskingOperator {
    protected override string PreprocessMask(string mask, Match match) {
        var parts = match.Value.Split('@');

        return mask + "@" + parts[1];
    }
}