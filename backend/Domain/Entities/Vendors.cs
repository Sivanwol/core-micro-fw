using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Application.Queries;
using Domain.Entities.Common;
using Infrastructure.Enums;
using Infrastructure.GQL;
namespace Domain.Entities;

[Table("vendors")]
public class Vendors : BaseEntity, ISoftDeletable {
    [ForeignKey("Client")]
    public int? ClientId { get; set; }

    public Clients? Client { get; set; }
    [ForeignKey("User")]
    [StringLength(100)]
    public string UserId { get; set; }

    public ApplicationUser User { get; set; }
    [StringLength(100)]
    public string Name { get; set; }

    [Column(TypeName = "text")]
    public string Description { get; set; }

    [ForeignKey("Countries")]
    public int CountryId { get; set; }
    public Countries Country { get; set; }
    public int? LogoId { get; set; }

    [ForeignKey("Media")]
    public Media? Logo { get; set; }
    [StringLength(500)]
    public string SiteUrl { get; set; }
    [StringLength(500)]
    public string SupportUrl { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
    public string? SupportPhone { get; set; }
    public string? SupportEmail { get; set; }
    public VendorSupportResponseType SupportResponseType { get; set; }
    public IEnumerable<VendorHasContact> Contacts { get; set; }
    public DateTime? DisabledAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    
    public Vendor ToGql() {
        return new Vendor
        {
            Id = Id,
            Name = Name,
            Description = Description,
            Country = Country.ToGql(),
            Logo = Logo?.ToGql(),
            SiteUrl = SiteUrl,
            SupportUrl = SupportUrl,
            SupportEmail = SupportEmail,
            SupportPhone = SupportPhone,
            SupportResponseType = SupportResponseType,
            Contacts = Contacts.Any()?Contacts.Select(c => c.Contact.ToGql()).ToList(): new List<Contact>()
        };
    }
}