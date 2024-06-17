using Application.Contract;

namespace Infrastructure.Services.S3;

public class StorageUploadStatus
{
    public IList<FileUpload> SuccessFiles { get; set; }
    public IList<FileUpload> FailedFiles { get; set; }
}
