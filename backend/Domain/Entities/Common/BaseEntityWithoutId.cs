using System.ComponentModel.DataAnnotations;
namespace Domain.Entities.Common;

public abstract class BaseEntityWithoutId {
    [DataType(DataType.DateTime)]
    public DateTime UpdatedAt { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }
}