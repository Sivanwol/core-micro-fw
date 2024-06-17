using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Application.Queries;
using Domain.Entities.Common;
using Infrastructure.Enums;
namespace Domain.Entities;

[Table("assets")]
public class Assets : BaseEntity, ISoftDeletable {
    [ForeignKey("Client")]
    public int? ClientId { get; set; }

    public Clients? Client { get; set; }

    [ForeignKey("Vendor")]
    public int VendorId { get; set; }

    public Vendors Vendor { get; set; }

    [ForeignKey("Provider")]
    public int ProviderId { get; set; }

    public Providers Provider { get; set; }

    [StringLength(100)]
    public string Label { get; set; }

    [Column(TypeName = "text")]
    public string Notes { get; set; }

    [StringLength(250)]
    public string Model { get; set; }

    [StringLength(100)]
    public string SubModel { get; set; }

    [StringLength(100)]
    public string ServiceTag { get; set; }

    [ForeignKey("ServiceTagMedia")]
    public int ServiceTagMediaId { get; set; }

    [ForeignKey("ExtraMedia")]
    public int ExtraMediaId { get; set; }

    public Media ExtraMedia { get; set; }

    [StringLength(20)]
    public string? Cpu { get; set; }

    [StringLength(20)]
    public string? Memory { get; set; }

    [StringLength(20)]
    public string? StorageHDD1 { get; set; }

    [StringLength(20)]
    public string? StorageHDD2 { get; set; }

    [StringLength(20)]
    public string? StorageHDD3 { get; set; }

    [StringLength(20)]
    public string? StorageHDD4 { get; set; }

    [StringLength(20)]
    public string? StorageSSD1 { get; set; }

    [StringLength(20)]
    public string? StorageSSD2 { get; set; }

    [StringLength(20)]
    public string? StorageSSD3 { get; set; }

    [StringLength(20)]
    public string? StorageSSD4 { get; set; }
    public AssetType Type { get; set; }
    public bool? IsVMSupported { get; set; }

    public bool? IsRaidSupported { get; set; }
    public int? RaidNumber { get; set; }

    [DefaultValue(AssetStatus.UNASSIGNED)]
    public AssetStatus Status { get; set; }
    public ICollection<ClientEmployeesHasAssets> Employees { get; set; }
    public ICollection<ClientServersHasAssets> Servers { get; set; }
    public ICollection<ClientNetworksHasAssets> Networks { get; set; }
    public DateTime? DeletedAt { get; set; }
}