using Infrastructure.Responses.Controllers.General;
namespace Domain.Entities.User;

public class QuestionInfo {
    public Questions Question { get; set; }
    public IEnumerable<QuestionAnswers>? Answers { get; set; }
    public QuestionResponse ToResponse() {
        return new QuestionResponse {
            Id = Question.Id,
            CategoryId = Question.CategoryId,
            LanguageId = Question.LanguageId,
            Text = Question.Text,
            Type = Enum.GetName(Question.Type)!,
            Answers = Answers?.Select(a => a.ToResponse())
        };
    }
}