using System.ComponentModel.DataAnnotations.Schema;
using Application.Queries;
using Domain.Entities.Common;
using Elastic.Apm.Api.Constraints;
using Infrastructure.GQL;
namespace Domain.Entities;

[Table("clients")]
public class Clients : BaseEntity, ISoftDeletable {
    [ForeignKey("OwnerUser")]
    [MaxLength(36)]
    public string OwnerUserId { get; set; }

    public ApplicationUser OwnerUser { get; set; }

    [ForeignKey("Parent")]
    public int? ParentId { get; set; }

    public Clients? Parent { get; set; }

    [Column(TypeName = "varchar(100)")]
    public string Name { get; set; }

    public string Description { get; set; }

    [Column(TypeName = "varchar(500)")]
    public string Website { get; set; }

    public DateTime? DisabledAt { get; set; }

    [ForeignKey("Countries")]
    public int CountryId { get; set; }

    public Countries Country { get; set; }

    [Column(TypeName = "varchar(100)")]
    public string Address { get; set; }

    [Column(TypeName = "varchar(100)")]
    public string City { get; set; }

    public ICollection<Clients> Children { get; set; }

    public ICollection<ClientContacts> Contacts { get; set; }
    public DateTime? DeletedAt { get; set; }

    public Client ToGql() {
        return new() {
            Id = Id,
            Parent = Parent?.ToGql(),
            Name = Name,
            Description = Description,
            Country = Country?.ToGql(),
            Website = Website,
            Address = Address,
            City = City,
            OwnerUser = OwnerUser?.ToGql(),
            CreatedAt = CreatedAt,
        };
    }
}