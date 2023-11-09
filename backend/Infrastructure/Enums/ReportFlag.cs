using System.ComponentModel;
namespace Infrastructure.Enums;

public enum ReportFlag {
    [Description("Fake Profile")]
    FakeProfile = 1,

    [Description("Inappropriate Content")]
    InappropriateContent = 2,

    [Description("Harassment")]
    Harassment = 3,

    [Description("Spam")]
    Spam = 4,

    [Description("Other")]
    Other = 5
}