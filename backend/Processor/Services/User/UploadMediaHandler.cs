using Application.Constraints;
using Domain.Persistence.Repositories.Interfaces;
using Infrastructure.Enums;
using Infrastructure.GQL;
using Infrastructure.Requests.Processor.Services.User;
using Infrastructure.Services.S3;
using MediatR;

namespace Processor.Services.User;

public class UploadMediaHandler : IRequestHandler<UploadMediaRequest, IEnumerable<Media>>
{
    private readonly IStorageControlService storageControlService;
    private readonly IMediator mediator;
    private readonly IMediaRepository mediaRepository;
    public UploadMediaHandler(IMediator mediator, IStorageControlService storageControlService, IMediaRepository mediaRepository)
    {
        this.mediator = mediator;
        this.storageControlService = storageControlService;
        this.mediaRepository = mediaRepository;
    }

    public async Task<IEnumerable<Media>> Handle(UploadMediaRequest request, CancellationToken cancellationToken)
    {
        var media = new List<Media>();
        var bucketName = request.IsGlobalFiles ? StorageAws.BucketGlobalMedia : StorageAws.BucketUploadBins;
        var filePath = GetPathFromOperationType(request.OperationType);
        var statusUploads = await storageControlService.UploadFiles(request.Files.Select(x => x.Media), bucketName, filePath);
        foreach (var fileStatus in statusUploads.SuccessFiles)
        {
            var fileName = fileStatus.FileName;
            var mediaEntity = await mediaRepository.CreateMedia(bucketName, fileStatus.FileName, fileStatus.Path, fileStatus.MineType, fileStatus.FileSize);
        }
        return media;
    }

    private string GetPathFromOperationType(UploadOperationType operationType) {
        switch (operationType) {
            case UploadOperationType.Vendor:
            case UploadOperationType.VendorClient:
                return "vendors";
            case UploadOperationType.Provider:
            case UploadOperationType.ProviderClient:
                return "providers";
            default:
                return string.Empty;
        }
    }
}
