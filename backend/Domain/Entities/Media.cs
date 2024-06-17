using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;
namespace Domain.Entities;

[Table("media")]
public class Media : BaseEntity
{
    [StringLength(500)]
    public string FileName { get; set; }

    [StringLength(500)]
    public string Path { get; set; }

    [StringLength(20)]
    public string MimeType { get; set; }

    public long Size { get; set; }

    [StringLength(100)]
    public string BucketName { get; set; }
    
    public Infrastructure.GQL.Media ToGql()
    {
        return new Infrastructure.GQL.Media()
        {
            Id = Id,
            FileName = FileName,
            Path = Path,
            FileSize = Size,
            MimeType = MimeType,
            BucketName= BucketName
        };
    }
}