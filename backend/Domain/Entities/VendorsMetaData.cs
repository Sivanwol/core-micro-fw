using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;
namespace Domain.Entities;

[Table("vendor_metadata")]
public class VendorsMetaData : BaseEntity {
    [ForeignKey("Vendor")]
    public int VendorId { get; set; }

    public Vendors Vendor { get; set; }
    public string Key { get; set; }
    public string Description { get; set; }
    public string Value { get; set; }
}