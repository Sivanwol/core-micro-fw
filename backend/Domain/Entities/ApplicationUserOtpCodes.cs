using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Application.Utils;
using Domain.Entities.Common;
using Infrastructure.Enums;
namespace Domain.Entities;

[Table("user-otps")]
public class ApplicationUserOtpCodes : BaseEntity {

    [ForeignKey("ApplicationUser")]
    public Guid UserId { get; set; }

    public ApplicationUser User { get; set; }
    public MFAProvider ProviderType { get; set; }

    [StringLength(128)]
    public string Token { get; set; }

    [StringLength(100)]
    public string Code { get; set; }

    public DateTime ExpirationDate { get; set; }
    public DateTime? ComplateAt { get; set; }

    public static string GenerateCode() {
        var random = new Random();
        var code = random.Next(100000, 999999).ToString();
        return code;
    }
    public static string GenerateCodeAsToken() {
        var random = new Random();
        var code = StringExtensions.RandomUniqueString(random.Next(10, 15));
        return code;
    }
}