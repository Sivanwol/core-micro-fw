using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Application.Queries;
using Domain.Entities.Common;
namespace Domain.Entities;

[Table("provider_category")]
public class ProviderCategory : BaseEntity, ISoftDeletable
{

    [ForeignKey("User")]
    [StringLength(100)]
    public string UserId { get; set; }

    public ApplicationUser User { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<ProviderHasProviderCategory> Providers { get; set; }
    public DateTime? DeletedAt { get; set; }

    public Infrastructure.GQL.ProviderCategory ToGQL()
    {
        return new Infrastructure.GQL.ProviderCategory
        {
            Id = Id,
            Name = Name,
            Description = Description,
        };
    }
}