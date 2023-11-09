using Domain.Entities.Message;
namespace Domain.Entities.Session;

public class QuestionInfo {
    public Questions Question { get; set; }
    public IEnumerable<ChatQuestionAnswer> Answers { get; set; }
}