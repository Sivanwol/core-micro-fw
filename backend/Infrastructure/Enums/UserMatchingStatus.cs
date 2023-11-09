using System.ComponentModel;
namespace Infrastructure.Enums;

public enum UserMatchingStatus {
    [Description("New")]
    New,

    [Description("Active")]
    Active,

    [Description("Reject")]
    Reject,

    [Description("End")]
    End
}