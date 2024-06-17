using Infrastructure.Enums;
using Infrastructure.GQL;
using Infrastructure.GQL.Inputs.Media;
using Infrastructure.Requests.Processor.Common;
namespace Infrastructure.Requests.Processor.Services.User;

public class UploadMediaRequest : BaseRequest<IEnumerable<Media>> {
    public UploadOperationType OperationType { get; set; }
    public bool IsGlobalFiles { get; set; }
    public IEnumerable<ImageMedia> Files { get; set; }
}