using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Persistence.Repositories.Common.Interfaces;

namespace Domain.Persistence.Repositories.Interfaces;

public interface IMediaRepository: IGenericEmptyRepository<Media>
{
    Task<Media> CreateMedia(string bucketName,string fileName, string path, string mineType, long fileSize);
    Task RemoveMedia(IEnumerable<int> mediaIds);
    Task<IEnumerable<Media>> GetMedia(IEnumerable<int> mediaIds);
    Task<Media?> GetMedia(int mediaId);
    Task<bool> MediaExists(int id);
}
