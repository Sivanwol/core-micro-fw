using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Persistence.Context;
using Domain.Persistence.Repositories.Common;
using Domain.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Persistence.Repositories;

public class MediaRepository : BaseRepository, IMediaRepository
{
    public MediaRepository(IDomainContext context) : base(context)
    {
    }


    public async Task<Media> CreateMedia(string bucketName, string fileName, string path, string mineType, long fileSize)
    {
        var media = new Media{
            FileName = fileName,
            Path = path,
            BucketName = bucketName,
            MimeType = mineType,
            Size = fileSize
        };
        await Context.Media.AddAsync(media);
        await Context.Instance.SaveChangesAsync();
        return media;
    }

    public async Task<IEnumerable<Media>> GetMedia(IEnumerable<int> mediaIds)
    {
        var media = await Context.Media.Where(m => mediaIds.Contains(m.Id)).ToListAsync();
        return media;
    }

    public async Task<Media?> GetMedia(int mediaId)
    {
        if (await MediaExists(mediaId))
        {
            var media = await Context.Media.FindAsync(mediaId);
            return media;
        }
        return null;
    }
    
    public async Task<bool> MediaExists(int id) {
        var media = await Context.Media.FindAsync(id);
        return media!= null;
    }

    public async Task RemoveMedia(IEnumerable<int> mediaIds)
    {
        foreach (var mediaId in mediaIds) {
            if (await MediaExists(mediaId))
            {
                var media = await Context.Media.FindAsync(mediaId);
                Context.Media.Remove(media!);
            }
        }
        await Context.Instance.SaveChangesAsync();
    }
}
