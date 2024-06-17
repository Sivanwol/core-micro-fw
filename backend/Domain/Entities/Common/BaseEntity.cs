using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
namespace Domain.Entities.Common;

public abstract class BaseEntity {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime UpdatedAt { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }

    public string ToJson() {
        return JsonConvert.SerializeObject(this);
    }
}