using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;
namespace Domain.Entities;

[Table("vendor_has_contacts")]
public class VendorHasContact : BaseEntityWithoutId {
    public int VendorId { get; set; }
    public Vendors Vendor { get; set; }
    public int ContactId { get; set; }
    public Contacts Contact { get; set; }
}