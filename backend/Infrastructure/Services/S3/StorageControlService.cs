using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Application.Configs;
using Application.Constraints;
using Application.Contract;
using Application.Enums;
using Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace Infrastructure.Services.S3;

public class StorageControlService : IStorageControlService
{
    private readonly BackendApplicationConfig _config;
    private readonly IAmazonS3 _client;
    public StorageControlService(IAmazonS3 client, BackendApplicationConfig config)
    {
        _client = client;
        _config = config;
    }

    public async Task CreateFolder(string bucketName, string filePath)
    {
        if (!IsBucketExists(bucketName))
        {
            throw new StorageException("Invalid bucket name", StatusCodeErrors.BadRequest);
        }
        try
        {
            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = filePath.Trim(),
                ContentBody = string.Empty
            };
            await _client.PutObjectAsync(request);
        }
        catch (AmazonS3Exception ex)
        {
            Log.Logger.Error($"CreateFolder: {ex.Message}", ex);
            throw new StorageException(ex.Message, StatusCodeErrors.InternalServerError);
        }
        catch (Exception ex)
        {
            Log.Logger.Error($"CreateFolder: {ex.Message}", ex);
            throw new StorageException(ex.Message, StatusCodeErrors.InternalServerError);
        }
    }


    public async Task<bool> DeleteFolder(string bucketName, string filePath)
    {
        if (!IsBucketExists(bucketName) || !await HasFolderExists(bucketName, filePath))
        {
            throw new StorageException("Invalid bucket name or folder path", StatusCodeErrors.BadRequest);
        }
        try
        {
            var request = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = filePath.Trim()
            };
            await _client.DeleteObjectAsync(request);
            return true;
        }
        catch (AmazonS3Exception ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return true; // File does not exist
            }
            else
            {
                Log.Logger.Error($"HasFolderExists: {ex.Message}", ex);
            }
        }
        catch (Exception ex)
        {
            Log.Logger.Error($"HasFolderExists: {ex.Message}", ex);
        }
        return false;
    }
    public async Task<bool> HasFolderExists(string bucketName, string filePath)
    {
        if (!IsBucketExists(bucketName) || string.IsNullOrEmpty(filePath.Trim()))
        {
            throw new StorageException("Invalid params for storage operation", StatusCodeErrors.BadRequest);
        }
        try
        {
            var request = new GetObjectMetadataRequest
            {
                BucketName = bucketName,
                Key = filePath.Trim()
            };
            await _client.GetObjectMetadataAsync(request);
            return true;
        }
        catch (AmazonS3Exception ex)
        {
            if (ex.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                Log.Logger.Error($"HasFolderExists: {ex.Message}", ex);
            }
        }
        catch (Exception ex)
        {
            Log.Logger.Error($"HasFolderExists: {ex.Message}", ex);
        }
        return false;
    }

    public async Task<bool> DeleteFile(FileUpload file, string bucketName)
    {
        if (!IsBucketExists(bucketName))
        {
            throw new StorageException("Invalid bucket name", StatusCodeErrors.BadRequest);
        }
        try
        {

            if (!await HasFileExists(file, bucketName))
            {
                return false;
            }
            var request = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = $"{file.Path}/{file.FileName}"
            };
            await _client.DeleteObjectAsync(request);
            return true;
        }
        catch (AmazonS3Exception ex)
        {
            if (ex.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                Log.Logger.Error($"DeleteFile: {ex.Message}", ex);
            }
        }
        catch (Exception ex)
        {
            Log.Logger.Error($"DeleteFile: {ex.Message}", ex);
        }
        return false;
    }

    public async Task<StorageUploadStatus> DeleteFiles(IEnumerable<FileUpload> files, string bucketName)
    {
        var result = new StorageUploadStatus();
        if (!IsBucketExists(bucketName))
        {
            throw new StorageException("Invalid bucket name", StatusCodeErrors.BadRequest);
        }
        foreach (var file in files)
        {
            try
            {
                if (!await HasFileExists(file, bucketName))
                {
                    result.FailedFiles.Add(file);
                    continue; // File does not exist
                }
                var request = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = $"{file.Path}/{file.FileName}"
                };
                await _client.DeleteObjectAsync(request);
                result.SuccessFiles.Add(file);
                continue;
            }
            catch (AmazonS3Exception ex)
            {
                if (ex.StatusCode != System.Net.HttpStatusCode.NotFound)
                {
                    Log.Logger.Error($"DeleteFiles: {ex.Message}", ex);
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error($"DeleteFiles: {ex.Message}", ex);
            }
            result.FailedFiles.Add(file); // error on operation
        }
        return result;
    }

    public async Task<IEnumerable<FileUpload>> GetFiles(string bucketName, string filePath)
    {
        if (!IsBucketExists(bucketName) || !await HasFolderExists(bucketName, filePath))
        {
            throw new StorageException("Invalid bucket name or folder path", StatusCodeErrors.BadRequest);
        }
        List<FileUpload> files = new List<FileUpload>();
        try
        {
            var request = new ListObjectsRequest
            {
                BucketName = bucketName,
                Prefix = filePath.Trim()
            };
            var response = await _client.ListObjectsAsync(request);
            if (response.S3Objects != null && response.S3Objects.Count > 0)
            {
                foreach (var s3Object in response.S3Objects)
                {
                    try
                    {
                        var requestObjData = new GetObjectMetadataRequest
                        {
                            BucketName = bucketName,
                            Key = s3Object.Key
                        };
                        var responseMetaData = await _client.GetObjectMetadataAsync(requestObjData);
                        var mineType = responseMetaData.Headers.ContentType;
                        var fileSize = responseMetaData.Headers.ContentLength;
                        files.Add(new FileUpload
                        {
                            FileName = s3Object.Key,
                            Path = s3Object.Key.Replace($"{filePath.Trim()}/", string.Empty),
                            FileSize = fileSize,
                            MineType = mineType
                        });
                    }
                    catch (AmazonS3Exception ex)
                    {
                        if (ex.StatusCode != System.Net.HttpStatusCode.NotFound)
                        {
                            Log.Logger.Error($"GetFiles: failed fetch inner look lookup for metadata - {ex.Message}", ex);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.Error($"GetFiles: failed fetch inner look lookup for metadata -{ex.Message}", ex);
                    }
                }
            }
        }
        catch (AmazonS3Exception ex)
        {
            if (ex.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                Log.Logger.Error($"GetFiles: {ex.Message}", ex);
            }
        }
        catch (Exception ex)
        {
            Log.Logger.Error($"GetFiles: {ex.Message}", ex);
        }
        return files;
    }

    public async Task<bool> HasFileExists(FileUpload file, string bucketName)
    {
        if (!IsBucketExists(bucketName))
        {
            throw new StorageException("Invalid bucket name", StatusCodeErrors.BadRequest);
        }
        try
        {
            var request = new GetObjectMetadataRequest
            {
                BucketName = bucketName,
                Key = $"{file.Path}/{file.FileName}"
            };
            await _client.GetObjectMetadataAsync(request);
            return true;
        }
        catch (AmazonS3Exception ex)
        {
            if (ex.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                Log.Logger.Error($"HasFileExists: {ex.Message}", ex);
            }
        }
        catch (Exception ex)
        {
            Log.Logger.Error($"HasFileExists: {ex.Message}", ex);
        }
        return false;

    }

    public async Task<StorageUploadStatus> UploadFiles(IEnumerable<IFormFile> files, string bucketName, string filePath)
    {
        Log.Logger.Information($"Uploading files to bucket: {bucketName} on path: {filePath} with {files.Count()} files");
        var result = new StorageUploadStatus();
        List<FileUpload> fileUploads = new List<FileUpload>();
        var maxFileSize = Convert.ToInt64(_config.MaxFileSize * 1024 * 1024);
        if (!IsBucketExists(bucketName) || !await HasFolderExists(bucketName, filePath))
        {
            throw new StorageException("Invalid bucket name or folder path", StatusCodeErrors.BadRequest);
        }
        for (int i = 0; i < files.Count(); i++)
        {
            var file = files.ElementAt(i);
            long fileSize = file.Length;
            if (fileSize > maxFileSize)
            {
                fileUploads.Add(new FileUpload()
                {
                    FileSize = fileSize,
                    Path = file.FileName,
                    FileName = file.FileName,
                    MineType = file.ContentType
                });
            }
            else
            {
                result.FailedFiles.Add(new FileUpload()
                {
                    FileSize = fileSize,
                    Path = file.FileName,
                    FileName = file.FileName,
                    MineType = file.ContentType
                });
            }
        }
        Log.Logger.Information($"Uploading files to bucket: {bucketName} on path: {filePath} total failed files: {result.FailedFiles.Count()} total to upload: {fileUploads.Count()}");
        try
        {
            foreach (var file in fileUploads)
            {
                // get file name only
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var uploadObj = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = GenerateObjectName(fileName),
                    FilePath = filePath.Trim(),
                };
                var response = await _client.PutObjectAsync(uploadObj);
                if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                {
                    result.FailedFiles.Add(file);
                    continue;
                }
                result.SuccessFiles.Add(file);
            }
        }
        catch (AmazonS3Exception ex)
        {
            Log.Logger.Error($"UploadFiles: Failed Upload files, received {ex.Message}", ex);
            foreach (var file in fileUploads)
            {
                result.FailedFiles.Add(file);
            }
        }
        catch (Exception ex)
        {
            Log.Logger.Error($"UploadFiles: {ex.Message}", ex);
            foreach (var file in fileUploads)
            {
                result.FailedFiles.Add(file);
            }
        }
        return result;
    }

    private string GenerateObjectName(string fileName) => $"{fileName.Replace(" ", string.Empty)}_{DateTime.Now.ToString("yyyyMMdd")}_{Guid.NewGuid().ToString().Replace("-", string.Empty)}";
    private bool IsBucketExists(string bucketName)
    {
        switch (bucketName)
        {
            case StorageAws.BucketUploadBins:
            case StorageAws.BucketUserMedia:
            case StorageAws.BucketGlobalMedia:
                return true;
        }
        return false;
    }
}
