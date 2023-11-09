namespace Infrastructure.Responses.Controllers.General;

public class QuestionResponse {
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public int LanguageId { get; set; }
    public string Text { get; set; }
    public string Type { get; set; }
    public int? Rating { get; set; }
    public IEnumerable<QuestionAnswerResponse>? Answers { get; set; }
}