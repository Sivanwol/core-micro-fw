using System.ComponentModel.DataAnnotations.Schema;
using Application.Queries;
using Domain.Entities.Common;
using Infrastructure.GQL;
namespace Domain.Entities;

[Table("client-contacts")]
public class ClientContacts : BaseEntity, ISoftDeletable {
    [ForeignKey("Client")]
    public int ClientId { get; set; }

    public Clients Client { get; set; }

    [Column(TypeName = "varchar(75)")]
    public string FirstName { get; set; }

    [Column(TypeName = "varchar(75)")]
    public string LastName { get; set; }

    [Column(TypeName = "varchar(500)")]
    public string Email { get; set; }

    [Column(TypeName = "varchar(20)")]
    public string Phone1 { get; set; }

    [Column(TypeName = "varchar(20)")]
    public string? Phone2 { get; set; }

    [Column(TypeName = "varchar(20)")]
    public string? Fax { get; set; }

    [Column(TypeName = "varchar(20)")]
    public string? Whatsapp { get; set; }

    [ForeignKey("Countries")]
    public int CountryId { get; set; }

    public Countries Country { get; set; }

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

    public DateTime? DeletedAt { get; set; }

    public ClientContact ToGql() {
        return new ClientContact() {
            Id = Id,
            Client = Client.ToGql(),
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            Phone1 = Phone1,
            Phone2 = Phone2,
            Fax = Fax,
            Whatsapp = Whatsapp,
            Country = Country?.ToGql(),
            PostalCode = PostalCode,
            Address = Address,
            City = City,
            State = State,
            JobTitle = JobTitle,
            Department = Department,
            Company = Company,
            Notes = Notes,
            CreatedAt = CreatedAt
        };
    }
}