using System.ComponentModel;
namespace Infrastructure.Enums;

public enum SessionStatus {
    [Description("New")]
    New = 0,

    [Description("Open")]
    Open = 1,

    [Description("Match")]
    Match = 2,

    [Description("Reject")]
    Reject = 3
}