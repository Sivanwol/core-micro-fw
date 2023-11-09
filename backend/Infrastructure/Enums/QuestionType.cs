using System.ComponentModel;
namespace Infrastructure.Enums;

public enum QuestionType {
    [Description("Radio")]
    Radio = 1,

    [Description("Checkbox")]
    Checkbox = 2,

    [Description("Test")]
    OpenText = 3,
}