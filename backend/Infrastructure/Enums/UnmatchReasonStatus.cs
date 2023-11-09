using System.ComponentModel;
namespace Infrastructure.Enums;

public enum UnmatchReasonStatus {

    [Description("User Unmatch")]
    UserUnmatch = 1,

    [Description("User Blocked")]
    UserBlocked = 2,
}