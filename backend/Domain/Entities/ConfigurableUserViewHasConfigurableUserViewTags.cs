using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;
namespace Domain.Entities;

[Table("configurable-user-view-has-configurable-user-view-tags")]
public class ConfigurableUserViewHasConfigurableUserViewTags : BaseEntityWithoutId {

    public int ConfigurableUserViewId { get; set; }
    public ConfigurableUserView ConfigurableUserView { get; set; }
    public int ConfigurableUserViewTagsId { get; set; }
    public ConfigurableUserViewTags ConfigurableUserViewTags { get; set; }
}