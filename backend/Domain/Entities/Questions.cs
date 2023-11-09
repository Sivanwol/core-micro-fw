using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Infrastructure.Enums;
using Infrastructure.Responses.Controllers.General;
namespace Domain.Entities;

[Table("Questions")]
public class Questions : BaseEntity {
    [Column("QuestionCategoryId")]
    public int CategoryId { get; set; }

    public int LanguageId { get; set; }
    public string Text { get; set; }
    public QuestionType Type { get; set; }
    public int Score { get; set; }
    public bool Active { get; set; }

    public QuestionResponse ToResponse() {
        return new QuestionResponse {
            Id = Id,
            CategoryId = CategoryId,
            LanguageId = LanguageId,
            Text = Text,
            Type = Enum.GetName(Type)!
        };
    }
}