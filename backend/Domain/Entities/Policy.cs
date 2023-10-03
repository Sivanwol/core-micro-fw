using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Policy {
    public int Id { get; set; }

    [Required] [MaxLength(50)] public string Name { get; set; }
}