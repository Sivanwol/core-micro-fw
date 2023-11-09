using Domain.Entities.User;
namespace Domain.Persistence.Interfaces.Repositories;

public interface IQuestionsRepository : IGenericRepository<QuestionInfo> {
    Task<IEnumerable<QuestionInfo>> GetAll();
    Task<IEnumerable<QuestionInfo>> GetBySessionId(int sessionId);
}