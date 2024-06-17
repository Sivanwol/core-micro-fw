using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Application.Queries;
using Domain.Entities.Common;
using Infrastructure.Enums;
namespace Domain.Entities;

[Table("providers")]
public class Providers : BaseEntity, ISoftDeletable
{

    [ForeignKey("Client")]
    public int? ClientId { get; set; }

    public Clients? Client { get; set; }
    [ForeignKey("User")]
    [StringLength(100)]
    public string UserId { get; set; }

    public ApplicationUser User { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    [ForeignKey("Countries")]
    public int CountryId { get; set; }
    public Countries Country { get; set; }
    public int? LogoId { get; set; }

    [ForeignKey("Media")]
    public Media? Logo { get; set; }
    public string SiteUrl { get; set; }
    public string SupportUrl { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
    public string? SupportPhone { get; set; }
    public string? SupportEmail { get; set; }
    public ProviderType ProviderType { get; set; }
    public ICollection<ProviderHasContact> Contacts { get; set; }
    public ICollection<ProviderHasProviderCategory> Categories { get; set; }
    public DateTime? DisabledAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}