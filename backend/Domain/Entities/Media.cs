using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Infrastructure.Responses.Common;
namespace Domain.Entities;

[Table("Media")]
public class Media : BaseEntity {
    public int UserId { get; set; }
    public string Path { get; set; }
    public string FileName { get; set; }
    public string FileUrl { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public MediaInfo ToMediaInfo() {
        return new MediaInfo {
            Id = Id,
            ImageWidth = Width,
            ImageHeight = Height,
            Url = FileUrl
        };
    }
}