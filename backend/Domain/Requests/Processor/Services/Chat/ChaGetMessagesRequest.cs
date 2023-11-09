using Domain.Responses.Controllers.Chat;
using MediatR;
namespace Domain.Requests.Processor.Services.Chat;

public class ChaGetMessagesRequest : Infrastructure.Requests.Processor.Services.Chat.ChaGetMessagesRequest, IRequest<MessagesResponse> { }