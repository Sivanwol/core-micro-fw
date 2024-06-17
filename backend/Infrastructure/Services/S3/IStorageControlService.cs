using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Contract;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.S3;

public interface IStorageControlService
{
    Task<StorageUploadStatus> UploadFiles(IEnumerable<IFormFile> files, string bucketName, string filePath);
    Task CreateFolder(string bucketName, string filePath);
    Task<bool> DeleteFolder(string bucketName, string filePath);
    Task<StorageUploadStatus> DeleteFiles(IEnumerable<FileUpload> files, string bucketName);
    public Task<bool> DeleteFile(FileUpload file, string bucketName);
    public Task<IEnumerable<FileUpload>> GetFiles(string bucketName, string filePath);
    public Task<bool> HasFileExists(FileUpload file, string bucketName);
    public Task<bool> HasFolderExists(string bucketName, string filePath);
}
