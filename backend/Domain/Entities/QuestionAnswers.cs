using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Infrastructure.Responses.Controllers.General;
namespace Domain.Entities;

[Table("QuestionAnswers")]
public class QuestionAnswers : BaseEntity {
    public int QuestionId { get; set; }
    public string Text { get; set; }

    public QuestionAnswerResponse ToResponse() {
        return new QuestionAnswerResponse {
            Id = Id,
            Text = Text
        };
    }
}