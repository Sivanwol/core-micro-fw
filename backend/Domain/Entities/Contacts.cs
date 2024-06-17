using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Application.Queries;
using Domain.Entities.Common;
namespace Domain.Entities;

[Table("contacts")]
public class Contacts : BaseEntity, ISoftDeletable
{
    [Column(TypeName = "varchar(75)")]
    public string FirstName { get; set; }

    [Column(TypeName = "varchar(75)")]
    public string LastName { get; set; }

    [MaxLength(500)]
    public string Email { get; set; }

    [Column(TypeName = "varchar(20)")]
    public string? Phone1 { get; set; }

    [Column(TypeName = "varchar(20)")]
    public string? Phone2 { get; set; }

    [Column(TypeName = "varchar(20)")]
    public string? Fax { get; set; }

    [Column(TypeName = "varchar(20)")]
    public string? Whatsapp { get; set; }

    [ForeignKey("Countries")]
    public int CountryId { get; set; }

    public Countries? Country { get; set; }

    [Column(TypeName = "varchar(10)")]
    public string? PostalCode { get; set; }

    [Column(TypeName = "varchar(100)")]
    public string? Address { get; set; }

    [Column(TypeName = "varchar(100)")]
    public string? City { get; set; }

    [Column(TypeName = "varchar(100)")]
    public string? State { get; set; }

    [Column(TypeName = "varchar(100)")]
    public string JobTitle { get; set; }

    [Column(TypeName = "varchar(100)")]
    public string? Department { get; set; }

    [Column(TypeName = "varchar(100)")]
    public string Company { get; set; }

    [Column(TypeName = "text")]
    public string Notes { get; set; }

    public IEnumerable<VendorHasContact> Vendors { get; set; }
    public IEnumerable<ProviderHasContact> Providers { get; set; }
    public DateTime? DeletedAt { get; set; }

    public Infrastructure.GQL.Contact ToGql()
    {
        return new Infrastructure.GQL.Contact
        {
            Id = Id,
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            Phone1 = Phone1,
            Phone2 = Phone2,
            Fax = Fax,
            Whatsapp = Whatsapp,
            Country = Country?.ToGql(),
            City = City,
            State = State,
            Address = Address,
            PostalCode = PostalCode,
            JobTitle = JobTitle,
            Department = Department,
            Company = Company,
            Notes = Notes,
        };
    }
}